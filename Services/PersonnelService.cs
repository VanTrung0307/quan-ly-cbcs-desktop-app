using Microsoft.EntityFrameworkCore;
using QuanLyCBCS.Data;
using QuanLyCBCS.DTOs;
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
        PermissionHelper.Require("Personnel.View");

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
        PermissionHelper.Require("Personnel.View");

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
        PermissionHelper.Require("Personnel.View");

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
        PermissionHelper.Require("Personnel.Create");

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
        PermissionHelper.Require("Personnel.Update");

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

    public async Task DeleteAsync(int personnelId)
    {
        PermissionHelper.Require("Personnel.Delete");

        using ApplicationDbContext db =
            new ApplicationDbContext();

        Personnel? personnel =
            await db.Personnel
                .FirstOrDefaultAsync(x =>
                    x.PersonnelID == personnelId);

        if (personnel == null)
        {
            return;
        }

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
        PermissionHelper.Require("Dashboard.View");

        using ApplicationDbContext db =
            new ApplicationDbContext();
        return await db.Personnel.CountAsync();
    }

    public async Task<int> GetChienSiCountAsync()
    {
        PermissionHelper.Require("Dashboard.View");

        using ApplicationDbContext db =
            new ApplicationDbContext();

        return await db.Personnel
            .CountAsync(x =>
                x.PersonnelType == 1);
    }

    public async Task<int> GetCanBoCountAsync()
    {
        PermissionHelper.Require("Dashboard.View");

        using ApplicationDbContext db =
            new ApplicationDbContext();

        return await db.Personnel
            .CountAsync(x =>
                x.PersonnelType == 2);
    }

    public async Task<int> GetDangVienCountAsync()
    {
        PermissionHelper.Require("Dashboard.View");

        using ApplicationDbContext db =
            new ApplicationDbContext();

        return await db.Personnel
            .CountAsync(x =>
                x.DangVien);
    }

    public async Task<PersonnelDashboard>
        GetPersonnelDashboardAsync()
    {
        PermissionHelper.Require("Dashboard.View");

        using ApplicationDbContext db =
            new ApplicationDbContext();

        var data =
            await db.Personnel.ToListAsync();

        PersonnelDashboard dto =
            new PersonnelDashboard();

        dto.TongCBCS = data.Count;

        dto.Nam =
            data.Count(x => x.GioiTinh == "Nam");

        dto.Nu =
            data.Count(x => x.GioiTinh == "Nữ");

        var ages = data
            .Where(x => x.NgaySinh.HasValue)
            .Select(x => DateTime.Now.Year - x.NgaySinh!.Value.Year)
            .ToList();

        dto.TuoiTrungBinh = ages.Count > 0
            ? Math.Round(ages.Average(), 1)
            : 0;

        dto.DonViDongNhat =
            data.GroupBy(x => x.DonVi)
                .OrderByDescending(x => x.Count())
                .FirstOrDefault()
                ?.Key ?? "";

        return dto;
    }

    public async Task<bool>
        IsCCCDExistsAsync(
            string cccd,
            int? ignorePersonnelId = null)
    {
        using ApplicationDbContext db =
            new ApplicationDbContext();

        return await db.Personnel.AnyAsync(x =>
            x.CCCD == cccd
            &&
            (!ignorePersonnelId.HasValue
             ||
             x.PersonnelID != ignorePersonnelId.Value));
    }

    public async Task<bool>
        IsPhoneExistsAsync(
            string phone,
            int? ignorePersonnelId = null)
    {
        using ApplicationDbContext db =
            new ApplicationDbContext();

        return await db.Personnel.AnyAsync(x =>
            x.SoDienThoai == phone
            &&
            (!ignorePersonnelId.HasValue
             ||
             x.PersonnelID != ignorePersonnelId.Value));
    }
}