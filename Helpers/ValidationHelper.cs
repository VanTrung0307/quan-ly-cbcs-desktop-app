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

    public static bool ValidateRequired(
        Control control,
        ErrorProvider ep,
        string message)
    {
        if (string.IsNullOrWhiteSpace(control.Text))
        {
            ep.SetError(control, message);
            control.Focus();
            return false;
        }

        ep.SetError(control, "");
        return true;
    }

    public static bool ValidateCCCD(
        TextBox txt,
        ErrorProvider ep)
    {
        string cccd =
            txt.Text.Trim();

        if (cccd.Length != 12)
        {
            ep.SetError(
                txt,
                "CCCD phải gồm 12 số");

            txt.Focus();
            return false;
        }

        if (!cccd.All(char.IsDigit))
        {
            ep.SetError(
                txt,
                "CCCD chỉ được chứa số");

            txt.Focus();
            return false;
        }

        ep.SetError(txt, "");
        return true;
    }

    public static bool ValidatePhone(
        TextBox txt,
        ErrorProvider ep)
    {
        string phone =
            txt.Text.Trim();

        if (phone.Length < 10 ||
            phone.Length > 11)
        {
            ep.SetError(
                txt,
                "Số điện thoại không hợp lệ");

            txt.Focus();
            return false;
        }

        if (!phone.All(char.IsDigit))
        {
            ep.SetError(
                txt,
                "Số điện thoại chỉ được chứa số");

            txt.Focus();
            return false;
        }

        ep.SetError(txt, "");
        return true;
    }

    public static bool ValidateDecimal(
        TextBox txt,
        ErrorProvider ep,
        string fieldName)
    {
        if (!decimal.TryParse(
            txt.Text,
            out _))
        {
            ep.SetError(
                txt,
                $"{fieldName} không hợp lệ");

            txt.Focus();
            return false;
        }

        ep.SetError(txt, "");
        return true;
    }
}