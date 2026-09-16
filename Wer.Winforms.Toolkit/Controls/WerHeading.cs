using System;
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
        private Font _ownedFont;

        private static readonly float SizeH1 = 20f;
        private static readonly float SizeH2 = 16f;
        private static readonly float SizeH3 = 13f;

        public WerHeading()
        {
            AutoSize  = true;
            ForeColor = WerTheme.TextColor;
            ApplyLevel();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            WerTheme.ThemeChanged += OnThemeChanged;
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);
            WerTheme.ThemeChanged -= OnThemeChanged;
        }

        private void OnThemeChanged(object sender, EventArgs e)
        {
            ForeColor = WerTheme.TextColor;
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
            float size;
            switch (_level)
            {
                case WerHeadingLevel.H1: size = SizeH1; break;
                case WerHeadingLevel.H2: size = SizeH2; break;
                default:                 size = SizeH3; break;
            }
            var newFont = new Font(WerTheme.FontFamily, size, FontStyle.Bold);
            _ownedFont?.Dispose();
            _ownedFont = newFont;
            Font = newFont;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _ownedFont?.Dispose();
            base.Dispose(disposing);
        }
    }
}
