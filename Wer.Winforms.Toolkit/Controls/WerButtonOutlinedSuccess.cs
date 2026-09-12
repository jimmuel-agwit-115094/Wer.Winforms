using System.ComponentModel;
using System.Drawing;

namespace Wer.Winforms.Toolkit.Controls
{
    [ToolboxItem(true)]
    [Description("Outlined success button — white fill, green border and text.")]
    public class WerButtonOutlinedSuccess : WerButton
    {
        public WerButtonOutlinedSuccess()
        {
            ButtonColor  = Color.White;
            HoverColor   = Color.FromArgb(235, 250, 240);
            PressedColor = Color.FromArgb(220, 245, 230);
            BorderColor  = WerTheme.SuccessColor;
            BorderWidth  = 2;
            ForeColor    = WerTheme.SuccessColor;
        }
    }
}
