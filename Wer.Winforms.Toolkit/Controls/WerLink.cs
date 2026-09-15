using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    [ToolboxItem(true)]
    [Description("Hyperlink-style label that changes color on hover and shows a hand cursor.")]
    [DefaultEvent("Click")]
    [DefaultProperty("Text")]
    public class WerLink : Label
    {
        private Color _linkColor = WerTheme.PrimaryColor;
        private Color _hoverColor = Color.FromArgb(8, 86, 102);
        private Color _visitedColor = WerTheme.PrimaryColor;
        private bool _underlineOnHover = true;
        private bool _underline = false;
        private bool _isHovering;

        public WerLink()
        {
            AutoSize = true;
            Font = WerTheme.BodyFont;
            ForeColor = _linkColor;
            Cursor = Cursors.Hand;
            ApplyUnderline();
        }

        [Category("WerLink")]
        [Description("Link color in normal state.")]
        public Color LinkColor
        {
            get => _linkColor;
            set { _linkColor = value; if (!_isHovering) ForeColor = value; }
        }

        [Category("WerLink")]
        [Description("Link color on hover.")]
        public Color HoverColor
        {
            get => _hoverColor;
            set => _hoverColor = value;
        }

        [Category("WerLink")]
        [Description("Link color after visited (not tracked automatically).")]
        public Color VisitedColor
        {
            get => _visitedColor;
            set => _visitedColor = value;
        }

        [Category("WerLink")]
        [DefaultValue(false)]
        [Description("Always show underline.")]
        public bool Underline
        {
            get => _underline;
            set { _underline = value; ApplyUnderline(); }
        }

        [Category("WerLink")]
        [DefaultValue(true)]
        [Description("Show underline only on hover.")]
        public bool UnderlineOnHover
        {
            get => _underlineOnHover;
            set { _underlineOnHover = value; ApplyUnderline(); }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _isHovering = true;
            ForeColor = _hoverColor;
            ApplyUnderline();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _isHovering = false;
            ForeColor = _linkColor;
            ApplyUnderline();
            base.OnMouseLeave(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            ForeColor = Enabled ? _linkColor : WerTheme.DisabledColor;
            Cursor = Enabled ? Cursors.Hand : Cursors.Default;
            base.OnEnabledChanged(e);
        }

        private void ApplyUnderline()
        {
            bool show = _underline || (_underlineOnHover && _isHovering);
            var style = show ? FontStyle.Underline : FontStyle.Regular;
            if (Font.Style != style)
            {
                var old = Font;
                Font = new Font(Font.FontFamily, Font.Size, style);
                if (old != null && old != Font) old.Dispose();
            }
        }
    }
}
