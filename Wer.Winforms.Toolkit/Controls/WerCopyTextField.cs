using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    /// <summary>
    /// Read-only text field with a copy button. Used for displaying IDs or values
    /// that the user needs to copy but not edit.
    /// Usage: werCopyTextField1.Value = "abc-123-def";
    /// Click the copy icon to copy to clipboard.
    /// </summary>
    [ToolboxItem(true)]
    [Description("Read-only text field with copy-to-clipboard button.")]
    [DefaultEvent("Copied")]
    [DefaultProperty("Value")]
    public class WerCopyTextField : Control
    {
        private readonly TextBox _input;
        private readonly Panel   _inputBorder;

        private string _labelText = "CopyText Label";
        private string _value = "";
        private bool   _hoverCopy;
        private bool   _showCopied;
        private Timer  _copiedTimer;

        private int LabelHeight => Math.Max(20, (int)(Font.GetHeight() + 4));
        private const int LabelGap     = 4;
        private const int BorderRadius = 8;
        private const int InputPadH    = 10;
        private const int CopyBtnW     = 36;

        private static readonly Color LabelNormal      = Color.Black;
        private static readonly Color LabelDisabled     = Color.FromArgb(150, 150, 150);
        private static readonly Color BorderNormal      = Color.FromArgb(200, 210, 220);
        private static readonly Color BorderHover       = Color.FromArgb(12, 124, 146);
        private static readonly Color PlaceholderColor  = Color.FromArgb(160, 170, 180);
        private static readonly Color BgNormal          = Color.FromArgb(245, 246, 248);
        private static readonly Color CopyIconColor     = Color.FromArgb(100, 110, 120);
        private static readonly Color CopyIconHover     = Color.FromArgb(12, 124, 146);
        private static readonly Color CopiedBg          = Color.FromArgb(12, 124, 146);

        /// <summary>Fired after the value is copied to clipboard.</summary>
        public event EventHandler Copied;

        public WerCopyTextField()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.SupportsTransparentBackColor, true);

            BackColor = Color.Transparent;
            Font      = WerTheme.BodyFont;
            Size      = new Size(350, 60);

            _inputBorder = new Panel { BackColor = Color.Transparent };
            _inputBorder.Paint += OnBorderPaint;
            _inputBorder.MouseMove += OnBorderMouseMove;
            _inputBorder.MouseLeave += (s, e) => { _hoverCopy = false; _inputBorder.Cursor = Cursors.Default; _inputBorder.Invalidate(); };
            _inputBorder.MouseClick += OnBorderClick;
            Controls.Add(_inputBorder);

            _input = new TextBox
            {
                BorderStyle = BorderStyle.None,
                BackColor   = BgNormal,
                ForeColor   = WerTheme.TextColor,
                Font        = WerTheme.BodyFont,
                ReadOnly    = true,
                TabStop     = false,
            };
            _inputBorder.Controls.Add(_input);

            _copiedTimer = new Timer { Interval = 1500 };
            _copiedTimer.Tick += (s, e) => { _showCopied = false; _copiedTimer.Stop(); _inputBorder.Invalidate(); };

            LayoutInternals();
        }

        // ── Properties ──────────────────────────────────────────

        private bool _showLabel = true;

        [Category("WerCopyTextField")]
        [DefaultValue(true)]
        [Description("Show or hide the label above the input.")]
        public bool ShowLabel
        {
            get => _showLabel;
            set { _showLabel = value; LayoutInternals(); Invalidate(); }
        }

        [Category("WerCopyTextField")]
        [DefaultValue("CopyText Label")]
        [Description("Label displayed above the field.")]
        public string LabelText
        {
            get => _labelText;
            set { _labelText = value; RecalcHeight(); Invalidate(); }
        }

        /// <summary>
        /// The text value to display and copy.
        /// Usage: werCopyTextField1.Value = "abc-123";
        /// </summary>
        [Category("WerCopyTextField")]
        [DefaultValue("")]
        [Description("The read-only text value. Click copy icon to copy to clipboard.")]
        public string Value
        {
            get => _value;
            set { _value = value ?? ""; _input.Text = _value; _inputBorder.Invalidate(); }
        }

        [Browsable(true)]
        [Category("WerCopyTextField")]
        public override string Text
        {
            get => _value;
            set => Value = value;
        }

        // ── Layout ──────────────────────────────────────────────

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            LayoutInternals();
        }

        private void RecalcHeight()
        {
            // Adjust height if label is shown/hidden
            LayoutInternals();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            if (_input == null) return;
            _input.Font = Font;
            LayoutInternals();
        }

        private void LayoutInternals()
        {
            if (_inputBorder == null || _input == null) return;

            int borderTop = _showLabel ? LabelHeight + LabelGap : 0;
            int borderH   = Height - borderTop;

            _inputBorder.SetBounds(0, borderTop, Width, borderH);

            int textH = _input.PreferredHeight;
            int textY = (borderH - textH) / 2;
            _input.SetBounds(InputPadH, Math.Max(0, textY), Width - InputPadH - CopyBtnW - 4, textH);
        }

        // ── Mouse on border — detect copy button hover ──────────

        private void OnBorderMouseMove(object sender, MouseEventArgs e)
        {
            bool wasHover = _hoverCopy;
            _hoverCopy = e.X >= _inputBorder.Width - CopyBtnW;
            _inputBorder.Cursor = _hoverCopy ? Cursors.Hand : Cursors.Default;
            if (wasHover != _hoverCopy) _inputBorder.Invalidate();
        }

        private void OnBorderClick(object sender, MouseEventArgs e)
        {
            if (e.X >= _inputBorder.Width - CopyBtnW && !string.IsNullOrEmpty(_value))
            {
                try
                {
                    Clipboard.SetText(_value);
                    _showCopied = true;
                    _copiedTimer.Start();
                    _inputBorder.Invalidate();
                    Copied?.Invoke(this, EventArgs.Empty);
                }
                catch { }
            }
        }

        // ── Paint label ─────────────────────────────────────────

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            if (!_showLabel) return;

            var labelRect = new Rectangle(0, 0, Width, LabelHeight);
            TextRenderer.DrawText(g, _labelText, Font, labelRect,
                Enabled ? LabelNormal : LabelDisabled,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
        }

        // ── Paint border + copy button ──────────────────────────

        private void OnBorderPaint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode     = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            var panel = (Panel)sender;
            var rect  = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);

            // Background
            using (var path = RoundedRect(rect, BorderRadius))
            using (var brush = new SolidBrush(BgNormal))
                g.FillPath(brush, path);

            // Border
            var borderColor = _hoverCopy ? BorderHover : BorderNormal;
            using (var path = RoundedRect(rect, BorderRadius))
            using (var pen = new Pen(borderColor, 1f))
                g.DrawPath(pen, path);

            // Vertical separator before copy button
            int sepX = panel.Width - CopyBtnW;
            using (var pen = new Pen(BorderNormal, 1f)) g.DrawLine(pen, sepX, 6, sepX, panel.Height - 6);

            // Copy button area
            if (_showCopied)
            {
                // "Copied" feedback
                var btnRect = new Rectangle(sepX + 1, 1, CopyBtnW - 2, panel.Height - 2);
                using (var brush = new SolidBrush(CopiedBg))
                {
                    // Fill right side with rounded right corners
                    g.FillRectangle(brush, btnRect);
                }
                using (var font = new Font(WerTheme.FontFamily, 7f, FontStyle.Bold))
                    TextRenderer.DrawText(g, "✓", font, btnRect, Color.White,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
            }
            else
            {
                // Draw copy icon (two overlapping rectangles)
                var iconColor = _hoverCopy ? CopyIconHover : CopyIconColor;
                int iconSize = 14;
                int ix = sepX + (CopyBtnW - iconSize) / 2;
                int iy = (panel.Height - iconSize) / 2;

                using (var pen = new Pen(iconColor, 1.2f))
                {
                    // Back rectangle
                    g.DrawRectangle(pen, ix + 3, iy, iconSize - 4, iconSize - 4);
                    // Front rectangle (overlapping)
                    using (var brush = new SolidBrush(BgNormal))
                        g.FillRectangle(brush, ix, iy + 3, iconSize - 4, iconSize - 4);
                    g.DrawRectangle(pen, ix, iy + 3, iconSize - 4, iconSize - 4);
                }
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _copiedTimer?.Dispose();
            base.Dispose(disposing);
        }
    }
}
