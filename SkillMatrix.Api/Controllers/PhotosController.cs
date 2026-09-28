using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillMatrix.Api.Data;

namespace SkillMatrix.Api.Controllers;

// Serves stored photos. Anonymous on purpose: an <img src> tag can't send the login
// token. The ids are random GUIDs, so links can't be guessed.
[ApiController]
[Route("api/photos")]
public class PhotosController : ControllerBase
{
    private readonly AppDbContext _db;
    public PhotosController(AppDbContext db) => _db = db;

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var photo = await _db.Photos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        if (photo is null) return NotFound();

        Response.Headers.CacheControl = "public,max-age=31536000,immutable";
        return File(photo.Data, photo.ContentType);
    }
}
