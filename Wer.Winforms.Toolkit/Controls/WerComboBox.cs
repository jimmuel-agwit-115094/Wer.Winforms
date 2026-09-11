using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    [ToolboxItem(true)]
    [Description("Custom combobox with label, clear button, and styled dropdown.")]
    [DefaultEvent("SelectedIndexChanged")]
    [DefaultProperty("Items")]
    public class WerComboBox : Control
    {
        private readonly List<string> _items = new List<string>();
        private int  _selectedIndex = -1;
        private bool _isOpen;
        private bool _isHovering;
        private DropdownForm _dropdown;

        // ── Label state ──────────────────────────────────────────
        private string _labelText = "ComboBox Label";
        private bool   _required;

        // ── Layout constants (match WerTextField) ────────────────
        private const int LabelHeight  = 20;
        private const int LabelGap     = 4;
        private const int BorderRadius = 8;
        private const int InputPadH    = 10;
        private const int ChevronWidth = 30;
        private const int ClearWidth   = 24;

        // ── Colors (same as WerTextField) ────────────────────────
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
            BackColor = Color.White;
            Size      = new Size(220, LabelHeight + LabelGap + 36);
            Cursor    = Cursors.Hand;
        }

        // ── Public API ──────────────────────────────────────────────

        [Category("WerComboBox")]
        [DefaultValue("Label")]
        [Description("Label displayed above the combobox.")]
        public string LabelText
        {
            get => _labelText;
            set { _labelText = value; Invalidate(); }
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
        [Description("The list of items to display in the dropdown.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<string> Items => _items;

        [Category("WerComboBox")]
        [Description("Items to display — set this in the designer.")]
        public string[] ItemsArray
        {
            get => _items.ToArray();
            set
            {
                _items.Clear();
                if (value != null) _items.AddRange(value);
                SelectedIndex = -1;
                Invalidate();
            }
        }

        [Category("WerComboBox")]
        [DefaultValue(-1)]
        [Description("Index of the selected item. -1 means nothing selected.")]
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (value < -1 || value >= _items.Count) value = -1;
                if (_selectedIndex == value) return;
                _selectedIndex = value;
                Invalidate();
                SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        [Browsable(false)]
        public string SelectedItem => _selectedIndex >= 0 ? _items[_selectedIndex] : null;

        // ── Painting ─────────────────────────────────────────────────

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode     = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // ── Label ────────────────────────────────────────────
            Color labelColor;
            if (!Enabled)        labelColor = LabelDisabled;
            else if (_required)  labelColor = LabelRequired;
            else                 labelColor = LabelNormal;

            var labelRect = new Rectangle(0, 0, Width, LabelHeight);
            TextRenderer.DrawText(g, _labelText, Font, labelRect, labelColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);

            if (_required && Enabled)
            {
                int lw = TextRenderer.MeasureText(g, _labelText, Font).Width;
                TextRenderer.DrawText(g, "*", Font, new Rectangle(lw + 2, 0, 12, LabelHeight),
                    RequiredStar, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            }

            // ── Input area ───────────────────────────────────────
            int inputTop = LabelHeight + LabelGap;
            int inputH   = Height - inputTop;
            var rect     = new Rectangle(0, inputTop, Width - 1, inputH - 1);

            // Background
            var bgColor = Enabled ? BgNormal : BgDisabled;
            using (var path = RoundedRect(rect, BorderRadius))
            using (var brush = new SolidBrush(bgColor))
                g.FillPath(brush, path);

            // Border
            var borderColor = (_isOpen || _isHovering) ? BorderFocus : BorderNormal;
            float borderW   = (_isOpen || _isHovering) ? 1.5f : 1f;
            using (var path = RoundedRect(rect, BorderRadius))
            using (var pen  = new Pen(Enabled ? borderColor : BorderNormal, borderW))
                g.DrawPath(pen, path);

            // Selected value text or placeholder
            int rightZone = ChevronWidth + (_selectedIndex >= 0 ? ClearWidth : 0);
            var textRect  = new Rectangle(InputPadH, inputTop, Width - InputPadH - rightZone, inputH);

            if (_selectedIndex >= 0)
            {
                TextRenderer.DrawText(g, _items[_selectedIndex], Font, textRect,
                    Enabled ? WerTheme.TextColor : LabelDisabled,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
            }

            // × clear button
            if (_selectedIndex >= 0 && Enabled)
            {
                var clearRect = new Rectangle(Width - ChevronWidth - ClearWidth, inputTop, ClearWidth, inputH);
                TextRenderer.DrawText(g, "×", new Font(Font.FontFamily, 11f), clearRect,
                    PlaceholderColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }

            // ∨ chevron
            var chevRect = new Rectangle(Width - ChevronWidth, inputTop, ChevronWidth, inputH);
            DrawChevron(g, chevRect);
        }

        private void DrawChevron(Graphics g, Rectangle bounds)
        {
            int cx = bounds.X + bounds.Width / 2;
            int cy = bounds.Y + bounds.Height / 2;
            int w = 6, h = 4;

            using (var pen = new Pen(PlaceholderColor, 1.8f) { LineJoin = LineJoin.Round })
            {
                if (_isOpen)
                    g.DrawLines(pen, new[]
                    {
                        new PointF(cx - w, cy + h / 2f),
                        new PointF(cx, cy - h / 2f),
                        new PointF(cx + w, cy + h / 2f)
                    });
                else
                    g.DrawLines(pen, new[]
                    {
                        new PointF(cx - w, cy - h / 2f),
                        new PointF(cx, cy + h / 2f),
                        new PointF(cx + w, cy - h / 2f)
                    });
            }
        }

        // ── Interaction ───────────────────────────────────────────────

        private Rectangle InputRect
        {
            get
            {
                int inputTop = LabelHeight + LabelGap;
                return new Rectangle(0, inputTop, Width, Height - inputTop);
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _isHovering = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _isHovering = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!Enabled) return;

            // Only respond to clicks in the input area (not the label)
            if (!InputRect.Contains(e.Location))
            {
                base.OnMouseDown(e);
                return;
            }

            // × clear hit
            if (_selectedIndex >= 0)
            {
                int inputTop  = LabelHeight + LabelGap;
                int inputH    = Height - inputTop;
                var clearRect = new Rectangle(Width - ChevronWidth - ClearWidth, inputTop, ClearWidth, inputH);
                if (clearRect.Contains(e.Location))
                {
                    SelectedIndex = -1;
                    return;
                }
            }

            if (_isOpen) CloseDropdown();
            else         OpenDropdown();

            base.OnMouseDown(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            Cursor = Enabled ? Cursors.Hand : Cursors.Default;
            Invalidate();
            base.OnEnabledChanged(e);
        }

        // ── Dropdown ─────────────────────────────────────────────────

        private void OpenDropdown()
        {
            if (_items.Count == 0) return;

            _isOpen = true;
            Invalidate();

            _dropdown = new DropdownForm(_items, _selectedIndex, Font, Width);
            _dropdown.ItemSelected += (s, idx) =>
            {
                SelectedIndex = idx;
                CloseDropdown();
            };
            _dropdown.Closed += (s, ev) => CloseDropdown();

            int inputTop = LabelHeight + LabelGap;
            var screen   = PointToScreen(new Point(0, inputTop + (Height - inputTop)));
            _dropdown.Location = screen;
            _dropdown.Show(FindForm());
        }

        private void CloseDropdown()
        {
            if (!_isOpen) return;
            _isOpen = false;
            Invalidate();
            _dropdown?.Hide();
        }

        // ── Helpers ───────────────────────────────────────────────────

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

        // ── Dropdown Form ─────────────────────────────────────────────

        private class DropdownForm : Form
        {
            private readonly List<string> _items;
            private readonly int  _selectedIndex;
            private readonly Font _font;
            private int _hoverIdx = -1;
            private const int ItemHeight = 38;
            private const int PaddingX   = 12;
            private const int DropRadius = 8;

            public event Action<object, int> ItemSelected;

            public DropdownForm(List<string> items, int selectedIndex, Font font, int width)
            {
                _items         = items;
                _selectedIndex = selectedIndex;
                _font          = font;

                FormBorderStyle = FormBorderStyle.None;
                StartPosition   = FormStartPosition.Manual;
                ShowInTaskbar   = false;
                BackColor       = Color.White;
                Width           = width;
                Height          = items.Count * ItemHeight + 2;

                SetStyle(ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.UserPaint |
                         ControlStyles.OptimizedDoubleBuffer, true);
            }

            protected override CreateParams CreateParams
            {
                get
                {
                    var cp = base.CreateParams;
                    cp.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT — prevent flicker
                    cp.ClassStyle |= 0x00020000; // CS_DROPSHADOW
                    return cp;
                }
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                var g = e.Graphics;
                g.SmoothingMode     = SmoothingMode.AntiAlias;
                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                var outerRect = new Rectangle(0, 0, Width - 1, Height - 1);

                // Rounded background
                using (var path = RoundedRect(outerRect, DropRadius))
                using (var brush = new SolidBrush(Color.White))
                    g.FillPath(brush, path);

                // Rounded border
                using (var path = RoundedRect(outerRect, DropRadius))
                using (var pen  = new Pen(Color.FromArgb(200, 210, 220), 1f))
                    g.DrawPath(pen, path);

                for (int i = 0; i < _items.Count; i++)
                {
                    var itemRect = new Rectangle(0, i * ItemHeight + 1, Width, ItemHeight);

                    // Hover highlight
                    if (i == _hoverIdx)
                        using (var brush = new SolidBrush(Color.FromArgb(245, 247, 250)))
                            g.FillRectangle(brush, itemRect);

                    // Item text — bold if selected
                    var itemFont = (i == _selectedIndex)
                        ? new Font(_font.FontFamily, _font.Size, FontStyle.Bold)
                        : _font;
                    var textRect = new Rectangle(PaddingX, itemRect.Y, Width - PaddingX * 2, ItemHeight);
                    TextRenderer.DrawText(g, _items[i], itemFont, textRect,
                        WerTheme.TextColor,
                        TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
                    if (i == _selectedIndex) itemFont.Dispose();

                    // Divider
                    if (i < _items.Count - 1)
                        using (var pen = new Pen(Color.FromArgb(235, 238, 242)))
                            g.DrawLine(pen, PaddingX, itemRect.Bottom, Width - PaddingX, itemRect.Bottom);
                }
            }

            protected override void OnMouseMove(MouseEventArgs e)
            {
                int idx = (e.Y - 1) / ItemHeight;
                if (idx < 0 || idx >= _items.Count) idx = -1;
                if (idx != _hoverIdx) { _hoverIdx = idx; Invalidate(); }
                base.OnMouseMove(e);
            }

            protected override void OnMouseLeave(EventArgs e)
            {
                _hoverIdx = -1;
                Invalidate();
                base.OnMouseLeave(e);
            }

            protected override void OnMouseDown(MouseEventArgs e)
            {
                int idx = (e.Y - 1) / ItemHeight;
                if (idx >= 0 && idx < _items.Count)
                    ItemSelected?.Invoke(this, idx);
                base.OnMouseDown(e);
            }

            protected override void OnDeactivate(EventArgs e)
            {
                Hide();
                base.OnDeactivate(e);
            }

            protected override bool ShowWithoutActivation => false;

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
}
