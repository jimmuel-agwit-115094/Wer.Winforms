using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    [ToolboxItem(false)]
    [Description("Banner message with colored top border, icon, title, and body text.")]
    [DefaultProperty("Title")]
    public class WerBanner : Control
    {
        private WerBannerType _bannerType = WerBannerType.Informative;
        private string _title = "Title";
        private string _body  = "Banner text";

        private const int BorderRadius  = 6;
        private const int TopBarHeight  = 3;
        private const int PadLeft       = 16;
        private const int PadRight      = 16;
        private const int PadTop        = 14;
        private const int IconSize      = 22;
        private const int IconTextGap   = 10;
        private const int TitleBodyGap  = 2;
        private const int PadBottom     = 14;

        // ── Palette ──────────────────────────────────────────────────
        private static readonly Color InfoColor    = Color.FromArgb(12, 124, 146);   // teal
        private static readonly Color PositiveColor = Color.FromArgb(25, 135, 84);   // green
        private static readonly Color NoticeColor  = Color.FromArgb(200, 140, 20);   // amber
        private static readonly Color NegativeColor = Color.FromArgb(179, 58, 58);   // red

        private static readonly Color CardBorder = Color.FromArgb(210, 215, 220);
        private static readonly Color BodyColor  = Color.FromArgb(108, 117, 125);

        public WerBanner()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.SupportsTransparentBackColor, true);

            BackColor = Color.Transparent;
            Font      = WerTheme.BodyFont;
            Size      = new Size(400, 70);
        }

        // ── Properties ───────────────────────────────────────────────

        [Category("WerBanner")]
        [DefaultValue(WerBannerType.Informative)]
        [Description("Banner variant — controls the top bar color and icon.")]
        public WerBannerType BannerType
        {
            get => _bannerType;
            set { _bannerType = value; Invalidate(); }
        }

        [Category("WerBanner")]
        [DefaultValue("Title")]
        [Description("Bold title text.")]
        public string Title
        {
            get => _title;
            set { _title = value; Invalidate(); }
        }

        [Category("WerBanner")]
        [DefaultValue("Banner text")]
        [Description("Body / description text below the title.")]
        public string Body
        {
            get => _body;
            set { _body = value; Invalidate(); }
        }

        // ── Paint ────────────────────────────────────────────────────

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode     = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            var accentColor = GetAccentColor();
            var cardRect    = new Rectangle(0, 0, Width - 1, Height - 1);

            // ── Card background ──────────────────────────────────
            using (var path = RoundedRect(cardRect, BorderRadius))
            using (var brush = new SolidBrush(Color.White))
                g.FillPath(brush, path);

            // ── Card border ──────────────────────────────────────
            using (var path = RoundedRect(cardRect, BorderRadius))
            using (var pen  = new Pen(CardBorder, 1f))
                g.DrawPath(pen, path);

            // ── Colored top bar ──────────────────────────────────
            var topBarRect = new Rectangle(1, 1, Width - 3, TopBarHeight);
            using (var path = RoundedRectTop(topBarRect, BorderRadius - 1))
            using (var brush = new SolidBrush(accentColor))
                g.FillPath(brush, path);

            // ── Icon ─────────────────────────────────────────────
            int contentTop = TopBarHeight + PadTop;
            int iconX      = PadLeft;
            int iconY      = contentTop;
            DrawIcon(g, iconX, iconY, IconSize, accentColor);

            // ── Title (bold) ─────────────────────────────────────
            int textX = iconX + IconSize + IconTextGap;
            using (var boldFont = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
            {
                var titleSize = TextRenderer.MeasureText(g, _title, boldFont);
                var titleRect = new Rectangle(textX, contentTop, Width - textX - PadRight, titleSize.Height);
                TextRenderer.DrawText(g, _title, boldFont, titleRect, WerTheme.TextColor,
                    TextFormatFlags.Left | TextFormatFlags.SingleLine);

                // ── Body text ────────────────────────────────────
                int bodyY    = titleRect.Bottom + TitleBodyGap;
                var bodyRect = new Rectangle(textX, bodyY, Width - textX - PadRight, Height - bodyY - PadBottom);
                TextRenderer.DrawText(g, _body, Font, bodyRect, BodyColor,
                    TextFormatFlags.Left | TextFormatFlags.WordBreak);
            }
        }

        // ── Icon drawing ─────────────────────────────────────────────

        private void DrawIcon(Graphics g, int x, int y, int size, Color color)
        {
            var rect = new Rectangle(x, y, size, size);
            using (var pen = new Pen(color, 1.6f))
            {
                // Circle
                g.DrawEllipse(pen, rect);

                int cx = x + size / 2;
                int cy = y + size / 2;

                switch (_bannerType)
                {
                    case WerBannerType.Informative:
                        // "i" — dot + line
                        g.FillEllipse(new SolidBrush(color), cx - 1, cy - 5, 3, 3);
                        g.DrawLine(pen, cx, cy - 1, cx, cy + 6);
                        break;

                    case WerBannerType.Positive:
                        // checkmark
                        using (var checkPen = new Pen(color, 2f) { LineJoin = LineJoin.Round })
                            g.DrawLines(checkPen, new[]
                            {
                                new PointF(cx - 4, cy),
                                new PointF(cx - 1, cy + 3),
                                new PointF(cx + 5, cy - 4)
                            });
                        break;

                    case WerBannerType.Notice:
                        // "!" — line + dot (triangle would be complex; circle with ! matches)
                        g.DrawLine(pen, cx, cy - 5, cx, cy + 1);
                        g.FillEllipse(new SolidBrush(color), cx - 1, cy + 4, 3, 3);
                        break;

                    case WerBannerType.Negative:
                        // "!" — same exclamation
                        g.DrawLine(pen, cx, cy - 5, cx, cy + 1);
                        g.FillEllipse(new SolidBrush(color), cx - 1, cy + 4, 3, 3);
                        break;
                }
            }
        }

        // ── Helpers ──────────────────────────────────────────────────

        private Color GetAccentColor()
        {
            switch (_bannerType)
            {
                case WerBannerType.Positive:    return PositiveColor;
                case WerBannerType.Notice:      return NoticeColor;
                case WerBannerType.Negative:    return NegativeColor;
                case WerBannerType.Informative:
                default:                        return InfoColor;
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

        private static GraphicsPath RoundedRectTop(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddLine(rect.Right, rect.Y + radius, rect.Right, rect.Bottom);
            path.AddLine(rect.Right, rect.Bottom, rect.X, rect.Bottom);
            path.AddLine(rect.X, rect.Bottom, rect.X, rect.Y + radius);
            path.CloseFigure();
            return path;
        }
    }

    public enum WerBannerType
    {
        Informative,
        Positive,
        Notice,
        Negative
    }

    [ToolboxItem(true)]
    [Description("Informative banner — teal top bar with info icon.")]
    public class WerInfoBanner : WerBanner
    {
        public WerInfoBanner() { BannerType = WerBannerType.Informative; Title = "Informative"; }
    }

    [ToolboxItem(true)]
    [Description("Positive banner — green top bar with checkmark icon.")]
    public class WerPositiveBanner : WerBanner
    {
        public WerPositiveBanner() { BannerType = WerBannerType.Positive; Title = "Positive"; }
    }

    [ToolboxItem(true)]
    [Description("Notice banner — amber top bar with warning icon.")]
    public class WerNoticeBanner : WerBanner
    {
        public WerNoticeBanner() { BannerType = WerBannerType.Notice; Title = "Notice"; }
    }

    [ToolboxItem(true)]
    [Description("Negative banner — red top bar with error icon.")]
    public class WerNegativeBanner : WerBanner
    {
        public WerNegativeBanner() { BannerType = WerBannerType.Negative; Title = "Negative"; }
    }
}
