namespace SkillMatrix.Api.Models;

public class Contractor
{
    public string ContractorId { get; set; } = string.Empty; // PK
    public string ContractorName { get; set; } = string.Empty;
    public string? ContractorPhoto { get; set; } // stored file path / URL
    public DateTime DateOfJoining { get; set; }
}
