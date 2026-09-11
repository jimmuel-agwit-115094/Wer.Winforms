using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    [ToolboxItem(false)]
    [Description("A tab page hosted inside WerTabControl. Add controls directly to this panel.")]
    [DefaultProperty("Title")]
    public class WerTabPage : Panel
    {
        private string _title = "Tab";

        public WerTabPage()
        {
            BackColor   = Color.White;
            BorderStyle = BorderStyle.None;
        }

        [Category("WerTabPage")]
        [DefaultValue("Tab")]
        [Description("Label shown on the tab header.")]
        public string Title
        {
            get => _title;
            set
            {
                _title = value ?? string.Empty;
                (Parent as WerTabControl)?.Invalidate();
            }
        }
    }
}
