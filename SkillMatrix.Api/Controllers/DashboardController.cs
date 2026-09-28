using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillMatrix.Api.Data;

namespace SkillMatrix.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly AppDbContext _db;
    public DashboardController(AppDbContext db) => _db = db;

    [HttpGet("summary")]
    public async Task<IActionResult> Summary()
    {
        return Ok(new
        {
            employees = await _db.Employees.CountAsync(),
            departments = await _db.Departments.CountAsync(),
            areas = await _db.Areas.CountAsync(),
            workStations = await _db.WorkStations.CountAsync(),
            contractors = await _db.Contractors.CountAsync(),
            eatons = await _db.Eatons.CountAsync(),
            activeAssignments = await _db.AssignEmployees.CountAsync(a => !a.C)
        });
    }
}
