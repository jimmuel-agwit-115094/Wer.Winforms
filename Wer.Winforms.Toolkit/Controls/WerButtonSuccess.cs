using System.ComponentModel;
using System.Drawing;

namespace Wer.Winforms.Toolkit.Controls
{
    [ToolboxItem(true)]
    [Description("Success button — uses WerTheme.SuccessColor (green).")]
    public class WerButtonSuccess : WerButton
    {
        public WerButtonSuccess()
        {
            ButtonColor = WerTheme.SuccessColor;
            HoverColor = DarkenColor(WerTheme.SuccessColor, 0.15);
            PressedColor = DarkenColor(WerTheme.SuccessColor, 0.30);
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
