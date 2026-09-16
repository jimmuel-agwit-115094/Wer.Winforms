using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    /// <summary>
    /// Small colored pill/chip badge. Auto-sizes to text content.
    /// </summary>
    [ToolboxItem(true)]
    [Category("Wer Controls")]
    [Description("Small colored pill badge that auto-sizes to its text content.")]
    [DefaultProperty("Text")]
    public class WerBadge : Control
    {
        private Color _badgeColor;
        private Font  _ownFont;
        private const int PadH     = 12;
        private const int MinH     = 24;

        public WerBadge()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.SupportsTransparentBackColor,
                true);

            _badgeColor = WerTheme.PrimaryColor;
            BackColor   = Color.Transparent;
            Text        = "Badge";
            _ownFont    = new Font(WerTheme.FontFamily, 8.5f, FontStyle.Bold);
            Font        = _ownFont;
            AutoSize    = true;
            AutoSizeControl();
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

        private void OnThemeChanged(object sender, EventArgs e) => Invalidate();

        [Category("WerBadge")]
        [Description("Color used for the badge fill (light tint), border, and text.")]
        public Color BadgeColor
        {
            get => _badgeColor;
            set { _badgeColor = value; Invalidate(); }
        }

        public override bool AutoSize
        {
            get => base.AutoSize;
            set { base.AutoSize = value; if (value) AutoSizeControl(); }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            if (AutoSize) AutoSizeControl();
            Invalidate();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            if (AutoSize) AutoSizeControl();
        }

        private void AutoSizeControl()
        {
            using (var g = CreateGraphics())
            {
                var sz = TextRenderer.MeasureText(g, Text ?? string.Empty, Font,
                    new Size(int.MaxValue, int.MaxValue), TextFormatFlags.SingleLine);
                int w = sz.Width + PadH * 2;
                int h = Math.Max(sz.Height + 6, MinH);
                Size = new Size(w, h);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode     = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            var rect   = new Rectangle(0, 0, Width - 1, Height - 1);
            int radius = Height / 2;

            // Light tint fill (15% opacity)
            Color fillColor   = Color.FromArgb(38, _badgeColor.R, _badgeColor.G, _badgeColor.B);
            // Border (30% opacity)
            Color borderColor = Color.FromArgb(77, _badgeColor.R, _badgeColor.G, _badgeColor.B);

            using (var path = RoundedRect(rect, radius))
            {
                using (var brush = new SolidBrush(fillColor))
                    g.FillPath(brush, path);
                using (var pen = new Pen(borderColor, 1f))
                    g.DrawPath(pen, path);
            }

            TextRenderer.DrawText(g, Text, Font, rect, _badgeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix);
        }

        private static GraphicsPath RoundedRect(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int d    = radius * 2;
            if (d > rect.Width)  d = rect.Width;
            if (d > rect.Height) d = rect.Height;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) { _ownFont?.Dispose(); _ownFont = null; }
            base.Dispose(disposing);
        }
    }
}
