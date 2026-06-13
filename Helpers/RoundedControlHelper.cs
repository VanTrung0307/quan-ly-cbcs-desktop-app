using System.Drawing.Drawing2D;

namespace QuanLyCBCS.Helpers
{
    public static class RoundedControlHelper
    {
        public static void SetRounded(Control control, int radius)
        {
            GraphicsPath path =
                new GraphicsPath();

            path.AddArc(
                0, 0,
                radius, radius,
                180, 90);

            path.AddArc(
                control.Width - radius,
                0,
                radius,
                radius,
                270,
                90);

            path.AddArc(
                control.Width - radius,
                control.Height - radius,
                radius,
                radius,
                0,
                90);

            path.AddArc(
                0,
                control.Height - radius,
                radius,
                radius,
                90,
                90);

            path.CloseFigure();

            control.Region =
                new Region(path);
        }
    }
}