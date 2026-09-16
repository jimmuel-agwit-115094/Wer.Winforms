using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    /// <summary>
    /// Date range picker with two date fields (From — To) and an optional label above.
    ///
    /// Usage:
    ///   werDateRangePicker1.LabelText = "Date Range";
    ///   werDateRangePicker1.StartDate = DateTime.Today.AddDays(-7);
    ///   werDateRangePicker1.EndDate   = DateTime.Today;
    ///   werDateRangePicker1.RangeChanged += (s, e) => { ... };
    /// </summary>
    [ToolboxItem(true)]
    [Category("Wer Controls")]
    [Description("Date range picker with From and To date fields and an optional label.")]
    [DefaultEvent("RangeChanged")]
    [DefaultProperty("LabelText")]
    public class WerDateRangePicker : UserControl
    {
        private readonly Label        _label;
        private readonly WerDatePicker _fromPicker;
        private readonly WerDatePicker _toPicker;
        private readonly Label        _separator;

        private string _labelText  = "Date Range";
        private bool   _showLabel  = true;

        private const int LabelH    = 20;
        private const int LabelGap  = 4;
        private const int SepW      = 24;
        private const int SepGap    = 6;

        public event EventHandler RangeChanged;

        public WerDateRangePicker()
        {
            // Prevent designer noise
            AutoScaleMode = AutoScaleMode.None;
            BackColor     = Color.Transparent;
            Size          = new Size(500, 60);
            Font          = WerTheme.BodyFont;

            // Label above
            _label = new Label
            {
                Text      = _labelText,
                Font      = WerTheme.BodyFont,
                ForeColor = WerTheme.LabelColor,
                AutoSize  = false,
                BackColor = Color.Transparent,
            };

            // From picker — no sub-label (we draw the parent label)
            _fromPicker = new WerDatePicker
            {
                ShowLabel = false,
                Font      = WerTheme.BodyFont,
            };
            _fromPicker.ValueChanged += OnPickerChanged;

            // Separator dash
            _separator = new Label
            {
                Text      = "—",
                Font      = WerTheme.BodyFont,
                ForeColor = WerTheme.MutedColor,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent,
                AutoSize  = false,
            };

            // To picker
            _toPicker = new WerDatePicker
            {
                ShowLabel = false,
                Font      = WerTheme.BodyFont,
            };
            _toPicker.ValueChanged += OnPickerChanged;

            Controls.Add(_label);
            Controls.Add(_fromPicker);
            Controls.Add(_separator);
            Controls.Add(_toPicker);

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
            if (_label != null) _label.ForeColor = WerTheme.LabelColor;
            if (_separator != null) _separator.ForeColor = WerTheme.MutedColor;
        }

        // ── Public properties ──────────────────────────────────────

        [Category("WerDateRangePicker")]
        [DefaultValue("Date Range")]
        [Description("Label displayed above the date range pickers.")]
        public string LabelText
        {
            get => _labelText;
            set
            {
                _labelText  = value;
                _label.Text = value;
            }
        }

        [Category("WerDateRangePicker")]
        [DefaultValue(true)]
        [Description("Show or hide the label above the pickers.")]
        public bool ShowLabel
        {
            get => _showLabel;
            set { _showLabel = value; LayoutInternals(); }
        }

        [Category("WerDateRangePicker")]
        [Browsable(false)]
        public DateTime? StartDate
        {
            get => _fromPicker.Value;
            set => _fromPicker.Value = value;
        }

        [Category("WerDateRangePicker")]
        [Browsable(false)]
        public DateTime? EndDate
        {
            get => _toPicker.Value;
            set => _toPicker.Value = value;
        }

        // ── Layout ────────────────────────────────────────────────

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            LayoutInternals();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            if (_label == null) return;
            _label.Font      = Font;
            _fromPicker.Font = Font;
            _toPicker.Font   = Font;
            _separator.Font  = Font;
            LayoutInternals();
        }

        private void LayoutInternals()
        {
            if (_label == null) return;

            int pickerY;

            if (_showLabel)
            {
                _label.SetBounds(0, 0, Width, LabelH);
                _label.Visible = true;
                pickerY        = LabelH + LabelGap;
            }
            else
            {
                _label.Visible = false;
                pickerY        = 0;
            }

            int pickerH = Height - pickerY;
            // Available width: Width - separator area
            int availW   = Width - SepW - SepGap * 2;
            int pickerW  = availW / 2;

            _fromPicker.SetBounds(0, pickerY, pickerW, pickerH);
            _separator.SetBounds(pickerW + SepGap, pickerY, SepW, pickerH);
            _toPicker.SetBounds(pickerW + SepGap + SepW + SepGap, pickerY,
                Width - (pickerW + SepGap + SepW + SepGap), pickerH);
        }

        private void OnPickerChanged(object sender, EventArgs e)
        {
            RangeChanged?.Invoke(this, EventArgs.Empty);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_fromPicker != null) _fromPicker.ValueChanged -= OnPickerChanged;
                if (_toPicker != null)   _toPicker.ValueChanged   -= OnPickerChanged;
            }
            base.Dispose(disposing);
        }
    }
}
