using Microsoft.EntityFrameworkCore;
using QuanLyCBCS.Data;
using QuanLyCBCS.DTOs;

namespace QuanLyCBCS.Services
{
    internal class NotificationService
    {
        public async Task<List<NotificationDto>> GetNotificationsAsync()
        {
            List<NotificationDto> notifications = new();

            using ApplicationDbContext db = new();

            DateTime today = DateTime.Today;

            // ==========================
            // Sinh nhật CBCS
            // ==========================

            var personnels = await db.Personnel.ToListAsync();

            foreach (var person in personnels)
            {
                if (person.NgaySinh == null)
                    continue;

                DateTime birthday =
                    new DateTime(
                        today.Year,
                        person.NgaySinh.Value.Month,
                        person.NgaySinh.Value.Day);

                int days = (birthday - today).Days;

                if (days < 0)
                    days += 365;

                if (days <= 7)
                {
                    notifications.Add(new NotificationDto
                    {
                        Type = "BIRTHDAY",
                        Title = person.HoTen,
                        Message =
                            days == 0
                            ? "Hôm nay sinh nhật"
                            : $"Còn {days} ngày đến sinh nhật",
                        Date = birthday
                    });
                }
            }

            // ==========================
            // Công tác sắp kết thúc
            // ==========================

            var endingSoon =
                await db.Missions
                    .Where(x =>
                        x.EndTime >= today &&
                        EF.Functions.DateDiffDay(today, x.EndTime) <= 3)
                    .ToListAsync();

            foreach (var mission in endingSoon)
            {
                int days =
                    (mission.EndTime.Date - today).Days;

                notifications.Add(new NotificationDto
                {
                    Type = "MISSION_ENDING",
                    Title = mission.MissionName,
                    Message = days switch
                    {
                        0 => "Kết thúc hôm nay",
                        1 => "Còn 1 ngày kết thúc",
                        _ => $"Còn {days} ngày kết thúc"
                    },
                    Date = mission.EndTime
                });
            }

            // ==========================
            // Công tác quá hạn
            // ==========================

            var overdue =
                await db.Missions
                    .Where(x =>
                        x.EndTime < today &&
                        x.IsActive)
                    .ToListAsync();

            foreach (var mission in overdue)
            {
                int days =
                    (today - mission.EndTime.Date).Days;

                notifications.Add(new NotificationDto
                {
                    Type = "MISSION_OVERDUE",
                    Title = mission.MissionName,
                    Message = $"Quá hạn {days} ngày",
                    Date = mission.EndTime
                });
            }

            return notifications
                .OrderBy(x => x.Date)
                .ToList();
        }

        public async Task<int> GetNotificationCountAsync()
        {
            var notifications =
                await GetNotificationsAsync();

            return notifications.Count;
        }
    }
}
