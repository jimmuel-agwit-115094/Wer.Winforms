using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    [ToolboxItem(false)]
    public class WerButton : Control
    {
        private Color _buttonColor = Color.FromArgb(12, 124, 146);
        private Color _hoverColor = Color.FromArgb(10, 105, 124);
        private Color _pressedColor = Color.FromArgb(8, 86, 102);
        private Color _borderColor = Color.Empty;
        private int _borderRadius = 20;
        private int _borderWidth = 0;
        private bool _isHovering;
        private bool _isPressed;

        public WerButton()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.SupportsTransparentBackColor,
                true);

            Font = WerTheme.ButtonFont;
            ForeColor = Color.White;
            Size = new Size(100, 36);
            Cursor = Cursors.Hand;
        }

        [Category("WerButtons")]
        [Description("Fill color in normal state.")]
        public Color ButtonColor
        {
            get => _buttonColor;
            set { _buttonColor = value; Invalidate(); }
        }

        [Category("WerButtons")]
        [Description("Fill color on hover.")]
        public Color HoverColor
        {
            get => _hoverColor;
            set { _hoverColor = value; Invalidate(); }
        }

        [Category("WerButtons")]
        [Description("Fill color when pressed.")]
        public Color PressedColor
        {
            get => _pressedColor;
            set { _pressedColor = value; Invalidate(); }
        }

        [Category("WerButtons")]
        [Description("Border color. Empty = no border.")]
        public Color BorderColor
        {
            get => _borderColor;
            set { _borderColor = value; Invalidate(); }
        }

        [Category("WerButtons")]
        [DefaultValue(0)]
        [Description("Border thickness in pixels.")]
        public int BorderWidth
        {
            get => _borderWidth;
            set { _borderWidth = Math.Max(0, value); Invalidate(); }
        }

        [Category("WerButtons")]
        [DefaultValue(20)]
        [Description("Corner radius in pixels.")]
        public int BorderRadius
        {
            get => _borderRadius;
            set { _borderRadius = Math.Max(0, value); Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            var radius = Math.Min(_borderRadius, Math.Min(rect.Width, rect.Height) / 2);

            if (!Enabled)
            {
                // Disabled look: white fill, gray border, gray text
                using (var path = CreateRoundedRect(rect, radius))
                {
                    using (var brush = new SolidBrush(Color.White))
                        g.FillPath(brush, path);

                    using (var pen = new Pen(WerTheme.BorderColor, 1))
                        g.DrawPath(pen, path);
                }

                if (!string.IsNullOrEmpty(Text))
                {
                    var flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                                TextFormatFlags.SingleLine | TextFormatFlags.NoPadding;
                    TextRenderer.DrawText(g, Text, Font, ClientRectangle, WerTheme.DisabledColor, flags);
                }
                return;
            }

            Color fill = _isPressed ? _pressedColor : _isHovering ? _hoverColor : _buttonColor;

            using (var path = CreateRoundedRect(rect, radius))
            {
                using (var brush = new SolidBrush(fill))
                    g.FillPath(brush, path);

                if (_borderWidth > 0 && _borderColor != Color.Empty)
                {
                    using (var pen = new Pen(_borderColor, _borderWidth))
                        g.DrawPath(pen, path);
                }
            }

            if (!string.IsNullOrEmpty(Text))
            {
                var flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                            TextFormatFlags.SingleLine | TextFormatFlags.NoPadding;
                TextRenderer.DrawText(g, Text, Font, ClientRectangle, ForeColor, flags);
            }
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            Cursor = Enabled ? Cursors.Hand : Cursors.Default;
            Invalidate();
            base.OnEnabledChanged(e);
        }

        private static GraphicsPath CreateRoundedRect(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            var d = radius * 2;

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

        protected override void OnMouseEnter(EventArgs e)
        {
            _isHovering = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _isHovering = false;
            _isPressed = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isPressed = true;
                Invalidate();
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _isPressed = false;
            Invalidate();
            base.OnMouseUp(e);
        }
    }
}
