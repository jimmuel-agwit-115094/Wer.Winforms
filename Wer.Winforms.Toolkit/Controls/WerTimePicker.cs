using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    /// <summary>
    /// Modern time picker built from three styled dropdown fields: [HH ▾] : [MM ▾]  AM PM
    /// Matches WerComboBox/WerDatePicker visual style. No native DateTimePicker used.
    /// Usage: TimeSpan? time = werTimePicker1.Value;
    /// </summary>
    [ToolboxItem(true)]
    [Description("Modern time picker with hour/minute dropdowns and AM/PM toggle.")]
    [DefaultEvent("ValueChanged")]
    [DefaultProperty("LabelText")]
    public class WerTimePicker : Control
    {
        // ── Hidden native combos (dropdown only) ────────────────
        private readonly ComboBox _hourCombo;
        private readonly ComboBox _minuteCombo;

        // ── Painted panels ──────────────────────────────────────
        private readonly Panel _hourBorder;
        private readonly Panel _minuteBorder;
        private readonly Panel _amBtn;
        private readonly Panel _pmBtn;

        // ── State ───────────────────────────────────────────────
        private string _labelText = "TimePicker Label";
        private bool   _required;
        private bool   _readOnly;
        private bool   _hourFocus;
        private bool   _minuteFocus;
        private bool   _isAm = true;

        // ── Layout ──────────────────────────────────────────────
        private int LabelHeight => Math.Max(20, (int)(Font.GetHeight() + 4));
        private const int LabelGap      = 4;
        private const int BorderRadius  = 8;
        private const int InputPadH     = 8;
        private const int ChevronW      = 22;
        private const int DropW         = 70;   // each dropdown width
        private const int ColonW        = 16;   // colon separator
        private const int AmPmW         = 36;   // AM/PM button width
        private const int AmPmH         = 16;   // AM/PM button height
        private const int AmPmGap       = 4;
        private const int Gap           = 6;    // between elements

        // ── Colors (same as WerComboBox) ────────────────────────
        private static readonly Color LabelNormal      = Color.Black;
        private static readonly Color LabelRequired    = Color.FromArgb(200, 100, 20);
        private static readonly Color LabelDisabled    = Color.FromArgb(150, 150, 150);
        private static readonly Color RequiredStar     = Color.FromArgb(210, 50, 50);
        private static readonly Color BorderNormal     = Color.FromArgb(200, 210, 220);
        private static readonly Color BorderFocus      = Color.FromArgb(12, 124, 146);
        private static readonly Color PlaceholderColor = Color.FromArgb(160, 170, 180);
        private static readonly Color BgNormal         = Color.White;
        private static readonly Color BgDisabled       = Color.FromArgb(242, 242, 242);
        private static readonly Color AmPmActive       = Color.FromArgb(12, 124, 146);
        private static readonly Color AmPmInactive     = Color.FromArgb(245, 247, 249);

        public event EventHandler ValueChanged;

        public WerTimePicker()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.SupportsTransparentBackColor, true);

            BackColor = Color.Transparent;
            Font      = WerTheme.BodyFont;
            Size      = new Size(220, LabelHeight + LabelGap + 36);

            // --- Hour combo (hidden, behind painted panel) ---
            _hourCombo = CreateCombo();
            for (int h = 1; h <= 12; h++) _hourCombo.Items.Add(h.ToString("D2"));
            _hourCombo.SelectedIndex = 0;
            _hourCombo.SelectedIndexChanged += (s, e) => { _hourBorder.Invalidate(); FireChanged(); };
            _hourCombo.DropDown      += (s, e) => { _hourFocus = true; _hourBorder.Invalidate(); };
            _hourCombo.DropDownClosed += (s, e) => { _hourFocus = false; _hourBorder.Invalidate(); };
            Controls.Add(_hourCombo);

            // --- Minute combo (hidden) ---
            _minuteCombo = CreateCombo();
            for (int m = 0; m < 60; m++) _minuteCombo.Items.Add(m.ToString("D2"));
            _minuteCombo.SelectedIndex = 0;
            _minuteCombo.SelectedIndexChanged += (s, e) => { _minuteBorder.Invalidate(); FireChanged(); };
            _minuteCombo.DropDown      += (s, e) => { _minuteFocus = true; _minuteBorder.Invalidate(); };
            _minuteCombo.DropDownClosed += (s, e) => { _minuteFocus = false; _minuteBorder.Invalidate(); };
            Controls.Add(_minuteCombo);

            // --- Hour painted face ---
            _hourBorder = CreateBorderPanel();
            _hourBorder.Paint += (s, e) => PaintDropdown(e.Graphics, (Panel)s, _hourCombo, _hourFocus);
            _hourBorder.MouseClick += (s, e) => OpenCombo(_hourCombo);
            Controls.Add(_hourBorder);

            // --- Minute painted face ---
            _minuteBorder = CreateBorderPanel();
            _minuteBorder.Paint += (s, e) => PaintDropdown(e.Graphics, (Panel)s, _minuteCombo, _minuteFocus);
            _minuteBorder.MouseClick += (s, e) => OpenCombo(_minuteCombo);
            Controls.Add(_minuteBorder);

            // --- AM button ---
            _amBtn = CreateAmPmPanel();
            _amBtn.Paint += (s, e) => PaintAmPm(e.Graphics, (Panel)s, "AM", _isAm);
            _amBtn.MouseClick += (s, e) => { if (Enabled && !_readOnly && !_isAm) { _isAm = true; _amBtn.Invalidate(); _pmBtn.Invalidate(); FireChanged(); } };
            Controls.Add(_amBtn);

            // --- PM button ---
            _pmBtn = CreateAmPmPanel();
            _pmBtn.Paint += (s, e) => PaintAmPm(e.Graphics, (Panel)s, "PM", !_isAm);
            _pmBtn.MouseClick += (s, e) => { if (Enabled && !_readOnly && _isAm) { _isAm = false; _amBtn.Invalidate(); _pmBtn.Invalidate(); FireChanged(); } };
            Controls.Add(_pmBtn);

            // Bring painted panels to front
            _hourBorder.BringToFront();
            _minuteBorder.BringToFront();
            _amBtn.BringToFront();
            _pmBtn.BringToFront();

            LayoutInternals();
        }

        private ComboBox CreateCombo()
        {
            var combo = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font          = WerTheme.BodyFont,
                FlatStyle     = FlatStyle.Standard,
                DrawMode      = DrawMode.OwnerDrawFixed,
                ItemHeight    = 26,
                TabStop       = false,
            };
            combo.DrawItem += OnComboDrawItem;
            return combo;
        }

        private Panel CreateBorderPanel()
        {
            return new Panel { BackColor = Color.Transparent, Cursor = Cursors.Hand };
        }

        private Panel CreateAmPmPanel()
        {
            return new Panel { BackColor = Color.Transparent, Cursor = Cursors.Hand };
        }

        private void OpenCombo(ComboBox combo)
        {
            if (!Enabled || _readOnly) return;
            combo.Focus();
            combo.DroppedDown = true;
        }

        private void FireChanged()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        // ── Properties ──────────────────────────────────────────

        private bool _showLabel = true;

        [Category("WerTimePicker")]
        [DefaultValue(true)]
        [Description("Show or hide the label above the input.")]
        public bool ShowLabel
        {
            get => _showLabel;
            set { _showLabel = value; LayoutInternals(); Invalidate(); }
        }

        [Category("WerTimePicker")]
        [DefaultValue("TimePicker Label")]
        public string LabelText
        {
            get => _labelText;
            set { _labelText = value; Invalidate(); }
        }

        [Category("WerTimePicker")]
        [DefaultValue(false)]
        public bool Required
        {
            get => _required;
            set { _required = value; Invalidate(); }
        }

        [Category("WerTimePicker")]
        [DefaultValue(false)]
        public bool ReadOnly
        {
            get => _readOnly;
            set { _readOnly = value; Invalidate(); InvalidateAll(); }
        }

        /// <summary>
        /// The current time as TimeSpan, or null if not set.
        /// Usage: TimeSpan? time = werTimePicker1.Value;
        /// </summary>
        [Category("WerTimePicker")]
        [Browsable(false)]
        public TimeSpan? Value
        {
            get
            {
                int h12 = _hourCombo.SelectedIndex + 1;  // 1-12
                int m = _minuteCombo.SelectedIndex;       // 0-59
                int h24 = _isAm ? (h12 == 12 ? 0 : h12) : (h12 == 12 ? 12 : h12 + 12);
                return new TimeSpan(h24, m, 0);
            }
            set
            {
                if (value.HasValue)
                {
                    var ts = value.Value;
                    int h24 = ts.Hours;
                    _isAm = h24 < 12;
                    int h12 = h24 % 12;
                    if (h12 == 0) h12 = 12;
                    _hourCombo.SelectedIndex = h12 - 1;
                    _minuteCombo.SelectedIndex = ts.Minutes;
                }
                else
                {
                    _hourCombo.SelectedIndex = 0;
                    _minuteCombo.SelectedIndex = 0;
                    _isAm = true;
                }
                InvalidateAll();
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void InvalidateAll()
        {
            _hourBorder?.Invalidate();
            _minuteBorder?.Invalidate();
            _amBtn?.Invalidate();
            _pmBtn?.Invalidate();
        }

        // ── Enable/disable ──────────────────────────────────────

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            _hourCombo.Enabled = Enabled;
            _minuteCombo.Enabled = Enabled;
            Invalidate();
            InvalidateAll();
        }

        // ── Layout ──────────────────────────────────────────────

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            LayoutInternals();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            if (_hourCombo == null) return;
            _hourCombo.Font = Font;
            _minuteCombo.Font = Font;
            LayoutInternals();
        }

        private void LayoutInternals()
        {
            if (_hourBorder == null) return;

            int borderTop = _showLabel ? LabelHeight + LabelGap : 0;
            int borderH   = Height - borderTop;
            int x = 0;

            // Hour dropdown
            _hourBorder.SetBounds(x, borderTop, DropW, borderH);
            _hourCombo.SetBounds(x, borderTop, DropW, borderH);
            _hourBorder.BringToFront();
            x += DropW;

            // Colon — painted in OnPaint
            x += ColonW;

            // Minute dropdown
            _minuteBorder.SetBounds(x, borderTop, DropW, borderH);
            _minuteCombo.SetBounds(x, borderTop, DropW, borderH);
            _minuteBorder.BringToFront();
            x += DropW + Gap;

            // AM/PM stacked
            int amPmTop = borderTop + (borderH - AmPmH * 2 - AmPmGap) / 2;
            _amBtn.SetBounds(x, amPmTop, AmPmW, AmPmH);
            _pmBtn.SetBounds(x, amPmTop + AmPmH + AmPmGap, AmPmW, AmPmH);
        }

        // ── Paint label + colon ─────────────────────────────────

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // Label
            if (_showLabel)
            {
                Color labelColor;
                if (!Enabled)          labelColor = LabelDisabled;
                else if (_readOnly)    labelColor = LabelRequired;
                else                   labelColor = LabelNormal;

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

            // Colon between hour and minute
            int colonX = DropW;
            int borderTop = _showLabel ? LabelHeight + LabelGap : 0;
            int borderH = Height - borderTop;
            var colonRect = new Rectangle(colonX, borderTop, ColonW, borderH);
            using (var font = new Font(WerTheme.FontFamily, 11f, FontStyle.Bold))
                TextRenderer.DrawText(g, ":", font, colonRect, WerTheme.TextColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
        }

        // ── Paint dropdown face (shared for hour/minute) ────────

        private void PaintDropdown(Graphics g, Panel panel, ComboBox combo, bool focused)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            var rect = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
            bool inactive = !Enabled || _readOnly;

            // Background
            using (var path = RoundedRect(rect, BorderRadius))
            using (var brush = new SolidBrush(inactive ? BgDisabled : BgNormal))
                g.FillPath(brush, path);

            // Border
            var borderColor = (focused && Enabled && !_readOnly) ? BorderFocus : BorderNormal;
            float bw = (focused && Enabled && !_readOnly) ? 1.5f : 1f;
            using (var path = RoundedRect(rect, BorderRadius))
            using (var pen = new Pen(borderColor, bw))
                g.DrawPath(pen, path);

            // Text
            var textRect = new Rectangle(InputPadH, 0, panel.Width - InputPadH - ChevronW, panel.Height);
            string text = combo.SelectedItem?.ToString() ?? "--";
            var textColor = !Enabled ? LabelDisabled : WerTheme.TextColor;
            TextRenderer.DrawText(g, text, Font, textRect, textColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix);

            // Chevron
            if (Enabled && !_readOnly)
            {
                var chevronRect = new Rectangle(panel.Width - ChevronW, 0, ChevronW, panel.Height);
                var chevronColor = focused ? BorderFocus : Color.FromArgb(140, 150, 160);
                TextRenderer.DrawText(g, "▾", Font, chevronRect, chevronColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }

        // ── Paint AM/PM buttons ─────────────────────────────────

        private void PaintAmPm(Graphics g, Panel panel, string text, bool active)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            var rect = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);

            using (var path = RoundedRect(rect, 4))
            {
                using (var brush = new SolidBrush(active ? AmPmActive : AmPmInactive))
                    g.FillPath(brush, path);

                if (!active)
                {
                    using (var pen = new Pen(BorderNormal, 1f))
                        g.DrawPath(pen, path);
                }
            }

            var textColor = active ? Color.White : Color.FromArgb(100, 110, 120);
            using (var font = new Font(WerTheme.FontFamily, 7f, FontStyle.Bold))
                TextRenderer.DrawText(g, text, font, rect, textColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
        }

        // ── Combo item draw ──────────────────────────────────────

        private void OnComboDrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            var combo = (ComboBox)sender;
            bool selected = (e.State & DrawItemState.Selected) != 0;

            using (var bg = new SolidBrush(selected ? AmPmActive : Color.White))
                e.Graphics.FillRectangle(bg, e.Bounds);

            var text = combo.Items[e.Index].ToString();
            var textColor = selected ? Color.White : WerTheme.TextColor;
            TextRenderer.DrawText(e.Graphics, text, WerTheme.BodyFont, e.Bounds, textColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix |
                TextFormatFlags.SingleLine | TextFormatFlags.LeftAndRightPadding);
        }

        // ── Helpers ─────────────────────────────────────────────

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
