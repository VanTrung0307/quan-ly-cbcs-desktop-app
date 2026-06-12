using Microsoft.EntityFrameworkCore;
using QuanLyCBCS.Data;
using QuanLyCBCS.Helpers;
using QuanLyCBCS.Models;

namespace QuanLyCBCS.Services;

public class PersonnelService
{

    // =========================
    // GET ALL
    // =========================
    public async Task<List<Personnel>> GetAllAsync()
    {
        using ApplicationDbContext db =
            new ApplicationDbContext();

        return await db.Personnel
            .AsNoTracking()
            .OrderBy(x => x.HoTen)
            .ToListAsync();
    }

    public async Task<List<Personnel>>
        SearchAsync(string keyword)
    {
        using ApplicationDbContext db =
            new ApplicationDbContext();

        if (string.IsNullOrWhiteSpace(keyword))
        {
            return await GetAllAsync();
        }

        string keywordLower =
            keyword.Trim().ToLower();

        string keywordKhongDau =
            VietnameseHelper
            .RemoveVietnameseSigns(keywordLower);

        return await db.Personnel
            .AsNoTracking()
            .Where(x =>

                // Họ tên có dấu

                (x.HoTen != null &&
                 x.HoTen.ToLower()
                 .Contains(keywordLower))

                ||

                // Họ tên không dấu

                (x.HoTenKhongDau != null &&
                 x.HoTenKhongDau.ToLower()
                 .Contains(keywordKhongDau))

                ||

                // Số hiệu

                (x.SoHieu != null &&
                 x.SoHieu.ToLower()
                 .Contains(keywordLower))

                ||

                // CCCD

                (x.CCCD != null &&
                 x.CCCD.Contains(keyword))

                ||

                // SĐT

                (x.SoDienThoai != null &&
                 x.SoDienThoai.Contains(keyword))
            )
            .OrderBy(x => x.HoTen)
            .ToListAsync();
    }

    // =========================
    // GET BY ID
    // =========================

    public async Task<Personnel?>
        GetByIdAsync(int id)
    {
        using ApplicationDbContext db =
            new ApplicationDbContext();

        return await db.Personnel
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.PersonnelID == id);
    }

    // =========================
    // CREATE
    // =========================

    public async Task AddAsync(
        Personnel personnel)
    {
        personnel.HoTenKhongDau =
            VietnameseHelper
            .RemoveVietnameseSigns(
                personnel.HoTen);

        personnel.CreatedAt =
            DateTime.Now;
        using ApplicationDbContext db =
            new ApplicationDbContext();

        await db.Personnel
            .AddAsync(personnel);

        await db.SaveChangesAsync();
    }

    // =========================
    // UPDATE
    // =========================

    public async Task UpdateAsync(
        Personnel personnel)
    {
        personnel.HoTenKhongDau =
            VietnameseHelper
            .RemoveVietnameseSigns(
                personnel.HoTen);

        personnel.UpdatedAt =
            DateTime.Now;

        using ApplicationDbContext db =
            new ApplicationDbContext();

        db.Personnel.Update(personnel);

        await db.SaveChangesAsync();
    }

    // =========================
    // DELETE
    // =========================

    public async Task DeleteAsync(int id)
    {
        using ApplicationDbContext db =
            new ApplicationDbContext();

        var personnel =
            await db.Personnel
                .FirstOrDefaultAsync(
                    x => x.PersonnelID == id);

        if (personnel == null)
            return;

        db.Personnel.Remove(personnel);

        await db.SaveChangesAsync();
    }

    // =========================
    // VALIDATION
    // =========================

    public async Task<bool>
        IsSoHieuExistsAsync(
            string soHieu)
    {
        using ApplicationDbContext db =
            new ApplicationDbContext();

        return await db.Personnel
            .AnyAsync(x =>
                x.SoHieu == soHieu);
    }

    public async Task<bool>
        IsCCCDExistsAsync(
            string cccd)
    {
        using ApplicationDbContext db =
            new ApplicationDbContext();

        return await db.Personnel
            .AnyAsync(x =>
                x.CCCD == cccd);
    }

    // =========================
    // DASHBOARD
    // =========================

    public async Task<int> GetTotalCountAsync()
    {
        using ApplicationDbContext db =
            new ApplicationDbContext();
        return await db.Personnel.CountAsync();
    }

    public async Task<int> GetChienSiCountAsync()
    {
        using ApplicationDbContext db =
            new ApplicationDbContext();

        return await db.Personnel
            .CountAsync(x =>
                x.PersonnelType == 1);
    }

    public async Task<int> GetCanBoCountAsync()
    {
        using ApplicationDbContext db =
            new ApplicationDbContext();

        return await db.Personnel
            .CountAsync(x =>
                x.PersonnelType == 2);
    }

    public async Task<int> GetDangVienCountAsync()
    {
        using ApplicationDbContext db =
            new ApplicationDbContext();

        return await db.Personnel
            .CountAsync(x =>
                x.DangVien);
    }
}