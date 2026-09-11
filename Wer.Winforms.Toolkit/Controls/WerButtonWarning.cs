using System.ComponentModel;
using System.Drawing;

namespace Wer.Winforms.Toolkit.Controls
{
    [ToolboxItem(true)]
    [Description("Warning button — red background, white text.")]
    public class WerButtonWarning : WerButton
    {
        public WerButtonWarning()
        {
            ButtonColor = Color.FromArgb(179, 58, 58);
            HoverColor = Color.FromArgb(153, 45, 45);
            PressedColor = Color.FromArgb(128, 35, 35);
            ForeColor = Color.White;
        }
    }
}
