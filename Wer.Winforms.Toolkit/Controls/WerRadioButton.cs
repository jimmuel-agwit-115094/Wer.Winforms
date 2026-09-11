using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    [ToolboxItem(true)]
    [Description("Custom radio button with teal circle indicator — styled to WerTheme.")]
    [DefaultEvent("CheckedChanged")]
    [DefaultProperty("Text")]
    public class WerRadioButton : Control
    {
        private bool _checked;
        private bool _isHovering;
        private const int CircleSize = 18;
        private const int DotSize = 8;
        private const int Gap = 6;

        public event EventHandler CheckedChanged;

        public WerRadioButton()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.SupportsTransparentBackColor,
                true);

            Font = WerTheme.BodyFont;
            ForeColor = WerTheme.TextColor;
            BackColor = Color.Transparent;
            Cursor = Cursors.Hand;
            Size = new Size(120, 22);
            Text = "Option";
        }

        [Category("WerRadioButton")]
        [DefaultValue(false)]
        [Description("Whether the radio button is selected.")]
        public bool Checked
        {
            get => _checked;
            set
            {
                if (_checked == value) return;
                _checked = value;
                Invalidate();
                if (_checked) UncheckSiblings();
                CheckedChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            int top = (Height - CircleSize) / 2;
            var circleRect = new Rectangle(0, top, CircleSize, CircleSize);

            // Outer circle fill (white)
            using (var brush = new SolidBrush(Color.White))
                g.FillEllipse(brush, circleRect);

            // Outer circle border
            var borderColor = _isHovering || _checked ? WerTheme.PrimaryColor : WerTheme.BorderColor;
            using (var pen = new Pen(borderColor, _checked ? 2f : 1.5f))
                g.DrawEllipse(pen, circleRect);

            // Inner filled dot when checked
            if (_checked)
            {
                int dotX = circleRect.X + (CircleSize - DotSize) / 2;
                int dotY = circleRect.Y + (CircleSize - DotSize) / 2;
                using (var brush = new SolidBrush(WerTheme.PrimaryColor))
                    g.FillEllipse(brush, dotX, dotY, DotSize, DotSize);
            }

            // Label text
            if (!string.IsNullOrEmpty(Text))
            {
                var textRect = new Rectangle(CircleSize + Gap, 0, Width - CircleSize - Gap, Height);
                var flags = TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine;
                TextRenderer.DrawText(g, Text, Font, textRect, Enabled ? ForeColor : WerTheme.DisabledColor, flags);
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _isHovering = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _isHovering = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnClick(EventArgs e)
        {
            if (Enabled && !_checked) Checked = true;
            base.OnClick(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            Cursor = Enabled ? Cursors.Hand : Cursors.Default;
            Invalidate();
            base.OnEnabledChanged(e);
        }

        private void UncheckSiblings()
        {
            if (Parent == null) return;
            foreach (Control c in Parent.Controls)
            {
                if (c != this && c is WerRadioButton sibling && sibling._checked)
                {
                    sibling._checked = false;
                    sibling.Invalidate();
                    sibling.CheckedChanged?.Invoke(sibling, EventArgs.Empty);
                }
            }
        }
    }
}
