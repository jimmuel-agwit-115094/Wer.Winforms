using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    /// <summary>
    /// Date range dropdown selector. Provides predefined date ranges like
    /// "Past 30 Days", "Past 60 Days", etc. Uses the same WerComboBox pattern.
    ///
    /// Usage:
    ///   DateTime from = werDateRange1.FromDate;
    ///   DateTime to   = werDateRange1.ToDate;
    ///   string range  = werDateRange1.SelectedRange;
    /// </summary>
    [ToolboxItem(true)]
    [Description("Date range dropdown selector with predefined ranges.")]
    [DefaultEvent("RangeChanged")]
    [DefaultProperty("LabelText")]
    public class WerDateRange : Control
    {
        private readonly ComboBox _combo;
        private readonly Panel    _inputBorder;

        private string _labelText = "View By";
        private bool   _required;
        private bool   _readOnly;
        private bool   _hasFocus;

        private int LabelHeight => Math.Max(20, (int)(Font.GetHeight() + 4));
        private const int LabelGap     = 4;
        private const int BorderRadius = 8;
        private const int InputPadH    = 10;
        private const int ChevronW     = 28;
        private const int CheckW       = 24;

        private static readonly Color LabelNormal      = Color.Black;
        private static readonly Color LabelRequired    = Color.FromArgb(200, 100, 20);
        private static readonly Color LabelDisabled    = Color.FromArgb(150, 150, 150);
        private static readonly Color RequiredStar     = Color.FromArgb(210, 50, 50);
        private static readonly Color BorderNormal     = Color.FromArgb(200, 210, 220);
        private static readonly Color BorderFocus      = Color.FromArgb(12, 124, 146);
        private static readonly Color PlaceholderColor = Color.FromArgb(160, 170, 180);
        private static readonly Color BgNormal         = Color.White;
        private static readonly Color BgDisabled       = Color.FromArgb(242, 242, 242);
        private static readonly Color AccentColor      = Color.FromArgb(12, 124, 146);

        /// <summary>Fired when the selected range changes.</summary>
        public event EventHandler RangeChanged;

        // Default ranges
        private static readonly string[] DefaultRanges = new[]
        {
            "Past 7 Days",
            "Past 30 Days",
            "Past 60 Days",
            "Past 90 Days",
            "Past 6 Months",
            "Past 1 Year"
        };

        public WerDateRange()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.SupportsTransparentBackColor, true);

            Font      = WerTheme.BodyFont;
            ForeColor = WerTheme.TextColor;
            BackColor = Color.Transparent;
            Size      = new Size(220, LabelHeight + LabelGap + 36);

            // Painted input face
            _inputBorder = new Panel { BackColor = Color.Transparent, Cursor = Cursors.Hand };
            _inputBorder.Paint      += OnBorderPaint;
            _inputBorder.MouseClick += (s, e) => OpenCombo();
            Controls.Add(_inputBorder);

            // Native ComboBox behind the panel
            _combo = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font          = WerTheme.BodyFont,
                FlatStyle     = FlatStyle.Standard,
                DrawMode      = DrawMode.OwnerDrawFixed,
                ItemHeight    = 32,
                TabStop       = false,
            };
            _combo.DrawItem             += OnComboDrawItem;
            _combo.SelectedIndexChanged += (s, e) => { _inputBorder.Invalidate(); RangeChanged?.Invoke(this, EventArgs.Empty); };
            _combo.DropDown             += (s, e) => { _hasFocus = true;  _inputBorder.Invalidate(); };
            _combo.DropDownClosed       += (s, e) => { _hasFocus = false; _inputBorder.Invalidate(); };
            Controls.Add(_combo);

            // Populate defaults
            _combo.Items.AddRange(DefaultRanges);
            _combo.SelectedIndex = 1; // "Past 30 Days"

            _inputBorder.BringToFront();
            LayoutInternals();
        }

        private void OpenCombo()
        {
            if (!Enabled || _readOnly) return;
            _combo.Focus();
            _combo.DroppedDown = true;
        }

        // ── Properties ──────────────────────────────────────────

        private bool _showLabel = true;

        [Category("WerDateRange")]
        [DefaultValue(true)]
        [Description("Show or hide the label above the input.")]
        public bool ShowLabel
        {
            get => _showLabel;
            set { _showLabel = value; LayoutInternals(); Invalidate(); }
        }

        [Category("WerDateRange")]
        [DefaultValue("View By")]
        [Description("Label displayed above the dropdown.")]
        public string LabelText
        {
            get => _labelText;
            set { _labelText = value; Invalidate(); }
        }

        [Category("WerDateRange")]
        [DefaultValue(false)]
        public bool Required
        {
            get => _required;
            set { _required = value; Invalidate(); }
        }

        [Category("WerDateRange")]
        [DefaultValue(false)]
        public bool ReadOnly
        {
            get => _readOnly;
            set { _readOnly = value; Invalidate(); _inputBorder.Invalidate(); }
        }

        /// <summary>
        /// The range options to display. Defaults to Past 7/30/60/90 Days, 6 Months, 1 Year, All Time.
        /// </summary>
        [Category("WerDateRange")]
        [Description("Custom range options. Set to override defaults.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string[] RangeOptions
        {
            get
            {
                var arr = new string[_combo.Items.Count];
                for (int i = 0; i < arr.Length; i++)
                    arr[i] = _combo.Items[i].ToString();
                return arr;
            }
            set
            {
                _combo.Items.Clear();
                if (value != null) _combo.Items.AddRange(value);
                if (_combo.Items.Count > 0) _combo.SelectedIndex = 0;
                _inputBorder.Invalidate();
            }
        }

        [Category("WerDateRange")]
        [DefaultValue(1)]
        public int SelectedIndex
        {
            get => _combo.SelectedIndex;
            set { _combo.SelectedIndex = value; _inputBorder.Invalidate(); }
        }

        /// <summary>
        /// The selected range text, e.g. "Past 30 Days".
        /// </summary>
        [Browsable(false)]
        public string SelectedRange => _combo.SelectedItem?.ToString() ?? "";

        /// <summary>
        /// Computed date range based on selection.
        /// StartDate = today, EndDate = calculated past date.
        /// Usage:
        ///   var startDate = werDateRange1.Result.StartDate;  // today
        ///   var endDate   = werDateRange1.Result.EndDate;    // e.g. 30 days ago
        /// </summary>
        [Browsable(false)]
        public DateRangeResult Result
        {
            get
            {
                var today = DateTime.Today;
                var sel = SelectedRange;
                DateTime endDate;

                if (sel.Contains("7 Days"))        endDate = today.AddDays(-7);
                else if (sel.Contains("30 Days"))  endDate = today.AddDays(-30);
                else if (sel.Contains("60 Days"))  endDate = today.AddDays(-60);
                else if (sel.Contains("90 Days"))  endDate = today.AddDays(-90);
                else if (sel.Contains("6 Months")) endDate = today.AddMonths(-6);
                else if (sel.Contains("1 Year"))   endDate = today.AddYears(-1);
                else endDate = today.AddDays(-30);

                return new DateRangeResult(today, endDate);
            }
        }

        // ── Enable/disable ──────────────────────────────────────

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            _combo.Enabled = Enabled;
            Invalidate();
            _inputBorder.Invalidate();
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
            if (_combo == null) return;
            _combo.Font = Font;
            LayoutInternals();
        }

        private void LayoutInternals()
        {
            if (_inputBorder == null || _combo == null) return;

            int borderTop = _showLabel ? LabelHeight + LabelGap : 0;
            int borderH   = Height - borderTop;

            _inputBorder.SetBounds(0, borderTop, Width, borderH);
            _combo.SetBounds(0, borderTop, Width, borderH);
            _inputBorder.BringToFront();
        }

        // ── Paint label ─────────────────────────────────────────

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            if (!_showLabel) return;

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

        // ── Paint input border ──────────────────────────────────

        private void OnBorderPaint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode     = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            var panel = (Panel)sender;
            var rect  = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
            bool inactive = !Enabled || _readOnly;

            // Background
            using (var path = RoundedRect(rect, BorderRadius))
            using (var brush = new SolidBrush(inactive ? BgDisabled : BgNormal))
                g.FillPath(brush, path);

            // Border
            var borderColor = (_hasFocus && Enabled && !_readOnly) ? BorderFocus : BorderNormal;
            float bw = (_hasFocus && Enabled && !_readOnly) ? 1.5f : 1f;
            using (var path = RoundedRect(rect, BorderRadius))
            using (var pen = new Pen(borderColor, bw))
                g.DrawPath(pen, path);

            // Selected text
            var textRect = new Rectangle(InputPadH, 0, panel.Width - InputPadH - ChevronW, panel.Height);
            if (_combo.SelectedIndex >= 0)
            {
                var textColor = !Enabled ? LabelDisabled : WerTheme.TextColor;
                TextRenderer.DrawText(g, _combo.SelectedItem.ToString(), Font, textRect, textColor,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix);
            }
            else
            {
                TextRenderer.DrawText(g, "Select range...", Font, textRect, PlaceholderColor,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
            }

            // Chevron
            if (Enabled && !_readOnly)
            {
                var chevronRect = new Rectangle(panel.Width - ChevronW, 0, ChevronW, panel.Height);
                var chevronColor = _hasFocus ? BorderFocus : Color.FromArgb(140, 150, 160);
                string chevron = _hasFocus ? "▲" : "▼";
                TextRenderer.DrawText(g, chevron, Font, chevronRect, chevronColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }

        // ── Combo item draw ─────────────────────────────────────

        private void OnComboDrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            var combo = (ComboBox)sender;
            bool selected = (e.State & DrawItemState.Selected) != 0;
            bool isChosen = e.Index == combo.SelectedIndex;

            // Background
            using (var bg = new SolidBrush(selected ? Color.FromArgb(240, 250, 252) : Color.White))
                e.Graphics.FillRectangle(bg, e.Bounds);

            // Text
            var text = combo.Items[e.Index].ToString();
            var textRect = new Rectangle(e.Bounds.X + InputPadH, e.Bounds.Y, e.Bounds.Width - InputPadH * 2, e.Bounds.Height);
            var textColor = selected ? AccentColor : WerTheme.TextColor;
            TextRenderer.DrawText(e.Graphics, text, WerTheme.BodyFont, textRect, textColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);

            // Bottom separator
            if (e.Index < combo.Items.Count - 1)
            {
                using (var pen = new Pen(Color.FromArgb(235, 238, 242), 1f))
                    e.Graphics.DrawLine(pen, e.Bounds.X + 8, e.Bounds.Bottom - 1, e.Bounds.Right - 8, e.Bounds.Bottom - 1);
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
