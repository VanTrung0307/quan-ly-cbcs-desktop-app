using System.Drawing;
using System.Windows.Forms;

namespace QuanLyCBCS.Helpers
{
    public static class AuditStyleHelper
    {
        public static void Apply(
            string action,
            DataGridViewCellStyle? style)
        {
            if (style is null)
                return;

            switch (action)
            {
                case AuditAction.Login:

                    SetColor(
                        style,
                        Color.FromArgb(39, 174, 96));

                    break;

                case AuditAction.FailedLogin:

                    SetColor(
                        style,
                        Color.FromArgb(231, 76, 60));

                    break;

                case AuditAction.Logout:

                    SetColor(
                        style,
                        Color.FromArgb(243, 156, 18));

                    break;

                case AuditAction.ResetPassword:

                    SetColor(
                        style,
                        Color.FromArgb(52, 152, 219));

                    break;

                case AuditAction.ChangePassword:

                    SetColor(
                        style,
                        Color.FromArgb(155, 89, 182));

                    break;

                case AuditAction.AddPersonnel:

                    SetColor(
                        style,
                        Color.FromArgb(46, 204, 113));

                    break;

                case AuditAction.UpdatePersonnel:

                    SetColor(
                        style,
                        Color.FromArgb(52, 152, 219));

                    break;

                case AuditAction.DeletePersonnel:

                    SetColor(
                        style,
                        Color.FromArgb(192, 57, 43));

                    break;

                case AuditAction.AddMission:

                    SetColor(
                        style,
                        Color.FromArgb(26, 188, 156));

                    break;

                case AuditAction.AssignMission:

                    SetColor(
                        style,
                        Color.FromArgb(41, 128, 185));

                    break;

                case AuditAction.AddUser:

                    SetColor(
                        style,
                        Color.FromArgb(142, 68, 173));

                    break;

                case AuditAction.DeleteUser:

                    SetColor(
                        style,
                        Color.FromArgb(127, 140, 141));

                    break;
            }
        }

        private static void SetColor(
            DataGridViewCellStyle style,
            Color color)
        {
            style.BackColor = color;

            style.ForeColor =
                Color.White;

            style.SelectionBackColor =
                color;

            style.SelectionForeColor =
                Color.White;
        }
    }
}