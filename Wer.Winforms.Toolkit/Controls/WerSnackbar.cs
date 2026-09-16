using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    public enum WerSnackbarType { Success, Error, Warning, Info }

    /// <summary>
    /// Auto-dismiss toast notification displayed at bottom-right of the owner form.
    /// Multiple toasts stack vertically.
    ///
    /// Usage:
    ///   WerSnackbar.Success(this, "Record saved successfully.");
    ///   WerSnackbar.Error(this, "Something went wrong.");
    ///   WerSnackbar.Warning(this, "Connection unstable.");
    ///   WerSnackbar.Info(this, "3 new messages.");
    /// </summary>
    public static class WerSnackbar
    {
        private const int ToastW       = 350;
        private const int ToastH       = 52;
        private const int MarginRight  = 16;
        private const int MarginBottom = 16;
        private const int StackGap     = 8;
        private const int AutoDismissMs = 3000;

        // Active snackbars tracked for stacking
        private static readonly List<WerSnackbarForm> _active = new List<WerSnackbarForm>();

        public static void Success(IWin32Window owner, string message) =>
            Show(owner, message, WerSnackbarType.Success);

        public static void Error(IWin32Window owner, string message) =>
            Show(owner, message, WerSnackbarType.Error);

        public static void Warning(IWin32Window owner, string message) =>
            Show(owner, message, WerSnackbarType.Warning);

        public static void Info(IWin32Window owner, string message) =>
            Show(owner, message, WerSnackbarType.Info);

        private static void Show(IWin32Window owner, string message, WerSnackbarType type)
        {
            Form ownerForm = null;
            if (owner is Form f) ownerForm = f;
            else if (owner is Control c) ownerForm = c.FindForm();
            if (ownerForm == null) ownerForm = Form.ActiveForm;

            // Calculate Y offset for stacking
            int stackOffset = _active.Count * (ToastH + StackGap);

            var toast = new WerSnackbarForm(message, type);

            // Position bottom-center of the owner
            if (ownerForm != null)
            {
                var ownerBounds = ownerForm.Bounds;
                toast.Location = new Point(
                    ownerBounds.X + (ownerBounds.Width - ToastW) / 2,
                    ownerBounds.Bottom - ToastH - MarginBottom - stackOffset);
            }
            else
            {
                var screen = Screen.PrimaryScreen.WorkingArea;
                toast.Location = new Point(
                    (screen.Width - ToastW) / 2,
                    screen.Bottom - ToastH - MarginBottom - stackOffset);
            }

            _active.Add(toast);

            toast.FormClosed += (s, e) =>
            {
                _active.Remove(toast);
                // Re-stack remaining toasts (bottom-center)
                for (int i = 0; i < _active.Count; i++)
                {
                    if (ownerForm != null && !ownerForm.IsDisposed)
                    {
                        var ownerBounds = ownerForm.Bounds;
                        _active[i].Location = new Point(
                            ownerBounds.X + (ownerBounds.Width - ToastW) / 2,
                            ownerBounds.Bottom - ToastH - MarginBottom - (i * (ToastH + StackGap)));
                    }
                    else
                    {
                        var screen = Screen.PrimaryScreen.WorkingArea;
                        _active[i].Location = new Point(
                            (screen.Width - ToastW) / 2,
                            screen.Bottom - ToastH - MarginBottom - (i * (ToastH + StackGap)));
                    }
                }
            };

            toast.Show(ownerForm);

            // Auto-dismiss timer
            var timer = new Timer { Interval = AutoDismissMs };
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                timer.Dispose();
                if (!toast.IsDisposed)
                {
                    toast.Close();
                    toast.Dispose();
                }
            };
            timer.Start();
        }
    }

    internal class WerSnackbarForm : Form
    {
        private readonly string _message;
        private readonly WerSnackbarType _type;

        private const int AccentBarW  = 4;
        private const int IconSize    = 28;
        private const int IconMarginL = 14;
        private const int IconGap     = 10;
        private const int CornerR     = 8;

        private static readonly Color BgColor     = Color.White;
        private static readonly Color BorderColor = Color.FromArgb(220, 225, 230);

        public WerSnackbarForm(string message, WerSnackbarType type)
        {
            _message = message;
            _type    = type;

            FormBorderStyle = FormBorderStyle.None;
            StartPosition   = FormStartPosition.Manual;
            ShowInTaskbar   = false;
            BackColor       = BgColor;
            DoubleBuffered  = true;
            TopMost         = true;
            Size            = new Size(350, 52);

            Region = CreateRoundedRegion(new Rectangle(0, 0, Width, Height), CornerR);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode     = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            GetPalette(out Color accent, out string icon);

            var cardRect = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var path = RoundedRect(cardRect, CornerR))
            {
                using (var brush = new SolidBrush(BgColor))
                    g.FillPath(brush, path);
                using (var pen = new Pen(BorderColor, 1f))
                    g.DrawPath(pen, path);
            }

            // Left accent bar (clipped to rounded corners on left side)
            using (var path = RoundedRect(new Rectangle(0, 0, CornerR * 2 + AccentBarW, Height - 1), CornerR))
            {
                g.SetClip(new Rectangle(0, 0, AccentBarW, Height));
                using (var brush = new SolidBrush(accent))
                    g.FillPath(brush, path);
                g.ResetClip();
            }
            // Fill accent bar solid where corners don't reach
            using (var brush = new SolidBrush(accent))
                g.FillRectangle(brush, new Rectangle(0, CornerR, AccentBarW, Height - CornerR * 2));

            // Icon circle
            int iconX = AccentBarW + IconMarginL;
            int iconY = (Height - IconSize) / 2;
            using (var brush = new SolidBrush(accent))
                g.FillEllipse(brush, iconX, iconY, IconSize, IconSize);
            using (var font = new Font(WerTheme.FontFamily, 11f, FontStyle.Bold))
                TextRenderer.DrawText(g, icon, font,
                    new Rectangle(iconX, iconY, IconSize, IconSize), Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            // Message text
            int textX = iconX + IconSize + IconGap;
            var textRect = new Rectangle(textX, 0, Width - textX - 12, Height);
            using (var font = new Font(WerTheme.FontFamily, 9.75f, FontStyle.Regular))
                TextRenderer.DrawText(g, _message, font, textRect, WerTheme.TextColor,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix);
        }

        private void GetPalette(out Color accent, out string icon)
        {
            switch (_type)
            {
                case WerSnackbarType.Success: accent = Color.FromArgb(40, 167, 69);   icon = "✓"; break;
                case WerSnackbarType.Error:   accent = Color.FromArgb(179, 58, 58);   icon = "✕"; break;
                case WerSnackbarType.Warning: accent = Color.FromArgb(230, 126, 34);  icon = "!"; break;
                default:                      accent = Color.FromArgb(12, 124, 146);  icon = "i"; break;
            }
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
