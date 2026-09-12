using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    [ToolboxItem(true)]
    [Description("Multi-line rich text field with label, required indicator, disabled and read-only states.")]
    [DefaultEvent("TextChanged")]
    [DefaultProperty("LabelText")]
    public class WerRichTextField : Control
    {
        // ── Sub-controls ────────────────────────────────────────────
        private readonly RichTextBox _input;
        private readonly Panel      _inputBorder;

        // ── State ────────────────────────────────────────────────────
        private string _labelText   = "RichText Label";
        private string _placeholder = string.Empty;
        private bool   _required;
        private bool   _readOnly;
        private bool   _hasFocus;

        // ── Layout ───────────────────────────────────────────────────
        private const int LabelHeight  = 20;
        private const int LabelGap     = 4;
        private const int BorderRadius = 8;
        private const int InputPadH    = 10;
        private const int InputPadV    = 6;

        // ── Colors (same as WerTextField) ────────────────────────────
        private static readonly Color LabelNormal      = Color.Black;
        private static readonly Color LabelRequired    = Color.FromArgb(200, 100, 20);
        private static readonly Color LabelDisabled    = Color.FromArgb(150, 150, 150);
        private static readonly Color RequiredStar     = Color.FromArgb(210, 50, 50);
        private static readonly Color BorderNormal     = Color.FromArgb(200, 210, 220);
        private static readonly Color BorderFocus      = Color.FromArgb(12, 124, 146);
        private static readonly Color PlaceholderColor = Color.FromArgb(160, 170, 180);
        private static readonly Color BgNormal         = Color.White;
        private static readonly Color BgDisabled       = Color.FromArgb(242, 242, 242);

        public WerRichTextField()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.SupportsTransparentBackColor, true);

            BackColor = Color.Transparent;
            Font      = WerTheme.BodyFont;
            Size      = new Size(300, LabelHeight + LabelGap + 120);

            // Border panel
            _inputBorder = new Panel { BackColor = Color.Transparent };
            _inputBorder.Paint += OnBorderPaint;
            Controls.Add(_inputBorder);

            // Inner RichTextBox
            _input = new RichTextBox
            {
                BorderStyle  = BorderStyle.None,
                BackColor    = BgNormal,
                ForeColor    = WerTheme.TextColor,
                Font         = WerTheme.BodyFont,
                Multiline    = true,
                ScrollBars   = RichTextBoxScrollBars.Vertical,
                WordWrap     = true,
                DetectUrls   = false,
            };
            _input.TextChanged  += (s, e) => { OnTextChanged(e); _inputBorder.Invalidate(); };
            _input.GotFocus     += (s, e) => { _hasFocus = true;  _inputBorder.Invalidate(); };
            _input.LostFocus    += (s, e) => { _hasFocus = false; _inputBorder.Invalidate(); };
            _inputBorder.Controls.Add(_input);

            LayoutInternals();
        }

        // ── Public properties ────────────────────────────────────────

        [Category("WerRichTextField")]
        [DefaultValue("Label")]
        [Description("Label displayed above the input.")]
        public string LabelText
        {
            get => _labelText;
            set { _labelText = value; Invalidate(); }
        }

        [Category("WerRichTextField")]
        [DefaultValue("")]
        [Description("Placeholder text shown when the field is empty.")]
        public string Placeholder
        {
            get => _placeholder;
            set { _placeholder = value; _inputBorder.Invalidate(); }
        }

        [Category("WerRichTextField")]
        [DefaultValue(false)]
        [Description("Show red asterisk and orange label when true.")]
        public bool Required
        {
            get => _required;
            set { _required = value; Invalidate(); }
        }

        [Category("WerRichTextField")]
        [DefaultValue(false)]
        [Description("Field is read-only: gray background, orange label, no editing.")]
        public bool ReadOnly
        {
            get => _readOnly;
            set
            {
                _readOnly        = value;
                _input.ReadOnly  = value;
                ApplyVisualState();
                Invalidate();
                _inputBorder.Invalidate();
            }
        }

        [Browsable(true)]
        [Category("WerRichTextField")]
        [DefaultValue("")]
        public override string Text
        {
            get => _input.Text;
            set => _input.Text = value;
        }

        [Browsable(false)]
        [Description("The rich text (RTF) content.")]
        public string Rtf
        {
            get => _input.Rtf;
            set => _input.Rtf = value;
        }

        [Category("WerRichTextField")]
        [DefaultValue(true)]
        [Description("Enable word wrapping.")]
        public bool WordWrap
        {
            get => _input.WordWrap;
            set => _input.WordWrap = value;
        }

        [Category("WerRichTextField")]
        [DefaultValue(0)]
        [Description("Maximum character count. 0 = unlimited.")]
        public int MaxLength
        {
            get => _input.MaxLength;
            set => _input.MaxLength = value;
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

            int borderTop = LabelHeight + LabelGap;
            int borderH   = Height - borderTop;
            _inputBorder.SetBounds(0, borderTop, Width, borderH);

            _input.SetBounds(InputPadH, InputPadV, Width - InputPadH * 2, borderH - InputPadV * 2);
        }

        // ── Paint label ──────────────────────────────────────────────

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            Color labelColor;
            if (!Enabled)                    labelColor = LabelDisabled;
            else if (_readOnly) labelColor = LabelRequired;
            else                             labelColor = LabelNormal;

            var labelRect = new Rectangle(0, 0, Width, LabelHeight);
            TextRenderer.DrawText(g, _labelText, Font, labelRect, labelColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);

            if (_required && Enabled)
            {
                int labelW   = TextRenderer.MeasureText(g, _labelText, Font).Width;
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

            // Border
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

            // Placeholder
            if (string.IsNullOrEmpty(_input.Text) && !_hasFocus && Enabled && !string.IsNullOrEmpty(_placeholder))
            {
                var ph = new Rectangle(InputPadH, InputPadV, panel.Width - InputPadH * 2, panel.Height - InputPadV * 2);
                TextRenderer.DrawText(g, _placeholder, _input.Font, ph, PlaceholderColor,
                    TextFormatFlags.Top | TextFormatFlags.Left | TextFormatFlags.WordBreak);
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
