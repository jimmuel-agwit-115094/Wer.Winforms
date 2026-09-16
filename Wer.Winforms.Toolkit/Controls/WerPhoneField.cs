using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    [ToolboxItem(true)]
    [Description("Philippine mobile number input. Accepts 09XX-XXX-XXXX format (11 digits).")]
    [DefaultEvent("TextChanged")]
    [DefaultProperty("LabelText")]
    public class WerPhoneField : Control
    {
        private readonly TextBox _input;
        private readonly Panel   _inputBorder;

        private string _labelText   = "Mobile Number";
        private bool   _required;
        private bool   _readOnly;
        private bool   _hasFocus;
        private bool   _isFormatting;

        private int LabelHeight => Math.Max(20, (int)(Font.GetHeight() + 4));
        private const int LabelGap     = 4;
        private const int BorderRadius = 8;
        private const int InputPadH    = 8;
        private int PrefixWidth => TextRenderer.MeasureText("+63", Font).Width + 2;

        public WerPhoneField()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.SupportsTransparentBackColor, true);

            BackColor = Color.Transparent;
            Font      = WerTheme.BodyFont;
            Size      = new Size(350, 60);

            _inputBorder = new Panel { BackColor = Color.Transparent };
            _inputBorder.Paint += OnBorderPaint;
            Controls.Add(_inputBorder);

            _input = new TextBox
            {
                BorderStyle = BorderStyle.None,
                BackColor   = WerTheme.InputBg,
                ForeColor   = WerTheme.TextColor,
                Font        = WerTheme.BodyFont,
                Multiline   = false,
                MaxLength   = 13, // 09XX-XXX-XXXX
            };
            _input.KeyPress    += OnInputKeyPress;
            _input.TextChanged += OnInputTextChanged;
            _input.GotFocus    += (s, e) => { _hasFocus = true;  _inputBorder.Invalidate(); };
            _input.LostFocus   += (s, e) => { _hasFocus = false; _inputBorder.Invalidate(); };
            _inputBorder.Controls.Add(_input);

            LayoutInternals();
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

        private void OnThemeChanged(object sender, EventArgs e)
        {
            ApplyVisualState();
            Invalidate();
            _inputBorder?.Invalidate();
        }

        private bool _showLabel = true;

        [Category("WerPhoneField")]
        [DefaultValue(true)]
        [Description("Show or hide the label above the input.")]
        public bool ShowLabel
        {
            get => _showLabel;
            set { _showLabel = value; LayoutInternals(); Invalidate(); }
        }

        [Category("WerPhoneField")]
        [DefaultValue("Mobile Number")]
        public string LabelText
        {
            get => _labelText;
            set { _labelText = value; Invalidate(); }
        }

        [Category("WerPhoneField")]
        [DefaultValue(false)]
        public bool Required
        {
            get => _required;
            set { _required = value; Invalidate(); }
        }

        [Category("WerPhoneField")]
        [DefaultValue(false)]
        public bool ReadOnly
        {
            get => _readOnly;
            set
            {
                _readOnly       = value;
                _input.ReadOnly = value;
                ApplyVisualState();
                Invalidate();
                _inputBorder.Invalidate();
            }
        }

        /// <summary>Raw digits only (e.g. "09171234567").</summary>
        [Browsable(false)]
        public string RawNumber
        {
            get
            {
                var raw = _input.Text.Replace("-", "");
                return raw;
            }
        }

        [Browsable(true)]
        [Category("WerPhoneField")]
        [DefaultValue("")]
        public override string Text
        {
            get => _input.Text;
            set => _input.Text = value;
        }

        // ── Input filtering ──────────────────────────────────────────

        private void OnInputKeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar)) return;
            if (!char.IsDigit(e.KeyChar)) { e.Handled = true; return; }

            // Count existing digits
            int digitCount = 0;
            foreach (char c in _input.Text)
                if (char.IsDigit(c)) digitCount++;

            if (digitCount >= 11) { e.Handled = true; }
        }

        private void OnInputTextChanged(object sender, EventArgs e)
        {
            if (_isFormatting) return;
            _isFormatting = true;

            // Strip non-digits
            var digits = "";
            foreach (char c in _input.Text)
                if (char.IsDigit(c)) digits += c;

            if (digits.Length > 11) digits = digits.Substring(0, 11);

            // Format as 09XX-XXX-XXXX
            string formatted;
            if (digits.Length <= 4)
                formatted = digits;
            else if (digits.Length <= 7)
                formatted = digits.Substring(0, 4) + "-" + digits.Substring(4);
            else
                formatted = digits.Substring(0, 4) + "-" + digits.Substring(4, 3) + "-" + digits.Substring(7);

            _input.Text = formatted;
            _input.SelectionStart = _input.Text.Length;

            _isFormatting = false;
            OnTextChanged(EventArgs.Empty);
        }

        // ── Enable/disable ───────────────────────────────────────────

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            _input.Enabled = Enabled;
            ApplyVisualState();
            Invalidate();
            _inputBorder.Invalidate();
        }

        private void ApplyVisualState()
        {
            bool inactive = !Enabled || _readOnly;
            _input.BackColor = inactive ? WerTheme.InputBgDisabled : WerTheme.InputBg;
            _input.ForeColor = !Enabled ? WerTheme.LabelDisabledColor : WerTheme.TextColor;
            _input.ReadOnly  = !Enabled || _readOnly;
        }

        // ── Layout ───────────────────────────────────────────────────

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            LayoutInternals();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            if (_input == null) return;
            _input.Font = Font;
            LayoutInternals();
        }

        private void LayoutInternals()
        {
            if (_inputBorder == null || _input == null) return;

            int borderTop = _showLabel ? LabelHeight + LabelGap : 0;
            int borderH   = Height - borderTop;
            _inputBorder.SetBounds(0, borderTop, Width, borderH);

            int leftPad = InputPadH + PrefixWidth;
            int textH = _input.PreferredHeight;
            int textY = (borderH - textH) / 2;
            _input.SetBounds(leftPad, Math.Max(0, textY), Width - leftPad - InputPadH, textH);
        }

        // ── Paint label ──────────────────────────────────────────────

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            if (!_showLabel) return;

            Color labelColor;
            if (!Enabled)       labelColor = WerTheme.LabelDisabledColor;
            else if (_readOnly) labelColor = WerTheme.LabelReadOnlyColor;
            else                labelColor = WerTheme.LabelColor;

            var labelRect = new Rectangle(0, 0, Width, LabelHeight);
            TextRenderer.DrawText(g, _labelText, Font, labelRect, labelColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);

            if (_required && Enabled)
            {
                int labelW = TextRenderer.MeasureText(g, _labelText, Font).Width;
                var starRect = new Rectangle(labelW + 2, 0, 12, LabelHeight);
                TextRenderer.DrawText(g, "*", Font, starRect, WerTheme.RequiredStarColor,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            }
        }

        // ── Paint border panel ───────────────────────────────────────

        private void OnBorderPaint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var panel = (Panel)sender;
            var rect  = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
            bool inactive = !Enabled || _readOnly;

            using (var path = RoundedRect(rect, BorderRadius))
            using (var brush = new SolidBrush(inactive ? WerTheme.InputBgDisabled : WerTheme.InputBg))
                g.FillPath(brush, path);

            if (Enabled && !_readOnly)
            {
                var borderColor = _hasFocus ? WerTheme.InputBorderFocus : WerTheme.InputBorder;
                using (var path = RoundedRect(rect, BorderRadius))
                using (var pen  = new Pen(borderColor, _hasFocus ? 1.5f : 1f))
                    g.DrawPath(pen, path);
            }
            else if (_readOnly)
            {
                using (var path = RoundedRect(rect, BorderRadius))
                using (var pen  = new Pen(WerTheme.InputBorder, 1f))
                    g.DrawPath(pen, path);
            }

            // +63 prefix
            var prefixRect = new Rectangle(InputPadH, 0, PrefixWidth, panel.Height);
            var prefixCol  = inactive ? WerTheme.LabelDisabledColor : WerTheme.MutedColor;
            TextRenderer.DrawText(g, "+63", Font, prefixRect, prefixCol,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);

            // Placeholder
            if (string.IsNullOrEmpty(_input.Text) && !_hasFocus && Enabled)
            {
                int leftPad = InputPadH + PrefixWidth;
                var ph = new Rectangle(leftPad, 0, panel.Width - leftPad - InputPadH, panel.Height);
                TextRenderer.DrawText(g, "9XX-XXX-XXXX", _input.Font, ph, WerTheme.PlaceholderColor,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
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
