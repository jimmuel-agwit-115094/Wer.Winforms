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
        private Panel _titlePanel;
        private Label _titleLabel;
        private Panel _formHost;
        private WerMenuButton _activeButton;
        private Form _currentForm;
        private readonly Dictionary<Type, Form> _formCache = new Dictionary<Type, Form>();
        private string _logoText = "";
        private int _logoHeight = 60;
        private Font _titleLabelFont;

        // Header panel that never scrolls (holds logo painting)
        private Panel _headerPanel;
        // Scroll panel that holds all WerMenuButton children
        private Panel _scrollPanel;

        private static readonly Color NavBg = Color.White;
        private static readonly Color NavBorder = Color.FromArgb(232, 235, 240);
        private static readonly Color LogoColor = Color.FromArgb(130, 140, 150);

        public event EventHandler NavigationChanged;

        public WerLeftNavMenu()
        {
            BackColor    = NavBg;
            Dock         = DockStyle.Left;
            Width        = 220;
            AutoScroll   = false;          // outer panel never scrolls
            DoubleBuffered = true;

            BuildInternalLayout();
        }

        private void BuildInternalLayout()
        {
            // Non-scrolling header — painted logo lives here
            _headerPanel = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = _logoHeight,
                BackColor = NavBg,
            };
            _headerPanel.Paint += OnHeaderPaint;

            // Scrollable button area below the header
            _scrollPanel = new Panel
            {
                Dock       = DockStyle.Fill,
                BackColor  = NavBg,
                AutoScroll = true,
                Padding    = new Padding(4, 4, 4, 8),
            };

            // Fill must be added before Top
            Controls.Add(_scrollPanel);
            Controls.Add(_headerPanel);
        }

        // ── Properties ──────────────────────────────────────────

        /// <summary>Logo/app name displayed at the top of the nav.</summary>
        [Category("WerLeftNavMenu")]
        [DefaultValue("")]
        [Description("Logo or app name shown at the top of the navigation.")]
        public string LogoText
        {
            get => _logoText;
            set { _logoText = value; _headerPanel?.Invalidate(); }
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
                if (_headerPanel != null)
                    _headerPanel.Height = _logoHeight;
                _headerPanel?.Invalidate();
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
            set
            {
                _contentPanel = value;
                if (_contentPanel != null && _formHost == null)
                {
                    // Title panel — Dock Top
                    _titlePanel = new Panel
                    {
                        Dock = DockStyle.Top,
                        Height = 32,
                        BackColor = Color.White,
                        Padding = new Padding(16, 6, 0, 2),
                        Visible = false,
                    };
                    _titleLabelFont = new Font(WerTheme.FontFamily, 14f, FontStyle.Bold);
                    _titleLabel = new Label
                    {
                        Dock = DockStyle.Fill,
                        Font = _titleLabelFont,
                        ForeColor = Color.FromArgb(33, 37, 41),
                        BackColor = Color.White,
                        TextAlign = ContentAlignment.MiddleLeft,
                    };
                    _titlePanel.Controls.Add(_titleLabel);

                    // Form host — Dock Fill (remaining space below title)
                    _formHost = new Panel
                    {
                        Dock = DockStyle.Fill,
                        BackColor = Color.White,
                    };

                    // Add order: Fill first, then Top
                    _contentPanel.Controls.Add(_formHost);
                    _contentPanel.Controls.Add(_titlePanel);
                }
            }
        }

        /// <summary>Currently active button.</summary>
        [Browsable(false)]
        public WerMenuButton ActiveButton => _activeButton;

        // ── Auto-wire child buttons ─────────────────────────────
        // Designer drops WerMenuButtons onto WerLeftNavMenu.
        // Redirect them into _scrollPanel so they scroll independently of the logo header.

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);

            if (e.Control is WerMenuButton btn && _scrollPanel != null
                && !_scrollPanel.Controls.Contains(btn))
            {
                // Move from outer panel into scroll panel.
                // Remove fires OnControlRemoved which unwires Click (no-op here since not yet wired).
                Controls.Remove(btn);
                btn.Click += OnButtonClick;
                _scrollPanel.Controls.Add(btn);
            }
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

            // Sub-item clicked — delegate to parent logic then fire nav
            if (btn.IsSubItem && btn.ParentButton != null)
            {
                SetActive(btn.ParentButton, btn);
                btn.SourceItem?.Action?.Invoke();
                NavigationChanged?.Invoke(this, EventArgs.Empty);
                return;
            }

            // Parent button with sub-items — toggle accordion, don't fire nav
            if (btn.HasSubItems)
            {
                ToggleAccordion(btn);
                return;
            }

            // Plain parent button — activate and fire nav
            SetActive(btn, null);
            NavigationChanged?.Invoke(this, EventArgs.Empty);
        }

        private void SetActive(WerMenuButton parent, WerMenuButton subBtn)
        {
            // Deactivate old
            if (_activeButton != null && _activeButton != parent)
            {
                _activeButton.IsActive      = false;
                _activeButton.HasActiveChild = false;
            }

            if (subBtn != null)
            {
                // Deactivate old sub-item
                foreach (Control c in _scrollPanel.Controls)
                {
                    if (c is WerMenuButton sb && sb.IsSubItem && sb != subBtn)
                        sb.IsActive = false;
                }
                subBtn.IsActive      = true;
                parent.IsActive      = false;
                parent.HasActiveChild = true;
            }
            else
            {
                parent.IsActive       = true;
                parent.HasActiveChild = false;
            }

            _activeButton = parent;
        }

        private void ToggleAccordion(WerMenuButton btn)
        {
            if (btn.IsExpanded)
                CollapseButton(btn);
            else
                ExpandButton(btn);
        }

        private void ExpandButton(WerMenuButton btn)
        {
            // Collapse any other expanded button first.
            // Collect before iterating — CollapseButton modifies _scrollPanel.Controls.
            var toCollapse = new List<WerMenuButton>();
            foreach (Control c in _scrollPanel.Controls)
            {
                if (c is WerMenuButton other && other != btn && other.IsExpanded)
                    toCollapse.Add(other);
            }
            foreach (var other in toCollapse)
                CollapseButton(other);

            btn.IsExpanded = true;

            // With Dock=Top, lower Z-index = visually lower (closer to bottom).
            // Parent button sits at some Z-index; we insert sub-items at that same index
            // each time (loop forward) so each sub-item ends up just below the parent,
            // pushing parent up by 1 each iteration.
            int insertIndex = _scrollPanel.Controls.GetChildIndex(btn);

            _scrollPanel.SuspendLayout();
            for (int i = 0; i < btn.SubItems.Count; i++)
            {
                var item = btn.SubItems[i];
                var subBtn = new WerMenuButton
                {
                    Text         = item.Text,
                    IsSubItem    = true,
                    ParentButton = btn,
                    SourceItem   = item,
                };
                subBtn.Click += OnButtonClick;
                _scrollPanel.Controls.Add(subBtn);
                _scrollPanel.Controls.SetChildIndex(subBtn, insertIndex);
            }
            _scrollPanel.ResumeLayout(true);
        }

        private void CollapseButton(WerMenuButton btn)
        {
            btn.IsExpanded    = false;
            btn.HasActiveChild = false;

            _scrollPanel.SuspendLayout();
            var toRemove = new List<WerMenuButton>();
            foreach (Control c in _scrollPanel.Controls)
            {
                if (c is WerMenuButton sub && sub.IsSubItem && sub.ParentButton == btn)
                    toRemove.Add(sub);
            }
            foreach (var sub in toRemove)
            {
                sub.Click -= OnButtonClick;
                _scrollPanel.Controls.Remove(sub);
                sub.Dispose();
            }
            _scrollPanel.ResumeLayout(true);
        }

        // ── Form hosting API ────────────────────────────────────

        /// <summary>
        /// Show a form in ContentPanel. Forms cached and reused.
        /// Call from a WerMenuButton Click handler.
        /// Usage: werLeftNavMenu1.ShowForm&lt;CustomerForm&gt;();
        /// </summary>
        /// <summary>
        /// Show a form with optional page title.
        /// Usage: werLeftNavMenu1.ShowForm&lt;UserForm&gt;("User Management");
        /// </summary>
        public void ShowForm<TForm>(string pageTitle = null) where TForm : Form, new()
        {
            ShowForm(typeof(TForm), pageTitle);
        }

        /// <summary>Show a form by type with optional title.</summary>
        public void ShowForm(Type formType, string pageTitle = null)
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

            SetPageTitle(pageTitle);
            EmbedForm(form);
        }

        /// <summary>Show a specific form instance.</summary>
        public void ShowForm(Form formInstance)
        {
            if (formInstance == null || _contentPanel == null) return;
            _formCache[formInstance.GetType()] = formInstance;
            EmbedForm(formInstance);
        }

        private void SetPageTitle(string title)
        {
            if (_titlePanel == null || _titleLabel == null) return;
            if (!string.IsNullOrEmpty(title))
            {
                _titleLabel.Text = title;
                _titlePanel.Visible = true;
            }
            else
            {
                _titlePanel.Visible = false;
            }
        }

        private void EmbedForm(Form form)
        {
            if (_formHost == null) return;
            if (_currentForm == form) return;

            _formHost.SuspendLayout();

            // Prepare form for hosting (only needed once)
            if (!_formHost.Controls.Contains(form))
            {
                form.TopLevel = false;
                form.FormBorderStyle = FormBorderStyle.None;
                form.Dock = DockStyle.Fill;
                form.Visible = false;
                _formHost.Controls.Add(form);
            }

            // Toggle visibility — no remove/add, no handle churn
            if (_currentForm != null)
                _currentForm.Visible = false;

            form.Visible = true;
            form.BringToFront();
            _currentForm = form;

            _formHost.ResumeLayout(false);
        }

        // ── Paint ───────────────────────────────────────────────

        // Logo header — fixed, never scrolls
        private void OnHeaderPaint(object sender, PaintEventArgs e)
        {
            var g   = e.Graphics;
            var w   = _headerPanel.Width;
            var h   = _headerPanel.Height;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            if (!string.IsNullOrEmpty(_logoText))
            {
                var logoRect = new Rectangle(12, 0, w - 24, h);
                using (var f = new Font("Segoe UI Light", 14f, FontStyle.Regular))
                    TextRenderer.DrawText(g, _logoText, f, logoRect, LogoColor,
                        TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
            }

            // Separator at bottom of header
            using (var pen = new Pen(NavBorder, 1f))
                g.DrawLine(pen, 12, h - 1, w - 12, h - 1);
        }

        // Right border on the outer panel
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (var pen = new Pen(NavBorder, 1f))
                e.Graphics.DrawLine(pen, Width - 1, 0, Width - 1, Height);
        }

        // ── Dispose ─────────────────────────────────────────────

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _titleLabelFont?.Dispose();
                _titleLabelFont = null;

                // Unwire Paint event before base disposes the panel
                if (_headerPanel != null)
                {
                    _headerPanel.Paint -= OnHeaderPaint;
                    _headerPanel = null;
                }

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
            // base.Dispose disposes all child Controls including _scrollPanel, _headerPanel, etc.
            base.Dispose(disposing);
        }
    }
}
