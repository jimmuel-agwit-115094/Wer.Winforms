using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    [ToolboxItem(true)]
    [Description("Tab control with custom-painted headers. Behaves exactly like the native TabControl — add TabPages in the designer, drop controls onto each page.")]
    [DefaultEvent("SelectedIndexChanged")]
    public class WerTabControl : TabControl
    {
        private int _hoverIndex = -1;

        private const int IndicatorH   = 3;
        private const int BorderRadius = 6;

        private static readonly Color BorderColor = Color.FromArgb(210, 215, 220);

        public WerTabControl()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);

            Font      = WerTheme.BodyFont;
            BackColor = Color.White;
            ItemSize  = new Size(0, 38);
            Padding   = new Point(16, 0);
        }

        // ── Paint ─────────────────────────────────────────────────────

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // Fill with parent color, then white rounded rect on top
            var g = pevent.Graphics;
            using (var brush = new SolidBrush(Parent != null ? Parent.BackColor : SystemColors.Control))
                g.FillRectangle(brush, ClientRectangle);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode     = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            if (TabCount == 0) return;

            // White rounded fill for entire control
            var outerRect = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var path = RoundedRect(outerRect, BorderRadius))
            using (var brush = new SolidBrush(Color.White))
                g.FillPath(brush, path);

            // Rounded border
            using (var path = RoundedRect(outerRect, BorderRadius))
            using (var pen = new Pen(BorderColor, 1f))
                g.DrawPath(pen, path);

            // Separator line
            var firstTab = GetTabRect(0);
            using (var pen = new Pen(Color.FromArgb(220, 220, 220), 1))
                g.DrawLine(pen, 1, firstTab.Bottom + 1, Width - 1, firstTab.Bottom + 1);

            // Tab headers
            for (int i = 0; i < TabCount; i++)
                DrawTab(g, i);
        }

        private void DrawTab(Graphics g, int index)
        {
            var  bounds   = GetTabRect(index);
            bool selected = index == SelectedIndex;
            bool hover    = index == _hoverIndex;

            // Tab background — white
            using (var brush = new SolidBrush(Color.White))
                g.FillRectangle(brush, bounds);

            // Hover tint
            if (hover && !selected)
                using (var brush = new SolidBrush(Color.FromArgb(15, WerTheme.PrimaryColor)))
                    g.FillRectangle(brush, bounds);

            // Label
            Font selectedFont = selected ? new Font(Font.FontFamily, Font.Size, FontStyle.Bold) : null;
            Font font = selectedFont ?? Font;
            Color color;
            if (selected)
                color = WerTheme.PrimaryColor;
            else if (hover)
                color = WerTheme.PrimaryColor;
            else
                color = Color.FromArgb(108, 117, 125);

            TextRenderer.DrawText(g, TabPages[index].Text, font, bounds, color,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                TextFormatFlags.SingleLine);

            // Active underline indicator
            if (selected)
                using (var brush = new SolidBrush(WerTheme.PrimaryColor))
                    g.FillRectangle(brush,
                        bounds.X, bounds.Bottom - IndicatorH,
                        bounds.Width, IndicatorH);

            selectedFont?.Dispose();
        }

        // ── Hover tracking ───────────────────────────────────────────

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            int h = -1;
            for (int i = 0; i < TabCount; i++)
                if (GetTabRect(i).Contains(e.Location)) { h = i; break; }
            Cursor = h >= 0 ? Cursors.Hand : Cursors.Default;
            if (h != _hoverIndex) { _hoverIndex = h; Invalidate(); }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _hoverIndex = -1;
            Invalidate();
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            Invalidate();
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            if (e.Control is TabPage tp)
                tp.BackColor = Color.White;
        }

        // ── Helpers ──────────────────────────────────────────────────

        private static GraphicsPath RoundedRect(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int d    = radius * 2;
            path.AddArc(rect.X,           rect.Y,          d, d, 180, 90);
            path.AddArc(rect.Right - d,   rect.Y,          d, d, 270, 90);
            path.AddArc(rect.Right - d,   rect.Bottom - d, d, d,   0, 90);
            path.AddArc(rect.X,           rect.Bottom - d, d, d,  90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
