using System.ComponentModel;
using System.Drawing;

namespace Wer.Winforms.Toolkit.Controls
{
    [ToolboxItem(true)]
    [Description("Warning button — uses WerTheme.WarningColor.")]
    public class WerButtonWarning : WerButton
    {
        public WerButtonWarning()
        {
            ButtonColor = WerTheme.WarningColor;
            HoverColor = DarkenColor(WerTheme.WarningColor, 0.15);
            PressedColor = DarkenColor(WerTheme.WarningColor, 0.30);
            ForeColor = Color.White;
        }

        private static Color DarkenColor(Color color, double factor)
        {
            return Color.FromArgb(
                color.A,
                (int)(color.R * (1 - factor)),
                (int)(color.G * (1 - factor)),
                (int)(color.B * (1 - factor)));
        }
    }
}
