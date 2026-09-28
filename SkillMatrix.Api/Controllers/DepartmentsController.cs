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
public class DepartmentsController : ControllerBase
{
    private readonly AppDbContext _db;
    public DepartmentsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _db.Departments.ToListAsync());

    [HttpGet("{deptCode}")]
    public async Task<IActionResult> GetOne(string deptCode)
    {
        var item = await _db.Departments.FindAsync(deptCode);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Department dept)
    {
        _db.Departments.Add(dept);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetOne), new { deptCode = dept.DeptCode }, dept);
    }

    [HttpPut("{deptCode}")]
    public async Task<IActionResult> Update(string deptCode, Department dept)
    {
        if (deptCode != dept.DeptCode) return BadRequest();
        _db.Entry(dept).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [RequireDeletePassword]
    [HttpDelete("{deptCode}")]
    public async Task<IActionResult> Delete(string deptCode)
    {
        var item = await _db.Departments.FindAsync(deptCode);
        if (item is null) return NotFound();
        _db.Departments.Remove(item);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
