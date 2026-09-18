using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    /// <summary>
    /// Base form with theme styling applied automatically.
    /// Inherit from WerForm instead of Form for consistent look.
    ///
    /// Usage:
    ///   public partial class CustomerForm : WerForm { ... }
    ///
    /// In designer .cs file, change:
    ///   partial class CustomerForm : Wer.Winforms.Toolkit.Controls.WerForm
    /// </summary>
    [ToolboxItem(false)]
    public class WerForm : Form
    {
        public WerForm()
        {
            // Theme defaults
            BackColor = Color.White;
            Font = WerTheme.BodyFont;
            Padding = new Padding(16);
            DoubleBuffered = true;
            StartPosition = FormStartPosition.CenterParent;
            AutoScaleMode = AutoScaleMode.Font;

            // Clean look — no icon, no minimize/maximize, not resizable
            ShowIcon = false;
            MinimizeBox = false;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Text = "Manage";
        }
    }
}
