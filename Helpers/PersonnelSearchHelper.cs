using QuanLyCBCS.Models;

namespace QuanLyCBCS.Helpers;

public static class PersonnelSearchHelper
{
    public static bool IsMatch(
        Personnel p,
        string keyword)
    {
        string key =
            VietnameseHelper
                .RemoveVietnameseSigns(
                    keyword)
                .ToLower();

        return Contains(p.HoTen, key)
            || Contains(p.SoHieu, key)
            || Contains(p.DonVi, key)
            || Contains(p.ChucVu, key)
            || Contains(p.CapBacHam, key)
            || Contains(p.ViTriCongTac, key)
            || Contains(p.NoiSinh, key)
            || Contains(p.QueQuanMoi, key)
            || Contains(p.QueQuanCu, key)
            || Contains(p.HKTTMoi, key)
            || Contains(p.HKTTCu, key)
            || Contains(p.TinhMoi, key)
            || Contains(p.TinhCu, key)
            || Contains(p.TrinhDoChuyenMon, key)
            || Contains(p.ChuyenNganhDaoTao, key)
            || ContainsRaw(p.CCCD, keyword)
            || ContainsRaw(p.SoDienThoai, keyword);
    }

    private static bool Contains(
        string? value,
        string keyword)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        return VietnameseHelper
            .RemoveVietnameseSigns(value)
            .ToLower()
            .Contains(keyword);
    }

    private static bool ContainsRaw(
        string? value,
        string keyword)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        return value.Contains(keyword);
    }
}