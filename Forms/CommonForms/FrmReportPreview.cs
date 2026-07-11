using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCBCS.Forms.CommonForms
{
    public partial class FrmReportPreview : Form
    {
        public FrmReportPreview(string reportText)
        {
            InitializeComponent();

            rtbReport.Text = reportText;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(rtbReport.Text);

            MessageBox.Show(
                "Đã copy báo cáo.",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}
