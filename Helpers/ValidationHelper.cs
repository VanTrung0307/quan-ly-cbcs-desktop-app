using System.Text.RegularExpressions;

namespace QuanLyCBCS.Helpers;

public static class ValidationHelper
{
    public static bool IsRequired(
        string? value)
    {
        return !string.IsNullOrWhiteSpace(
            value);
    }

    public static bool IsPhoneNumber(
        string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return false;

        return Regex.IsMatch(
            phone,
            @"^0[0-9]{9}$");
    }

    public static bool IsCCCD(
        string? cccd)
    {
        if (string.IsNullOrWhiteSpace(cccd))
            return false;

        return Regex.IsMatch(
            cccd,
            @"^[0-9]{12}$");
    }
}