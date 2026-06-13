using System.Drawing;

namespace QuanLyCBCS.Helpers
{
    public static class DashboardHelper
    {
        public static void ApplyPanelColor(
            Panel panel,
            Color color)
        {
            panel.BackColor = color;
            panel.BorderStyle = BorderStyle.None;
        }

    }
}