using Microsoft.EntityFrameworkCore;
using QuanLyCBCS.Data;
using QuanLyCBCS.Helpers;
using QuanLyCBCS.Models;

namespace QuanLyCBCS.Services;

public class UserService
{
    public async Task<User?>
        LoginAsync(
            string username,
            string password)
    {
        using ApplicationDbContext db =
            new ApplicationDbContext();

        return await db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x =>
                    x.Username == username &&
                    x.Password == password);
    }

    public async Task<List<User>>
    GetAllAsync()
    {
        using ApplicationDbContext db =
            new ApplicationDbContext();

        return await db.Users
            .AsNoTracking()
            .OrderBy(x => x.Username)
            .ToListAsync();
    }

    public async Task<bool>
    IsUsernameExistsAsync(
        string username)
    {
        using ApplicationDbContext db =
            new ApplicationDbContext();

        return await db.Users
            .AnyAsync(x =>
                x.Username == username);
    }

    public async Task AddAsync(
    User user)
    {
        using ApplicationDbContext db =
            new ApplicationDbContext();

        await db.Users.AddAsync(user);

        await db.SaveChangesAsync();
    }

    public async Task<bool>
        ExistsUsernameAsync(
            string username)
    {
        using ApplicationDbContext db =
            new ApplicationDbContext();

        return await db.Users
            .AnyAsync(x =>
                x.Username == username);
    }

    public async Task<User?>
        GetByIdAsync(
            int id)
    {
        using ApplicationDbContext db =
            new ApplicationDbContext();

        return await db.Users
            .FirstOrDefaultAsync(
                x => x.UserId == id);
    }

    public async Task<int>
        GetAdminCountAsync()
    {
        using ApplicationDbContext db =
            new ApplicationDbContext();

        return await db.Users
            .CountAsync(
                x => x.Role == "Admin");
    }

    public async Task DeleteAsync(
        int userId)
    {
        using ApplicationDbContext db =
            new ApplicationDbContext();

        User? user =
            await db.Users
                .FirstOrDefaultAsync(
                    x => x.UserId == userId);

        if (user == null)
        {
            return;
        }

        db.Users.Remove(user);

        await db.SaveChangesAsync();
    }

    public async Task
        ResetPasswordAsync(
            int userId)
    {
        using ApplicationDbContext db =
            new ApplicationDbContext();

        User? user =
            await db.Users
                .FirstOrDefaultAsync(
                    x => x.UserId == userId);

        if (user == null)
        {
            return;
        }

        user.Password =
            PasswordHelper.HashPassword(
                "123456");

        user.MustChangePassword = true;

        await db.SaveChangesAsync();

        AuditLogService auditService =
            new AuditLogService();

        await auditService.AddLogAsync(
            "FORCE_CHANGE_PASSWORD",
            $"Tài khoản {user.Username} buộc đổi mật khẩu");
    }

    public async Task ChangePasswordAsync(
        int userId,
        string oldPassword,
        string newPassword)
    {
        using ApplicationDbContext db =
            new ApplicationDbContext();

        User? user =
            await db.Users
                .FirstOrDefaultAsync(
                    x => x.UserId == userId);

        if (user == null)
            throw new Exception(
                "Không tìm thấy tài khoản.");

        bool isValid =
            PasswordHelper.VerifyPassword(
                oldPassword,
                user.Password);

        if (!isValid)
            throw new Exception(
                "Mật khẩu cũ không đúng.");

        user.Password =
            PasswordHelper.HashPassword(
                newPassword);

        user.MustChangePassword = false;

        await db.SaveChangesAsync();
    }

    public async Task UnlockUserAsync(
        int userId)
    {
        using ApplicationDbContext db =
            new ApplicationDbContext();

        User? user =
            await db.Users
                .FirstOrDefaultAsync(
                    x => x.UserId == userId);

        if (user == null)
            return;

        user.FailedLoginCount = 0;

        user.LockedUntil = null;

        await db.SaveChangesAsync();
    }
}