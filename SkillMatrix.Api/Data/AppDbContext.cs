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

    // Business-code max length for every string that is a primary key, a foreign key,
    // or otherwise indexed. SQL Server (unlike Postgres) refuses to index an
    // unbounded nvarchar(max) column, so every one of these needs an explicit length
    // or `dotnet ef database update` fails with "column ... is invalid for use as a
    // key column in an index".
    private const int CodeLength = 20;

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Department>(e =>
        {
            e.HasKey(x => x.DeptCode);
            e.Property(x => x.DeptCode).HasMaxLength(CodeLength);
        });

        b.Entity<Area>(e =>
        {
            e.HasKey(x => x.AreaCode);
            e.Property(x => x.AreaCode).HasMaxLength(CodeLength);
        });

        b.Entity<WorkStation>(e =>
        {
            e.HasKey(x => x.WorkStationCode);
            e.Property(x => x.WorkStationCode).HasMaxLength(CodeLength);
            e.Property(x => x.AreaCode).HasMaxLength(CodeLength);
        });

        b.Entity<Contractor>(e =>
        {
            e.HasKey(x => x.ContractorId);
            e.Property(x => x.ContractorId).HasMaxLength(CodeLength);
        });

        b.Entity<Eaton>(e =>
        {
            e.HasKey(x => x.EatonId);
            e.Property(x => x.EatonId).HasMaxLength(CodeLength);
        });

        b.Entity<Employee>(e =>
        {
            e.HasKey(x => x.EmpId);
            e.Property(x => x.EmpId).HasMaxLength(CodeLength);
            e.Property(x => x.DeptCode).HasMaxLength(CodeLength);
        });

        b.Entity<AssignEmployee>(e =>
        {
            e.Property(x => x.EmpId).HasMaxLength(CodeLength);
            e.Property(x => x.AreaCode).HasMaxLength(CodeLength);
            e.Property(x => x.WorkStationCode).HasMaxLength(CodeLength);
        });

        b.Entity<User>(e =>
        {
            e.Property(x => x.DeptCode).HasMaxLength(CodeLength);
            e.Property(x => x.UserName).HasMaxLength(100);
            e.Property(x => x.Status).HasMaxLength(20);
            e.HasIndex(x => x.UserName).IsUnique();
        });

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
    }
}
