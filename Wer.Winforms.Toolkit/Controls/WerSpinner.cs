using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    /// <summary>
    /// Animated loading spinner with a rotating arc.
    /// </summary>
    [ToolboxItem(true)]
    [Category("Wer Controls")]
    [Description("Animated loading spinner with a rotating arc. Set IsSpinning = true to start.")]
    [DefaultProperty("IsSpinning")]
    public class WerSpinner : Control
    {
        private bool  _isSpinning    = false;
        private Color _spinnerColor;
        private int   _thickness     = 3;
        private float _angle         = 0f;
        private Timer _timer;

        private const float ArcSweep     = 270f;
        private const float AngleStep    = 10f;
        private const int   TimerInterval = 30;

        public WerSpinner()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.SupportsTransparentBackColor,
                true);

            _spinnerColor = WerTheme.PrimaryColor;
            BackColor     = Color.Transparent;
            Size          = new Size(40, 40);

            _timer          = new Timer { Interval = TimerInterval };
            _timer.Tick    += OnTick;
        }

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

        private void OnThemeChanged(object sender, EventArgs e) => Invalidate();

        [Category("WerSpinner")]
        [DefaultValue(false)]
        [Description("Start or stop the spinning animation.")]
        public bool IsSpinning
        {
            get => _isSpinning;
            set
            {
                _isSpinning = value;
                if (_timer == null) return;
                if (value)
                    _timer.Start();
                else
                    _timer.Stop();
                Invalidate();
            }
        }

        [Category("WerSpinner")]
        [Description("Color of the spinning arc.")]
        public Color SpinnerColor
        {
            get => _spinnerColor;
            set { _spinnerColor = value; Invalidate(); }
        }

        [Category("WerSpinner")]
        [DefaultValue(3)]
        [Description("Stroke thickness of the arc in pixels.")]
        public int Thickness
        {
            get => _thickness;
            set { _thickness = Math.Max(1, value); Invalidate(); }
        }

        private void OnTick(object sender, EventArgs e)
        {
            _angle = (_angle + AngleStep) % 360f;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int margin = _thickness + 1;
            var rect   = new Rectangle(margin, margin, Width - margin * 2 - 1, Height - margin * 2 - 1);

            if (rect.Width <= 0 || rect.Height <= 0) return;

            // Track circle
            using (var pen = new Pen(WerTheme.ProgressTrack, _thickness))
                g.DrawEllipse(pen, rect);

            // Spinning arc
            using (var pen = new Pen(_spinnerColor, _thickness))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap   = LineCap.Round;
                g.DrawArc(pen, rect, _angle, ArcSweep);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _timer.Stop();
                _timer.Dispose();
                _timer = null;
            }
            base.Dispose(disposing);
        }
    }
}
