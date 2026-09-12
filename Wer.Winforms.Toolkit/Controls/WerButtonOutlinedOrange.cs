using System.ComponentModel;
using System.Drawing;

namespace Wer.Winforms.Toolkit.Controls
{
    [ToolboxItem(true)]
    [Description("Outlined orange button — white fill, orange border and text.")]
    public class WerButtonOutlinedOrange : WerButton
    {
        public WerButtonOutlinedOrange()
        {
            ButtonColor  = Color.White;
            HoverColor   = Color.FromArgb(254, 245, 235);
            PressedColor = Color.FromArgb(252, 235, 220);
            BorderColor  = WerTheme.OrangeColor;
            BorderWidth  = 2;
            ForeColor    = WerTheme.OrangeColor;
        }
    }
}
