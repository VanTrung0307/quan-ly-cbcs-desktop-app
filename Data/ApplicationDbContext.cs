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
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                @"Server=.\VANTRUNG;
                  Database=QuanLyCBCS_New;
                  Trusted_Connection=True;
                  TrustServerCertificate=True;
                  MultipleActiveResultSets=True;
                  Connect Timeout=30",
                sqlOptions =>
                {
                    // Cho phép truy vấn chạy tối đa 60 giây.
                    sqlOptions.CommandTimeout(60);
                });
        }
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

        modelBuilder.Entity<UserPermission>()
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserPermission>()
            .HasOne(x => x.Permission)
            .WithMany()
            .HasForeignKey(x => x.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserPermission>()
            .HasIndex(x => new { x.UserId, x.PermissionId })
            .IsUnique();
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
    public DbSet<UserPermission> UserPermissions { get; set; }
}