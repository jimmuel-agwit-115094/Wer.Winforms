using System.ComponentModel;
using System.Drawing;

namespace Wer.Winforms.Toolkit.Controls
{
    [ToolboxItem(true)]
    [Description("Outlined warning button — white fill, red border and text.")]
    public class WerButtonOutlinedWarning : WerButton
    {
        public WerButtonOutlinedWarning()
        {
            ButtonColor  = Color.White;
            HoverColor   = Color.FromArgb(253, 240, 240);
            PressedColor = Color.FromArgb(250, 225, 225);
            BorderColor  = WerTheme.WarningColor;
            BorderWidth  = 2;
            ForeColor    = WerTheme.WarningColor;
        }
    }
}
