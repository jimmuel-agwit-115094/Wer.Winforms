using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    [ToolboxItem(true)]
    [Description("Horizontal divider line.")]
    public class WerDivider : Control
    {
        private Color _lineColor = Color.FromArgb(220, 220, 220);
        private int _lineThickness = 1;

        public WerDivider()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.SupportsTransparentBackColor,
                true);

            BackColor = Color.Transparent;
            Size = new Size(300, 10);
        }

        [Category("WerDivider")]
        [Description("Color of the divider line.")]
        public Color LineColor
        {
            get => _lineColor;
            set { _lineColor = value; Invalidate(); }
        }

        [Category("WerDivider")]
        [DefaultValue(1)]
        [Description("Thickness of the divider line in pixels.")]
        public int LineThickness
        {
            get => _lineThickness;
            set { _lineThickness = Math.Max(1, value); Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int y = Height / 2;
            using (var pen = new Pen(_lineColor, _lineThickness))
                e.Graphics.DrawLine(pen, 0, y, Width, y);
        }
    }
}
