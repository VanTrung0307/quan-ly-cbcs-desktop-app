using QuanLyCBCS.DTOs;
using System.Text;

namespace QuanLyCBCS.Services
{
    public class PersonnelReportBuilderService
    {
        public string Build(
            DateTime reportDate,
            ReportHeaderDto header,
            List<MissionGroupDto> groups)
        {
            var sb = new StringBuilder();

            sb.AppendLine("BÁO CÁO QUÂN SỐ");
            sb.AppendLine();

            sb.AppendLine($"{reportDate:HH:mm} ngày {reportDate:dd/MM/yyyy}");
            sb.AppendLine();

            sb.AppendLine(new string('=', 55));
            sb.AppendLine();

            sb.AppendLine($"Trực chỉ huy : {header.DutyCommander}");
            sb.AppendLine($"Trực ban     : {header.DutyOfficer}");

            sb.AppendLine();

            sb.AppendLine($"Tổng quân số : {header.TotalPersonnel}");
            sb.AppendLine($"Có mặt       : {header.PresentPersonnel}");
            sb.AppendLine($"Vắng mặt     : {header.AbsentPersonnel}");

            sb.AppendLine();
            sb.AppendLine(new string('=', 55));
            sb.AppendLine();

            if (header.AbsentPersonnel == 0)
            {
                sb.AppendLine("I. DANH SÁCH VẮNG MẶT");
                sb.AppendLine();
                sb.AppendLine("Không có CBCS vắng mặt.");
            }
            else
            {
                sb.AppendLine("I. DANH SÁCH VẮNG MẶT");
                sb.AppendLine();

                int groupIndex = 1;

                foreach (var group in groups)
                {
                    sb.AppendLine(new string('=', 55));
                    sb.AppendLine();

                    sb.AppendLine($"[{groupIndex:00}]");
                    sb.AppendLine();

                    sb.AppendLine($"Loại công tác : {group.MissionType}");
                    sb.AppendLine();

                    sb.AppendLine($"Nhiệm vụ      : {group.MissionName}");
                    sb.AppendLine($"Địa điểm      : {group.Location}");
                    sb.AppendLine($"Quân số       : {group.Personnel.Count:00}");
                    sb.AppendLine();

                    sb.AppendLine("CBCS tham gia");
                    sb.AppendLine();

                    int stt = 1;

                    foreach (var p in group.Personnel)
                    {
                        sb.AppendLine($"    {stt:00}. {p.PersonnelName}");
                        stt++;
                    }

                    sb.AppendLine();

                    groupIndex++;
                }
            }

            sb.AppendLine(new string('=', 55));
            sb.AppendLine();

            sb.AppendLine("KẾT LUẬN");
            sb.AppendLine();

            sb.AppendLine($"Có mặt  : {header.PresentPersonnel:00}");
            sb.AppendLine($"Vắng mặt: {header.AbsentPersonnel:00}");

            sb.AppendLine();
            sb.AppendLine("HẾT.");

            return sb.ToString();

        }

    }
}