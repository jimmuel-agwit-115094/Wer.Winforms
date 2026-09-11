using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    [ToolboxItem(true)]
    [Description("Disabled outline button — light gray border, gray text, no fill.")]
    public class WerButtonDisabled : WerButton
    {
        public WerButtonDisabled()
        {
            ButtonColor = Color.White;
            HoverColor = Color.White;
            PressedColor = Color.White;
            ForeColor = Color.FromArgb(180, 180, 180);
            BorderColor = Color.FromArgb(200, 200, 200);
            BorderWidth = 1;
            Cursor = Cursors.Default;
            Enabled = false;
        }
    }
}
