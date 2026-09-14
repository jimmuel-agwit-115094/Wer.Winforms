using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    /// <summary>
    /// Left navigation menu panel. Dock = Left by default.
    /// Drag WerMenuButton controls into it from the Toolbox.
    /// Set ContentPanel to the panel where forms display.
    ///
    /// Usage:
    ///   // Designer: drag WerLeftNavMenu, it docks left.
    ///   //           drag WerMenuButtons into it.
    ///   //           add a Panel, set Dock = Fill (content area).
    ///   // Code:
    ///   werLeftNavMenu1.ContentPanel = panel1;
    ///   // In each button's Click handler:
    ///   werLeftNavMenu1.ShowForm&lt;CustomerForm&gt;();
    /// </summary>
    [ToolboxItem(true)]
    [Description("Left navigation panel. Add WerMenuButton controls. Set ContentPanel for form hosting.")]
    [DefaultProperty("ContentPanel")]
    public class WerLeftNavMenu : Panel
    {
        private Panel _contentPanel;
        private WerMenuButton _activeButton;
        private Form _currentForm;
        private readonly Dictionary<Type, Form> _formCache = new Dictionary<Type, Form>();
        private string _logoText = "";
        private int _logoHeight = 60;

        private static readonly Color NavBg = Color.White;
        private static readonly Color NavBorder = Color.FromArgb(232, 235, 240);
        private static readonly Color LogoColor = Color.FromArgb(33, 37, 41);

        public event EventHandler NavigationChanged;

        public WerLeftNavMenu()
        {
            BackColor = NavBg;
            Dock = DockStyle.Left;
            Width = 220;
            Padding = new Padding(4, 68, 4, 8); // top padding = logoHeight + 8
            AutoScroll = true;
            DoubleBuffered = true;
        }

        // ── Properties ──────────────────────────────────────────

        /// <summary>Logo/app name displayed at the top of the nav.</summary>
        [Category("WerLeftNavMenu")]
        [DefaultValue("")]
        [Description("Logo or app name shown at the top of the navigation.")]
        public string LogoText
        {
            get => _logoText;
            set { _logoText = value; Invalidate(); }
        }

        /// <summary>Height of the logo area at the top. 0 = no logo area.</summary>
        [Category("WerLeftNavMenu")]
        [DefaultValue(60)]
        [Description("Height of the logo area at the top of the navigation.")]
        public int LogoHeight
        {
            get => _logoHeight;
            set
            {
                _logoHeight = Math.Max(0, value);
                Padding = new Padding(4, _logoHeight + 8, 4, 8);
                Invalidate();
            }
        }

        /// <summary>
        /// Panel where forms are displayed. Add a Panel to your form, set Dock=Fill, assign here.
        /// </summary>
        [Category("WerLeftNavMenu")]
        [DefaultValue(null)]
        [Description("Panel where forms are hosted. Add a Panel (Dock=Fill) and assign it here.")]
        public Panel ContentPanel
        {
            get => _contentPanel;
            set => _contentPanel = value;
        }

        /// <summary>Currently active button.</summary>
        [Browsable(false)]
        public WerMenuButton ActiveButton => _activeButton;

        // ── Auto-wire child buttons ─────────────────────────────

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            if (e.Control is WerMenuButton btn)
                btn.Click += OnButtonClick;
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);
            if (e.Control is WerMenuButton btn)
            {
                btn.Click -= OnButtonClick;
                if (_activeButton == btn) _activeButton = null;
            }
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            if (!(sender is WerMenuButton btn)) return;

            // Deactivate previous
            if (_activeButton != null && _activeButton != btn)
                _activeButton.IsActive = false;

            btn.IsActive = true;
            _activeButton = btn;

            NavigationChanged?.Invoke(this, EventArgs.Empty);
        }

        // ── Form hosting API ────────────────────────────────────

        /// <summary>
        /// Show a form in ContentPanel. Forms cached and reused.
        /// Call from a WerMenuButton Click handler.
        /// Usage: werLeftNavMenu1.ShowForm&lt;CustomerForm&gt;();
        /// </summary>
        public void ShowForm<TForm>() where TForm : Form, new()
        {
            ShowForm(typeof(TForm));
        }

        /// <summary>Show a form by type.</summary>
        public void ShowForm(Type formType)
        {
            if (formType == null || _contentPanel == null) return;
            if (!typeof(Form).IsAssignableFrom(formType))
                throw new ArgumentException("Type must derive from Form.", nameof(formType));

            Form form;
            if (_formCache.ContainsKey(formType) && !_formCache[formType].IsDisposed)
                form = _formCache[formType];
            else
            {
                form = (Form)Activator.CreateInstance(formType);
                _formCache[formType] = form;
            }

            EmbedForm(form);
        }

        /// <summary>Show a specific form instance.</summary>
        public void ShowForm(Form formInstance)
        {
            if (formInstance == null || _contentPanel == null) return;
            _formCache[formInstance.GetType()] = formInstance;
            EmbedForm(formInstance);
        }

        private void EmbedForm(Form form)
        {
            if (_currentForm == form) return;

            // Remove current (don't dispose — cached)
            if (_currentForm != null)
            {
                _currentForm.Hide();
                _contentPanel.Controls.Remove(_currentForm);
            }

            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;

            _contentPanel.Controls.Add(form);
            form.Show();
            form.BringToFront();
            _currentForm = form;
        }

        // ── Paint ───────────────────────────────────────────────

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            // Logo area
            if (_logoHeight > 0 && !string.IsNullOrEmpty(_logoText))
            {
                var logoRect = new Rectangle(12, 0, Width - 24, _logoHeight);
                using (var font = new Font(WerTheme.FontFamily, 14f, FontStyle.Bold))
                    TextRenderer.DrawText(g, _logoText, font, logoRect, LogoColor,
                        TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
            }

            // Separator under logo
            if (_logoHeight > 0)
            {
                using (var pen = new Pen(NavBorder, 1f))
                    g.DrawLine(pen, 12, _logoHeight, Width - 12, _logoHeight);
            }

            // Right border
            using (var pen = new Pen(NavBorder, 1f))
                g.DrawLine(pen, Width - 1, 0, Width - 1, Height);
        }

        // ── Dispose ─────────────────────────────────────────────

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var kvp in _formCache)
                {
                    if (kvp.Value != null && !kvp.Value.IsDisposed)
                    {
                        kvp.Value.Close();
                        kvp.Value.Dispose();
                    }
                }
                _formCache.Clear();
            }
            base.Dispose(disposing);
        }
    }
}
