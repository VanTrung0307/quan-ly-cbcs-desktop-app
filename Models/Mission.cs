namespace QuanLyCBCS.Models;

public class Mission
{
    public int MissionId { get; set; }

    public string MissionName { get; set; } = "";

    public string? MissionType { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public string? Location { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<MissionPersonnel>
        MissionPersonnels
    {
        get;
        set;
    }
    =
    new List<MissionPersonnel>();
}