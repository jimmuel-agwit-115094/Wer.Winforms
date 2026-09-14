using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    /// <summary>
    /// Navigation menu button — child of WerMenu.
    /// Associates a Form type to display in the content area when clicked.
    ///
    /// Usage:
    ///   werMenuButton1.Show&lt;CustomerForm&gt;();     // generic
    ///   werMenuButton1.Show(typeof(CustomerForm));  // type
    ///   werMenuButton1.Show(new CustomerForm());    // instance
    /// </summary>
    [ToolboxItem(true)]
    [Description("Navigation menu button for WerMenu.")]
    [DefaultEvent("Click")]
    [DefaultProperty("Text")]
    public class WerMenuButton : Control
    {
        private bool _isActive;
        private bool _isHovering;
        private Type _targetFormType;
        private Form _targetFormInstance;

        private const int ItemH = 40;
        private const int PadLeft = 16;
        private const int CornerRadius = 8;
        private const int IndicatorW = 3;

        private static readonly Color NormalBg = Color.White;
        private static readonly Color ActiveBg = Color.FromArgb(236, 247, 250);
        private static readonly Color HoverBg = Color.FromArgb(246, 248, 250);
        private static readonly Color NormalText = Color.FromArgb(75, 85, 100);
        private static readonly Color ActiveText = Color.FromArgb(12, 124, 146);
        private static readonly Color IndicatorColor = Color.FromArgb(12, 124, 146);

        public WerMenuButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);

            BackColor = NormalBg;
            Font = WerTheme.BodyFont;
            Size = new Size(200, ItemH);
            Cursor = Cursors.Hand;
            Dock = DockStyle.Top;
        }

        // ── Properties ──────────────────────────────────────────

        /// <summary>Whether this button is the currently active/selected one.</summary>
        [Browsable(false)]
        public bool IsActive
        {
            get => _isActive;
            set { _isActive = value; Invalidate(); }
        }

        /// <summary>The Form type to display when clicked. Set via Show&lt;T&gt;() or Show(type).</summary>
        [Browsable(false)]
        public Type TargetFormType => _targetFormType;

        /// <summary>Cached form instance for reuse.</summary>
        [Browsable(false)]
        public Form TargetFormInstance
        {
            get => _targetFormInstance;
            internal set => _targetFormInstance = value;
        }

        // ── Show API ────────────────────────────────────────────

        /// <summary>
        /// Associate a Form type with this button. Form created on first click, reused after.
        /// Usage: werMenuButton1.Show&lt;CustomerForm&gt;();
        /// </summary>
        public void Show<TForm>() where TForm : Form, new()
        {
            _targetFormType = typeof(TForm);
            _targetFormInstance = null;
        }

        /// <summary>
        /// Associate a Form type with this button.
        /// Usage: werMenuButton1.Show(typeof(CustomerForm));
        /// </summary>
        public void Show(Type formType)
        {
            if (formType == null) return;
            if (!typeof(Form).IsAssignableFrom(formType))
                throw new ArgumentException("Type must derive from Form.", nameof(formType));
            _targetFormType = formType;
            _targetFormInstance = null;
        }

        /// <summary>
        /// Associate a specific Form instance with this button.
        /// Usage: werMenuButton1.Show(new CustomerForm());
        /// </summary>
        public void Show(Form formInstance)
        {
            if (formInstance == null) return;
            _targetFormType = formInstance.GetType();
            _targetFormInstance = formInstance;
        }

        /// <summary>
        /// No-op if no target configured. Safe to call always.
        /// If target is set, triggers navigation via parent WerMenu.
        /// </summary>
        public new void Show()
        {
            // Trigger click programmatically — WerMenu handles the rest
            OnClick(EventArgs.Empty);
        }

        /// <summary>Get or create the target form instance.</summary>
        internal Form GetOrCreateForm()
        {
            if (_targetFormType == null) return null;

            if (_targetFormInstance != null && !_targetFormInstance.IsDisposed)
                return _targetFormInstance;

            _targetFormInstance = (Form)Activator.CreateInstance(_targetFormType);
            return _targetFormInstance;
        }

        // ── Mouse ───────────────────────────────────────────────

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _isHovering = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _isHovering = false;
            Invalidate();
        }

        // ── Paint ───────────────────────────────────────────────

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            var rect = new Rectangle(4, 2, Width - 8, Height - 4);

            // Background
            if (_isActive)
            {
                using (var path = RoundedRect(rect, CornerRadius))
                using (var brush = new SolidBrush(ActiveBg))
                    g.FillPath(brush, path);

                // Left indicator
                var barRect = new Rectangle(1, 8, IndicatorW, Height - 16);
                using (var path = RoundedRect(barRect, 2))
                using (var brush = new SolidBrush(IndicatorColor))
                    g.FillPath(brush, path);
            }
            else if (_isHovering)
            {
                using (var path = RoundedRect(rect, CornerRadius))
                using (var brush = new SolidBrush(HoverBg))
                    g.FillPath(brush, path);
            }
            else
            {
                g.Clear(NormalBg);
            }

            // Text
            var textRect = new Rectangle(PadLeft, 0, Width - PadLeft - 8, Height);
            var textCol = _isActive ? ActiveText : NormalText;
            var fontStyle = _isActive ? FontStyle.Bold : FontStyle.Regular;
            using (var font = new Font(WerTheme.FontFamily, 9.75f, fontStyle))
                TextRenderer.DrawText(g, Text, font, textRect, textCol,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
                    TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_targetFormInstance != null && !_targetFormInstance.IsDisposed)
                {
                    _targetFormInstance.Close();
                    _targetFormInstance.Dispose();
                }
                _targetFormInstance = null;
                _targetFormType = null;
            }
            base.Dispose(disposing);
        }
    }
}
