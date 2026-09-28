namespace SkillMatrix.Api.Models;

public class WorkStation
{
    public string WorkStationCode { get; set; } = string.Empty; // PK
    public string WorkStationName { get; set; } = string.Empty;
    public string AreaCode { get; set; } = string.Empty; // FK -> Area
    public Area? Area { get; set; }
}
