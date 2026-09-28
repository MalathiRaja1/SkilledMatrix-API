using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SkillMatrix.Api.Data;
using SkillMatrix.Api.Filters;
using SkillMatrix.Api.Models;
using SkillMatrix.Api.Services;

namespace SkillMatrix.Api.Controllers;

public record RegisterRequest(string DeptCode, string UserName, string Password, string ConfirmPassword);
public record LoginRequest(string UserName, string Password);

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    [Authorize] // only logged-in users can create accounts (seed the first admin before enabling this)
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest req)
    {
        if (req.Password != req.ConfirmPassword)
            return BadRequest(new { message = "Passwords do not match." });

        if (await _db.Users.AnyAsync(u => u.UserName == req.UserName))
            return Conflict(new { message = "Username already exists." });

        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = HashPassword(req.Password, salt);

        var user = new User
        {
            DeptCode = req.DeptCode,
            UserName = req.UserName,
            PasswordHash = Convert.ToBase64String(hash),
            PasswordSalt = Convert.ToBase64String(salt),
            PasswordEncrypted = PasswordVault.Encrypt(req.Password, ViewKey)
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return Ok(new { message = "User created." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest req)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == req.UserName);
        if (user is null) return Unauthorized(new { message = "Invalid username or password." });

        var salt = Convert.FromBase64String(user.PasswordSalt);
        var hash = HashPassword(req.Password, salt);

        if (Convert.ToBase64String(hash) != user.PasswordHash)
            return Unauthorized(new { message = "Invalid username or password." });

        var token = GenerateJwt(user);
        return Ok(new { token, userName = user.UserName, deptCode = user.DeptCode });
    }

    [Authorize]
    [HttpGet("users")]
    public async Task<IActionResult> Users() =>
        Ok(await _db.Users
            .OrderBy(u => u.UserName)
            .Select(u => new { u.Id, u.UserName, u.DeptCode, DeptName = u.Department!.DeptName, HasPassword = u.PasswordEncrypted != null })
            .ToListAsync());

    [Authorize]
    [RequireDeletePassword]
    [HttpDelete("users/{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user is null) return NotFound();

        var me = User.Identity?.Name ?? User.FindFirst("unique_name")?.Value;
        if (user.UserName == me)
            return BadRequest(new { message = "You can't delete the account you are logged in with." });

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // Guarded by the same admin password as delete.
    [Authorize]
    [RequireDeletePassword]
    [HttpGet("users/{id:int}/password")]
    public async Task<IActionResult> RevealPassword(int id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user is null) return NotFound();

        var plain = PasswordVault.Decrypt(user.PasswordEncrypted, ViewKey);
        if (plain is null)
            return NotFound(new { message = "No viewable password is stored for this user (created before this feature). Delete and re-create the user to set one." });

        return Ok(new { password = plain });
    }

    private string ViewKey => _config["PasswordViewKey"] ?? _config["Jwt:Key"]!;

    private static byte[] HashPassword(string password, byte[] salt) =>
        Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), salt, 100_000, HashAlgorithmName.SHA256, 32);

    private string GenerateJwt(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim("deptCode", user.DeptCode)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpiryMinutes"] ?? "480"));

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: expiry,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
