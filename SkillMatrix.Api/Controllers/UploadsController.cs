using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillMatrix.Api.Data;
using SkillMatrix.Api.Models;

namespace SkillMatrix.Api.Controllers;

// Saves an uploaded image into the Photos table and returns its URL, which is what
// gets stored on the Employee / Contractor / Eaton record. (Database, not disk, because
// hosts like Render wipe local files on redeploy.)
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UploadsController : ControllerBase
{
    private const long MaxBytes = 2 * 1024 * 1024; // 2 MB - the app shrinks photos before upload

    private static readonly Dictionary<string, string> Types = new()
    {
        [".jpg"] = "image/jpeg",
        [".jpeg"] = "image/jpeg",
        [".png"] = "image/png",
        [".gif"] = "image/gif",
        [".webp"] = "image/webp"
    };

    private readonly AppDbContext _db;
    public UploadsController(AppDbContext db) => _db = db;

    [HttpPost]
    [RequestSizeLimit(MaxBytes + 4096)]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file is null || file.Length == 0)
            return BadRequest(new { message = "No file uploaded." });

        if (file.Length > MaxBytes)
            return BadRequest(new { message = "File is too large (2 MB max)." });

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!Types.TryGetValue(ext, out var contentType))
            return BadRequest(new { message = "Unsupported file type. Use JPG, PNG, GIF, or WEBP." });

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);

        var photo = new Photo { Id = Guid.NewGuid(), ContentType = contentType, Data = ms.ToArray() };
        _db.Photos.Add(photo);
        await _db.SaveChangesAsync();

        var url = $"{Request.Scheme}://{Request.Host}/api/photos/{photo.Id}";
        return Ok(new { url });
    }
}
