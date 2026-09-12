using System.ComponentModel;
using System.Drawing;

namespace Wer.Winforms.Toolkit.Controls
{
    [ToolboxItem(true)]
    [Description("Outlined primary button — white fill, teal border and text.")]
    public class WerButtonOutlinedPrimary : WerButton
    {
        public WerButtonOutlinedPrimary()
        {
            ButtonColor  = Color.White;
            HoverColor   = Color.FromArgb(240, 250, 252);
            PressedColor = Color.FromArgb(225, 245, 248);
            BorderColor  = WerTheme.PrimaryColor;
            BorderWidth  = 2;
            ForeColor    = WerTheme.PrimaryColor;
        }
    }
}
