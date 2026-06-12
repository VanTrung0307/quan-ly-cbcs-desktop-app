using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCBCS.DTOs
{
    public class DashboardDto
    {
        public int TongCBCS { get; set; }

        public int ChienSi { get; set; }

        public int CanBo { get; set; }

        public int DangVien { get; set; }

        public int Nam { get; set; }

        public int Nu { get; set; }

        public double TuoiTrungBinh { get; set; }

        public string DonViDongNhat
        {
            get;
            set;
        } = "";
    }
}
