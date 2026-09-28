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
public class EatonsController : ControllerBase
{
    private readonly AppDbContext _db;
    public EatonsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _db.Eatons.ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOne(string id)
    {
        var item = await _db.Eatons.FindAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Eaton e)
    {
        _db.Eatons.Add(e);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetOne), new { id = e.EatonId }, e);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Eaton e)
    {
        if (id != e.EatonId) return BadRequest();
        _db.Entry(e).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [RequireDeletePassword]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var item = await _db.Eatons.FindAsync(id);
        if (item is null) return NotFound();
        _db.Eatons.Remove(item);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
