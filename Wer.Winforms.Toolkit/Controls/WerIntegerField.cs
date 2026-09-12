using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    [ToolboxItem(true)]
    [Description("Integer-only text field with label, required indicator, disabled and read-only states.")]
    [DefaultEvent("ValueChanged")]
    [DefaultProperty("LabelText")]
    public class WerIntegerField : Control
    {
        // ── Sub-controls ────────────────────────────────────────────
        private readonly TextBox _input;
        private readonly Panel   _inputBorder;

        // ── State ────────────────────────────────────────────────────
        private string _labelText    = "Integer Label";
        private string _placeholder  = "0";
        private bool   _required;
        private bool   _readOnly;
        private bool   _hasFocus;
        private bool   _allowNegative;
        private int?   _minValue;
        private int?   _maxValue;

        // ── Layout ───────────────────────────────────────────────────
        private const int LabelHeight  = 20;
        private const int LabelGap     = 4;
        private const int BorderRadius = 8;
        private const int InputPadH    = 10;

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

        public event EventHandler ValueChanged;

        public WerIntegerField()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.SupportsTransparentBackColor, true);

            BackColor = Color.Transparent;
            Font      = WerTheme.BodyFont;
            Size      = new Size(220, LabelHeight + LabelGap + 36);

            // Border panel
            _inputBorder = new Panel { BackColor = Color.Transparent };
            _inputBorder.Paint += OnBorderPaint;
            _inputBorder.Click += (s, ev) => { _input.Visible = true; _input.Focus(); };
            Controls.Add(_inputBorder);

            // Inner TextBox
            _input = new TextBox
            {
                BorderStyle = BorderStyle.None,
                BackColor   = BgNormal,
                ForeColor   = WerTheme.TextColor,
                Font        = WerTheme.BodyFont,
                Multiline   = false,
                TextAlign   = HorizontalAlignment.Left,
            };
            _input.KeyPress    += OnInputKeyPress;
            _input.KeyDown     += OnInputKeyDown;
            _input.TextChanged += OnInputTextChanged;
            _input.GotFocus    += (s, e) => { _hasFocus = true; _input.Visible = true; _inputBorder.Invalidate(); };
            _input.LostFocus   += (s, e) =>
            {
                _hasFocus = false;
                ClampValue();
                _input.Visible = !string.IsNullOrEmpty(_input.Text);
                _inputBorder.Invalidate();
            };
            _inputBorder.Controls.Add(_input);

            _input.Visible = false;
            LayoutInternals();
        }

        // ── Public properties ────────────────────────────────────────

        [Category("WerIntegerField")]
        [DefaultValue("Label")]
        [Description("Label displayed above the input.")]
        public string LabelText
        {
            get => _labelText;
            set { _labelText = value; Invalidate(); }
        }

        [Category("WerIntegerField")]
        [DefaultValue("")]
        [Description("Placeholder text shown when the field is empty.")]
        public string Placeholder
        {
            get => _placeholder;
            set { _placeholder = value; _inputBorder.Invalidate(); }
        }

        [Category("WerIntegerField")]
        [DefaultValue(false)]
        [Description("Show red asterisk and orange label when true.")]
        public bool Required
        {
            get => _required;
            set { _required = value; Invalidate(); }
        }

        [Category("WerIntegerField")]
        [DefaultValue(false)]
        [Description("Field is read-only: gray background, orange label, no editing.")]
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

        [Category("WerIntegerField")]
        [DefaultValue(false)]
        [Description("Allow negative integers.")]
        public bool AllowNegative
        {
            get => _allowNegative;
            set => _allowNegative = value;
        }

        [Category("WerIntegerField")]
        [DefaultValue(null)]
        [Description("Minimum allowed value. Null = no limit.")]
        public int? MinValue
        {
            get => _minValue;
            set => _minValue = value;
        }

        [Category("WerIntegerField")]
        [DefaultValue(null)]
        [Description("Maximum allowed value. Null = no limit.")]
        public int? MaxValue
        {
            get => _maxValue;
            set => _maxValue = value;
        }

        /// <summary>
        /// Returns the current value as int. Default is 0.
        /// No parsing needed — just use: int qty = werIntegerField1.Value;
        /// </summary>
        [Category("WerIntegerField")]
        [Browsable(false)]
        [Description("The current integer value. Returns 0 if empty or invalid. No parsing needed.")]
        public int Value
        {
            get => int.TryParse(_input.Text, out int v) ? v : 0;
            set
            {
                _input.Text = value != 0 ? value.ToString() : string.Empty;
                _input.Visible = !string.IsNullOrEmpty(_input.Text);
                _inputBorder.Invalidate();
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        [Browsable(true)]
        [Category("WerIntegerField")]
        [DefaultValue("")]
        public override string Text
        {
            get => _input.Text;
            set => _input.Text = value;
        }

        // ── Input filtering ──────────────────────────────────────────

        private void OnInputKeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow control chars (backspace, etc.)
            if (char.IsControl(e.KeyChar)) return;

            // Allow digits
            if (char.IsDigit(e.KeyChar)) return;

            // Allow minus sign at start if negative allowed
            if (_allowNegative && e.KeyChar == '-' && _input.SelectionStart == 0 && !_input.Text.Contains("-"))
                return;

            e.Handled = true; // block everything else
        }

        private void OnInputKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                if (Clipboard.ContainsText())
                {
                    if (!int.TryParse(Clipboard.GetText().Trim(), out _))
                        e.SuppressKeyPress = true;
                }
                else
                {
                    e.SuppressKeyPress = true;
                }
            }
        }

        private void OnInputTextChanged(object sender, EventArgs e)
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        private void ClampValue()
        {
            if (string.IsNullOrEmpty(_input.Text)) return;
            if (!int.TryParse(_input.Text, out int val)) return;

            bool clamped = false;
            if (_minValue.HasValue && val < _minValue.Value) { val = _minValue.Value; clamped = true; }
            if (_maxValue.HasValue && val > _maxValue.Value) { val = _maxValue.Value; clamped = true; }

            if (clamped)
            {
                _input.Text = val.ToString();
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
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

            int textH = _input.PreferredHeight;
            int textY = (borderH - textH) / 2;
            _input.SetBounds(InputPadH, Math.Max(0, textY), Width - InputPadH * 2, textH);
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
                var ph = new Rectangle(InputPadH, 0, panel.Width - InputPadH * 2, panel.Height);
                TextRenderer.DrawText(g, _placeholder, _input.Font, ph, PlaceholderColor,
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
