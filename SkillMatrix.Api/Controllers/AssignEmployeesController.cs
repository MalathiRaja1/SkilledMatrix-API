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
public class AssignEmployeesController : ControllerBase
{
    private readonly AppDbContext _db;
    public AssignEmployeesController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _db.AssignEmployees
            .Include(a => a.Employee)
            .Include(a => a.Area)
            .Include(a => a.WorkStation)
            .ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOne(int id)
    {
        var item = await _db.AssignEmployees.FindAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create(AssignEmployee a)
    {
        ClearUnusedStages(a);
        var error = ValidateStages(a);
        if (error is not null) return BadRequest(new { message = error });

        _db.AssignEmployees.Add(a);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetOne), new { id = a.Id }, a);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, AssignEmployee a)
    {
        if (id != a.Id) return BadRequest();

        ClearUnusedStages(a);
        var error = ValidateStages(a);
        if (error is not null) return BadRequest(new { message = error });

        _db.Entry(a).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [RequireDeletePassword]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.AssignEmployees.FindAsync(id);
        if (item is null) return NotFound();
        _db.AssignEmployees.Remove(item);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // Dates of a stage that isn't ticked are dropped.
    private static void ClearUnusedStages(AssignEmployee a)
    {
        if (!a.T) { a.TStartDate = null; a.TEndDate = null; }
        if (!a.U) { a.UStartDate = null; a.UEndDate = null; }
        if (!a.A) { a.AStartDate = null; }
    }

    // Enforces T -> U -> A -> C order and the dates each stage needs.
    private static string? ValidateStages(AssignEmployee a)
    {
        if (a.U && !(a.T && a.TEndDate != null))
            return "U (Under Training) can only be set after T (Training) has an end date.";
        if (a.A && !(a.U && a.UEndDate != null))
            return "A (Authorised) can only be set after U (Under Training) has an end date.";
        if (a.C && !(a.A && a.AStartDate != null))
            return "C (Competent) can only be set after A (Authorised) has a start date.";

        if (a.T && a.TStartDate == null) return "Enter the T start date.";
        if (a.U && a.UStartDate == null) return "Enter the U start date.";
        if (a.A && a.AStartDate == null) return "Enter the A start date.";

        if (a.TStartDate != null && a.TEndDate != null && a.TEndDate < a.TStartDate)
            return "T end date can't be before its start date.";
        if (a.UStartDate != null && a.UEndDate != null && a.UEndDate < a.UStartDate)
            return "U end date can't be before its start date.";
        if (a.UStartDate != null && a.TEndDate != null && a.UStartDate < a.TEndDate)
            return "U start date can't be before the T end date.";
        if (a.AStartDate != null && a.UEndDate != null && a.AStartDate < a.UEndDate)
            return "A start date can't be before the U end date.";

        return null;
    }
}
