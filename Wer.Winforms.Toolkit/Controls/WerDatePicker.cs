using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    [ToolboxItem(true)]
    [Description("Date picker with label, calendar dropdown, required indicator, disabled and read-only states.")]
    [DefaultEvent("ValueChanged")]
    [DefaultProperty("LabelText")]
    public class WerDatePicker : Control
    {
        // ── Sub-controls ────────────────────────────────────────────
        private readonly DateTimePicker _dtp;       // hidden — provides calendar popup
        private readonly Panel         _inputBorder;

        // ── State ─────────────────────────────────────────────────
        private string  _labelText  = "DatePicker Label";
        private bool    _required;
        private bool    _readOnly;
        private bool    _hasFocus;
        private bool    _hasValue;   // false = show placeholder

        // ── Layout constants ──────────────────────────────────────
        private int LabelHeight => Math.Max(20, (int)(Font.GetHeight() + 4));
        private const int LabelGap     = 4;
        private const int BorderRadius = 8;
        private const int InputPadH    = 10;
        private const int DropBtnW     = 28;

        // ── Colors (same as WerTextField) ─────────────────────────
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

        public WerDatePicker()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.SupportsTransparentBackColor, true);

            BackColor = Color.Transparent;
            Font      = WerTheme.BodyFont;
            Size      = new Size(350, 60);

            // Border panel — we paint everything ourselves
            _inputBorder = new Panel { BackColor = Color.Transparent, Cursor = Cursors.Hand };
            _inputBorder.Paint      += OnBorderPaint;
            _inputBorder.MouseClick += OnInputAreaClick;
            Controls.Add(_inputBorder);

            // Hidden DateTimePicker — only used for its calendar popup
            _dtp = new DateTimePicker
            {
                Format       = DateTimePickerFormat.Custom,
                CustomFormat = "MM/dd/yyyy",
                Font         = WerTheme.BodyFont,
                CalendarFont = WerTheme.BodyFont,
                ShowCheckBox = false,
                ShowUpDown   = false,
                Visible      = false,        // hidden — we draw the text ourselves
            };

            _dtp.ValueChanged += (s, e) =>
            {
                _hasValue = true;
                _inputBorder.Invalidate();
                ValueChanged?.Invoke(this, EventArgs.Empty);
            };
            _dtp.CloseUp += (s, e) =>
            {
                _hasFocus = false;
                _inputBorder.Invalidate();
            };

            Controls.Add(_dtp);
            LayoutInternals();
        }

        // ── Public properties ─────────────────────────────────────

        private bool _showLabel = true;

        [Category("WerDatePicker")]
        [DefaultValue(true)]
        [Description("Show or hide the label above the input.")]
        public bool ShowLabel
        {
            get => _showLabel;
            set { _showLabel = value; LayoutInternals(); Invalidate(); }
        }

        [Category("WerDatePicker")]
        [DefaultValue("Label")]
        [Description("Label displayed above the date input.")]
        public string LabelText
        {
            get => _labelText;
            set { _labelText = value; Invalidate(); }
        }

        [Category("WerDatePicker")]
        [DefaultValue(false)]
        [Description("Show red asterisk and orange label.")]
        public bool Required
        {
            get => _required;
            set { _required = value; Invalidate(); }
        }

        [Category("WerDatePicker")]
        [DefaultValue(false)]
        [Description("Field is read-only: disabled appearance, orange label, no editing.")]
        public bool ReadOnly
        {
            get => _readOnly;
            set
            {
                _readOnly = value;
                Invalidate();
                _inputBorder.Invalidate();
            }
        }

        [Category("WerDatePicker")]
        [Browsable(false)]
        public DateTime? Value
        {
            get => _hasValue ? (DateTime?)_dtp.Value : null;
            set
            {
                if (value.HasValue)
                {
                    _dtp.Value = value.Value;
                    _hasValue  = true;
                }
                else
                {
                    _hasValue = false;
                }
                _inputBorder.Invalidate();
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        // ── Enable/disable ────────────────────────────────────────

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
            _inputBorder.Invalidate();
        }

        // ── Click → open calendar ─────────────────────────────────

        private void OnInputAreaClick(object sender, MouseEventArgs e)
        {
            if (!Enabled || _readOnly) return;

            _hasFocus = true;
            _inputBorder.Invalidate();

            // Programmatically open the DTP dropdown
            SendMessage(_dtp.Handle, WM_SYSKEYDOWN, (IntPtr)Keys.Down, IntPtr.Zero);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
        private const int WM_SYSKEYDOWN = 0x0104;

        // ── Layout ────────────────────────────────────────────────

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            LayoutInternals();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            if (_dtp == null) return;
            _dtp.Font         = Font;
            _dtp.CalendarFont = Font;
            LayoutInternals();
        }

        private void LayoutInternals()
        {
            if (_inputBorder == null || _dtp == null) return;

            int borderTop = _showLabel ? LabelHeight + LabelGap : 0;
            int borderH   = Height - borderTop;
            _inputBorder.SetBounds(0, borderTop, Width, borderH);

            // DTP is hidden but needs a size for the calendar popup positioning
            _dtp.SetBounds(0, borderTop, Width, borderH);
        }

        // ── Paint label ───────────────────────────────────────────

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

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
                int lw = TextRenderer.MeasureText(g, _labelText, Font).Width;
                TextRenderer.DrawText(g, "*", Font, new Rectangle(lw + 2, 0, 12, LabelHeight),
                    RequiredStar, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            }
        }

        // ── Paint input area ──────────────────────────────────────

        private void OnBorderPaint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode     = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

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

            // Text: date value or placeholder
            var textRect = new Rectangle(InputPadH, 0, panel.Width - InputPadH - DropBtnW, panel.Height);

            if (_hasValue)
            {
                var textColor = !Enabled ? LabelDisabled : WerTheme.TextColor;
                TextRenderer.DrawText(g, _dtp.Value.ToString("MMM. dd, yyyy"), Font, textRect, textColor,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
            }
            else
            {
                TextRenderer.DrawText(g, "MM/DD/YYYY", Font, textRect, PlaceholderColor,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
            }

            // Dropdown chevron ▾
            if (Enabled && !_readOnly)
            {
                var chevronRect = new Rectangle(panel.Width - DropBtnW, 0, DropBtnW, panel.Height);
                var chevronColor = _hasFocus ? BorderFocus : Color.FromArgb(140, 150, 160);
                TextRenderer.DrawText(g, "▾", Font, chevronRect, chevronColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
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
