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
public class EmployeesController : ControllerBase
{
    private readonly AppDbContext _db;
    public EmployeesController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _db.Employees.Include(e => e.Department).ToListAsync());

    [HttpGet("{empId}")]
    public async Task<IActionResult> GetOne(string empId)
    {
        var item = await _db.Employees.FindAsync(empId);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Employee emp)
    {
        _db.Employees.Add(emp);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetOne), new { empId = emp.EmpId }, emp);
    }

    [HttpPut("{empId}")]
    public async Task<IActionResult> Update(string empId, Employee emp)
    {
        if (empId != emp.EmpId) return BadRequest();
        _db.Entry(emp).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [RequireDeletePassword]
    [HttpDelete("{empId}")]
    public async Task<IActionResult> Delete(string empId)
    {
        var item = await _db.Employees.FindAsync(empId);
        if (item is null) return NotFound();
        _db.Employees.Remove(item);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
