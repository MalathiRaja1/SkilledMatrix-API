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
public class WorkStationsController : ControllerBase
{
    private readonly AppDbContext _db;
    public WorkStationsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _db.WorkStations.Include(w => w.Area).ToListAsync());

    [HttpGet("{code}")]
    public async Task<IActionResult> GetOne(string code)
    {
        var item = await _db.WorkStations.FindAsync(code);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create(WorkStation ws)
    {
        _db.WorkStations.Add(ws);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetOne), new { code = ws.WorkStationCode }, ws);
    }

    [HttpPut("{code}")]
    public async Task<IActionResult> Update(string code, WorkStation ws)
    {
        if (code != ws.WorkStationCode) return BadRequest();
        _db.Entry(ws).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [RequireDeletePassword]
    [HttpDelete("{code}")]
    public async Task<IActionResult> Delete(string code)
    {
        var item = await _db.WorkStations.FindAsync(code);
        if (item is null) return NotFound();
        _db.WorkStations.Remove(item);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
