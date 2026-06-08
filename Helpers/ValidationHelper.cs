using System.Text.RegularExpressions;

namespace QuanLyCBCS.Helpers;

public static class ValidationHelper
{
    public static bool IsPhoneNumber(
        string phone)
    {
        return Regex.IsMatch(
            phone,
            @"^0[0-9]{9}$");
    }

    public static bool IsCCCD(
        string cccd)
    {
        return Regex.IsMatch(
            cccd,
            @"^[0-9]{12}$");
    }
}