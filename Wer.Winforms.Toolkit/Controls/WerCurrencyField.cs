using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    [ToolboxItem(true)]
    [Description("Currency text field with label, required indicator, $ prefix, thousand separators, disabled and read-only states.")]
    [DefaultEvent("ValueChanged")]
    [DefaultProperty("LabelText")]
    public class WerCurrencyField : Control
    {
        // ── Sub-controls ────────────────────────────────────────────
        private readonly TextBox _input;
        private readonly Panel   _inputBorder;

        // ── State ────────────────────────────────────────────────────
        private string  _labelText   = "Currency Label";
        private string  _placeholder = "0.00";
        private bool    _required;
        private bool    _readOnly;
        private bool    _hasFocus;
        private string  _currencySymbol = "₱";
        private int     _decimalPlaces  = 2;
        private decimal? _minValue;
        private decimal? _maxValue;

        // ── Layout ───────────────────────────────────────────────────
        private int LabelHeight => Math.Max(20, (int)(Font.GetHeight() + 4));
        private const int LabelGap     = 4;
        private const int BorderRadius = 8;
        private const int InputPadH    = 10;
        private int PrefixWidth => TextRenderer.MeasureText(_currencySymbol, Font).Width + 2;

        // ── Colors (same as WerTextField) ────────────────────────────
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

        public event EventHandler ValueChanged;

        public WerCurrencyField()
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
            _input.TextChanged += (s, e) => ValueChanged?.Invoke(this, EventArgs.Empty);
            _input.GotFocus    += OnInputGotFocus;
            _input.LostFocus   += OnInputLostFocus;
            _inputBorder.Controls.Add(_input);

            LayoutInternals();

            // Start with input hidden so placeholder shows
            _input.Visible = false;
        }

        // ── Public properties ────────────────────────────────────────

        private bool _showLabel = true;

        [Category("WerCurrencyField")]
        [DefaultValue(true)]
        [Description("Show or hide the label above the input.")]
        public bool ShowLabel
        {
            get => _showLabel;
            set { _showLabel = value; LayoutInternals(); Invalidate(); }
        }

        [Category("WerCurrencyField")]
        [DefaultValue("Label")]
        [Description("Label displayed above the input.")]
        public string LabelText
        {
            get => _labelText;
            set { _labelText = value; Invalidate(); }
        }

        [Category("WerCurrencyField")]
        [DefaultValue("0.00")]
        [Description("Placeholder text shown when the field is empty.")]
        public string Placeholder
        {
            get => _placeholder;
            set { _placeholder = value; _inputBorder.Invalidate(); }
        }

        [Category("WerCurrencyField")]
        [DefaultValue(false)]
        [Description("Show red asterisk and orange label when true.")]
        public bool Required
        {
            get => _required;
            set { _required = value; Invalidate(); }
        }

        [Category("WerCurrencyField")]
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

        [Category("WerCurrencyField")]
        [DefaultValue("₱")]
        [Description("Currency symbol shown before the value.")]
        public string CurrencySymbol
        {
            get => _currencySymbol;
            set { _currencySymbol = value ?? "₱"; _inputBorder.Invalidate(); }
        }

        [Category("WerCurrencyField")]
        [DefaultValue(2)]
        [Description("Number of decimal places (0-4).")]
        public int DecimalPlaces
        {
            get => _decimalPlaces;
            set => _decimalPlaces = Math.Max(0, Math.Min(4, value));
        }

        [Category("WerCurrencyField")]
        [DefaultValue(null)]
        [Description("Minimum allowed value. Null = no limit.")]
        public decimal? MinValue
        {
            get => _minValue;
            set => _minValue = value;
        }

        [Category("WerCurrencyField")]
        [DefaultValue(null)]
        [Description("Maximum allowed value. Null = no limit.")]
        public decimal? MaxValue
        {
            get => _maxValue;
            set => _maxValue = value;
        }

        /// <summary>
        /// Returns the current value as decimal. Default is 0.
        /// No parsing needed — just use: decimal amount = werCurrencyField1.Value;
        /// </summary>
        [Category("WerCurrencyField")]
        [Browsable(false)]
        [Description("The current decimal value. Returns 0 if empty or invalid. No parsing needed.")]
        public decimal Value
        {
            get
            {
                var raw = _input.Text.Replace(",", "");
                return decimal.TryParse(raw, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal v)
                    ? v : 0m;
            }
            set
            {
                _input.Text = value != 0m ? FormatForDisplay(value) : string.Empty;
                _input.Visible = !string.IsNullOrEmpty(_input.Text);
                _inputBorder.Invalidate();
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        [Browsable(true)]
        [Category("WerCurrencyField")]
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
            if (char.IsDigit(e.KeyChar)) return;

            // Allow one decimal point
            if (e.KeyChar == '.' && _decimalPlaces > 0 && !_input.Text.Contains("."))
                return;

            // Allow minus at start
            if (e.KeyChar == '-' && _input.SelectionStart == 0 && !_input.Text.Contains("-"))
                return;

            e.Handled = true;
        }

        private void OnInputKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                if (Clipboard.ContainsText())
                {
                    var text = Clipboard.GetText().Replace(",", "").Trim();
                    if (!decimal.TryParse(text, System.Globalization.NumberStyles.Number,
                        System.Globalization.CultureInfo.InvariantCulture, out _))
                    {
                        e.SuppressKeyPress = true;
                    }
                }
                else
                {
                    e.SuppressKeyPress = true;
                }
            }
        }

        // ── Focus: strip formatting on enter, format on leave ────────

        private void OnInputGotFocus(object sender, EventArgs e)
        {
            _hasFocus = true;
            _input.Visible = true;
            _inputBorder.Invalidate();

            // Strip thousand separators for easier editing
            _input.Text = _input.Text.Replace(",", "");
        }

        private void OnInputLostFocus(object sender, EventArgs e)
        {
            _hasFocus = false;

            // Parse, clamp, and format with thousand separators
            var raw = _input.Text.Replace(",", "").Trim();
            if (!string.IsNullOrEmpty(raw) && decimal.TryParse(raw, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal val))
            {
                val = Math.Round(val, _decimalPlaces);
                if (_minValue.HasValue && val < _minValue.Value) val = _minValue.Value;
                if (_maxValue.HasValue && val > _maxValue.Value) val = _maxValue.Value;
                _input.Text = FormatForDisplay(val);
            }
            else
            {
                _input.Text = "";
            }

            // Hide TextBox when empty so painted placeholder shows through
            _input.Visible = !string.IsNullOrEmpty(_input.Text);
            _inputBorder.Invalidate();
        }

        private string FormatForDisplay(decimal val)
        {
            return val.ToString("N" + _decimalPlaces, CultureInfo.InvariantCulture);
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

            // TextBox offset to the right of the currency symbol
            int textH = _input.PreferredHeight;
            int textY = (borderH - textH) / 2;
            int leftPad = InputPadH + PrefixWidth;
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

            // Currency symbol prefix
            var prefixRect = new Rectangle(InputPadH, 0, PrefixWidth, panel.Height);
            var prefixCol  = inactive ? LabelDisabled : PrefixColor;
            TextRenderer.DrawText(g, _currencySymbol, Font, prefixRect, prefixCol,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);

            // Placeholder
            if (string.IsNullOrEmpty(_input.Text) && !_hasFocus && Enabled && !string.IsNullOrEmpty(_placeholder))
            {
                int leftPad = InputPadH + PrefixWidth;
                var ph = new Rectangle(leftPad, 0, panel.Width - leftPad - InputPadH, panel.Height);
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
