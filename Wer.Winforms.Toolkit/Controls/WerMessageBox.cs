using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    public enum WerMessageIcon { None, Info, Success, Warning, Error }
    public enum WerMessageButtons { OK, OKCancel, YesNo, YesNoCancel }

    /// <summary>
    /// Modern styled message box with dimmed overlay.
    ///
    /// Usage:
    ///   WerMessageBox.Success.Show("Record saved.");
    ///   WerMessageBox.Error.Show("Something went wrong.", "Oops");
    ///   WerMessageBox.Warning.Show("Delete this?", "Confirm", WerMessageButtons.YesNo);
    ///   WerMessageBox.Info.Show("Version 1.0");
    ///
    /// var result = WerMessageBox.Warning.Show("Continue?", buttons: WerMessageButtons.OKCancel);
    /// if (result == DialogResult.OK) { ... }
    /// </summary>
    public static class WerMessageBox
    {
        public static readonly WerMessageBoxType Success = new WerMessageBoxType(WerMessageIcon.Success, "Success");
        public static readonly WerMessageBoxType Error = new WerMessageBoxType(WerMessageIcon.Error, "Error");
        public static readonly WerMessageBoxType Warning = new WerMessageBoxType(WerMessageIcon.Warning, "Warning");
        public static readonly WerMessageBoxType Info = new WerMessageBoxType(WerMessageIcon.Info, "Information");

        /// <summary>Show with no icon.</summary>
        public static DialogResult Show(string message, string title = "Message",
            WerMessageButtons buttons = WerMessageButtons.OK, IWin32Window owner = null)
        {
            return ShowInternal(message, title, WerMessageIcon.None, buttons, owner);
        }

        internal static DialogResult ShowInternal(string message, string title,
            WerMessageIcon icon, WerMessageButtons buttons, IWin32Window owner)
        {
            Form ownerForm = null;
            if (owner is Form f) ownerForm = f;
            else if (owner is Control c) ownerForm = c.FindForm();
            if (ownerForm == null) ownerForm = Form.ActiveForm;

            Form overlay = null;
            if (ownerForm != null)
            {
                overlay = new Form
                {
                    FormBorderStyle = FormBorderStyle.None,
                    BackColor = Color.Black,
                    Opacity = 0.35,
                    ShowInTaskbar = false,
                    StartPosition = FormStartPosition.Manual,
                    Location = ownerForm.PointToScreen(Point.Empty),
                    Size = ownerForm.ClientSize,
                };
                overlay.Show(ownerForm);
            }

            DialogResult result;
            using (var dlg = new WerMessageForm(message, title, icon, buttons))
            {
                result = overlay != null ? dlg.ShowDialog(overlay) : dlg.ShowDialog();
            }

            if (overlay != null)
            {
                overlay.Close();
                overlay.Dispose();
            }

            return result;
        }
    }

    /// <summary>Typed message box accessor (Success, Error, Warning, Info).</summary>
    public class WerMessageBoxType
    {
        private readonly WerMessageIcon _icon;
        private readonly string _defaultTitle;

        internal WerMessageBoxType(WerMessageIcon icon, string defaultTitle)
        {
            _icon = icon;
            _defaultTitle = defaultTitle;
        }

        /// <summary>
        /// Show message box with this icon type.
        /// Title defaults to the type name (e.g. "Success") but can be overridden.
        /// </summary>
        public DialogResult Show(string message, string title = null,
            WerMessageButtons buttons = WerMessageButtons.OK, IWin32Window owner = null)
        {
            return WerMessageBox.ShowInternal(message, title ?? _defaultTitle, _icon, buttons, owner);
        }
    }

    internal class WerMessageForm : Form
    {
        private readonly string _message;
        private readonly string _title;
        private readonly WerMessageIcon _icon;
        private readonly WerMessageButtons _buttons;

        private const int DialogW = 480;
        private const int Pad = 24;
        private const int IconSize = 28;
        private const int IconGap = 10;
        private const int TitleH = 24;
        private const int MsgGap = 12;
        private const int BtnW = 100;
        private const int BtnH = 36;
        private const int BtnGap = 10;
        private const int BtnAreaH = 58;
        private const int CornerRadius = 12;

        private static readonly Color BgColor = Color.White;
        private static readonly Color TitleColor = Color.FromArgb(33, 37, 41);
        private static readonly Color MessageColor = Color.FromArgb(100, 110, 120);
        private static readonly Color BorderColor = Color.FromArgb(225, 228, 232);
        private static readonly Color SepColor = Color.FromArgb(235, 238, 242);

        private static readonly Color InfoColor = Color.FromArgb(12, 124, 146);
        private static readonly Color SuccessColor = Color.FromArgb(40, 167, 69);
        private static readonly Color WarningColor = Color.FromArgb(230, 126, 34);
        private static readonly Color ErrorColor = Color.FromArgb(179, 58, 58);

        public WerMessageForm(string message, string title, WerMessageIcon icon, WerMessageButtons buttons)
        {
            _message = message;
            _title = title;
            _icon = icon;
            _buttons = buttons;

            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            BackColor = BgColor;
            DoubleBuffered = true;
            KeyPreview = true;

            CalculateSize();
            Region = CreateRoundedRegion(new Rectangle(0, 0, Width, Height), CornerRadius);
            CreateButtons();

            KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape)
                {
                    DialogResult = _buttons == WerMessageButtons.OK ? DialogResult.OK : DialogResult.Cancel;
                    Close();
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    DialogResult = (_buttons == WerMessageButtons.YesNo || _buttons == WerMessageButtons.YesNoCancel)
                        ? DialogResult.Yes : DialogResult.OK;
                    Close();
                }
            };
        }

        private void CalculateSize()
        {
            using (var g = CreateGraphics())
            {
                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                int textAreaW = DialogW - Pad * 2;
                using (var font = new Font(WerTheme.FontFamily, 9.75f, FontStyle.Regular))
                {
                    var msgSize = TextRenderer.MeasureText(g, _message, font,
                        new Size(textAreaW, 300),
                        TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix);
                    int contentH = Pad + TitleH + MsgGap + Math.Max(msgSize.Height, 20) + MsgGap + 1 + BtnAreaH;
                    Size = new Size(DialogW, Math.Max(170, contentH));
                }
            }
        }

        private void CreateButtons()
        {
            string[] labels;
            DialogResult[] results;

            switch (_buttons)
            {
                case WerMessageButtons.OKCancel:
                    labels = new[] { "Cancel", "OK" };
                    results = new[] { DialogResult.Cancel, DialogResult.OK };
                    break;
                case WerMessageButtons.YesNo:
                    labels = new[] { "No", "Yes" };
                    results = new[] { DialogResult.No, DialogResult.Yes };
                    break;
                case WerMessageButtons.YesNoCancel:
                    labels = new[] { "Cancel", "No", "Yes" };
                    results = new[] { DialogResult.Cancel, DialogResult.No, DialogResult.Yes };
                    break;
                default:
                    labels = new[] { "OK" };
                    results = new[] { DialogResult.OK };
                    break;
            }

            int totalBtnW = labels.Length * BtnW + (labels.Length - 1) * BtnGap;
            int startX = Width - Pad - totalBtnW;
            int btnY = Height - BtnAreaH + (BtnAreaH - BtnH) / 2;

            for (int i = 0; i < labels.Length; i++)
            {
                bool isPrimary = i == labels.Length - 1;
                var btn = isPrimary
                    ? (WerButton)new WerButtonPrimary()
                    : new WerButtonOutlinedPrimary();

                btn.Text = labels[i];
                btn.Size = new Size(BtnW, BtnH);
                btn.Location = new Point(startX + i * (BtnW + BtnGap), btnY);
                btn.Font = new Font(WerTheme.FontFamily, 9.75f, FontStyle.Regular);

                var result = results[i];
                btn.Click += (s, e) => { DialogResult = result; Close(); };
                Controls.Add(btn);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            var cardRect = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var path = RoundedRect(cardRect, CornerRadius))
            {
                using (var brush = new SolidBrush(BgColor))
                    g.FillPath(brush, path);
                using (var pen = new Pen(BorderColor, 1f))
                    g.DrawPath(pen, path);
            }

            int x = Pad;
            int y = Pad;
            int textX = x;

            if (_icon != WerMessageIcon.None)
            {
                DrawIcon(g, x, y - 1);
                textX = x + IconSize + IconGap;
            }

            using (var font = new Font(WerTheme.FontFamily, 11f, FontStyle.Bold))
                TextRenderer.DrawText(g, _title, font,
                    new Rectangle(textX, y, Width - Pad - textX, TitleH), TitleColor,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            y += TitleH + MsgGap;

            using (var font = new Font(WerTheme.FontFamily, 9.75f, FontStyle.Regular))
            {
                var msgRect = new Rectangle(x, y, Width - Pad - x, Height - BtnAreaH - y - 4);
                TextRenderer.DrawText(g, _message, font, msgRect, MessageColor,
                    TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix);
            }

            int sepY = Height - BtnAreaH;
            using (var pen = new Pen(SepColor, 1f))
                g.DrawLine(pen, 1, sepY, Width - 2, sepY);
        }

        private void DrawIcon(Graphics g, int x, int y)
        {
            Color iconColor;
            string symbol;
            switch (_icon)
            {
                case WerMessageIcon.Info:    iconColor = InfoColor;    symbol = "i"; break;
                case WerMessageIcon.Success: iconColor = SuccessColor; symbol = "✓"; break;
                case WerMessageIcon.Warning: iconColor = WarningColor; symbol = "!"; break;
                case WerMessageIcon.Error:   iconColor = ErrorColor;   symbol = "✕"; break;
                default: return;
            }

            var rect = new Rectangle(x, y, IconSize, IconSize);
            using (var brush = new SolidBrush(iconColor))
                g.FillEllipse(brush, rect);
            using (var font = new Font(WerTheme.FontFamily, 11f, FontStyle.Bold))
                TextRenderer.DrawText(g, symbol, font, rect, Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
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

        private static Region CreateRoundedRegion(Rectangle rect, int radius)
        {
            using (var path = RoundedRect(rect, radius))
                return new Region(path);
        }
    }
}
