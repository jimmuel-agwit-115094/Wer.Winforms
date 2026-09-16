using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    /// <summary>
    /// Rounded themed progress bar with optional percentage label.
    /// </summary>
    [ToolboxItem(true)]
    [Category("Wer Controls")]
    [Description("Rounded pill-shaped progress bar with optional percentage label.")]
    [DefaultProperty("Value")]
    public class WerProgressBar : Control
    {
        private int   _value     = 0;
        private int   _maximum   = 100;
        private Color _barColor;
        private bool  _showLabel = true;

        private static readonly Color TrackColor = Color.FromArgb(235, 238, 242);

        public WerProgressBar()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.SupportsTransparentBackColor,
                true);

            _barColor = WerTheme.PrimaryColor;
            BackColor = Color.Transparent;
            Size      = new Size(300, 24);
        }

        [Category("WerProgressBar")]
        [DefaultValue(0)]
        [Description("Current progress value (0 to Maximum).")]
        public int Value
        {
            get => _value;
            set
            {
                _value = Math.Max(0, Math.Min(value, _maximum));
                Invalidate();
            }
        }

        [Category("WerProgressBar")]
        [DefaultValue(100)]
        [Description("Maximum value for the progress bar.")]
        public int Maximum
        {
            get => _maximum;
            set
            {
                _maximum = Math.Max(1, value);
                _value   = Math.Min(_value, _maximum);
                Invalidate();
            }
        }

        [Category("WerProgressBar")]
        [Description("Fill color of the progress bar.")]
        public Color BarColor
        {
            get => _barColor;
            set { _barColor = value; Invalidate(); }
        }

        [Category("WerProgressBar")]
        [DefaultValue(true)]
        [Description("Show percentage label centered on the bar.")]
        public bool ShowLabel
        {
            get => _showLabel;
            set { _showLabel = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode     = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            var trackRect = new Rectangle(0, 0, Width - 1, Height - 1);
            int radius    = Height / 2;

            // Track
            using (var path = RoundedRect(trackRect, radius))
            using (var brush = new SolidBrush(TrackColor))
                g.FillPath(brush, path);

            // Fill
            double pct    = _maximum > 0 ? (double)_value / _maximum : 0;
            int    fillW  = (int)Math.Round(pct * (Width - 1));

            if (fillW > 0)
            {
                // Clip the fill to the track shape
                using (var clipPath = RoundedRect(trackRect, radius))
                {
                    g.SetClip(clipPath);
                    using (var brush = new SolidBrush(_barColor))
                        g.FillRectangle(brush, 0, 0, fillW, Height);
                    g.ResetClip();
                }
            }

            // Label
            if (_showLabel)
            {
                string label = $"{(int)Math.Round(pct * 100)}%";

                // Determine whether center falls in fill or track
                bool centerInFill = (Width / 2) <= fillW;
                Color textColor   = centerInFill ? Color.White : WerTheme.TextColor;

                using (var font = new Font(WerTheme.FontFamily, 8f, FontStyle.Bold))
                    TextRenderer.DrawText(g, label, font, trackRect, textColor,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                        TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix);
            }
        }

        private static GraphicsPath RoundedRect(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;
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
