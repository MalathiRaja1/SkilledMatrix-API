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
public class AreasController : ControllerBase
{
    private readonly AppDbContext _db;
    public AreasController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _db.Areas.ToListAsync());

    [HttpGet("{areaCode}")]
    public async Task<IActionResult> GetOne(string areaCode)
    {
        var item = await _db.Areas.FindAsync(areaCode);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Area area)
    {
        _db.Areas.Add(area);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetOne), new { areaCode = area.AreaCode }, area);
    }

    [HttpPut("{areaCode}")]
    public async Task<IActionResult> Update(string areaCode, Area area)
    {
        if (areaCode != area.AreaCode) return BadRequest();
        _db.Entry(area).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [RequireDeletePassword]
    [HttpDelete("{areaCode}")]
    public async Task<IActionResult> Delete(string areaCode)
    {
        var item = await _db.Areas.FindAsync(areaCode);
        if (item is null) return NotFound();
        _db.Areas.Remove(item);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
