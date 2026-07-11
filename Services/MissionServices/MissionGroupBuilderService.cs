using QuanLyCBCS.DTOs;

namespace QuanLyCBCS.Services.MissionServices;

public class MissionGroupBuilderService
{
    public List<MissionGroupDto> Build(
        IEnumerable<MissionAssignmentDto> source)
    {
        return source

            .Where(x => !x.IsPresent)

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