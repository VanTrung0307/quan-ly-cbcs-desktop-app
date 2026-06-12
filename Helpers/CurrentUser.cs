namespace QuanLyCBCS.Helpers;

public static class CurrentUser
{
    public static int UserId
    {
        get;
        set;
    }

    public static string Username
    {
        get;
        set;
    } = string.Empty;

    public static string Role
    {
        get;
        set;
    } = string.Empty;
}