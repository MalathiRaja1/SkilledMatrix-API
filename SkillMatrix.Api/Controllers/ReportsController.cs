using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillMatrix.Api.Data;

namespace SkillMatrix.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly AppDbContext _db;
    public ReportsController(AppDbContext db) => _db = db;

    // Employee Skill Matrix Report: every employee x work-station assignment
    // with the T / U / A / C stage status and dates.
    [HttpGet("employee-skill-matrix")]
    public async Task<IActionResult> EmployeeSkillMatrix()
    {
        var rows = await _db.AssignEmployees
            .OrderBy(a => a.EmpId)
            .Select(a => new
            {
                a.EmpId,
                EmpName = a.Employee!.EmpName,
                a.AreaCode,
                AreaName = a.Area!.AreaName,
                a.WorkStationCode,
                WorkStationName = a.WorkStation!.WorkStationName,
                a.T, a.TStartDate, a.TEndDate,
                a.U, a.UStartDate, a.UEndDate,
                a.A, a.AStartDate,
                a.C
            })
            .ToListAsync();

        return Ok(rows);
    }
}
