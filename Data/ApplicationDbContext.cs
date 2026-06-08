using Microsoft.EntityFrameworkCore;
using QuanLyCBCS.Models;

namespace QuanLyCBCS.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Personnel> Personnel { get; set; }

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            @"Server=.\VANTRUNG;
              Database=QuanLyCBCS;
              Trusted_Connection=True;
              TrustServerCertificate=True");
    }
}