using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    [ToolboxItem(true)]
    public class WerButton : Control, IButtonControl
    {
        private WerButtonType _buttonType = WerButtonType.Primary;
        private WerButtonColor _colorType = WerButtonColor.Blue;
        private int _borderRadius = 20;
        private bool _isHovering;
        private bool _isPressed;
        private bool _isDefault;
        private string _iconCode = "";
        private float _fontSize = 0f;
        private bool _boldFont;
        private Font _customFont;
        private DialogResult _dialogResult = DialogResult.None;

        public WerButton()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.Selectable,
                true);

            Font = WerTheme.ButtonFont;
            Size = new Size(100, 36);
            Cursor = Cursors.Hand;
        }

        // ── IButtonControl ───────────────────────────────────────

        [Category("WerButtons")]
        [DefaultValue(DialogResult.None)]
        [Description("DialogResult returned when this button is clicked on a modal form.")]
        public DialogResult DialogResult
        {
            get => _dialogResult;
            set => _dialogResult = value;
        }

        public void NotifyDefault(bool value)
        {
            _isDefault = value;
            Invalidate();
        }

        public void PerformClick()
        {
            OnClick(EventArgs.Empty);
        }

        // ── Properties ──────────────────────────────────────────

        [Category("WerButtons")]
        [DefaultValue(WerButtonType.Primary)]
        [Description("Primary = filled, Secondary = outlined.")]
        public WerButtonType ButtonType
        {
            get => _buttonType;
            set { _buttonType = value; Invalidate(); }
        }

        [Category("WerButtons")]
        [DefaultValue(WerButtonColor.Blue)]
        [Description("Color variant (Blue, Primary/teal, Warning/red, Success/green, Orange).")]
        public WerButtonColor ColorType
        {
            get => _colorType;
            set { _colorType = value; Invalidate(); }
        }

        [Category("WerButtons")]
        [DefaultValue(20)]
        [Description("Corner radius in pixels.")]
        public int BorderRadius
        {
            get => _borderRadius;
            set { _borderRadius = Math.Max(0, value); Invalidate(); }
        }

        [Category("WerButtons")]
        [DefaultValue("")]
        [Description("Icon code from WerIcons. Drawn left of text.")]
        public string IconCode
        {
            get => _iconCode;
            set { _iconCode = value ?? ""; Invalidate(); }
        }

        /// <summary>
        /// Custom font size for button label. 0 = use default theme size.
        /// </summary>
        [Category("WerButtons")]
        [DefaultValue(0f)]
        [Description("Custom font size for label text. 0 = default theme size.")]
        public float FontSize
        {
            get => _fontSize;
            set
            {
                _fontSize = Math.Max(0f, value);
                RebuildCustomFont();
                Invalidate();
            }
        }

        /// <summary>
        /// Use bold font weight for button label.
        /// </summary>
        [Category("WerButtons")]
        [DefaultValue(false)]
        [Description("Use bold font weight for button label.")]
        public bool BoldFont
        {
            get => _boldFont;
            set
            {
                _boldFont = value;
                RebuildCustomFont();
                Invalidate();
            }
        }

        private void RebuildCustomFont()
        {
            _customFont?.Dispose();
            _customFont = null;

            float size = _fontSize > 0 ? _fontSize : WerTheme.ButtonSize;
            FontStyle style = _boldFont ? FontStyle.Bold : WerTheme.ButtonFont.Style;

            // Only create custom font if different from theme default
            if (_fontSize > 0 || _boldFont)
                _customFont = new Font(WerTheme.FontFamily, size, style);
        }

        private Font ActiveFont => _customFont ?? WerTheme.ButtonFont;

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            var radius = Math.Min(_borderRadius, Math.Min(rect.Width, rect.Height) / 2);
            var themeColor = WerTheme.GetButtonColor(_colorType);
            bool isOutlined = _buttonType == WerButtonType.Secondary;

            if (!Enabled)
            {
                PaintDisabled(g, rect, radius, isOutlined);
                return;
            }

            if (isOutlined)
                PaintOutlined(g, rect, radius, themeColor);
            else
                PaintFilled(g, rect, radius, themeColor);
        }

        private void PaintFilled(Graphics g, Rectangle rect, int radius, Color color)
        {
            Color fill = _isPressed ? DarkenColor(color, 0.30)
                       : _isHovering ? DarkenColor(color, 0.15)
                       : color;

            using (var path = CreateRoundedRect(rect, radius))
            using (var brush = new SolidBrush(fill))
                g.FillPath(brush, path);

            DrawContent(g, Color.White);
        }

        private void PaintOutlined(Graphics g, Rectangle rect, int radius, Color color)
        {
            Color fill = _isPressed ? LightenColor(color, 0.85)
                       : _isHovering ? LightenColor(color, 0.92)
                       : Color.White;

            using (var path = CreateRoundedRect(rect, radius))
            {
                using (var brush = new SolidBrush(fill))
                    g.FillPath(brush, path);
                using (var pen = new Pen(color, 2))
                    g.DrawPath(pen, path);
            }

            DrawContent(g, color);
        }

        private void PaintDisabled(Graphics g, Rectangle rect, int radius, bool isOutlined)
        {
            var disabledText = Color.FromArgb(170, 175, 182);

            using (var path = CreateRoundedRect(rect, radius))
            {
                if (isOutlined)
                {
                    using (var brush = new SolidBrush(Color.White))
                        g.FillPath(brush, path);
                    using (var pen = new Pen(Color.FromArgb(200, 205, 212), 2))
                        g.DrawPath(pen, path);
                }
                else
                {
                    using (var brush = new SolidBrush(Color.FromArgb(230, 232, 236)))
                        g.FillPath(brush, path);
                }
            }

            DrawContent(g, disabledText);
        }

        private void DrawContent(Graphics g, Color color)
        {
            bool hasIcon = !string.IsNullOrEmpty(_iconCode);
            bool hasText = !string.IsNullOrEmpty(Text);
            var textFont = ActiveFont;

            if (!hasIcon && !hasText) return;

            if (hasIcon && hasText)
            {
                using (var iconFont = new Font("Segoe MDL2 Assets", textFont.Size, FontStyle.Regular))
                {
                    int iconW = TextRenderer.MeasureText(g, _iconCode, iconFont).Width;
                    int textW = TextRenderer.MeasureText(g, Text, textFont).Width;
                    int gap = 6;
                    int totalW = iconW + gap + textW;
                    int startX = (Width - totalW) / 2;

                    var iconRect = new Rectangle(startX, 0, iconW, Height);
                    TextRenderer.DrawText(g, _iconCode, iconFont, iconRect, color,
                        TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

                    var textRect = new Rectangle(startX + iconW + gap, 0, textW, Height);
                    TextRenderer.DrawText(g, Text, textFont, textRect, color,
                        TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
                }
            }
            else if (hasIcon)
            {
                using (var iconFont = new Font("Segoe MDL2 Assets", textFont.Size, FontStyle.Regular))
                    TextRenderer.DrawText(g, _iconCode, iconFont, ClientRectangle, color,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
            }
            else
            {
                TextRenderer.DrawText(g, Text, textFont, ClientRectangle, color,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding);
            }
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (_dialogResult != DialogResult.None)
            {
                var form = FindForm();
                if (form != null)
                    form.DialogResult = _dialogResult;
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

        private static Color DarkenColor(Color color, double factor)
        {
            return Color.FromArgb(
                color.A,
                (int)(color.R * (1 - factor)),
                (int)(color.G * (1 - factor)),
                (int)(color.B * (1 - factor)));
        }

        private static Color LightenColor(Color color, double factor)
        {
            return Color.FromArgb(
                color.A,
                (int)(color.R + (255 - color.R) * factor),
                (int)(color.G + (255 - color.G) * factor),
                (int)(color.B + (255 - color.B) * factor));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _customFont?.Dispose();
            base.Dispose(disposing);
        }
    }
}
