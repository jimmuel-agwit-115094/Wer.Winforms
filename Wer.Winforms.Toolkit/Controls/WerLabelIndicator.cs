using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    /// <summary>
    /// Small colored pill label with configurable fill color, text color, radius, and font size.
    /// Auto-sizes to text content. Transparent control background — same approach as WerBadge.
    /// </summary>
    [ToolboxItem(true)]
    [Category("Wer Controls")]
    [Description("Colored pill label indicator with configurable colors, radius, and font size.")]
    [DefaultProperty("Text")]
    public class WerLabelIndicator : Control
    {
        private Color _fillColor;
        private int _borderRadius = 12;
        private const int PadH = 12;
        private const int MinH = 24;

        public WerLabelIndicator()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.SupportsTransparentBackColor,
                true);

            _fillColor = WerTheme.PrimaryColor;
            BackColor  = Color.Transparent;
            ForeColor  = Color.White;
            Font       = new Font(WerTheme.FontFamily, 8.5f, FontStyle.Bold);
            Text       = "Label";
            AutoSize   = true;
            AutoSizeControl();
        }

        [Category("WerLabelIndicator")]
        [Description("Fill color of the pill background.")]
        public Color FillColor
        {
            get => _fillColor;
            set { _fillColor = value; Invalidate(); }
        }

        [Category("WerLabelIndicator")]
        [DefaultValue(12)]
        [Description("Corner radius in pixels. Use Height/2 for full pill shape.")]
        public int BorderRadius
        {
            get => _borderRadius;
            set { _borderRadius = Math.Max(0, value); Invalidate(); }
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
            if (!IsHandleCreated) return;
            using (var g = CreateGraphics())
            {
                var sz = TextRenderer.MeasureText(g, Text ?? string.Empty, Font,
                    new Size(int.MaxValue, int.MaxValue), TextFormatFlags.SingleLine);
                int w = sz.Width + PadH * 2;
                int h = Math.Max(sz.Height + 6, MinH);
                Size = new Size(w, h);
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (AutoSize) AutoSizeControl();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode     = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            var rect   = new Rectangle(0, 0, Width - 1, Height - 1);
            int radius = Math.Min(_borderRadius, Math.Min(rect.Width, rect.Height) / 2);

            using (var path = RoundedRect(rect, radius))
            using (var brush = new SolidBrush(_fillColor))
                g.FillPath(brush, path);

            TextRenderer.DrawText(g, Text, Font, rect, ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix);
        }

        private static GraphicsPath RoundedRect(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int d    = radius * 2;
            if (d <= 0) { path.AddRectangle(rect); return path; }
            if (d > rect.Width)  d = rect.Width;
            if (d > rect.Height) d = rect.Height;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
