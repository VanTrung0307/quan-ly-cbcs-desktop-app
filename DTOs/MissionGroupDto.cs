namespace QuanLyCBCS.DTOs;

public class MissionGroupDto
{
    public string MissionType { get; set; } = "";

    public string MissionName { get; set; } = "";

    public string Location { get; set; } = "";

    public int PersonnelCount => Personnel.Count;

    public List<MissionAssignmentDto> Personnel { get; set; } = new();
}