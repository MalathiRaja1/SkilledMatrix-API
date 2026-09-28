namespace SkillMatrix.Api.Models;

public class User
{
    public int Id { get; set; } // PK, auto-increment
    public string DeptCode { get; set; } = string.Empty; // FK -> Department
    public Department? Department { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string PasswordSalt { get; set; } = string.Empty;
    // Reversible (AES) copy so an admin can view the password in User Creation. Login uses the hash.
    public string? PasswordEncrypted { get; set; }
}
