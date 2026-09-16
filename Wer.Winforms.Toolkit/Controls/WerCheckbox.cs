using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    [ToolboxItem(true)]
    [Description("Custom checkbox with label — styled to WerTheme.")]
    [DefaultEvent("CheckedChanged")]
    [DefaultProperty("Text")]
    public class WerCheckbox : Control
    {
        private bool _checked;
        private bool _isHovering;
        private const int BoxSize = 16;
        private const int Gap = 6;

        public event EventHandler CheckedChanged;

        public WerCheckbox()
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
            Text = "Checkbox Label";
        }

        [Category("WerCheckbox")]
        [DefaultValue(false)]
        [Description("Whether the checkbox is checked.")]
        public bool Checked
        {
            get => _checked;
            set
            {
                if (_checked == value) return;
                _checked = value;
                Invalidate();
                CheckedChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            int top = (Height - BoxSize) / 2;
            var boxRect = new Rectangle(0, top, BoxSize, BoxSize);

            // Box fill
            using (var brush = new SolidBrush(_checked ? WerTheme.PrimaryColor : WerTheme.InputBg))
                g.FillRectangle(brush, boxRect);

            // Box border
            var borderColor = _isHovering || _checked ? WerTheme.PrimaryColor : WerTheme.BorderColor;
            using (var pen = new Pen(borderColor, 1.5f))
                g.DrawRectangle(pen, boxRect);

            // Checkmark
            if (_checked)
            {
                using (var pen = new Pen(Color.White, 2f) { LineJoin = LineJoin.Round })
                {
                    var cx = boxRect.X;
                    var cy = boxRect.Y;
                    g.DrawLines(pen, new[]
                    {
                        new PointF(cx + 3, cy + 8),
                        new PointF(cx + 6, cy + 11),
                        new PointF(cx + 13, cy + 4)
                    });
                }
            }

            // Label
            if (!string.IsNullOrEmpty(Text))
            {
                var textRect = new Rectangle(BoxSize + Gap, 0, Width - BoxSize - Gap, Height);
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
            if (Enabled) Checked = !Checked;
            base.OnClick(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            Cursor = Enabled ? Cursors.Hand : Cursors.Default;
            Invalidate();
            base.OnEnabledChanged(e);
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
            Invalidate();
        }
    }
}
