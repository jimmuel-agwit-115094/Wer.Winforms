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
        private Font _ownedFont;

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
            Font source;
            Color color;
            switch (_style)
            {
                case WerLabelStyle.Heading:
                    source = WerTheme.HeadingFont;
                    color  = WerTheme.TextColor;
                    break;
                case WerLabelStyle.Subheading:
                    source = WerTheme.SubheadingFont;
                    color  = WerTheme.TextColor;
                    break;
                case WerLabelStyle.Caption:
                    source = WerTheme.CaptionFont;
                    color  = WerTheme.MutedColor;
                    break;
                default:
                    source = WerTheme.BodyFont;
                    color  = WerTheme.TextColor;
                    break;
            }
            // Clone so the base Label.Font setter doesn't dispose the shared WerTheme font
            var old = _ownedFont;
            _ownedFont = new Font(source, source.Style);
            Font      = _ownedFont;
            ForeColor = color;
            old?.Dispose();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _ownedFont?.Dispose();
            base.Dispose(disposing);
        }
    }
}
