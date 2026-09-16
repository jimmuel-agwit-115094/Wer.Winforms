using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    /// <summary>
    /// Top navigation bar. Dock = Top. Shows user name and optional logout on the right.
    ///
    /// Usage:
    ///   werTopNav1.UserName = "John Doe";
    ///   werTopNav1.LogoutClicked += (s, e) => Application.Exit();
    /// </summary>
    [ToolboxItem(true)]
    [Description("Top navigation bar with user info and logout.")]
    [DefaultEvent("LogoutClicked")]
    public class WerTopNav : Control
    {
        private string _userName = "User";
        private string _pageTitle = "";
        private bool _showLogout = true;
        private bool _hoverLogout;

        private const int BarHeight = 48;
        private const int AvatarSize = 32;
        private const int Pad = 16;

        private static readonly Color BgColor = Color.White;
        private static readonly Color BorderColor = Color.FromArgb(232, 235, 240);
        private static readonly Color TitleColor = Color.FromArgb(33, 37, 41);
        private static readonly Color UserColor = Color.FromArgb(60, 70, 85);
        private static readonly Color LogoutColor = Color.FromArgb(150, 155, 165);
        private static readonly Color LogoutHover = Color.FromArgb(179, 58, 58);
        private static readonly Color AvatarBg = Color.FromArgb(12, 124, 146);

        public event EventHandler LogoutClicked;

        public WerTopNav()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);

            BackColor = BgColor;
            Dock = DockStyle.Top;
            Height = BarHeight;
        }

        [Category("WerTopNav")]
        [DefaultValue("User")]
        [Description("User name displayed on the right.")]
        public string UserName
        {
            get => _userName;
            set { _userName = value; Invalidate(); }
        }

        [Category("WerTopNav")]
        [DefaultValue("")]
        [Description("Page title displayed on the left.")]
        public string PageTitle
        {
            get => _pageTitle;
            set { _pageTitle = value; Invalidate(); }
        }

        [Category("WerTopNav")]
        [DefaultValue(true)]
        [Description("Show logout text on the right.")]
        public bool ShowLogout
        {
            get => _showLogout;
            set { _showLogout = value; Invalidate(); }
        }

        // ── Mouse ───────────────────────────────────────────────

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            bool wasHover = _hoverLogout;
            _hoverLogout = _showLogout && GetLogoutRect().Contains(e.Location);
            Cursor = _hoverLogout ? Cursors.Hand : Cursors.Default;
            if (wasHover != _hoverLogout) Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (_hoverLogout) { _hoverLogout = false; Invalidate(); }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (_hoverLogout)
                LogoutClicked?.Invoke(this, EventArgs.Empty);
        }

        private Rectangle GetLogoutRect()
        {
            using (var g = CreateGraphics())
            {
                int logoutW = TextRenderer.MeasureText(g, "Logout", WerTheme.BodyFont).Width + 8;
                return new Rectangle(Width - Pad - logoutW, 0, logoutW, Height);
            }
        }

        // ── Paint ───────────────────────────────────────────────

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            g.Clear(BgColor);

            // Bottom border
            using (var pen = new Pen(BorderColor, 1f))
                g.DrawLine(pen, 0, Height - 1, Width, Height - 1);

            // Page title (left)
            if (!string.IsNullOrEmpty(_pageTitle))
            {
                var titleRect = new Rectangle(Pad, 0, Width / 2, Height);
                using (var font = new Font(WerTheme.FontFamily, 11f, FontStyle.Bold))
                    TextRenderer.DrawText(g, _pageTitle, font, titleRect, TitleColor,
                        TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
            }

            // Right side: avatar circle + username + logout
            int rx = Width - Pad;

            // Logout
            if (_showLogout)
            {
                int logoutW = TextRenderer.MeasureText(g, "Logout", WerTheme.BodyFont).Width + 4;
                rx -= logoutW;
                var logoutRect = new Rectangle(rx, 0, logoutW, Height);
                var logoutCol = _hoverLogout ? LogoutHover : LogoutColor;
                TextRenderer.DrawText(g, "Logout", WerTheme.BodyFont, logoutRect, logoutCol,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
                rx -= 12; // gap
            }

            // Username
            if (!string.IsNullOrEmpty(_userName))
            {
                int nameW = TextRenderer.MeasureText(g, _userName, WerTheme.BodyFont).Width + 4;
                rx -= nameW;
                var nameRect = new Rectangle(rx, 0, nameW, Height);
                TextRenderer.DrawText(g, _userName, WerTheme.BodyFont, nameRect, UserColor,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
                rx -= 8; // gap
            }

            // Avatar circle — color based on first letter
            int ay = (Height - AvatarSize) / 2;
            rx -= AvatarSize;
            using (var brush = new SolidBrush(GetAvatarColor(_userName)))
                g.FillEllipse(brush, rx, ay, AvatarSize, AvatarSize);

            // Initials
            string initials = GetInitials(_userName);
            var avatarRect = new Rectangle(rx, ay, AvatarSize, AvatarSize);
            using (var font = new Font(WerTheme.FontFamily, 10f, FontStyle.Bold))
                TextRenderer.DrawText(g, initials, font, avatarRect, Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
        }

        private static string GetInitials(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "?";
            var parts = name.Trim().Split(' ');
            if (parts.Length >= 2)
                return (parts[0][0].ToString() + parts[parts.Length - 1][0].ToString()).ToUpper();
            return parts[0][0].ToString().ToUpper();
        }

        private static readonly Color[] AvatarColors = new[]
        {
            Color.FromArgb(211, 47, 47),    // A - Red
            Color.FromArgb(56, 142, 60),    // B - Green
            Color.FromArgb(25, 118, 210),   // C - Blue
            Color.FromArgb(230, 126, 34),   // D - Orange
            Color.FromArgb(142, 68, 173),   // E - Purple
            Color.FromArgb(0, 151, 136),    // F - Teal
            Color.FromArgb(194, 24, 91),    // G - Pink
            Color.FromArgb(121, 85, 72),    // H - Brown
            Color.FromArgb(38, 166, 154),   // I - Sea green
            Color.FromArgb(255, 143, 0),    // J - Amber
            Color.FromArgb(103, 58, 183),   // K - Deep purple
            Color.FromArgb(0, 137, 123),    // L - Dark teal
            Color.FromArgb(216, 67, 21),    // M - Deep orange
            Color.FromArgb(46, 125, 50),    // N - Dark green
            Color.FromArgb(21, 101, 192),   // O - Dark blue
            Color.FromArgb(156, 39, 176),   // P - Violet
            Color.FromArgb(244, 67, 54),    // Q - Bright red
            Color.FromArgb(33, 150, 243),   // R - Light blue
            Color.FromArgb(76, 175, 80),    // S - Light green
            Color.FromArgb(255, 87, 34),    // T - Orange red
            Color.FromArgb(63, 81, 181),    // U - Indigo
            Color.FromArgb(0, 188, 212),    // V - Cyan
            Color.FromArgb(139, 195, 74),   // W - Lime green
            Color.FromArgb(233, 30, 99),    // X - Deep pink
            Color.FromArgb(205, 220, 57),   // Y - Yellow green
            Color.FromArgb(96, 125, 139),   // Z - Blue grey
        };

        private static Color GetAvatarColor(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return AvatarColors[0];
            char first = char.ToUpper(name.Trim()[0]);
            int index = first - 'A';
            if (index < 0 || index >= 26) return AvatarColors[0];
            return AvatarColors[index];
        }
    }
}
