using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCBCS.Models;

public class Personnel
{
    public int PersonnelID { get; set; }

    public int PersonnelType { get; set; }

    public string SoHieu { get; set; } = "";

    public string HoTen { get; set; } = "";

    public string HoTenKhongDau { get; set; } = "";

    public string DonVi { get; set; } = "";

    public string? SoDienThoai { get; set; }

    public string? CCCD { get; set; }

    public DateTime? NgaySinh { get; set; }
}
