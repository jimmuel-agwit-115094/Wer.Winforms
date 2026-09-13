using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    [ToolboxItem(true)]
    [Description("Text field with label, required indicator, password mode, disabled and read-only states.")]
    [DefaultEvent("TextChanged")]
    [DefaultProperty("LabelText")]
    public class WerTextField : Control
    {
        // ── Sub-controls ────────────────────────────────────────────
        private readonly TextBox _input;
        private readonly Panel   _inputBorder;   // custom-painted border panel

        // ── State ────────────────────────────────────────────────────
        private string _labelText = "TextField Label";
        private bool   _required;
        private bool   _readOnly;
        private char   _passwordChar = '\0';

        // ── Layout ───────────────────────────────────────────────────
        private int LabelHeight => Math.Max(20, (int)(Font.GetHeight() + 4));
        private const int LabelGap     = 4;
        private const int BorderRadius = 8;
        private const int InputPadH    = 8;
        private const int InputPadV    = 6;

        // ── Colors ───────────────────────────────────────────────────
        private static readonly Color LabelNormal   = Color.Black;
        private static readonly Color LabelRequired = Color.FromArgb(200, 100, 20);   // orange
        private static readonly Color LabelDisabled = Color.FromArgb(150, 150, 150);  // muted
        private static readonly Color RequiredStar  = Color.FromArgb(210, 50, 50);    // red *
        private static readonly Color BorderNormal  = Color.FromArgb(200, 210, 220);
        private static readonly Color BorderFocus   = Color.FromArgb(12, 124, 146);
        private static readonly Color BgNormal      = Color.White;
        private static readonly Color BgDisabled    = Color.FromArgb(242, 242, 242);

        private bool _hasFocus;

        public WerTextField()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.SupportsTransparentBackColor, true);

            BackColor = Color.Transparent;
            Font      = WerTheme.BodyFont;
            Size      = new Size(300, LabelHeight + LabelGap + 36);

            // Border panel — draws the rounded rectangle
            _inputBorder = new Panel { BackColor = Color.Transparent };
            _inputBorder.Paint += OnBorderPaint;
            Controls.Add(_inputBorder);

            // Inner TextBox — sits inside the border panel
            _input = new TextBox
            {
                BorderStyle = BorderStyle.None,
                BackColor   = BgNormal,
                ForeColor   = WerTheme.TextColor,
                Font        = WerTheme.BodyFont,
                Multiline   = false,
            };
            _input.TextChanged  += (s, e) => { OnTextChanged(e); };
            _input.GotFocus     += (s, e) => { _hasFocus = true;  _inputBorder.Invalidate(); };
            _input.LostFocus    += (s, e) => { _hasFocus = false; _inputBorder.Invalidate(); };
            _inputBorder.Controls.Add(_input);

            LayoutInternals();
        }

        // ── Public properties ────────────────────────────────────────

        private bool _showLabel = true;

        [Category("WerTextField")]
        [DefaultValue(true)]
        [Description("Show or hide the label above the input.")]
        public bool ShowLabel
        {
            get => _showLabel;
            set { _showLabel = value; LayoutInternals(); Invalidate(); }
        }

        [Category("WerTextField")]
        [DefaultValue("Label")]
        [Description("Label displayed above the input.")]
        public string LabelText
        {
            get => _labelText;
            set { _labelText = value; Invalidate(); }
        }

        [Category("WerTextField")]
        [DefaultValue(false)]
        [Description("Show red asterisk and orange label when true.")]
        public bool Required
        {
            get => _required;
            set { _required = value; Invalidate(); }
        }

        [Category("WerTextField")]
        [DefaultValue(false)]
        [Description("Field is read-only: gray background, orange label, no editing.")]
        public bool ReadOnly
        {
            get => _readOnly;
            set
            {
                _readOnly         = value;
                _input.ReadOnly   = value;
                ApplyVisualState();
                Invalidate();
                _inputBorder.Invalidate();
            }
        }

        [Category("WerTextField")]
        [DefaultValue('\0')]
        [Description("Set to e.g. '*' to mask input as a password field.")]
        public char PasswordChar
        {
            get => _passwordChar;
            set { _passwordChar = value; _input.PasswordChar = value; }
        }

        [Browsable(true)]
        [Category("WerTextField")]
        [DefaultValue("")]
        public override string Text
        {
            get => _input.Text;
            set => _input.Text = value;
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
            _input.BackColor  = inactive ? BgDisabled : BgNormal;
            _input.ForeColor  = !Enabled ? LabelDisabled : WerTheme.TextColor;
            _input.ReadOnly   = !Enabled || _readOnly;
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

            // TextBox inside with padding
            int textH = _input.PreferredHeight;
            int textY = (borderH - textH) / 2;
            _input.SetBounds(InputPadH, Math.Max(0, textY), Width - InputPadH * 2, textH);
        }

        // ── Paint label ──────────────────────────────────────────────

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            if (!_showLabel) return;

            // Choose label color
            Color labelColor;
            if (!Enabled)       labelColor = LabelDisabled;
            else if (_readOnly) labelColor = LabelRequired;
            else                labelColor = LabelNormal;

            var labelRect = new Rectangle(0, 0, Width, LabelHeight);

            // Draw label text
            TextRenderer.DrawText(g, _labelText, Font, labelRect, labelColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);

            // Draw red asterisk if required
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

            // Background fill
            using (var path = RoundedRect(rect, BorderRadius))
            using (var brush = new SolidBrush(inactive ? BgDisabled : BgNormal))
                g.FillPath(brush, path);

            // Border — no border when disabled, normal/focus otherwise
            if (Enabled && !_readOnly)
            {
                var borderColor = _hasFocus ? BorderFocus : BorderNormal;
                using (var path = RoundedRect(rect, BorderRadius))
                using (var pen  = new Pen(borderColor, _hasFocus ? 1.5f : 1f))
                    g.DrawPath(pen, path);
            }
            else if (_readOnly)
            {
                // Readonly: subtle border
                using (var path = RoundedRect(rect, BorderRadius))
                using (var pen  = new Pen(BorderNormal, 1f))
                    g.DrawPath(pen, path);
            }
            // Disabled: no border at all — just flat gray fill
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
