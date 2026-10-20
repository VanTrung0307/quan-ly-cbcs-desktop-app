using Microsoft.EntityFrameworkCore;
using QuanLyCBCS.Models;

namespace QuanLyCBCS.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Personnel> Personnel { get; set; }

    public DbSet<User> Users
    {
        get;
        set;
    }
    public DbSet<AuditLog> AuditLogs
    {
        get;
        set;
    }


    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            @"Server=.\VANTRUNG;
              Database=QuanLyCBCS_New   ;
              Trusted_Connection=True;
              TrustServerCertificate=True;
              MultipleActiveResultSets=True");
    }

    protected override void OnModelCreating(
    ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Personnel>()
            .Property(x => x.HeSoLuong)
            .HasPrecision(5, 2);

        modelBuilder.Entity<MissionPersonnel>()
            .HasOne(x => x.Mission)
            .WithMany(x => x.MissionPersonnels)
            .HasForeignKey(x => x.MissionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MissionPersonnel>()
            .HasOne(x => x.Personnel)
            .WithMany(x => x.MissionPersonnels)
            .HasForeignKey(x => x.PersonnelId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public DbSet<Mission> Missions
        => Set<Mission>();

    public DbSet<MissionPersonnel>
        MissionPersonnel
            => Set<MissionPersonnel>();

    public DbSet<PersonnelHistory>
        PersonnelHistories
    {
        get;
        set;
    }

    public DbSet<Permission> Permissions { get; set; }

    public DbSet<RolePermission> RolePermissions { get; set; }
}