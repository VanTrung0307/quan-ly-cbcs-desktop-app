namespace QuanLyCBCS.Helpers
{
    public static class DataGridViewHelper
    {
        public static void ApplyModernStyle(
            DataGridView dgv)
        {
            dgv.BorderStyle =
                BorderStyle.None;

            dgv.BackgroundColor =
                Color.White;

            dgv.EnableHeadersVisualStyles =
                false;

            dgv.ColumnHeadersDefaultCellStyle.BackColor =
                ThemeHelper.PrimaryColor;

            dgv.ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;

            dgv.ColumnHeadersHeight = 40;

            dgv.RowTemplate.Height = 35;

            dgv.AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(
                    248,
                    249,
                    250);
        }
    }
}