using Microsoft.EntityFrameworkCore;
using SkillMatrix.Api.Models;

namespace SkillMatrix.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Area> Areas => Set<Area>();
    public DbSet<WorkStation> WorkStations => Set<WorkStation>();
    public DbSet<Contractor> Contractors => Set<Contractor>();
    public DbSet<Eaton> Eatons => Set<Eaton>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<AssignEmployee> AssignEmployees => Set<AssignEmployee>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Photo> Photos => Set<Photo>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Department>().HasKey(x => x.DeptCode);
        b.Entity<Area>().HasKey(x => x.AreaCode);
        b.Entity<WorkStation>().HasKey(x => x.WorkStationCode);
        b.Entity<Contractor>().HasKey(x => x.ContractorId);
        b.Entity<Eaton>().HasKey(x => x.EatonId);
        b.Entity<Employee>().HasKey(x => x.EmpId);

        b.Entity<WorkStation>()
            .HasOne(w => w.Area)
            .WithMany()
            .HasForeignKey(w => w.AreaCode)
            .OnDelete(DeleteBehavior.Restrict);

        b.Entity<Employee>()
            .HasOne(e => e.Department)
            .WithMany()
            .HasForeignKey(e => e.DeptCode)
            .OnDelete(DeleteBehavior.SetNull);

        b.Entity<AssignEmployee>()
            .HasOne(a => a.Employee)
            .WithMany()
            .HasForeignKey(a => a.EmpId)
            .OnDelete(DeleteBehavior.Cascade);

        b.Entity<AssignEmployee>()
            .HasOne(a => a.Area)
            .WithMany()
            .HasForeignKey(a => a.AreaCode)
            .OnDelete(DeleteBehavior.Restrict);

        b.Entity<AssignEmployee>()
            .HasOne(a => a.WorkStation)
            .WithMany()
            .HasForeignKey(a => a.WorkStationCode)
            .OnDelete(DeleteBehavior.Restrict);

        b.Entity<User>()
            .HasOne(u => u.Department)
            .WithMany()
            .HasForeignKey(u => u.DeptCode)
            .OnDelete(DeleteBehavior.Restrict);

        b.Entity<User>().HasIndex(u => u.UserName).IsUnique();
    }
}
