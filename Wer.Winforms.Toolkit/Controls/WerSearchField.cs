using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    [ToolboxItem(true)]
    [Description("Search text field with a search icon on the right side.")]
    [DefaultEvent("TextChanged")]
    [DefaultProperty("Text")]
    public class WerSearchField : Control
    {
        // ── Sub-controls ────────────────────────────────────────────
        private readonly TextBox _input;
        private readonly Panel   _inputBorder;

        // ── State ────────────────────────────────────────────────────
        private string _placeholder = "Search...";
        private bool   _hasFocus;

        // ── Layout ───────────────────────────────────────────────────
        private const int BorderRadius = 8;
        private const int PadLeft      = 10;
        private const int IconWidth    = 36;

        public WerSearchField()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.SupportsTransparentBackColor, true);

            BackColor = Color.Transparent;
            Font      = WerTheme.BodyFont;
            Size      = new Size(240, 36);

            _inputBorder = new Panel { BackColor = Color.Transparent };
            _inputBorder.Paint += OnBorderPaint;
            Controls.Add(_inputBorder);

            _input = new TextBox
            {
                BorderStyle = BorderStyle.None,
                BackColor   = WerTheme.InputBg,
                ForeColor   = WerTheme.TextColor,
                Font        = WerTheme.BodyFont,
            };
            _input.GotFocus   += (s, e) => { _hasFocus = true;  _inputBorder.Invalidate(); };
            _input.LostFocus  += (s, e) => { _hasFocus = false; _inputBorder.Invalidate(); };
            _input.TextChanged += (s, e) => { _inputBorder.Invalidate(); OnTextChanged(e); };
            _input.KeyDown    += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; Search?.Invoke(this, EventArgs.Empty); } };
            _inputBorder.Controls.Add(_input);

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
            if (_input != null)
                _input.BackColor = Enabled ? WerTheme.InputBg : WerTheme.InputBgDisabled;
            _inputBorder?.Invalidate();
        }

        // ── Events ───────────────────────────────────────────────────

        [Description("Fired when Enter is pressed or the search icon is clicked.")]
        public event EventHandler Search;

        // ── Public properties ────────────────────────────────────────

        [Browsable(true)]
        [Category("WerSearchField")]
        [DefaultValue("")]
        public override string Text
        {
            get => _input?.Text ?? string.Empty;
            set { if (_input != null) _input.Text = value; }
        }

        [Category("WerSearchField")]
        [DefaultValue("Search...")]
        [Description("Placeholder text shown when the field is empty.")]
        public string Placeholder
        {
            get => _placeholder;
            set { _placeholder = value; _inputBorder?.Invalidate(); }
        }

        // ── Enable/disable ───────────────────────────────────────────

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            if (_input == null) return;
            _input.Enabled   = Enabled;
            _input.BackColor = Enabled ? WerTheme.InputBg : WerTheme.InputBgDisabled;
            _inputBorder.Invalidate();
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

            _inputBorder.SetBounds(0, 0, Width, Height);

            int textH = _input.PreferredHeight;
            int textY = (Height - textH) / 2;
            // Text area leaves room for the icon on the right
            _input.SetBounds(PadLeft, Math.Max(0, textY), Width - PadLeft - IconWidth, textH);
        }

        // ── Paint ────────────────────────────────────────────────────

        protected override void OnPaint(PaintEventArgs e) { /* label handled by border panel */ }

        private void OnBorderPaint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode      = SmoothingMode.AntiAlias;
            g.TextRenderingHint  = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            bool inactive = !Enabled;

            // Background
            using (var path  = RoundedRect(rect, BorderRadius))
            using (var brush = new SolidBrush(inactive ? WerTheme.InputBgDisabled : WerTheme.InputBg))
                g.FillPath(brush, path);

            // Border
            if (Enabled)
            {
                var borderColor = _hasFocus ? WerTheme.InputBorderFocus : WerTheme.InputBorder;
                using (var path = RoundedRect(rect, BorderRadius))
                using (var pen  = new Pen(borderColor, _hasFocus ? 1.5f : 1f))
                    g.DrawPath(pen, path);
            }

            // Placeholder
            bool isEmpty = string.IsNullOrEmpty(_input.Text);
            if (isEmpty && !_hasFocus)
            {
                var ph = new Rectangle(PadLeft, 0, Width - PadLeft - IconWidth, Height);
                TextRenderer.DrawText(g, _placeholder, _input.Font, ph, WerTheme.PlaceholderColor,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
            }

            // Search icon (magnifying glass drawn with GDI+)
            DrawSearchIcon(g);
        }

        private void DrawSearchIcon(Graphics g)
        {
            var iconColor = _hasFocus ? WerTheme.InputBorderFocus : WerTheme.IconColor;

            int iconAreaX = Width - IconWidth;
            int cx        = iconAreaX + IconWidth / 2 - 1;
            int cy        = Height / 2 - 1;
            int r         = 7;   // circle radius
            int handleLen = 4;   // handle length

            using (var pen = new Pen(iconColor, 1.8f))
            {
                // Circle
                g.DrawEllipse(pen, cx - r, cy - r, r * 2, r * 2);

                // Handle (bottom-right)
                float angle  = 45f * (float)(Math.PI / 180);
                float hx     = cx + (float)(r * Math.Cos(angle));
                float hy     = cy + (float)(r * Math.Sin(angle));
                g.DrawLine(pen, hx, hy, hx + handleLen, hy + handleLen);
            }
        }

        // ── Mouse — click on icon triggers Search ────────────────────

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.X >= Width - IconWidth)
            {
                Search?.Invoke(this, EventArgs.Empty);
                _input.Focus();
            }
            base.OnMouseDown(e);
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
    }
}
