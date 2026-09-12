using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    [ToolboxItem(true)]
    [Description("Empty group box with rounded border and white background.")]
    public class WerEmptyGroupBox : Panel
    {
        private int _borderRadius = 10;
        private Color _borderColor = Color.FromArgb(200, 210, 220);

        public WerEmptyGroupBox()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.SupportsTransparentBackColor,
                true);

            BackColor = Color.Transparent;
            Size = new Size(400, 200);
            Padding = new Padding(10);
        }

        [Category("WerEmptyGroupBox")]
        [DefaultValue(10)]
        [Description("Corner radius of the border.")]
        public int BorderRadius
        {
            get => _borderRadius;
            set { _borderRadius = Math.Max(0, value); Invalidate(); }
        }

        [Category("WerEmptyGroupBox")]
        [Description("Color of the rounded border.")]
        public Color BorderColor
        {
            get => _borderColor;
            set { _borderColor = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var rect = new Rectangle(1, 1, Width - 3, Height - 3);
            int radius = Math.Min(_borderRadius, Math.Min(rect.Width, rect.Height) / 2);

            // White fill
            using (var path = CreateRoundedRect(rect, radius))
            using (var brush = new SolidBrush(Color.White))
                g.FillPath(brush, path);

            // Border
            using (var path = CreateRoundedRect(rect, radius))
            using (var pen = new Pen(_borderColor, 1f))
                g.DrawPath(pen, path);
        }

        private static GraphicsPath CreateRoundedRect(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;
            if (d <= 0)
            {
                path.AddRectangle(rect);
                return path;
            }
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
