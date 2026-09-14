using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    [ToolboxItem(true)]
    [Description("Philippine telephone number input. Accepts (0XX) XXX-XXXX format.")]
    [DefaultEvent("TextChanged")]
    [DefaultProperty("LabelText")]
    public class WerTelephoneField : Control
    {
        private readonly TextBox _input;
        private readonly Panel   _inputBorder;

        private string _labelText   = "Telephone Number";
        private bool   _required;
        private bool   _readOnly;
        private bool   _hasFocus;
        private bool   _isFormatting;

        private int LabelHeight => Math.Max(20, (int)(Font.GetHeight() + 4));
        private const int LabelGap     = 4;
        private const int BorderRadius = 8;
        private const int InputPadH    = 8;
        private int PrefixWidth => TextRenderer.MeasureText("☎", Font).Width + 2;

        private static readonly Color LabelNormal      = Color.Black;
        private static readonly Color LabelRequired    = Color.FromArgb(200, 100, 20);
        private static readonly Color LabelDisabled    = Color.FromArgb(150, 150, 150);
        private static readonly Color RequiredStar     = Color.FromArgb(210, 50, 50);
        private static readonly Color BorderNormal     = Color.FromArgb(200, 210, 220);
        private static readonly Color BorderFocus      = Color.FromArgb(12, 124, 146);
        private static readonly Color PlaceholderColor = Color.FromArgb(160, 170, 180);
        private static readonly Color PrefixColor      = Color.FromArgb(108, 117, 125);
        private static readonly Color BgNormal         = Color.White;
        private static readonly Color BgDisabled       = Color.FromArgb(242, 242, 242);

        public WerTelephoneField()
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
                BackColor   = BgNormal,
                ForeColor   = WerTheme.TextColor,
                Font        = WerTheme.BodyFont,
                Multiline   = false,
                MaxLength   = 14, // (0XX) XXX-XXXX
            };
            _input.KeyPress    += OnInputKeyPress;
            _input.TextChanged += OnInputTextChanged;
            _input.GotFocus    += (s, e) => { _hasFocus = true;  _inputBorder.Invalidate(); };
            _input.LostFocus   += (s, e) => { _hasFocus = false; _inputBorder.Invalidate(); };
            _inputBorder.Controls.Add(_input);

            LayoutInternals();
        }

        private bool _showLabel = true;

        [Category("WerTelephoneField")]
        [DefaultValue(true)]
        [Description("Show or hide the label above the input.")]
        public bool ShowLabel
        {
            get => _showLabel;
            set { _showLabel = value; LayoutInternals(); Invalidate(); }
        }

        [Category("WerTelephoneField")]
        [DefaultValue("Telephone Number")]
        public string LabelText
        {
            get => _labelText;
            set { _labelText = value; Invalidate(); }
        }

        [Category("WerTelephoneField")]
        [DefaultValue(false)]
        public bool Required
        {
            get => _required;
            set { _required = value; Invalidate(); }
        }

        [Category("WerTelephoneField")]
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

        /// <summary>Raw digits only (e.g. "0281234567").</summary>
        [Browsable(false)]
        public string RawNumber
        {
            get
            {
                var raw = "";
                foreach (char c in _input.Text)
                    if (char.IsDigit(c)) raw += c;
                return raw;
            }
        }

        [Browsable(true)]
        [Category("WerTelephoneField")]
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

            int digitCount = 0;
            foreach (char c in _input.Text)
                if (char.IsDigit(c)) digitCount++;

            if (digitCount >= 10) { e.Handled = true; }
        }

        private void OnInputTextChanged(object sender, EventArgs e)
        {
            if (_isFormatting) return;
            _isFormatting = true;

            var digits = "";
            foreach (char c in _input.Text)
                if (char.IsDigit(c)) digits += c;

            if (digits.Length > 10) digits = digits.Substring(0, 10);

            // Format as (0XX) XXX-XXXX
            string formatted;
            if (digits.Length <= 3)
                formatted = digits.Length > 0 ? "(" + digits : "";
            else if (digits.Length <= 6)
                formatted = "(" + digits.Substring(0, 3) + ") " + digits.Substring(3);
            else
                formatted = "(" + digits.Substring(0, 3) + ") " + digits.Substring(3, 3) + "-" + digits.Substring(6);

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
            _input.BackColor = inactive ? BgDisabled : BgNormal;
            _input.ForeColor = !Enabled ? LabelDisabled : WerTheme.TextColor;
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
            if (!Enabled)                    labelColor = LabelDisabled;
            else if (_readOnly) labelColor = LabelNormal;
            else                             labelColor = LabelNormal;

            var labelRect = new Rectangle(0, 0, Width, LabelHeight);
            TextRenderer.DrawText(g, _labelText, Font, labelRect, labelColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);

            if (_required && Enabled)
            {
                int labelW = TextRenderer.MeasureText(g, _labelText, Font).Width;
                var starRect = new Rectangle(labelW + 2, 0, 12, LabelHeight);
                TextRenderer.DrawText(g, "*", Font, starRect, RequiredStar,
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
            using (var brush = new SolidBrush(inactive ? BgDisabled : BgNormal))
                g.FillPath(brush, path);

            if (Enabled && !_readOnly)
            {
                var borderColor = _hasFocus ? BorderFocus : BorderNormal;
                using (var path = RoundedRect(rect, BorderRadius))
                using (var pen  = new Pen(borderColor, _hasFocus ? 1.5f : 1f))
                    g.DrawPath(pen, path);
            }
            else if (_readOnly)
            {
                using (var path = RoundedRect(rect, BorderRadius))
                using (var pen  = new Pen(BorderNormal, 1f))
                    g.DrawPath(pen, path);
            }

            // Phone icon prefix
            var prefixRect = new Rectangle(InputPadH, 0, PrefixWidth, panel.Height);
            var prefixCol  = inactive ? LabelDisabled : PrefixColor;
            TextRenderer.DrawText(g, "☎", Font, prefixRect, prefixCol,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);

            // Placeholder
            if (string.IsNullOrEmpty(_input.Text) && !_hasFocus && Enabled)
            {
                int leftPad = InputPadH + PrefixWidth;
                var ph = new Rectangle(leftPad, 0, panel.Width - leftPad - InputPadH, panel.Height);
                TextRenderer.DrawText(g, "(0XX) XXX-XXXX", _input.Font, ph, PlaceholderColor,
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
