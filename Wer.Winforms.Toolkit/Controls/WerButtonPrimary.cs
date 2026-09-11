using System.ComponentModel;
using System.Drawing;

namespace Wer.Winforms.Toolkit.Controls
{
    [ToolboxItem(true)]
    [Description("Primary filled button — teal background, white text.")]
    public class WerButtonPrimary : WerButton
    {
        public WerButtonPrimary()
        {
            ButtonColor = Color.FromArgb(12, 124, 146);
            HoverColor = Color.FromArgb(10, 105, 124);
            PressedColor = Color.FromArgb(8, 86, 102);
            ForeColor = Color.White;
        }
    }
}
