using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillMatrix.Api.Data;
using SkillMatrix.Api.Filters;
using SkillMatrix.Api.Models;

namespace SkillMatrix.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ContractorsController : ControllerBase
{
    private readonly AppDbContext _db;
    public ContractorsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _db.Contractors.ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOne(string id)
    {
        var item = await _db.Contractors.FindAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Contractor c)
    {
        _db.Contractors.Add(c);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetOne), new { id = c.ContractorId }, c);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Contractor c)
    {
        if (id != c.ContractorId) return BadRequest();
        _db.Entry(c).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [RequireDeletePassword]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var item = await _db.Contractors.FindAsync(id);
        if (item is null) return NotFound();
        _db.Contractors.Remove(item);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
