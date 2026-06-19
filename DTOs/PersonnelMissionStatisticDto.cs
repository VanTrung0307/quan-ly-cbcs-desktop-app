namespace QuanLyCBCS.DTOs;

public class PersonnelMissionStatisticDto
{
    public int PersonnelId { get; set; }

    public string SoHieu { get; set; } = "";

    public string HoTen { get; set; } = "";

    public string DonVi { get; set; } = "";

    public string? CapBacHam { get; set; }

    public int SoLanCongTac { get; set; }
}