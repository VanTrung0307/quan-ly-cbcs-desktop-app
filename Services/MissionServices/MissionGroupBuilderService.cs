using QuanLyCBCS.DTOs;

namespace QuanLyCBCS.Services.MissionServices;

public class MissionGroupBuilderService
{
    public List<MissionGroupDto> Build(
        IEnumerable<MissionAssignmentDto> source)
    {
        // Nguồn đầu vào đã được PersonnelReportService lọc theo thời điểm
        // và loại nhiệm vụ bên ngoài đơn vị. Không lọc IsPresent ở đây vì
        // MissionAssignmentDto là bản ghi phân công, không phải bản ghi quân số.
        return source
            .GroupBy(x => new
            {
                x.MissionType,
                x.MissionName,
                x.Location
            })

            .Select(g => new MissionGroupDto
            {
                MissionType = g.Key.MissionType,

                MissionName = g.Key.MissionName,

                Location = g.Key.Location,

                Personnel = g
                    .OrderBy(x => x.PersonnelName)
                    .ToList()
            })

            .OrderBy(x => x.MissionType)

            .ToList();
    }
}