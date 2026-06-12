namespace QuanLyCBCS.Helpers;

public static class IdleTimeoutHelper
{
    public static DateTime LastActivityTime =
        DateTime.Now;

    public static void ResetTimer()
    {
        LastActivityTime =
            DateTime.Now;
    }
}