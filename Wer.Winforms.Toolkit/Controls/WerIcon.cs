using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    /// <summary>
    /// Icon control using Segoe MDL2 Assets (built into Windows 10/11).
    /// Usage: werIcon1.IconCode = WerIcons.Add;
    /// Or set IconCode in Properties panel to a Unicode char like "\uE710".
    /// </summary>
    [ToolboxItem(true)]
    [Description("Icon control using Segoe MDL2 Assets.")]
    [DefaultProperty("IconCode")]
    public class WerIcon : Control
    {
        private string _iconCode = "\uE710"; // default: Add
        private float _iconSize = 16f;

        private static readonly string IconFontFamily = "Segoe MDL2 Assets";

        public WerIcon()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.SupportsTransparentBackColor, true);

            BackColor = Color.Transparent;
            ForeColor = WerTheme.TextColor;
            Size = new Size(24, 24);
        }

        /// <summary>Unicode icon character. Use WerIcons constants or set directly.</summary>
        [Category("WerIcon")]
        [DefaultValue("\uE710")]
        [Description("Unicode icon code. Use WerIcons class for constants.")]
        public string IconCode
        {
            get => _iconCode;
            set { _iconCode = value; Invalidate(); }
        }

        /// <summary>Icon font size in points.</summary>
        [Category("WerIcon")]
        [DefaultValue(16f)]
        [Description("Icon size in points.")]
        public float IconSize
        {
            get => _iconSize;
            set { _iconSize = Math.Max(8, value); Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            using (var font = new Font(IconFontFamily, _iconSize, FontStyle.Regular))
            {
                TextRenderer.DrawText(g, _iconCode, font, ClientRectangle, ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
            }
        }
    }

    /// <summary>
    /// Common icon constants for Segoe MDL2 Assets.
    /// Usage: werIcon1.IconCode = WerIcons.Add;
    /// Full list: https://learn.microsoft.com/en-us/windows/apps/design/style/segoe-ui-symbol-font
    /// </summary>
    public static class WerIcons
    {
        // ── Actions ─────────────────────────────────────────────
        public const string Add = "\uE710";
        public const string Delete = "\uE74D";
        public const string Edit = "\uE70F";
        public const string Save = "\uE74E";
        public const string Cancel = "\uE711";
        public const string Close = "\uE8BB";
        public const string Check = "\uE73E";
        public const string Refresh = "\uE72C";
        public const string Undo = "\uE7A7";
        public const string Redo = "\uE7A6";
        public const string Copy = "\uE8C8";
        public const string Paste = "\uE77F";
        public const string Cut = "\uE8C6";
        public const string Print = "\uE749";
        public const string Share = "\uE72D";
        public const string Download = "\uE896";
        public const string Upload = "\uE898";
        public const string Send = "\uE724";
        public const string Search = "\uE721";
        public const string Filter = "\uE71C";
        public const string Sort = "\uE8CB";

        // ── Navigation ──────────────────────────────────────────
        public const string Back = "\uE72B";
        public const string Forward = "\uE72A";
        public const string Up = "\uE74A";
        public const string Down = "\uE74B";
        public const string Home = "\uE80F";
        public const string Menu = "\uE700";
        public const string More = "\uE712";
        public const string Settings = "\uE713";
        public const string ChevronLeft = "\uE76B";
        public const string ChevronRight = "\uE76C";
        public const string ChevronUp = "\uE70E";
        public const string ChevronDown = "\uE70D";

        // ── Status ──────────────────────────────────────────────
        public const string Info = "\uE946";
        public const string Warning = "\uE7BA";
        public const string Error = "\uEA39";
        public const string Success = "\uE73E";
        public const string Favorite = "\uE735";
        public const string FavoriteFilled = "\uE734";
        public const string Star = "\uE734";
        public const string StarEmpty = "\uE735";
        public const string Lock = "\uE72E";
        public const string Unlock = "\uE785";

        // ── Objects ─────────────────────────────────────────────
        public const string Person = "\uE77B";
        public const string People = "\uE716";
        public const string Mail = "\uE715";
        public const string Phone = "\uE717";
        public const string Calendar = "\uE787";
        public const string Clock = "\uE823";
        public const string Folder = "\uE8B7";
        public const string Document = "\uE8A5";
        public const string Photo = "\uE722";
        public const string Camera = "\uE722";
        public const string Globe = "\uE774";
        public const string Link = "\uE71B";
        public const string Attach = "\uE723";
        public const string Tag = "\uE8EC";
        public const string Flag = "\uE7C1";
        public const string Cart = "\uE7BF";
        public const string Money = "\uE7BF";
        public const string Database = "\uEE94";
        public const string Cloud = "\uE753";
        public const string Key = "\uE8D7";

        // ── View ────────────────────────────────────────────────
        public const string View = "\uE890";
        public const string Hide = "\uED1A";
        public const string FullScreen = "\uE740";
        public const string ExitFullScreen = "\uE73F";
        public const string ZoomIn = "\uE8A3";
        public const string ZoomOut = "\uE71F";
        public const string List = "\uE8FD";
        public const string Grid = "\uE80A";
    }
}
