namespace SkillMatrix.Api.Models;

public class Employee
{
    public string EmpId { get; set; } = string.Empty; // PK
    public string EmpName { get; set; } = string.Empty;
    public string EmpCategory { get; set; } = string.Empty;
    public string? Photo { get; set; }
    public string PhoneNo { get; set; } = string.Empty;
    public string? DeptCode { get; set; } // FK -> Department (optional link)
    public Department? Department { get; set; }
}
