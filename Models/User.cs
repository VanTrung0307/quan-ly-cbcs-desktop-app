namespace QuanLyCBCS.Models;

public class User
{
    public int UserId
    {
        get;
        set;
    }

    public string Username
    {
        get;
        set;
    } = string.Empty;

    public string Password
    {
        get;
        set;
    } = string.Empty;

    public string Role
    {
        get;
        set;
    } = string.Empty;

    public int FailedLoginCount { get; set; }

    public DateTime? LockedUntil { get; set; }

    public bool MustChangePassword { get; set; } = true;
    public bool PermissionsCustomized { get; set; }
}