using System.Globalization;
using System.Text;

namespace QuanLyCBCS.Helpers;

public static class VietnameseHelper
{
    public static string RemoveVietnameseSigns(
        string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return "";

        string normalized =
            text.Normalize(
                NormalizationForm.FormD);

        StringBuilder sb =
            new StringBuilder();

        foreach (char c in normalized)
        {
            UnicodeCategory uc =
                CharUnicodeInfo
                .GetUnicodeCategory(c);

            if (uc != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(c);
            }
        }

        return sb.ToString()
            .Normalize(NormalizationForm.FormC)
            .Replace('đ', 'd')
            .Replace('Đ', 'D');
    }
}