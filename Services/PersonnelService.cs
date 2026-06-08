using Microsoft.EntityFrameworkCore;
using QuanLyCBCS.Data;
using QuanLyCBCS.Helpers;
using QuanLyCBCS.Models;

namespace QuanLyCBCS.Services;

public class PersonnelService
{
    private readonly ApplicationDbContext _db;

    public PersonnelService()
    {
        _db = new ApplicationDbContext();
    }

    // =========================
    // GET ALL
    // =========================

    public async Task<List<Personnel>>
        GetAllAsync()
    {
        return await _db.Personnel
            .OrderBy(x => x.HoTen)
            .ToListAsync();
    }

    // =========================
    // GET BY ID
    // =========================

    public async Task<Personnel?>
        GetByIdAsync(int id)
    {
        return await _db.Personnel
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

        await _db.Personnel
            .AddAsync(personnel);

        await _db.SaveChangesAsync();
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

        _db.Personnel.Update(personnel);

        await _db.SaveChangesAsync();
    }

    // =========================
    // DELETE
    // =========================

    public async Task DeleteAsync(
        int id)
    {
        var personnel =
            await GetByIdAsync(id);

        if (personnel == null)
            return;

        _db.Personnel.Remove(personnel);

        await _db.SaveChangesAsync();
    }

    // =========================
    // SEARCH
    // =========================

    public async Task<List<Personnel>>
        SearchAsync(string keyword)
    {
        keyword =
            VietnameseHelper
            .RemoveVietnameseSigns(
                keyword);

        return await _db.Personnel
            .Where(x =>
                x.HoTenKhongDau
                    .Contains(keyword)
                ||
                x.SoHieu
                    .Contains(keyword))
            .ToListAsync();
    }

    // =========================
    // VALIDATION
    // =========================

    public async Task<bool>
        IsSoHieuExistsAsync(
            string soHieu)
    {
        return await _db.Personnel
            .AnyAsync(x =>
                x.SoHieu == soHieu);
    }

    public async Task<bool>
        IsCCCDExistsAsync(
            string cccd)
    {
        return await _db.Personnel
            .AnyAsync(x =>
                x.CCCD == cccd);
    }
}