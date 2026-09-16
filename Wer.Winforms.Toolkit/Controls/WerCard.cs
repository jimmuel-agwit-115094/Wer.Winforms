using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    /// <summary>
    /// Dashboard stat card with title, value, and subtitle text, plus a colored top accent bar.
    /// </summary>
    [ToolboxItem(true)]
    [Category("Wer Controls")]
    [Description("Dashboard stat card with title, large value, subtitle, and a colored top accent bar.")]
    [DefaultProperty("Title")]
    public class WerCard : Control
    {
        private string _title    = "Total Revenue";
        private string _value    = "$0.00";
        private string _subtitle = "";
        private Color  _accentColor;

        private const int CornerR      = 10;
        private const int AccentBarH   = 3;
        private const int PadH         = 16;
        private const int PadV         = 14;

        public WerCard()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.SupportsTransparentBackColor,
                true);

            _accentColor = WerTheme.PrimaryColor;
            BackColor    = Color.Transparent;
            Size         = new Size(250, 120);
        }

        // ── Theme subscription ───────────────────────────────────────

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

        private void OnThemeChanged(object sender, EventArgs e) => Invalidate();

        [Category("WerCard")]
        [DefaultValue("Total Revenue")]
        [Description("Small title displayed at the top of the card.")]
        public string Title
        {
            get => _title;
            set { _title = value; Invalidate(); }
        }

        [Category("WerCard")]
        [DefaultValue("$0.00")]
        [Description("Primary value displayed in large bold text.")]
        public string Value
        {
            get => _value;
            set { _value = value; Invalidate(); }
        }

        [Category("WerCard")]
        [DefaultValue("")]
        [Description("Subtitle displayed below the value.")]
        public string Subtitle
        {
            get => _subtitle;
            set { _subtitle = value; Invalidate(); }
        }

        [Category("WerCard")]
        [Description("Color of the top accent bar.")]
        public Color AccentColor
        {
            get => _accentColor;
            set { _accentColor = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode     = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            var cardRect = new Rectangle(0, 0, Width - 1, Height - 1);

            // Card background + border
            using (var path = RoundedRect(cardRect, CornerR))
            {
                using (var brush = new SolidBrush(WerTheme.SurfaceColor))
                    g.FillPath(brush, path);
                using (var pen = new Pen(WerTheme.InputBorder, 1f))
                    g.DrawPath(pen, path);
            }

            // Accent bar — clipped to top of card, respecting rounded corners
            var accentRect = new Rectangle(0, 0, Width - 1, AccentBarH + CornerR);
            using (var path = RoundedRect(new Rectangle(0, 0, Width - 1, Height - 1), CornerR))
            {
                g.SetClip(new Rectangle(0, 0, Width, AccentBarH + CornerR));
                using (var brush = new SolidBrush(_accentColor))
                    g.FillPath(brush, path);
                g.ResetClip();
            }
            // Fill the flat middle portion of the accent bar
            using (var brush = new SolidBrush(_accentColor))
                g.FillRectangle(brush, new Rectangle(0, CornerR, Width - 1, AccentBarH));

            // Content area starts below accent bar + padding
            int contentY = AccentBarH + PadV;
            int contentW = Width - PadH * 2;

            // Title
            using (var font = new Font(WerTheme.FontFamily, 9f, FontStyle.Regular))
                TextRenderer.DrawText(g, _title, font,
                    new Rectangle(PadH, contentY, contentW, 20),
                    WerTheme.MutedColor,
                    TextFormatFlags.Left | TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix);

            contentY += 22;

            // Value (large bold)
            using (var font = new Font(WerTheme.FontFamily, 20f, FontStyle.Bold))
                TextRenderer.DrawText(g, _value, font,
                    new Rectangle(PadH, contentY, contentW, 34),
                    WerTheme.TextColor,
                    TextFormatFlags.Left | TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix);

            contentY += 36;

            // Subtitle
            if (!string.IsNullOrEmpty(_subtitle))
            {
                using (var font = new Font(WerTheme.FontFamily, 8.5f, FontStyle.Regular))
                    TextRenderer.DrawText(g, _subtitle, font,
                        new Rectangle(PadH, contentY, contentW, 20),
                        WerTheme.MutedColor,
                        TextFormatFlags.Left | TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix);
            }
        }

        private static GraphicsPath RoundedRect(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
