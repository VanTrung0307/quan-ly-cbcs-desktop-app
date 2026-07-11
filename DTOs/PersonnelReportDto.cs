namespace QuanLyCBCS.DTOs
{
    public class PersonnelReportDto
    {
        public int PersonnelId { get; set; }

        public string SoHieu { get; set; } = "";

        public string HoTen { get; set; } = "";

        public string DonVi { get; set; } = "";

        public string CapBacHam { get; set; } = "";

        /// <summary>
        /// Có mặt / Vắng mặt
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// True = Có mặt
        /// False = Vắng mặt
        /// </summary>
        public bool IsPresent =>
            string.Equals(
                Status,
                "Có mặt",
                StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Lý do vắng mặt
        /// Ví dụ:
        /// Đội 13
        /// PC08
        /// Công binh
        /// Ngân hàng
        /// ...
        /// </summary>
        public string Reason { get; set; } = string.Empty;

        /// <summary>
        /// Tên nhiệm vụ hiện tại
        /// </summary>
        public string MissionName { get; set; } = string.Empty;

        /// <summary>
        /// Địa điểm làm nhiệm vụ
        /// </summary>
        public string Location { get; set; } = string.Empty;

        public DateTime? MissionStart { get; set; }

        public DateTime? MissionEnd { get; set; }
    }
}