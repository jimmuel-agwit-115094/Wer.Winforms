using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    [ToolboxItem(true)]
    [Description("Card-style group box with rounded border, header (heading + subheading) and body area.")]
    [DefaultProperty("HeadingText")]
    public class WerGroupBox : Panel
    {
        private string _headingText    = "Heading";
        private string _subheadingText = "Subheading";
        private int    _borderRadius   = 10;
        private int    _headerHeight   = 60;
        private Color  _dividerColor   = Color.FromArgb(220, 220, 220);
        private Color  _bodyBackColor  = Color.White;
        private Color  _headerBackColor = Color.White;
        private Color  _borderColor    = Color.FromArgb(200, 210, 220);

        private const int PadH            = 16;
        private const int HeadingTopPad   = 12;
        private const int SubheadingGap   = 2;
        private const int BodyInnerPad    = 8;

        public WerGroupBox()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.SupportsTransparentBackColor, true);

            BackColor = Color.Transparent;
            Size      = new Size(400, 200);
            UpdatePadding();
        }

        // ── Properties ────────────────────────────────────────────

        [Category("WerGroupBox")] [DefaultValue("Heading")]
        public string HeadingText
        {
            get => _headingText;
            set { _headingText = value; Invalidate(); }
        }

        [Category("WerGroupBox")] [DefaultValue("Subheading")]
        public string SubheadingText
        {
            get => _subheadingText;
            set { _subheadingText = value; Invalidate(); }
        }

        [Category("WerGroupBox")] [DefaultValue(10)]
        public int BorderRadius
        {
            get => _borderRadius;
            set { _borderRadius = Math.Max(0, value); Invalidate(); }
        }

        [Category("WerGroupBox")] [DefaultValue(60)]
        [Description("Height of the header area in pixels.")]
        public int HeaderHeight
        {
            get => _headerHeight;
            set { _headerHeight = Math.Max(30, value); UpdatePadding(); Invalidate(); }
        }

        [Category("WerGroupBox")]
        public Color BodyBackColor
        {
            get => _bodyBackColor;
            set { _bodyBackColor = value; Invalidate(); }
        }

        [Category("WerGroupBox")]
        public Color HeaderBackColor
        {
            get => _headerBackColor;
            set { _headerBackColor = value; Invalidate(); }
        }

        [Category("WerGroupBox")]
        public Color DividerColor
        {
            get => _dividerColor;
            set { _dividerColor = value; Invalidate(); }
        }

        [Category("WerGroupBox")]
        public Color BorderColor
        {
            get => _borderColor;
            set { _borderColor = value; Invalidate(); }
        }

        // ── Layout ────────────────────────────────────────────────

        // Push children below the header using Padding — no inner panel needed.
        private void UpdatePadding()
        {
            Padding = new Padding(BodyInnerPad, _headerHeight + BodyInnerPad, BodyInnerPad, BodyInnerPad);
        }

        // ── Paint ─────────────────────────────────────────────────

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode     = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            var outerRect = new Rectangle(1, 1, Width - 3, Height - 3);
            int radius    = Math.Min(_borderRadius, Math.Min(outerRect.Width, outerRect.Height) / 2);

            using (var path = RoundedRect(outerRect, radius))
            {
                g.SetClip(path);

                // Header fill
                using (var b = new SolidBrush(_headerBackColor))
                    g.FillRectangle(b, 0, 0, Width, _headerHeight);

                // Body fill
                using (var b = new SolidBrush(_bodyBackColor))
                    g.FillRectangle(b, 0, _headerHeight, Width, Height - _headerHeight);

                // Divider
                using (var p = new Pen(_dividerColor, 1f))
                    g.DrawLine(p, 0, _headerHeight, Width, _headerHeight);

                g.ResetClip();

                // Outer border
                using (var p = new Pen(_borderColor, 1f))
                    g.DrawPath(p, path);
            }

            // Heading
            if (!string.IsNullOrEmpty(_headingText))
            {
                using (var f = WerTheme.SubheadingFont)
                {
                    var r = new Rectangle(PadH, HeadingTopPad, Width - PadH * 2, 22);
                    TextRenderer.DrawText(g, _headingText, f, r, WerTheme.TextColor,
                        TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.SingleLine);
                }
            }

            // Subheading
            if (!string.IsNullOrEmpty(_subheadingText))
            {
                using (var sf = WerTheme.SubheadingFont)
                using (var cf = WerTheme.CaptionFont)
                {
                    int top = HeadingTopPad + (int)sf.GetHeight(g) + SubheadingGap;
                    var r   = new Rectangle(PadH, top, Width - PadH * 2, 18);
                    TextRenderer.DrawText(g, _subheadingText, cf, r, WerTheme.MutedColor,
                        TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.SingleLine);
                }
            }
        }

        private static GraphicsPath RoundedRect(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int d    = radius * 2;
            if (d <= 0) { path.AddRectangle(rect); return path; }
            path.AddArc(rect.X,          rect.Y,          d, d, 180, 90);
            path.AddArc(rect.Right - d,  rect.Y,          d, d, 270, 90);
            path.AddArc(rect.Right - d,  rect.Bottom - d, d, d,   0, 90);
            path.AddArc(rect.X,          rect.Bottom - d, d, d,  90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
