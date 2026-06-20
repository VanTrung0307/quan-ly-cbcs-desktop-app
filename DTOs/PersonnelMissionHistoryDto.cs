namespace QuanLyCBCS.DTOs;

public class PersonnelMissionHistoryDto
{
    public DateTime NgayPhanCong { get; set; }

    public string TenCongTac { get; set; } = "";

    public string LoaiCongTac { get; set; } = "";

    public string GhiChu { get; set; } = "";

    public string DiaDiem { get; set; } = "";

    public DateTime BatDau { get; set; }

    public DateTime KetThuc { get; set; }
}