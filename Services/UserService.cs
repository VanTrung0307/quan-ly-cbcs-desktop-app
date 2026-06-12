using Microsoft.EntityFrameworkCore;
using QuanLyCBCS.Data;
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
}