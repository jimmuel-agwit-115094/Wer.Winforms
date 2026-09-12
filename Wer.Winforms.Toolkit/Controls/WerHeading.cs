using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    public enum WerHeadingLevel
    {
        H1,
        H2,
        H3
    }

    [ToolboxItem(true)]
    [Description("Heading label with H1, H2, H3 size options.")]
    [DefaultProperty("Level")]
    public class WerHeading : Label
    {
        private WerHeadingLevel _level = WerHeadingLevel.H1;

        private static readonly float SizeH1 = 20f;
        private static readonly float SizeH2 = 16f;
        private static readonly float SizeH3 = 13f;

        public WerHeading()
        {
            AutoSize  = true;
            ForeColor = WerTheme.TextColor;
            ApplyLevel();
        }

        [Category("WerHeading")]
        [DefaultValue(WerHeadingLevel.H1)]
        [Description("Heading level — controls font size. H1 is largest.")]
        public WerHeadingLevel Level
        {
            get => _level;
            set { _level = value; ApplyLevel(); }
        }

        private void ApplyLevel()
        {
            switch (_level)
            {
                case WerHeadingLevel.H1:
                    Font = new Font(WerTheme.FontFamily, SizeH1, FontStyle.Bold);
                    break;
                case WerHeadingLevel.H2:
                    Font = new Font(WerTheme.FontFamily, SizeH2, FontStyle.Bold);
                    break;
                case WerHeadingLevel.H3:
                    Font = new Font(WerTheme.FontFamily, SizeH3, FontStyle.Bold);
                    break;
            }
        }
    }
}
