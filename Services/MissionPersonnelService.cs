using Microsoft.EntityFrameworkCore;
using QuanLyCBCS.Data;
using QuanLyCBCS.DTOs;
using QuanLyCBCS.Models;

namespace QuanLyCBCS.Services
{
    public class MissionPersonnelService
    {
        public async Task<List<MissionPersonnel>>
            GetByMissionAsync(
                int missionId)
        {
            using ApplicationDbContext db = new();

            return await db
                .MissionPersonnel
                .Include(x => x.Personnel)
                .Where(x =>
                    x.MissionId == missionId)
                .ToListAsync();
        }

        public async Task AddAsync(
            int missionId,
            int personnelId)
        {
            using ApplicationDbContext db = new();

            bool exists =
                await db
                    .MissionPersonnel
                    .AnyAsync(x =>
                        x.MissionId == missionId
                        &&
                        x.PersonnelId == personnelId);

            if (exists)
                return;

            MissionPersonnel item =
                new()
                {
                    MissionId = missionId,
                    PersonnelId = personnelId,
                    AssignedAt = DateTime.Now
                };

            db.MissionPersonnel.Add(item);

            await db.SaveChangesAsync();
        }

        public async Task RemoveAsync(
            int missionId,
            int personnelId)
        {
            using ApplicationDbContext db = new();

            var item =
                await db
                    .MissionPersonnel
                    .FirstOrDefaultAsync(x =>
                        x.MissionId == missionId
                        &&
                        x.PersonnelId == personnelId);

            if (item == null)
                return;

            db.MissionPersonnel.Remove(item);

            await db.SaveChangesAsync();
        }

        public async Task<List<MissionPersonnelDto>>
        GetAssignedPersonnelDtoAsync(int missionId)
        {
            using ApplicationDbContext db = new();

            return await db
                .MissionPersonnel
                .AsNoTracking()
                .Include(x => x.Personnel)
                .Where(x => x.MissionId == missionId)
                .Select(x =>
                    new MissionPersonnelDto
                    {
                        PersonnelId = x.PersonnelId,

                        SoHieu = x.Personnel!.SoHieu ?? "",

                        HoTen = x.Personnel.HoTen ?? "",

                        DonVi = x.Personnel.DonVi ?? "",

                        CapBacHam = x.Personnel.CapBacHam ?? "",

                        NgayPhanCong = x.AssignedAt,

                        SoLanCongTac =
                            db.MissionPersonnel.Count(mp =>
                                mp.PersonnelId ==
                                x.PersonnelId)
                    })
                .ToListAsync();
        }

        public async Task<List<MissionPersonnelDto>>
        GetAvailablePersonnelDtoAsync(
            int missionId)
        {
            using ApplicationDbContext db = new();

            var assignedIds =
                await db
                    .MissionPersonnel
                    .AsNoTracking()
                    .Where(x =>
                        x.MissionId == missionId)
                    .Select(x =>
                        x.PersonnelId)
                    .ToListAsync();

            return await db
                .Personnel
                .AsNoTracking()
                .Where(x =>
                    !assignedIds.Contains(
                        x.PersonnelID))
                .Select(x =>
                    new MissionPersonnelDto
                    {
                        PersonnelId = x.PersonnelID,

                        SoHieu = x.SoHieu ?? "",

                        HoTen = x.HoTen ?? "",

                        DonVi = x.DonVi ?? "",

                        CapBacHam = x.CapBacHam ?? "",

                        SoLanCongTac =
                            db.MissionPersonnel.Count(mp =>
                                mp.PersonnelId ==
                                x.PersonnelID)
                    })
                .ToListAsync();
        }

        public async Task<List<Personnel>> GetAllAsync()
        {
            using ApplicationDbContext db = new();

            return await db.Personnel
                .OrderBy(x => x.HoTen)
                .ToListAsync();
        }

        public async Task<int>
        GetMissionCountAsync(
            int personnelId)
        {
            using ApplicationDbContext db = new();

            return await db
                .MissionPersonnel
                .CountAsync(x =>
                    x.PersonnelId ==
                    personnelId);
        }

        public async Task<List<PersonnelMissionStatisticDto>>
            GetPersonnelMissionStatisticsAsync()
        {
            using ApplicationDbContext db = new();

            return await db.Personnel
                .AsNoTracking()
                .Select(x =>
                    new PersonnelMissionStatisticDto
                    {
                        PersonnelId = x.PersonnelID,

                        SoHieu = x.SoHieu,

                        HoTen = x.HoTen,

                        DonVi = x.DonVi,

                        CapBacHam = x.CapBacHam,

                        SoLanCongTac =
                            db.MissionPersonnel.Count(mp =>
                                mp.PersonnelId ==
                                x.PersonnelID)
                    })
                .OrderByDescending(x =>
                    x.SoLanCongTac)
                .ToListAsync();
        }

        public async Task<List<PersonnelMissionHistoryDto>>
            GetPersonnelMissionHistoryAsync(
                int personnelId)
        {
            using ApplicationDbContext db = new();

            return await db
                .MissionPersonnel
                .AsNoTracking()
                .Include(x => x.Mission)
                .Where(x =>
                    x.PersonnelId ==
                    personnelId)
                .OrderByDescending(x =>
                    x.AssignedAt)
                .Select(x =>
                    new PersonnelMissionHistoryDto
                    {
                        NgayPhanCong =
                            x.AssignedAt,

                        TenCongTac =
                            x.Mission!.MissionName,

                        LoaiCongTac =
                            x.Mission.MissionType ?? "",

                        GhiChu =
                            x.Mission.Description ?? "",

                        DiaDiem =
                            x.Mission.Location ?? "",

                        BatDau =
                            x.Mission.StartTime,

                        KetThuc =
                            x.Mission.EndTime
                    })
                .ToListAsync();
        }

    }
}