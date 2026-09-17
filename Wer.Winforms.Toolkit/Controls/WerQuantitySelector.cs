using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    /// <summary>
    /// Quantity selector with − and + buttons flanking a numeric value display.
    /// [ −  |  0  |  + ]
    /// </summary>
    [ToolboxItem(true)]
    [Description("Quantity selector with minus/plus buttons and a numeric value display.")]
    [DefaultEvent("ValueChanged")]
    [DefaultProperty("Value")]
    public class WerQuantitySelector : Control
    {
        // ── State ────────────────────────────────────────────────────
        private int    _value   = 0;
        private int    _minimum = 0;
        private int    _maximum = 999;
        private int    _step    = 1;
        private string _labelText  = "Quantity";
        private bool   _showLabel  = true;
        private bool   _required;
        private bool   _readOnly;

        // ── Hover / press tracking ───────────────────────────────────
        private bool _hoverMinus;
        private bool _hoverPlus;
        private bool _pressMinus;
        private bool _pressPlus;

        // ── Layout ───────────────────────────────────────────────────
        private int LabelHeight => Math.Max(20, (int)(Font.GetHeight() + 4));
        private const int LabelGap     = 4;
        private const int BorderRadius  = 10;
        private const int BtnZoneW      = 44;   // width of each − / + zone
        private const int InputH        = 38;   // height of the selector bar

        public event EventHandler ValueChanged;

        public WerQuantitySelector()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.SupportsTransparentBackColor, true);

            BackColor = Color.Transparent;
            Font      = WerTheme.BodyFont;
            Size      = new Size(140, 60);

            // Keyboard support
            SetStyle(ControlStyles.Selectable, true);
            TabStop = true;
        }

        // ── Properties ───────────────────────────────────────────────

        [Category("WerQuantitySelector")]
        [DefaultValue(0)]
        [Description("Current quantity value.")]
        public int Value
        {
            get => _value;
            set
            {
                int clamped = Math.Max(_minimum, Math.Min(_maximum, value));
                if (_value == clamped) return;
                _value = clamped;
                Invalidate();
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        [Category("WerQuantitySelector")]
        [DefaultValue(0)]
        [Description("Minimum allowed value.")]
        public int Minimum
        {
            get => _minimum;
            set { _minimum = value; Value = _value; }
        }

        [Category("WerQuantitySelector")]
        [DefaultValue(999)]
        [Description("Maximum allowed value.")]
        public int Maximum
        {
            get => _maximum;
            set { _maximum = value; Value = _value; }
        }

        [Category("WerQuantitySelector")]
        [DefaultValue(1)]
        [Description("Amount to increment or decrement per click.")]
        public int Step
        {
            get => _step;
            set { _step = Math.Max(1, value); }
        }

        [Category("WerQuantitySelector")]
        [DefaultValue("Quantity")]
        [Description("Label displayed above the control.")]
        public string LabelText
        {
            get => _labelText;
            set { _labelText = value; Invalidate(); }
        }

        [Category("WerQuantitySelector")]
        [DefaultValue(true)]
        [Description("Show or hide the label above the control.")]
        public bool ShowLabel
        {
            get => _showLabel;
            set { _showLabel = value; Invalidate(); }
        }

        [Category("WerQuantitySelector")]
        [DefaultValue(false)]
        [Description("Show red asterisk on the label.")]
        public bool Required
        {
            get => _required;
            set { _required = value; Invalidate(); }
        }

        [Category("WerQuantitySelector")]
        [DefaultValue(false)]
        [Description("Disables − and + interaction.")]
        public bool ReadOnly
        {
            get => _readOnly;
            set { _readOnly = value; Invalidate(); }
        }

        // ── Keyboard ─────────────────────────────────────────────────

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Left:
                case Keys.Right:
                case Keys.Up:
                case Keys.Down:
                    return true;
            }
            return base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (!_readOnly && Enabled)
            {
                if (e.KeyCode == Keys.Left  || e.KeyCode == Keys.Down)  { Value -= _step; e.Handled = true; }
                if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Up)    { Value += _step; e.Handled = true; }
            }
            base.OnKeyDown(e);
        }

        // ── Mouse ────────────────────────────────────────────────────

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_readOnly || !Enabled) return;
            var (minusRect, plusRect) = GetButtonRects();
            bool hm = minusRect.Contains(e.Location);
            bool hp = plusRect.Contains(e.Location);
            if (hm != _hoverMinus || hp != _hoverPlus)
            {
                _hoverMinus = hm;
                _hoverPlus  = hp;
                Cursor = (hm || hp) ? Cursors.Hand : Cursors.Default;
                Invalidate();
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _hoverMinus = _hoverPlus = false;
            _pressMinus = _pressPlus = false;
            Cursor = Cursors.Default;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || _readOnly || !Enabled) return;
            Focus();
            var (minusRect, plusRect) = GetButtonRects();
            _pressMinus = minusRect.Contains(e.Location);
            _pressPlus  = plusRect.Contains(e.Location);
            Invalidate();
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            var (minusRect, plusRect) = GetButtonRects();
            if (_pressMinus && minusRect.Contains(e.Location) && !_readOnly && Enabled)
                Value -= _step;
            if (_pressPlus  && plusRect.Contains(e.Location)  && !_readOnly && Enabled)
                Value += _step;
            _pressMinus = _pressPlus = false;
            Invalidate();
            base.OnMouseUp(e);
        }

        // ── Layout helpers ───────────────────────────────────────────

        private int InputTop => _showLabel ? LabelHeight + LabelGap : 0;

        private Rectangle GetInputRect() =>
            new Rectangle(0, InputTop, Width, InputH);

        private (Rectangle minus, Rectangle plus) GetButtonRects()
        {
            var bar = GetInputRect();
            var minus = new Rectangle(bar.X,              bar.Y, BtnZoneW, bar.Height);
            var plus  = new Rectangle(bar.Right - BtnZoneW, bar.Y, BtnZoneW, bar.Height);
            return (minus, plus);
        }

        // ── Paint ────────────────────────────────────────────────────

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode      = SmoothingMode.AntiAlias;
            g.TextRenderingHint  = TextRenderingHint.ClearTypeGridFit;

            // ── Label ────────────────────────────────────────────────
            if (_showLabel)
            {
                Color labelColor = !Enabled    ? WerTheme.LabelDisabledColor
                                 : _readOnly   ? WerTheme.LabelReadOnlyColor
                                 :               WerTheme.LabelColor;

                var labelRect = new Rectangle(0, 0, Width, LabelHeight);
                TextRenderer.DrawText(g, _labelText, Font, labelRect, labelColor,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);

                if (_required && Enabled)
                {
                    int lw = TextRenderer.MeasureText(g, _labelText, Font).Width;
                    TextRenderer.DrawText(g, "*", Font, new Rectangle(lw + 2, 0, 12, LabelHeight),
                        WerTheme.RequiredStarColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
                }
            }

            // ── Bar container ─────────────────────────────────────────
            var bar  = GetInputRect();
            var rect = new Rectangle(bar.X, bar.Y, bar.Width - 1, bar.Height - 1);

            bool inactive = !Enabled || _readOnly;
            Color bgColor   = inactive ? WerTheme.InputBgDisabled : WerTheme.InputBg;
            Color border    = WerTheme.InputBorder;
            Color accent    = inactive ? WerTheme.DisabledColor : WerTheme.PrimaryColor;
            Color accentHov = Color.FromArgb(
                Math.Min(255, accent.R + 20),
                Math.Min(255, accent.G + 20),
                Math.Min(255, accent.B + 20));
            Color accentPrs = Color.FromArgb(
                Math.Max(0, accent.R - 20),
                Math.Max(0, accent.G - 20),
                Math.Max(0, accent.B - 20));

            // Background fill
            using (var path = RoundedRect(rect, BorderRadius))
            using (var brush = new SolidBrush(bgColor))
                g.FillPath(brush, path);

            // ── Minus button zone ─────────────────────────────────────
            var (minusRect, plusRect) = GetButtonRects();

            if (!inactive && (_hoverMinus || _pressMinus))
            {
                Color btnBg = _pressMinus
                    ? Color.FromArgb(30, accent)
                    : Color.FromArgb(15, accent);
                var minusZone = new Rectangle(rect.X, rect.Y, BtnZoneW, rect.Height);
                using (var path = RoundedRectLeft(minusZone, BorderRadius))
                using (var brush = new SolidBrush(btnBg))
                    g.FillPath(brush, path);
            }

            // ── Plus button zone ──────────────────────────────────────
            if (!inactive && (_hoverPlus || _pressPlus))
            {
                Color btnBg = _pressPlus
                    ? Color.FromArgb(30, accent)
                    : Color.FromArgb(15, accent);
                var plusZone = new Rectangle(rect.Right - BtnZoneW, rect.Y, BtnZoneW, rect.Height);
                using (var path = RoundedRectRight(plusZone, BorderRadius))
                using (var brush = new SolidBrush(btnBg))
                    g.FillPath(brush, path);
            }

            // ── Divider lines ─────────────────────────────────────────
            using (var pen = new Pen(border, 1f))
            {
                g.DrawLine(pen,
                    rect.X + BtnZoneW, rect.Y + 6,
                    rect.X + BtnZoneW, rect.Bottom - 6);
                g.DrawLine(pen,
                    rect.Right - BtnZoneW, rect.Y + 6,
                    rect.Right - BtnZoneW, rect.Bottom - 6);
            }

            // ── Outer border ──────────────────────────────────────────
            using (var path = RoundedRect(rect, BorderRadius))
            using (var pen  = new Pen(border, 1f))
                g.DrawPath(pen, path);

            // ── − symbol ─────────────────────────────────────────────
            Color minusColor = inactive        ? WerTheme.DisabledColor
                             : _value <= _minimum ? WerTheme.MutedColor
                             : _pressMinus     ? accentPrs
                             : _hoverMinus     ? accentHov
                             :                   accent;

            var minusZoneRect = new Rectangle(rect.X, rect.Y, BtnZoneW, rect.Height + 1);
            using (var f = new Font(WerTheme.FontFamily, 13f, FontStyle.Regular))
                TextRenderer.DrawText(g, "−", f, minusZoneRect, minusColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            // ── Value ─────────────────────────────────────────────────
            Color valueColor = !Enabled ? WerTheme.DisabledColor : WerTheme.TextColor;
            var valueZoneRect = new Rectangle(rect.X + BtnZoneW, rect.Y, rect.Width - BtnZoneW * 2, rect.Height + 1);
            using (var f = new Font(WerTheme.FontFamily, 10f, FontStyle.Bold))
                TextRenderer.DrawText(g, _value.ToString(), f, valueZoneRect, valueColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            // ── + symbol ─────────────────────────────────────────────
            Color plusColor = inactive        ? WerTheme.DisabledColor
                            : _value >= _maximum ? WerTheme.MutedColor
                            : _pressPlus      ? accentPrs
                            : _hoverPlus      ? accentHov
                            :                   accent;

            var plusZoneRect = new Rectangle(rect.Right - BtnZoneW, rect.Y, BtnZoneW, rect.Height + 1);
            using (var f = new Font(WerTheme.FontFamily, 13f, FontStyle.Regular))
                TextRenderer.DrawText(g, "+", f, plusZoneRect, plusColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            // ── Focus ring ────────────────────────────────────────────
            if (Focused && !_readOnly && Enabled)
            {
                using (var path = RoundedRect(rect, BorderRadius))
                using (var pen  = new Pen(WerTheme.InputBorderFocus, 1.5f))
                    g.DrawPath(pen, path);
            }
        }

        // ── GDI+ helpers ─────────────────────────────────────────────

        private static GraphicsPath RoundedRect(Rectangle r, int radius)
        {
            int d = radius * 2;
            var p = new GraphicsPath();
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }

        // Left-rounded only (for minus hover zone)
        private static GraphicsPath RoundedRectLeft(Rectangle r, int radius)
        {
            int d = radius * 2;
            var p = new GraphicsPath();
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddLine(r.Right, r.Y, r.Right, r.Bottom);
            p.AddLine(r.Right, r.Bottom, r.X + radius, r.Bottom);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }

        // Right-rounded only (for plus hover zone)
        private static GraphicsPath RoundedRectRight(Rectangle r, int radius)
        {
            int d = radius * 2;
            var p = new GraphicsPath();
            p.AddLine(r.X, r.Y, r.Right - radius, r.Y);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddLine(r.Right - radius, r.Bottom, r.X, r.Bottom);
            p.CloseFigure();
            return p;
        }
    }
}
