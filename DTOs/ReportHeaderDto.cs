namespace QuanLyCBCS.DTOs;

public class ReportHeaderDto
{
    /// <summary>
    /// Thời gian lập báo cáo
    /// </summary>
    public DateTime ReportDate { get; set; }

    /// <summary>
    /// Trực chỉ huy
    /// </summary>
    public string DutyCommander { get; set; } = "";

    /// <summary>
    /// Trực ban
    /// </summary>
    public string DutyOfficer { get; set; } = "";

    /// <summary>
    /// Tổng quân số
    /// </summary>
    public int TotalPersonnel { get; set; }

    /// <summary>
    /// Có mặt
    /// </summary>
    public int PresentPersonnel { get; set; }

    /// <summary>
    /// Vắng mặt
    /// </summary>
    public int AbsentPersonnel { get; set; }
}