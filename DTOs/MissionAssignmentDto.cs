namespace QuanLyCBCS.DTOs;

public class MissionAssignmentDto
{
    public int PersonnelId { get; set; }

    public string PersonnelName { get; set; } = "";

    public string Rank { get; set; } = "";

    public bool IsPresent { get; set; }

    public string Reason { get; set; } = "";

    public int? MissionId { get; set; }

    public string MissionType { get; set; } = "";

    public string MissionName { get; set; } = "";

    public string Location { get; set; } = "";

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }
}