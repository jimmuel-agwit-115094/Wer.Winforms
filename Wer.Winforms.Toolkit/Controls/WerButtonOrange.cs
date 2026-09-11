using System.ComponentModel;
using System.Drawing;

namespace Wer.Winforms.Toolkit.Controls
{
    [ToolboxItem(true)]
    [Description("Orange button — uses WerTheme.OrangeColor.")]
    public class WerButtonOrange : WerButton
    {
        public WerButtonOrange()
        {
            ButtonColor = WerTheme.OrangeColor;
            HoverColor = DarkenColor(WerTheme.OrangeColor, 0.15);
            PressedColor = DarkenColor(WerTheme.OrangeColor, 0.30);
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
