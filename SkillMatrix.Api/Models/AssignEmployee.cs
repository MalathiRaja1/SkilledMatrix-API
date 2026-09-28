namespace SkillMatrix.Api.Models;

// One row = one employee on one work station, moving through 4 stages in order:
//   T  Training plan (primary/secondary job)      -> start + end date
//   U  Under training / perform under supervision -> start + end date
//   A  Authorised to perform task                 -> start date only
//   C  Competent to train others (final approval) -> tick only
// A stage can only be set once the previous one is finished (validated in the controller).
public class AssignEmployee
{
    public int Id { get; set; } // PK, auto-increment

    public string EmpId { get; set; } = string.Empty; // FK -> Employee
    public Employee? Employee { get; set; }

    public string AreaCode { get; set; } = string.Empty; // FK -> Area
    public Area? Area { get; set; }

    public string WorkStationCode { get; set; } = string.Empty; // FK -> WorkStation
    public WorkStation? WorkStation { get; set; }

    public bool T { get; set; }
    public DateTime? TStartDate { get; set; }
    public DateTime? TEndDate { get; set; }

    public bool U { get; set; }
    public DateTime? UStartDate { get; set; }
    public DateTime? UEndDate { get; set; }

    public bool A { get; set; }
    public DateTime? AStartDate { get; set; }

    public bool C { get; set; }
}
