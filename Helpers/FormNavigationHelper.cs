namespace QuanLyCBCS.Helpers;

public static class FormNavigationHelper
{
    public static void SetupEnterNavigation(
        Control parent)
    {
        foreach (Control control
            in parent.Controls)
        {
            if (control.HasChildren)
            {
                SetupEnterNavigation(
                    control);
            }

            if (control is TextBox
                || control is ComboBox
                || control is DateTimePicker
                || control is CheckBox)
            {
                control.KeyDown +=
                    Control_KeyDown;
            }
        }
    }

    private static void Control_KeyDown(
        object? sender,
        KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter)
            return;

        e.SuppressKeyPress = true;

        Control control =
            (Control)sender!;

        control.Parent
            ?.SelectNextControl(
                control,
                true,
                true,
                true,
                true);
    }
}