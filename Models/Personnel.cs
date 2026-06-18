using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCBCS.Models;

public class Personnel
{
    public int PersonnelID { get; set; }

    // 1 = Chiến sĩ
    // 2 = Cán bộ
    public int PersonnelType { get; set; }

    // =========================
    // THÔNG TIN CƠ BẢN
    // =========================

    public string SoHieu { get; set; } = "";

    public string HoTen { get; set; } = "";

    public string HoTenKhongDau { get; set; } = "";

    public string DonVi { get; set; } = "";

    public string GioiTinh { get; set; } = "";

    public DateTime? NgaySinh { get; set; }

    public string? NoiSinh { get; set; }

    // =========================
    // QUÊ QUÁN
    // =========================

    public string? QueQuanMoi { get; set; }

    public string? QueQuanCu { get; set; }

    public string? TinhMoi { get; set; }

    public string? TinhCu { get; set; }

    public string? HKTTMoi { get; set; }

    public string? HKTTCu { get; set; }

    // =========================
    // CÔNG TÁC
    // =========================

    public string? ViTriCongTac { get; set; }

    public string? CapBacHam { get; set; }

    public DateTime? NgayNhanCapBac { get; set; }

    public DateTime? ThangNamVaoNganh { get; set; }

    public DateTime? ThangNamNhanCongTac { get; set; }

    // =========================
    // ĐÀO TẠO
    // =========================

    public string? TrinhDoTinHoc { get; set; }

    public string? TrinhDoNgoaiNgu { get; set; }

    public string? TrinhDoVanHoa { get; set; }

    public string? TrinhDoChuyenMon { get; set; }

    public string? ChuyenNganhDaoTao { get; set; }

    public DateTime? ThangNamTotNghiep { get; set; }

    public string? TrinhDoLyLuanChinhTri { get; set; }

    // =========================
    // ĐẢNG
    // =========================

    public bool DangVien { get; set; }

    public DateTime? NgayVaoDangDuBi { get; set; }

    public DateTime? NgayVaoDangChinhThuc { get; set; }

    // =========================
    // CÁN BỘ
    // =========================

    public decimal? HeSoLuong { get; set; }

    public string? ChucVu { get; set; }

    public DateTime? NgayBoNhiem { get; set; }

    // =========================
    // GIẤY TỜ
    // =========================

    public string? CCCD { get; set; }

    public DateTime? NgayCapCCCD { get; set; }

    public string? SoDienThoai { get; set; }

    // =========================
    // SYSTEM
    // =========================

    public DateTime CreatedAt { get; set; }
        = DateTime.Now;

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<MissionPersonnel>
        MissionPersonnels
    {
        get;
        set;
    }
    =
    new List<MissionPersonnel>();
}