namespace SkillMatrix.Api.Models;

// "Eaton Master" as defined in the source spec (client/equipment master)
public class Eaton
{
    public string EatonId { get; set; } = string.Empty; // PK
    public string EatonName { get; set; } = string.Empty;
    public string? Photo { get; set; }
    public DateTime DateOfJoining { get; set; }
}
