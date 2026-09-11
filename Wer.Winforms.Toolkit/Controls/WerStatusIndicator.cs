using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    public enum WerStatus
    {
        Neutral,
        Informative,
        Positive,
        Notice,
        Negative
    }

    [ToolboxItem(true)]
    [Description("Pill-shaped status badge with a colored dot and label.")]
    [DefaultProperty("Status")]
    public class WerStatusIndicator : Control
    {
        private WerStatus _status = WerStatus.Neutral;
        private const int DotSize = 8;
        private const int DotMarginLeft = 10;
        private const int DotTextGap = 6;
        private const int PaddingRight = 12;
        private const int BorderRadius = 20;

        // ── Palette ──────────────────────────────────────────────────────
        private static readonly Color NeutralBg   = Color.FromArgb(232, 232, 232);
        private static readonly Color NeutralDot  = Color.FromArgb(150, 150, 150);
        private static readonly Color NeutralText = Color.FromArgb(80,  80,  80);

        private static readonly Color InfoBg   = Color.FromArgb(219, 234, 254);
        private static readonly Color InfoDot  = Color.FromArgb(59,  130, 246);
        private static readonly Color InfoText = Color.FromArgb(30,  64,  175);

        private static readonly Color PosBg   = Color.FromArgb(220, 252, 231);
        private static readonly Color PosDot  = Color.FromArgb(34,  197, 94);
        private static readonly Color PosText = Color.FromArgb(20,  120, 50);

        private static readonly Color NoticeBg   = Color.FromArgb(254, 249, 195);
        private static readonly Color NoticeDot  = Color.FromArgb(234, 179, 8);
        private static readonly Color NoticeText = Color.FromArgb(113, 85,  0);

        private static readonly Color NegBg   = Color.FromArgb(254, 226, 226);
        private static readonly Color NegDot  = Color.FromArgb(239, 68,  68);
        private static readonly Color NegText = Color.FromArgb(153, 27,  27);

        public WerStatusIndicator()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.SupportsTransparentBackColor,
                true);

            Font = WerTheme.BodyFont;
            BackColor = Color.Transparent;
            Text = "Neutral";
            AutoSize = true;
        }

        [Category("WerStatusIndicator")]
        [DefaultValue(WerStatus.Neutral)]
        [Description("Visual style of the status badge.")]
        public WerStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                if (string.IsNullOrEmpty(Text) || IsDefaultLabel(Text))
                    Text = value.ToString();
                Invalidate();
                if (AutoSize) AutoSizeControl();
            }
        }

        public override bool AutoSize
        {
            get => base.AutoSize;
            set { base.AutoSize = value; if (value) AutoSizeControl(); }
        }

        private bool IsDefaultLabel(string t) =>
            t == "Neutral" || t == "Informative" || t == "Positive" ||
            t == "Notice"  || t == "Negative";

        protected override void OnTextChanged(System.EventArgs e)
        {
            base.OnTextChanged(e);
            if (AutoSize) AutoSizeControl();
            Invalidate();
        }

        protected override void OnFontChanged(System.EventArgs e)
        {
            base.OnFontChanged(e);
            if (AutoSize) AutoSizeControl();
        }

        private void AutoSizeControl()
        {
            using (var g = CreateGraphics())
            {
                var textSize = TextRenderer.MeasureText(g, Text, Font,
                    new Size(int.MaxValue, int.MaxValue),
                    TextFormatFlags.SingleLine);

                int w = DotMarginLeft + DotSize + DotTextGap + textSize.Width + PaddingRight;
                int h = System.Math.Max(textSize.Height + 8, 26);
                Size = new Size(w, h);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            GetPalette(out Color bg, out Color dot, out Color text);

            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            int radius = System.Math.Min(BorderRadius, Height / 2);

            // Pill background
            using (var path = RoundedRect(rect, radius))
            using (var brush = new SolidBrush(bg))
                g.FillPath(brush, path);

            // Dot
            int dotY = (Height - DotSize) / 2;
            using (var brush = new SolidBrush(dot))
                g.FillEllipse(brush, DotMarginLeft, dotY, DotSize, DotSize);

            // Label
            int textX = DotMarginLeft + DotSize + DotTextGap;
            var textRect = new Rectangle(textX, 0, Width - textX - PaddingRight, Height);
            TextRenderer.DrawText(g, Text, Font, textRect, text,
                TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
        }

        private void GetPalette(out Color bg, out Color dot, out Color text)
        {
            switch (_status)
            {
                case WerStatus.Informative:
                    bg = InfoBg; dot = InfoDot; text = InfoText; break;
                case WerStatus.Positive:
                    bg = PosBg; dot = PosDot; text = PosText; break;
                case WerStatus.Notice:
                    bg = NoticeBg; dot = NoticeDot; text = NoticeText; break;
                case WerStatus.Negative:
                    bg = NegBg; dot = NegDot; text = NegText; break;
                default:
                    bg = NeutralBg; dot = NeutralDot; text = NeutralText; break;
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
