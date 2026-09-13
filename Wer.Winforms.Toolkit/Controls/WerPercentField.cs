using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    [ToolboxItem(true)]
    [Description("Percentage text field with label, % suffix, required indicator, disabled and read-only states.")]
    [DefaultEvent("ValueChanged")]
    [DefaultProperty("LabelText")]
    public class WerPercentField : Control
    {
        private readonly TextBox _input;
        private readonly Panel   _inputBorder;

        private string  _labelText     = "Percent Label";
        private string  _placeholder   = "0.00";
        private bool    _required;
        private bool    _readOnly;
        private bool    _hasFocus;
        private int     _decimalPlaces = 2;
        private decimal? _minValue     = 0;
        private decimal? _maxValue     = 100;

        private int LabelHeight => Math.Max(20, (int)(Font.GetHeight() + 4));
        private const int LabelGap     = 4;
        private const int BorderRadius = 8;
        private const int InputPadH    = 10;
        private const int SuffixWidth  = 20;

        private static readonly Color LabelNormal      = Color.Black;
        private static readonly Color LabelRequired    = Color.FromArgb(200, 100, 20);
        private static readonly Color LabelDisabled    = Color.FromArgb(150, 150, 150);
        private static readonly Color RequiredStar     = Color.FromArgb(210, 50, 50);
        private static readonly Color BorderNormal     = Color.FromArgb(200, 210, 220);
        private static readonly Color BorderFocus      = Color.FromArgb(12, 124, 146);
        private static readonly Color PlaceholderColor = Color.FromArgb(160, 170, 180);
        private static readonly Color SuffixColor      = Color.FromArgb(108, 117, 125);
        private static readonly Color BgNormal         = Color.White;
        private static readonly Color BgDisabled       = Color.FromArgb(242, 242, 242);

        public event EventHandler ValueChanged;

        public WerPercentField()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.SupportsTransparentBackColor, true);

            BackColor = Color.Transparent;
            Font      = WerTheme.BodyFont;
            Size      = new Size(220, LabelHeight + LabelGap + 36);

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
                TextAlign   = HorizontalAlignment.Left,
            };
            _input.KeyPress    += OnInputKeyPress;
            _input.KeyDown     += OnInputKeyDown;
            _input.TextChanged += (s, e) => ValueChanged?.Invoke(this, EventArgs.Empty);
            _input.GotFocus    += OnInputGotFocus;
            _input.LostFocus   += OnInputLostFocus;
            _inputBorder.Controls.Add(_input);

            LayoutInternals();
        }

        // ── Public properties ────────────────────────────────────────

        private bool _showLabel = true;

        [Category("WerPercentField")]
        [DefaultValue(true)]
        [Description("Show or hide the label above the input.")]
        public bool ShowLabel
        {
            get => _showLabel;
            set { _showLabel = value; LayoutInternals(); Invalidate(); }
        }

        [Category("WerPercentField")]
        [DefaultValue("Percent Label")]
        public string LabelText
        {
            get => _labelText;
            set { _labelText = value; Invalidate(); }
        }

        [Category("WerPercentField")]
        [DefaultValue("0.00")]
        public string Placeholder
        {
            get => _placeholder;
            set { _placeholder = value; _inputBorder.Invalidate(); }
        }

        [Category("WerPercentField")]
        [DefaultValue(false)]
        public bool Required
        {
            get => _required;
            set { _required = value; Invalidate(); }
        }

        [Category("WerPercentField")]
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

        [Category("WerPercentField")]
        [DefaultValue(2)]
        public int DecimalPlaces
        {
            get => _decimalPlaces;
            set => _decimalPlaces = Math.Max(0, Math.Min(4, value));
        }

        [Category("WerPercentField")]
        [DefaultValue(typeof(decimal), "0")]
        public decimal? MinValue
        {
            get => _minValue;
            set => _minValue = value;
        }

        [Category("WerPercentField")]
        [DefaultValue(typeof(decimal), "100")]
        public decimal? MaxValue
        {
            get => _maxValue;
            set => _maxValue = value;
        }

        [Category("WerPercentField")]
        [Browsable(false)]
        /// <summary>
        /// Returns the current value as decimal. Default is 0.
        /// No parsing needed — just use: decimal pct = werPercentField1.Value;
        /// </summary>
        [Description("The current percentage value. Returns 0 if empty or invalid. No parsing needed.")]
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
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        [Browsable(true)]
        [Category("WerPercentField")]
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

            // Allow decimal point (once)
            if (e.KeyChar == '.' && _decimalPlaces > 0 && !_input.Text.Contains(".")) return;

            if (!char.IsDigit(e.KeyChar)) { e.Handled = true; return; }

            // Build what the text would look like after this keystroke
            string current = _input.Text;
            int selStart = _input.SelectionStart;
            int selLen   = _input.SelectionLength;
            string proposed = current.Substring(0, selStart)
                            + e.KeyChar
                            + current.Substring(selStart + selLen);

            // Block if too many decimal places
            int dotIdx = proposed.IndexOf('.');
            if (dotIdx >= 0 && proposed.Length - dotIdx - 1 > _decimalPlaces)
            {
                e.Handled = true;
                return;
            }

            // Block if value would exceed max
            if (decimal.TryParse(proposed, System.Globalization.NumberStyles.Number,
                System.Globalization.CultureInfo.InvariantCulture, out decimal val))
            {
                if (_maxValue.HasValue && val > _maxValue.Value)
                {
                    e.Handled = true;
                    return;
                }
            }
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

        private void OnInputGotFocus(object sender, EventArgs e)
        {
            _hasFocus = true;
            _inputBorder.Invalidate();
        }

        private void OnInputLostFocus(object sender, EventArgs e)
        {
            _hasFocus = false;
            _inputBorder.Invalidate();

            if (decimal.TryParse(_input.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal val))
            {
                val = Math.Round(val, _decimalPlaces);
                if (_minValue.HasValue && val < _minValue.Value) val = _minValue.Value;
                if (_maxValue.HasValue && val > _maxValue.Value) val = _maxValue.Value;
                _input.Text = FormatForDisplay(val);
            }
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

            int textH = _input.PreferredHeight;
            int textY = (borderH - textH) / 2;
            _input.SetBounds(InputPadH, Math.Max(0, textY), Width - InputPadH - SuffixWidth - InputPadH, textH);
        }

        // ── Paint label ──────────────────────────────────────────────

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            if (!_showLabel) return;

            Color labelColor;
            if (!Enabled)        labelColor = LabelDisabled;
            else if (_readOnly)  labelColor = LabelRequired;
            else                 labelColor = LabelNormal;

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

            // % suffix
            var suffixRect = new Rectangle(panel.Width - SuffixWidth - InputPadH, 0, SuffixWidth, panel.Height);
            var suffixCol  = inactive ? LabelDisabled : SuffixColor;
            TextRenderer.DrawText(g, "%", Font, suffixRect, suffixCol,
                TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);

            // Placeholder
            if (string.IsNullOrEmpty(_input.Text) && !_hasFocus && Enabled && !string.IsNullOrEmpty(_placeholder))
            {
                var ph = new Rectangle(InputPadH, 0, panel.Width - InputPadH - SuffixWidth - InputPadH, panel.Height);
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
