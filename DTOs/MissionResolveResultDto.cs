namespace QuanLyCBCS.DTOs;

public class MissionResolveResultDto
{
    /// <summary>
    /// CBCS có nhiệm vụ trong ngày hay không.
    /// </summary>
    public bool HasMission { get; set; }

    /// <summary>
    /// CBCS có bị phân công trùng không.
    /// </summary>
    public bool HasConflict { get; set; }

    /// <summary>
    /// Có mặt tại đơn vị.
    /// </summary>
    public bool IsPresent => !HasMission;

    public int? MissionId { get; set; }

    public string MissionName { get; set; } = "";

    public string MissionType { get; set; } = "";

    public string Location { get; set; } = "";

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    /// <summary>
    /// Lý do hiển thị trên báo cáo.
    /// </summary>
    public string Reason
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(MissionType))
                return MissionType;

            return MissionName;
        }
    }
}