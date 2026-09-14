using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    [ToolboxItem(true)]
    [Description("Custom combobox with label, placeholder, and styled dropdown.")]
    [DefaultEvent("SelectedIndexChanged")]
    [DefaultProperty("Items")]
    public class WerComboBox : Control
    {
        private readonly ComboBox _combo;
        private readonly Panel _inputBorder;

        // ── State ───────────────────────────────────────────────
        private string _labelText = "ComboBox Label";
        private string _placeholder = "Select...";
        private bool _required;
        private bool _readOnly;
        private bool _hasFocus;

        // ── Layout constants (match WerTextField) ───────────────
        private int LabelHeight => Math.Max(20, (int)(Font.GetHeight() + 4));
        private const int LabelGap     = 4;
        private const int BorderRadius = 8;
        private const int InputPadH    = 10;
        private const int ChevronWidth = 28;

        // ── Colors ──────────────────────────────────────────────
        private static readonly Color LabelNormal      = Color.Black;
        private static readonly Color LabelRequired    = Color.FromArgb(200, 100, 20);
        private static readonly Color LabelDisabled    = Color.FromArgb(150, 150, 150);
        private static readonly Color RequiredStar     = Color.FromArgb(210, 50, 50);
        private static readonly Color BorderNormal     = Color.FromArgb(200, 210, 220);
        private static readonly Color BorderFocus      = Color.FromArgb(12, 124, 146);
        private static readonly Color PlaceholderColor = Color.FromArgb(160, 170, 180);
        private static readonly Color BgNormal         = Color.White;
        private static readonly Color BgDisabled       = Color.FromArgb(242, 242, 242);

        public event EventHandler SelectedIndexChanged;

        public WerComboBox()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.SupportsTransparentBackColor,
                true);

            Font      = WerTheme.BodyFont;
            ForeColor = WerTheme.TextColor;
            BackColor = Color.Transparent;
            Size      = new Size(350, 60);

            // Custom-painted input face
            _inputBorder = new Panel { BackColor = Color.Transparent, Cursor = Cursors.Hand };
            _inputBorder.Paint      += OnBorderPaint;
            _inputBorder.MouseClick += OnInputAreaClick;
            Controls.Add(_inputBorder);

            // Native ComboBox — positioned behind the panel, used only for its dropdown
            _combo = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font          = WerTheme.BodyFont,
                FlatStyle     = FlatStyle.Standard,
                DrawMode      = DrawMode.OwnerDrawFixed,
                ItemHeight    = 28,
                TabStop       = false,
            };
            _combo.DrawItem             += OnComboDrawItem;
            _combo.SelectedIndexChanged += (s, e) => { _inputBorder.Invalidate(); SelectedIndexChanged?.Invoke(this, EventArgs.Empty); };
            _combo.DropDown             += (s, e) => { _hasFocus = true;  _inputBorder.Invalidate(); };
            _combo.DropDownClosed       += (s, e) => { _hasFocus = false; _inputBorder.Invalidate(); };
            Controls.Add(_combo);

            // Input border on top
            _inputBorder.BringToFront();

            LayoutInternals();
        }

        // ── Public API ─────────────────────────────────────────────

        private bool _showLabel = true;

        [Category("WerComboBox")]
        [DefaultValue(true)]
        [Description("Show or hide the label above the input.")]
        public bool ShowLabel
        {
            get => _showLabel;
            set { _showLabel = value; LayoutInternals(); Invalidate(); }
        }

        [Category("WerComboBox")]
        [DefaultValue("ComboBox Label")]
        [Description("Label displayed above the combobox.")]
        public string LabelText
        {
            get => _labelText;
            set { _labelText = value; Invalidate(); }
        }

        [Category("WerComboBox")]
        [DefaultValue("Select...")]
        [Description("Placeholder text when nothing is selected.")]
        public string Placeholder
        {
            get => _placeholder;
            set { _placeholder = value; _inputBorder.Invalidate(); }
        }

        [Category("WerComboBox")]
        [DefaultValue(false)]
        [Description("Show red asterisk and orange label.")]
        public bool Required
        {
            get => _required;
            set { _required = value; Invalidate(); }
        }

        [Category("WerComboBox")]
        [DefaultValue(false)]
        [Description("Field is read-only: disabled appearance, no dropdown.")]
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

        [Category("WerComboBox")]
        [Description("The list of items to display in the dropdown.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ComboBox.ObjectCollection Items => _combo.Items;

        [Category("WerComboBox")]
        [Description("Items to display — set this in the designer as string array.")]
        public string[] ItemsArray
        {
            get
            {
                var arr = new string[_combo.Items.Count];
                for (int i = 0; i < arr.Length; i++)
                    arr[i] = _combo.Items[i]?.ToString() ?? "";
                return arr;
            }
            set
            {
                _combo.Items.Clear();
                if (value != null) _combo.Items.AddRange(value);
                _combo.SelectedIndex = -1;
                _inputBorder.Invalidate();
            }
        }

        [Category("WerComboBox")]
        [DefaultValue(-1)]
        [Description("Index of the selected item. -1 means nothing selected.")]
        public int SelectedIndex
        {
            get => _combo.SelectedIndex;
            set { _combo.SelectedIndex = value; _inputBorder.Invalidate(); }
        }

        [Browsable(false)]
        public object SelectedItem => _combo.SelectedItem;

        [Browsable(true)]
        [Category("WerComboBox")]
        public override string Text
        {
            get => _combo.SelectedItem?.ToString() ?? "";
            set
            {
                int idx = _combo.FindStringExact(value);
                if (idx >= 0) _combo.SelectedIndex = idx;
            }
        }

        // ── Click → open dropdown via native combo ───────────────

        private void OnInputAreaClick(object sender, MouseEventArgs e)
        {
            if (!Enabled || _readOnly) return;
            _combo.Focus();
            _combo.DroppedDown = true;
        }

        // ── Enable/disable ───────────────────────────────────────

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            _combo.Enabled = Enabled;
            Cursor = Enabled ? Cursors.Hand : Cursors.Default;
            Invalidate();
            _inputBorder.Invalidate();
        }

        // ── Layout ───────────────────────────────────────────────

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

            // Input border covers the full input area
            _inputBorder.SetBounds(0, borderTop, Width, borderH);

            // Native combo sits at the same position but behind the panel
            // It must be positioned here so the dropdown appears in the right spot
            _combo.SetBounds(0, borderTop, Width, borderH);

            _inputBorder.BringToFront();
        }

        // ── Paint label ──────────────────────────────────────────

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

        // ── Paint input area ─────────────────────────────────────

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

            // Text: selected value or placeholder
            var textRect = new Rectangle(InputPadH, 0, panel.Width - InputPadH - ChevronWidth, panel.Height);

            if (_combo.SelectedIndex >= 0)
            {
                var textColor = !Enabled ? LabelDisabled : WerTheme.TextColor;
                TextRenderer.DrawText(g, _combo.SelectedItem.ToString(), Font, textRect, textColor,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
            }
            else
            {
                TextRenderer.DrawText(g, _placeholder, Font, textRect, PlaceholderColor,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
            }

            // Chevron ∨
            if (Enabled && !_readOnly)
            {
                var chevronRect = new Rectangle(panel.Width - ChevronWidth, 0, ChevronWidth, panel.Height);
                DrawChevron(g, chevronRect);
            }
        }

        private void DrawChevron(Graphics g, Rectangle bounds)
        {
            int cx = bounds.X + bounds.Width / 2;
            int cy = bounds.Y + bounds.Height / 2;
            int w = 5, h = 3;

            using (var pen = new Pen(Color.FromArgb(140, 150, 160), 1.8f) { LineJoin = LineJoin.Round })
            {
                g.DrawLines(pen, new[]
                {
                    new PointF(cx - w, cy - h),
                    new PointF(cx, cy + h),
                    new PointF(cx + w, cy - h)
                });
            }
        }

        // ── Owner-draw dropdown items ────────────────────────────

        private void OnComboDrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            var g = e.Graphics;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            bool selected = (e.State & DrawItemState.Selected) != 0;

            var bgColor = selected ? Color.FromArgb(100, 100, 100) : Color.White;
            using (var brush = new SolidBrush(bgColor))
                g.FillRectangle(brush, e.Bounds);

            var textColor = selected ? Color.White : WerTheme.TextColor;
            var textRect = new Rectangle(e.Bounds.X + InputPadH, e.Bounds.Y, e.Bounds.Width - InputPadH * 2, e.Bounds.Height);
            TextRenderer.DrawText(g, _combo.Items[e.Index].ToString(), Font, textRect, textColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
        }

        // ── Helpers ──────────────────────────────────────────────

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
