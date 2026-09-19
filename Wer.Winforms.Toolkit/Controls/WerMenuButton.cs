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
    [Description("Navigation menu button for WerLeftNavMenu. Add SubItems in Properties for accordion sub-menu.")]
    [DefaultEvent("Click")]
    [DefaultProperty("Text")]
    public class WerMenuButton : Control
    {
        // ── State ────────────────────────────────────────────────
        private bool _isActive;
        private bool _isHovering;
        private bool _isExpanded;
        private bool _isSubItem;
        private bool _hasActiveChild;
        private Type _targetFormType;
        private Form _targetFormInstance;
        private readonly List<SubMenuItem> _subItems = new List<SubMenuItem>();

        // ── Layout ───────────────────────────────────────────────
        private const int ItemH       = 40;
        private const int SubItemH    = 36;
        private const int PadLeft     = 16;
        private const int SubPadLeft  = 28;
        private const int CornerRadius = 8;
        private const int IndicatorW  = 3;
        private const int ChevronW    = 24;

        // ── Colors ───────────────────────────────────────────────
        private static readonly Color NormalBg      = Color.White;
        private static readonly Color ActiveBg      = Color.FromArgb(236, 247, 250);
        private static readonly Color HoverBg       = Color.FromArgb(246, 248, 250);
        private static readonly Color SubHoverBg    = Color.FromArgb(241, 248, 251);
        private static readonly Color NormalText    = Color.FromArgb(75, 85, 100);
        private static readonly Color ActiveText    = Color.FromArgb(12, 124, 146);
        private static readonly Color SubNormalText = Color.FromArgb(90, 100, 115);
        private static readonly Color IndicatorColor = Color.FromArgb(12, 124, 146);
        private static readonly Color ChevronColor  = Color.FromArgb(150, 160, 170);

        public WerMenuButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
            BackColor = NormalBg;
            Font      = WerTheme.BodyFont;
            Size      = new Size(200, ItemH);
            Cursor    = Cursors.Hand;
            Dock      = DockStyle.Top;
        }

        // ── Public properties (designer-visible) ─────────────────

        /// <summary>Sub-menu items. Configure Text in the Collection Editor; set Action in code.</summary>
        [Category("WerMenuButton")]
        [Description("Sub-menu items shown when this button is expanded. Set Action on each item in code.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<SubMenuItem> SubItems => _subItems;

        // ── Internal state (not browsable) ───────────────────────

        [Browsable(false)]
        public bool IsActive
        {
            get => _isActive;
            set { _isActive = value; Invalidate(); }
        }

        [Browsable(false)]
        public bool IsExpanded
        {
            get => _isExpanded;
            set { _isExpanded = value; Invalidate(); }
        }

        [Browsable(false)]
        public bool IsSubItem
        {
            get => _isSubItem;
            set { _isSubItem = value; Height = _isSubItem ? SubItemH : ItemH; Invalidate(); }
        }

        [Browsable(false)]
        public bool HasActiveChild
        {
            get => _hasActiveChild;
            set { _hasActiveChild = value; Invalidate(); }
        }

        [Browsable(false)]
        public bool HasSubItems => _subItems.Count > 0;

        // Internal metadata set by WerLeftNavMenu on injected sub-item buttons
        internal WerMenuButton ParentButton { get; set; }
        internal SubMenuItem   SourceItem   { get; set; }

        // ── Form navigation (backward-compat) ────────────────────

        [Browsable(false)]
        public Type TargetFormType => _targetFormType;

        [Browsable(false)]
        public Form TargetFormInstance
        {
            get => _targetFormInstance;
            internal set => _targetFormInstance = value;
        }

        public void Show<TForm>() where TForm : Form, new()
        {
            _targetFormType = typeof(TForm);
            _targetFormInstance = null;
        }

        public void Show(Type formType)
        {
            if (formType == null) return;
            if (!typeof(Form).IsAssignableFrom(formType))
                throw new ArgumentException("Type must derive from Form.", nameof(formType));
            _targetFormType = formType;
            _targetFormInstance = null;
        }

        public void Show(Form formInstance)
        {
            if (formInstance == null) return;
            _targetFormType = formInstance.GetType();
            _targetFormInstance = formInstance;
        }

        public new void Show() => OnClick(EventArgs.Empty);

        internal Form GetOrCreateForm()
        {
            if (_targetFormType == null) return null;
            if (_targetFormInstance != null && !_targetFormInstance.IsDisposed)
                return _targetFormInstance;
            _targetFormInstance = (Form)Activator.CreateInstance(_targetFormType);
            return _targetFormInstance;
        }

        // ── Mouse ────────────────────────────────────────────────

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

        // ── Paint ────────────────────────────────────────────────

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode     = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            var rect = new Rectangle(2, 1, Width - 4, Height - 2);

            if (_isSubItem) PaintSubItem(g, rect);
            else            PaintParent(g, rect);
        }

        private void PaintParent(Graphics g, Rectangle rect)
        {
            bool showActive = _isActive || _hasActiveChild;

            if (showActive)
            {
                using (var path = RoundedRect(rect, CornerRadius))
                using (var b = new SolidBrush(ActiveBg))
                    g.FillPath(b, path);

                var bar = new Rectangle(0, 4, IndicatorW, Height - 8);
                using (var path = RoundedRect(bar, 2))
                using (var b = new SolidBrush(IndicatorColor))
                    g.FillPath(b, path);
            }
            else if (_isHovering)
            {
                using (var path = RoundedRect(rect, CornerRadius))
                using (var b = new SolidBrush(HoverBg))
                    g.FillPath(b, path);
            }
            else
            {
                g.Clear(NormalBg);
            }

            // Chevron when sub-items exist
            if (HasSubItems)
            {
                string ch = _isExpanded ? "▾" : "▸";
                var chevRect = new Rectangle(Width - ChevronW - 4, 0, ChevronW, Height);
                using (var f = new Font(WerTheme.FontFamily, 9f, FontStyle.Regular))
                    TextRenderer.DrawText(g, ch, f, chevRect, ChevronColor,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                        TextFormatFlags.NoPrefix);
            }

            int textRight = HasSubItems ? ChevronW + 8 : 8;
            var textRect  = new Rectangle(PadLeft, 0, Width - PadLeft - textRight, Height);
            var textColor = showActive ? ActiveText : NormalText;
            var style     = (showActive && _isActive) ? FontStyle.Bold : FontStyle.Regular;
            using (var f = new Font(WerTheme.FontFamily, 9.75f, style))
                TextRenderer.DrawText(g, Text, f, textRect, textColor,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
                    TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
        }

        private void PaintSubItem(Graphics g, Rectangle rect)
        {
            if (_isActive)
            {
                using (var path = RoundedRect(rect, CornerRadius))
                using (var b = new SolidBrush(ActiveBg))
                    g.FillPath(b, path);
            }
            else if (_isHovering)
            {
                using (var path = RoundedRect(rect, CornerRadius))
                using (var b = new SolidBrush(SubHoverBg))
                    g.FillPath(b, path);
            }
            else
            {
                g.Clear(NormalBg);
            }

            // Vertical connector line (left rail)
            using (var pen = new Pen(Color.FromArgb(210, 218, 226), 1f))
                g.DrawLine(pen, 14, 0, 14, Height);

            // Dot connector
            int dotY = Height / 2;
            int dotX = 10;
            using (var b = new SolidBrush(_isActive ? IndicatorColor : Color.FromArgb(190, 200, 210)))
                g.FillEllipse(b, dotX, dotY - 3, 6, 6);

            var textRect  = new Rectangle(SubPadLeft, 0, Width - SubPadLeft - 8, Height);
            var textColor = _isActive ? ActiveText : SubNormalText;
            var style     = _isActive ? FontStyle.Bold : FontStyle.Regular;
            using (var f = new Font(WerTheme.FontFamily, 9.25f, style))
                TextRenderer.DrawText(g, Text, f, textRect, textColor,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
                    TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
        }

        protected override void OnResize(EventArgs e) { base.OnResize(e); Invalidate(); }

        // ── Helpers ──────────────────────────────────────────────

        private static GraphicsPath RoundedRect(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(rect.X,         rect.Y,          d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y,          d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d,   0, 90);
            path.AddArc(rect.X,         rect.Bottom - d, d, d,  90, 90);
            path.CloseFigure();
            return path;
        }

        // ── Dispose ──────────────────────────────────────────────

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
                _targetFormType     = null;
            }
            base.Dispose(disposing);
        }
    }
}
