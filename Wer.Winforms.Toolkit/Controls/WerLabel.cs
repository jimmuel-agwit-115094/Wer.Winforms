using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    public enum WerLabelStyle
    {
        Heading,
        Subheading,
        Body,
        Caption
    }

    [ToolboxItem(true)]
    [Description("Label that inherits font and color from WerTheme.")]
    [DefaultProperty("LabelStyle")]
    public class WerLabel : Label
    {
        private WerLabelStyle _style = WerLabelStyle.Body;

        public WerLabel()
        {
            AutoSize = true;
            ApplyStyle();
        }

        [Category("WerLabel")]
        [DefaultValue(WerLabelStyle.Body)]
        [Description("Controls which WerTheme font and color are applied.")]
        public WerLabelStyle LabelStyle
        {
            get => _style;
            set { _style = value; ApplyStyle(); }
        }

        private void ApplyStyle()
        {
            switch (_style)
            {
                case WerLabelStyle.Heading:
                    Font      = WerTheme.HeadingFont;
                    ForeColor = WerTheme.TextColor;
                    break;
                case WerLabelStyle.Subheading:
                    Font      = WerTheme.SubheadingFont;
                    ForeColor = WerTheme.TextColor;
                    break;
                case WerLabelStyle.Caption:
                    Font      = WerTheme.CaptionFont;
                    ForeColor = WerTheme.MutedColor;
                    break;
                default: // Body
                    Font      = WerTheme.BodyFont;
                    ForeColor = WerTheme.TextColor;
                    break;
            }
        }
    }
}
