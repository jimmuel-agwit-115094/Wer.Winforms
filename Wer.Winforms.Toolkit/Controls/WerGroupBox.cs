using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    [ToolboxItem(true)]
    [Description("Card-style group box with rounded border, header area (heading + subheading) and body area separated by a divider line.")]
    [DefaultProperty("HeadingText")]
    public class WerGroupBox : Panel
    {
        private string _headingText = "Heading";
        private string _subheadingText = "Subheading";
        private int _borderRadius = 10;
        private int _headerHeight = 60;
        private Color _dividerColor = Color.FromArgb(220, 220, 220);
        private Color _bodyBackColor = Color.White;
        private Color _headerBackColor = Color.White;
        private Color _borderColor = Color.FromArgb(200, 210, 220);

        // Internal panel that hosts child controls in the body area
        private readonly Panel _bodyPanel;

        private const int Padding_H = 16;
        private const int HeadingTopPad = 12;
        private const int SubheadingGap = 2;

        public WerGroupBox()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.SupportsTransparentBackColor,
                true);

            BackColor = Color.Transparent;
            Size = new Size(400, 160);

            _bodyPanel = new Panel
            {
                BackColor = _bodyBackColor,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };
            Controls.Add(_bodyPanel);
            LayoutInternals();
        }

        // ── Public properties ─────────────────────────────────────

        [Category("WerGroupBox")]
        [DefaultValue("Heading")]
        [Description("Bold heading text in the header area.")]
        public string HeadingText
        {
            get => _headingText;
            set { _headingText = value; Invalidate(); }
        }

        [Category("WerGroupBox")]
        [DefaultValue("Subheading")]
        [Description("Muted subheading text below the heading.")]
        public string SubheadingText
        {
            get => _subheadingText;
            set { _subheadingText = value; Invalidate(); }
        }

        [Category("WerGroupBox")]
        [DefaultValue(10)]
        [Description("Corner radius of the outer border.")]
        public int BorderRadius
        {
            get => _borderRadius;
            set { _borderRadius = Math.Max(0, value); Invalidate(); }
        }

        [Category("WerGroupBox")]
        [DefaultValue(60)]
        [Description("Height of the header area in pixels.")]
        public int HeaderHeight
        {
            get => _headerHeight;
            set { _headerHeight = Math.Max(30, value); LayoutInternals(); Invalidate(); }
        }

        [Category("WerGroupBox")]
        [Description("Background color of the body area.")]
        public Color BodyBackColor
        {
            get => _bodyBackColor;
            set { _bodyBackColor = value; _bodyPanel.BackColor = value; Invalidate(); }
        }

        [Category("WerGroupBox")]
        [Description("Background color of the header area.")]
        public Color HeaderBackColor
        {
            get => _headerBackColor;
            set { _headerBackColor = value; Invalidate(); }
        }

        [Category("WerGroupBox")]
        [Description("Color of the divider line between header and body.")]
        public Color DividerColor
        {
            get => _dividerColor;
            set { _dividerColor = value; Invalidate(); }
        }

        [Category("WerGroupBox")]
        [Description("Color of the outer rounded border.")]
        public new Color BorderColor
        {
            get => _borderColor;
            set { _borderColor = value; Invalidate(); }
        }

        /// <summary>
        /// The body panel where child controls should be placed.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Panel BodyPanel => _bodyPanel;

        // ── Layout ────────────────────────────────────────────────

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            LayoutInternals();
        }

        private void LayoutInternals()
        {
            if (_bodyPanel == null) return;

            int bodyTop = _headerHeight + 2; // +2 for divider + inset
            _bodyPanel.SetBounds(2, bodyTop, Width - 4, Height - bodyTop - 3);
        }

        // ── Paint ─────────────────────────────────────────────────

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            var outerRect = new Rectangle(1, 1, Width - 3, Height - 3);
            int radius = Math.Min(_borderRadius, Math.Min(outerRect.Width, outerRect.Height) / 2);

            using (var outerPath = CreateRoundedRect(outerRect, radius))
            {
                // Clip to rounded rect so fills don't bleed
                g.SetClip(outerPath);

                // Header background
                var headerRect = new Rectangle(0, 0, Width, _headerHeight);
                using (var brush = new SolidBrush(_headerBackColor))
                    g.FillRectangle(brush, headerRect);

                // Body background
                var bodyRect = new Rectangle(0, _headerHeight, Width, Height - _headerHeight);
                using (var brush = new SolidBrush(_bodyBackColor))
                    g.FillRectangle(brush, bodyRect);

                // Divider line
                using (var pen = new Pen(_dividerColor, 1f))
                    g.DrawLine(pen, 0, _headerHeight, Width, _headerHeight);

                g.ResetClip();

                // Outer border
                using (var pen = new Pen(_borderColor, 1f))
                    g.DrawPath(pen, outerPath);
            }

            // Draw heading text
            if (!string.IsNullOrEmpty(_headingText))
            {
                var headingRect = new Rectangle(Padding_H, HeadingTopPad, Width - Padding_H * 2, 20);
                TextRenderer.DrawText(g, _headingText, WerTheme.SubheadingFont, headingRect, WerTheme.TextColor,
                    TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.SingleLine);
            }

            // Draw subheading text
            if (!string.IsNullOrEmpty(_subheadingText))
            {
                int headingBottom = HeadingTopPad + (int)WerTheme.SubheadingFont.GetHeight(g) + SubheadingGap;
                var subRect = new Rectangle(Padding_H, headingBottom, Width - Padding_H * 2, 18);
                TextRenderer.DrawText(g, _subheadingText, WerTheme.CaptionFont, subRect, WerTheme.MutedColor,
                    TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.SingleLine);
            }
        }

        private static GraphicsPath CreateRoundedRect(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;
            if (d <= 0)
            {
                path.AddRectangle(rect);
                return path;
            }
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
