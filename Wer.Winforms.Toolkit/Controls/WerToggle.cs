using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    /// <summary>
    /// Modern toggle switch control. Produces a bool value.
    /// Usage: bool isOn = werToggle1.Checked;
    /// </summary>
    [ToolboxItem(true)]
    [Description("Modern toggle switch with label text.")]
    [DefaultEvent("CheckedChanged")]
    [DefaultProperty("Checked")]
    public class WerToggle : Control
    {
        private bool _checked;
        private bool _hovering;
        private float _animPos; // 0 = off, 1 = on

        private const int TrackW      = 44;
        private const int TrackH      = 24;
        private const int KnobSize    = 18;
        private const int KnobPad     = 3;
        private const int TextGap     = 8;

        private static readonly Color TrackOn      = Color.FromArgb(12, 124, 146);
        private static readonly Color TrackOff     = Color.FromArgb(170, 175, 182);
        private static readonly Color KnobColor    = Color.White;
        private static readonly Color TextColor    = Color.FromArgb(33, 37, 41);
        private static readonly Color DisabledTrack = Color.FromArgb(210, 212, 215);

        private Timer _animTimer;

        public event EventHandler CheckedChanged;

        public WerToggle()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.SupportsTransparentBackColor, true);

            BackColor = Color.Transparent;
            Font      = WerTheme.BodyFont;
            Size      = new Size(130, 26);
            Cursor    = Cursors.Hand;

            _animTimer = new Timer { Interval = 12 };
            _animTimer.Tick += OnAnimTick;
        }

        /// <summary>
        /// The toggle state. true = on, false = off.
        /// Usage: bool isOn = werToggle1.Checked;
        /// </summary>
        [Category("WerToggle")]
        [DefaultValue(false)]
        [Description("Toggle state. true = on, false = off.")]
        public bool Checked
        {
            get => _checked;
            set
            {
                if (_checked == value) return;
                _checked = value;
                _animPos = value ? 1f : 0f;
                Invalidate();
                CheckedChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        // ── Mouse ───────────────────────────────────────────────

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _hovering = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _hovering = false;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left && Enabled)
            {
                _checked = !_checked;
                _animTimer.Start();
                CheckedChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if ((e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter) && Enabled)
            {
                _checked = !_checked;
                _animTimer.Start();
                CheckedChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        // ── Animation ───────────────────────────────────────────

        private void OnAnimTick(object sender, EventArgs e)
        {
            float target = _checked ? 1f : 0f;
            float step = 0.15f;

            if (Math.Abs(_animPos - target) < step)
            {
                _animPos = target;
                _animTimer.Stop();
            }
            else
            {
                _animPos += _checked ? step : -step;
            }
            Invalidate();
        }

        // ── Paint ───────────────────────────────────────────────

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode     = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            int trackY = (Height - TrackH) / 2;

            // Track color interpolation
            Color trackColor;
            if (!Enabled)
                trackColor = DisabledTrack;
            else
            {
                int r = (int)(TrackOff.R + (TrackOn.R - TrackOff.R) * _animPos);
                int gr = (int)(TrackOff.G + (TrackOn.G - TrackOff.G) * _animPos);
                int b = (int)(TrackOff.B + (TrackOn.B - TrackOff.B) * _animPos);
                trackColor = Color.FromArgb(r, gr, b);
            }

            // Draw track (pill shape)
            var trackRect = new Rectangle(0, trackY, TrackW, TrackH);
            using (var path = PillPath(trackRect))
            using (var brush = new SolidBrush(trackColor))
                g.FillPath(brush, path);

            // Hover glow
            if (_hovering && Enabled)
            {
                using (var path = PillPath(trackRect))
                using (var pen = new Pen(Color.FromArgb(40, trackColor), 2f))
                    g.DrawPath(pen, path);
            }

            // Knob position
            float knobMinX = KnobPad;
            float knobMaxX = TrackW - KnobSize - KnobPad;
            float knobX = knobMinX + (knobMaxX - knobMinX) * _animPos;
            float knobY = trackY + (TrackH - KnobSize) / 2f;

            // Knob shadow
            using (var shadow = new SolidBrush(Color.FromArgb(30, 0, 0, 0)))
                g.FillEllipse(shadow, knobX + 0.5f, knobY + 1f, KnobSize, KnobSize);

            // Knob
            using (var brush = new SolidBrush(KnobColor))
                g.FillEllipse(brush, knobX, knobY, KnobSize, KnobSize);

            // Label text
            if (!string.IsNullOrEmpty(Text))
            {
                var textRect = new Rectangle(TrackW + TextGap, 0, Width - TrackW - TextGap, Height);
                var textColor = Enabled ? TextColor : Color.FromArgb(150, 150, 150);
                TextRenderer.DrawText(g, Text, Font, textRect, textColor,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix);
            }
        }

        private static GraphicsPath PillPath(Rectangle rect)
        {
            var path = new GraphicsPath();
            int r = rect.Height;
            path.AddArc(rect.X, rect.Y, r, r, 90, 180);
            path.AddArc(rect.Right - r, rect.Y, r, r, 270, 180);
            path.CloseFigure();
            return path;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _animTimer?.Dispose();
            base.Dispose(disposing);
        }
    }
}
