namespace SkillMatrix.Api.Models;

// Uploaded employee / contractor / Eaton photos, kept in the database so they
// survive redeploys (hosts like Render don't keep files written to disk).
public class Photo
{
    public Guid Id { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public byte[] Data { get; set; } = Array.Empty<byte>();
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
}
