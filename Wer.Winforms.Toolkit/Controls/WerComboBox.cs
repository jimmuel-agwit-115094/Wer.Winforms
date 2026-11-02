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
    [Description("Custom combobox with label, placeholder, search, and styled scrollable dropdown.")]
    [DefaultEvent("SelectedIndexChanged")]
    [DefaultProperty("Items")]
    public class WerComboBox : Control
    {
        private readonly ComboBox _combo;
        private readonly Panel _inputBorder;
        private readonly PlaceholderTextBox _editor;
        private readonly PopupMessageFilter _messageFilter;
        private WerComboPopup _popup;
        private Form _ownerForm;
        private Font _boldFont;
        private Font _italicFont;

        // ── State ───────────────────────────────────────────────
        private string _labelText = "ComboBox Label";
        private string _placeholder = "Select...";
        private bool _required;
        private bool _readOnly;
        private bool _hasFocus;
        private bool _allowTyping;
        private int _maxVisibleItems = 10;
        private bool _suppressEditorSync;

        // Popup rows: original item indices currently shown (filtered)
        private readonly List<int> _matches = new List<int>();
        private string _filterText = "";
        private int _hotRow = -1;

        // ── Layout constants (match WerTextField) ───────────────
        private int LabelHeight => Math.Max(20, (int)(Font.GetHeight() + 4));
        private int RowHeight => Math.Max(32, Font.Height + 14);
        private const int LabelGap     = 4;
        private const int BorderRadius = 8;
        private const int InputPadH    = 10;
        private const int ChevronWidth = 28;
        private const int PopupGap     = 4;
        private const string NoMatchesText = "No matches found";

        // ── Colors ──────────────────────────────────────────────
        private static readonly Color LabelNormal      = Color.Black;
        private static readonly Color LabelDisabled    = Color.FromArgb(150, 150, 150);
        private static readonly Color RequiredStar     = Color.FromArgb(210, 50, 50);
        private static readonly Color BorderNormal     = Color.FromArgb(200, 210, 220);
        private static readonly Color BorderFocus      = Color.FromArgb(12, 124, 146);
        private static readonly Color PlaceholderColor = Color.FromArgb(160, 170, 180);
        private static readonly Color BgNormal         = Color.White;
        private static readonly Color BgDisabled       = Color.FromArgb(242, 242, 242);
        private static readonly Color RowHoverBg       = Color.FromArgb(243, 244, 246);
        private static readonly Color PopupBorder      = Color.FromArgb(225, 228, 232);

        private const TextFormatFlags RowTextFlags =
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine |
            TextFormatFlags.NoPrefix | TextFormatFlags.NoPadding;

        public event EventHandler SelectedIndexChanged;

        public WerComboBox()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.SupportsTransparentBackColor,
                true);

            Font      = WerTheme.BodyFont;
            ForeColor = WerTheme.TextColor;
            BackColor = Color.Transparent;
            Size      = new Size(350, 60);
            TabStop   = false;

            _messageFilter = new PopupMessageFilter(this);

            // Custom-painted input face — use DoubleBufferedPanel to prevent flicker
            _inputBorder = new DoubleBufferedPanel { BackColor = Color.Transparent, Cursor = Cursors.Hand };
            _inputBorder.Paint      += OnBorderPaint;
            _inputBorder.MouseClick += OnInputAreaClick;
            Controls.Add(_inputBorder);

            // Inline search editor — only visible when AllowTyping = true
            _editor = new PlaceholderTextBox
            {
                BorderStyle = BorderStyle.None,
                BackColor   = BgNormal,
                ForeColor   = WerTheme.TextColor,
                Font        = WerTheme.BodyFont,
                Placeholder = _placeholder,
                Visible     = false,
                TabStop     = false,
                Cursor      = Cursors.IBeam,
            };
            _editor.TextChanged    += OnEditorTextChanged;
            _editor.PreviewKeyDown += OnPreviewKeyDown;
            _editor.KeyDown        += OnEditorKeyDown;
            _editor.GotFocus       += OnEditorGotFocus;
            _editor.LostFocus      += OnInnerLostFocus;
            _editor.Leave          += OnEditorLeave;
            _editor.MouseClick     += (s, e) => { if (!IsPopupVisible) OpenPopup(""); };
            _inputBorder.Controls.Add(_editor);

            // Native ComboBox — hidden behind the panel; acts as item/data store and keyboard target
            _combo = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font          = WerTheme.BodyFont,
                FlatStyle     = FlatStyle.Standard,
                TabStop       = true,
            };
            _combo.SelectedIndexChanged += OnComboSelectedIndexChanged;
            _combo.DropDown             += OnNativeDropDown;
            _combo.PreviewKeyDown       += OnPreviewKeyDown;
            _combo.KeyDown              += OnComboKeyDown;
            _combo.KeyPress             += OnComboKeyPress;
            _combo.GotFocus             += (s, e) => UpdateFocusState();
            _combo.LostFocus            += OnInnerLostFocus;
            Controls.Add(_combo);

            // Input border on top
            _inputBorder.BringToFront();

            LayoutInternals();
        }

        // ── Public API ─────────────────────────────────────────────

        private bool _showLabel = true;

        [Category("WerComboBox")]
        [DefaultValue(true)]
        [Description("Show or hide the label above the input.")]
        public bool ShowLabel
        {
            get => _showLabel;
            set { _showLabel = value; LayoutInternals(); Invalidate(); }
        }

        [Category("WerComboBox")]
        [DefaultValue("ComboBox Label")]
        [Description("Label displayed above the combobox.")]
        public string LabelText
        {
            get => _labelText;
            set { _labelText = value; Invalidate(); }
        }

        [Category("WerComboBox")]
        [DefaultValue("Select...")]
        [Description("Placeholder text when nothing is selected.")]
        public string Placeholder
        {
            get => _placeholder;
            set
            {
                _placeholder = value;
                _editor.Placeholder = value;
                _editor.Invalidate();
                _inputBorder.Invalidate();
            }
        }

        [Category("WerComboBox")]
        [DefaultValue(false)]
        [Description("Show red asterisk and orange label.")]
        public bool Required
        {
            get => _required;
            set { _required = value; Invalidate(); }
        }

        [Category("WerComboBox")]
        [DefaultValue(false)]
        [Description("Field is read-only: disabled appearance, no dropdown.")]
        public bool ReadOnly
        {
            get => _readOnly;
            set
            {
                _readOnly = value;
                if (value) ClosePopup();
                UpdateEditorState();
                Invalidate();
                _inputBorder.Invalidate();
            }
        }

        [Category("WerComboBox")]
        [DefaultValue(false)]
        [Description("Allow typing in the field to search. The dropdown filters as you type.")]
        public bool AllowTyping
        {
            get => _allowTyping;
            set
            {
                if (_allowTyping == value) return;
                _allowTyping = value;
                ClosePopup();
                _editor.Visible = value;
                _editor.TabStop = value;
                _combo.TabStop  = !value;
                SyncEditorText();
                UpdateEditorState();
                LayoutInternals();
                _inputBorder.Invalidate();
            }
        }

        [Category("WerComboBox")]
        [DefaultValue(10)]
        [Description("Maximum rows shown in the dropdown before it scrolls.")]
        public int MaxVisibleItems
        {
            get => _maxVisibleItems;
            set => _maxVisibleItems = Math.Max(1, value);
        }

        [Category("WerComboBox")]
        [Description("The list of items to display in the dropdown.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ComboBox.ObjectCollection Items => _combo.Items;

        [Category("WerComboBox")]
        [Description("Items to display — set this in the designer as string array.")]
        public string[] ItemsArray
        {
            get
            {
                var arr = new string[_combo.Items.Count];
                for (int i = 0; i < arr.Length; i++)
                    arr[i] = _combo.Items[i]?.ToString() ?? "";
                return arr;
            }
            set
            {
                ClosePopup();
                _combo.Items.Clear();
                if (value != null) _combo.Items.AddRange(value);
                _combo.SelectedIndex = -1;
                SyncEditorText();
                _inputBorder.Invalidate();
            }
        }

        [Category("WerComboBox")]
        [DefaultValue(-1)]
        [Description("Index of the selected item. -1 means nothing selected.")]
        public int SelectedIndex
        {
            get => _combo.SelectedIndex;
            set { _combo.SelectedIndex = value; SyncEditorText(); _inputBorder.Invalidate(); }
        }

        [Browsable(false)]
        public object SelectedItem => _combo.SelectedItem;

        /// <summary>Selected value from ValueMember column. Returns the ID when DataSource is bound.</summary>
        [Browsable(false)]
        public object SelectedValue
        {
            get => _combo.SelectedValue;
            set { _combo.SelectedValue = value; SyncEditorText(); _inputBorder.Invalidate(); }
        }

        /// <summary>Bind a list/table as data source. Use with DisplayMember and ValueMember.</summary>
        [Category("WerComboBox")]
        [DefaultValue(null)]
        [Description("Data source (List, DataTable, etc). Use with DisplayMember and ValueMember.")]
        public object DataSource
        {
            get => _combo.DataSource;
            set { ClosePopup(); _combo.DataSource = value; SyncEditorText(); _inputBorder.Invalidate(); }
        }

        /// <summary>Property name to display in the dropdown.</summary>
        [Category("WerComboBox")]
        [DefaultValue("")]
        [Description("Property or column name to display.")]
        public string DisplayMember
        {
            get => _combo.DisplayMember;
            set { _combo.DisplayMember = value; SyncEditorText(); _inputBorder.Invalidate(); }
        }

        /// <summary>Property name to use as the value (e.g. ID column).</summary>
        [Category("WerComboBox")]
        [DefaultValue("")]
        [Description("Property or column name to return as SelectedValue (e.g. Id).")]
        public string ValueMember
        {
            get => _combo.ValueMember;
            set => _combo.ValueMember = value;
        }

        [Browsable(true)]
        [Category("WerComboBox")]
        public override string Text
        {
            get => SelectedText;
            set
            {
                int idx = _combo.FindStringExact(value);
                if (idx >= 0) _combo.SelectedIndex = idx;
            }
        }

        // ── Item text helpers ────────────────────────────────────

        private string ItemText(int index) => _combo.GetItemText(_combo.Items[index]) ?? "";

        private string SelectedText =>
            _combo.SelectedIndex >= 0 ? _combo.GetItemText(_combo.SelectedItem) ?? "" : "";

        private Font BoldFont   => _boldFont   ?? (_boldFont   = new Font(Font, FontStyle.Bold));
        private Font ItalicFont => _italicFont ?? (_italicFont = new Font(Font, FontStyle.Italic));

        // ── Selection sync ───────────────────────────────────────

        private void OnComboSelectedIndexChanged(object sender, EventArgs e)
        {
            SyncEditorText();
            _inputBorder.Invalidate();
            SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
        }

        private void SyncEditorText()
        {
            if (!_allowTyping) return;
            string text = SelectedText;
            if (_editor.Text == text) return;

            _suppressEditorSync = true;
            _editor.Text = text;
            _editor.SelectionStart = text.Length;
            _suppressEditorSync = false;
        }

        private void UpdateEditorState()
        {
            bool inactive = !Enabled || _readOnly;
            _editor.ReadOnly  = inactive;
            _editor.BackColor = inactive ? BgDisabled : BgNormal;
            _editor.ForeColor = !Enabled ? LabelDisabled : WerTheme.TextColor;
        }

        private void UpdateFocusState()
        {
            bool focused = _editor.Focused || _combo.Focused || IsPopupVisible;
            if (focused == _hasFocus) return;
            _hasFocus = focused;
            _inputBorder.Invalidate();
        }

        // ── Click → toggle dropdown ──────────────────────────────

        private void OnInputAreaClick(object sender, MouseEventArgs e)
        {
            if (!Enabled || _readOnly) return;

            if (IsPopupVisible)
            {
                ClosePopup();
                return;
            }

            if (_allowTyping) _editor.Focus();
            else              _combo.Focus();
            OpenPopup("");
        }

        private void OnNativeDropDown(object sender, EventArgs e)
        {
            // Native list should never show — swap it for the custom popup
            if (!IsHandleCreated) return;
            BeginInvoke(new Action(() =>
            {
                if (IsDisposed) return;
                _combo.DroppedDown = false;
                OpenPopup("");
            }));
        }

        // ── Keyboard ─────────────────────────────────────────────

        private void OnPreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // Route keys to the control instead of AcceptButton/CancelButton while the list is open
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                e.IsInputKey = true;
            else if (IsPopupVisible && (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape))
                e.IsInputKey = true;
        }

        private void OnEditorKeyDown(object sender, KeyEventArgs e)
        {
            if (!HandleListKey(e)) return;
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private void OnComboKeyDown(object sender, KeyEventArgs e)
        {
            if (!HandleListKey(e)) return;
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        /// <summary>Shared list navigation for editor and native combo. Returns true when handled.</summary>
        private bool HandleListKey(KeyEventArgs e)
        {
            if (!Enabled || _readOnly) return false;

            bool toggle = e.KeyCode == Keys.F4 || (e.Alt && (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up));
            if (toggle)
            {
                if (IsPopupVisible) ClosePopup();
                else                OpenPopup("");
                return true;
            }

            switch (e.KeyCode)
            {
                case Keys.Down:
                case Keys.Up:
                    // Native combo changes selection directly while closed (standard behavior)
                    if (!IsPopupVisible && !_allowTyping) return false;
                    if (!IsPopupVisible) { OpenPopup(""); return true; }
                    MoveHot(e.KeyCode == Keys.Down ? 1 : -1);
                    return true;

                case Keys.PageDown:
                case Keys.PageUp:
                    if (!IsPopupVisible) return false;
                    MoveHot((e.KeyCode == Keys.PageDown ? 1 : -1) * _maxVisibleItems);
                    return true;

                case Keys.Enter:
                    if (!IsPopupVisible) return false;
                    if (_hotRow >= 0 && _hotRow < _matches.Count) CommitRow(_hotRow);
                    else { ClosePopup(); CommitTyped(); }
                    return true;

                case Keys.Escape:
                    if (!IsPopupVisible) return false;
                    ClosePopup();
                    if (_allowTyping)
                    {
                        SyncEditorText();
                        _editor.SelectAll();
                    }
                    return true;
            }
            return false;
        }

        private void OnComboKeyPress(object sender, KeyPressEventArgs e)
        {
            // Non-typing mode: letter jumps to the next item starting with it while the list is open
            if (!IsPopupVisible || char.IsControl(e.KeyChar)) return;
            e.Handled = true;

            int count = _matches.Count;
            for (int step = 1; step <= count; step++)
            {
                int row = ((_hotRow < 0 ? -1 : _hotRow) + step) % count;
                string text = ItemText(_matches[row]);
                if (text.Length > 0 && char.ToUpperInvariant(text[0]) == char.ToUpperInvariant(e.KeyChar))
                {
                    SetHotRow(row, true);
                    return;
                }
            }
        }

        // ── Editor (AllowTyping) ─────────────────────────────────

        private void OnEditorTextChanged(object sender, EventArgs e)
        {
            if (_suppressEditorSync || !_editor.Focused) return;
            OpenPopup(_editor.Text.Trim());
        }

        private void OnEditorGotFocus(object sender, EventArgs e)
        {
            UpdateFocusState();
            // Select all after the click finishes placing the caret so typing replaces the value
            BeginInvoke(new Action(() => { if (!IsDisposed && _editor.Focused) _editor.SelectAll(); }));
        }

        private void OnEditorLeave(object sender, EventArgs e)
        {
            // Keyboard Tab arrives here with the list still open → take the highlighted match
            string text = _editor.Text.Trim();
            bool typedNew = text.Length > 0 && !string.Equals(text, SelectedText, StringComparison.OrdinalIgnoreCase);
            if (IsPopupVisible && typedNew && _hotRow >= 0 && _hotRow < _matches.Count)
            {
                CommitRow(_hotRow);
                return;
            }

            ClosePopup();
            CommitTyped();
        }

        private void OnInnerLostFocus(object sender, EventArgs e)
        {
            // Defer so Leave (keyboard) can still read popup state; mouse focus changes close here
            if (IsHandleCreated)
                BeginInvoke(new Action(() =>
                {
                    if (IsDisposed) return;
                    if (!_editor.Focused && !_combo.Focused) ClosePopup();
                    UpdateFocusState();
                }));
        }

        /// <summary>Exact (case-insensitive) match selects; anything else reverts to the last selection.</summary>
        private void CommitTyped()
        {
            if (!_allowTyping) return;

            string text = _editor.Text.Trim();
            if (text.Length == 0)
            {
                _combo.SelectedIndex = -1;
            }
            else
            {
                int exact = _combo.FindStringExact(text);
                if (exact >= 0) _combo.SelectedIndex = exact;
            }
            SyncEditorText();
        }

        // ── Popup ────────────────────────────────────────────────

        private bool IsPopupVisible => _popup != null && !_popup.IsDisposed && _popup.Visible;

        private void EnsurePopup()
        {
            if (_popup != null && !_popup.IsDisposed) return;

            _popup = new WerComboPopup(PopupBorder);
            _popup.List.Font        = Font;
            _popup.List.ItemHeight  = RowHeight;
            _popup.List.DrawItem    += OnPopupDrawItem;
            _popup.List.MouseMove   += OnPopupMouseMove;
            _popup.List.ItemClicked += OnPopupItemClicked;
        }

        private void OpenPopup(string filter)
        {
            if (!Enabled || _readOnly || IsDisposed) return;

            var form = TopLevelControl as Form;
            if (form == null) return;

            EnsurePopup();
            HookOwnerForm(form);
            if (_popup.Owner != form) _popup.Owner = form;

            BuildMatches(filter);
            PositionPopup();

            // Highlight: first match while searching, current selection when showing all
            int hot = -1;
            if (_matches.Count > 0)
                hot = _filterText.Length > 0 ? 0 : _matches.IndexOf(_combo.SelectedIndex);
            _hotRow = hot;
            EnsureRowVisible(hot);

            if (!_popup.Visible)
            {
                _popup.Show();
                Application.AddMessageFilter(_messageFilter);
            }
            _popup.List.Invalidate();
            UpdateFocusState();
            _inputBorder.Invalidate();
        }

        private void ClosePopup()
        {
            if (!IsPopupVisible) return;
            Application.RemoveMessageFilter(_messageFilter);
            _popup.Hide();
            _hotRow = -1;
            UpdateFocusState();
            _inputBorder.Invalidate();
        }

        private void BuildMatches(string filter)
        {
            _filterText = filter ?? "";
            _matches.Clear();

            int count = _combo.Items.Count;
            if (_filterText.Length == 0)
            {
                for (int i = 0; i < count; i++) _matches.Add(i);
            }
            else
            {
                // Starts-with matches first, then contains matches
                var contains = new List<int>();
                for (int i = 0; i < count; i++)
                {
                    int pos = ItemText(i).IndexOf(_filterText, StringComparison.OrdinalIgnoreCase);
                    if (pos == 0)     _matches.Add(i);
                    else if (pos > 0) contains.Add(i);
                }
                _matches.AddRange(contains);
            }

            var list = _popup.List;
            list.BeginUpdate();
            list.Items.Clear();
            if (_matches.Count == 0)
            {
                list.Items.Add(NoMatchesText);
            }
            else
            {
                var rows = new object[_matches.Count];
                for (int i = 0; i < rows.Length; i++) rows[i] = _matches[i];
                list.Items.AddRange(rows);
            }
            list.EndUpdate();
        }

        private void PositionPopup()
        {
            var list   = _popup.List;
            int rows   = Math.Max(1, Math.Min(list.Items.Count, _maxVisibleItems));
            int height = rows * list.ItemHeight + _popup.Padding.Vertical;
            int width  = Width;

            var area   = Screen.FromControl(this).WorkingArea;
            var origin = _inputBorder.PointToScreen(new Point(0, _inputBorder.Height + PopupGap));

            // Flip above the field when it would run off the bottom of the screen
            if (origin.Y + height > area.Bottom)
                origin.Y = _inputBorder.PointToScreen(Point.Empty).Y - height - PopupGap;
            if (origin.X + width > area.Right)
                origin.X = area.Right - width;

            _popup.Bounds = new Rectangle(origin, new Size(width, height));
        }

        private void MoveHot(int delta)
        {
            if (_matches.Count == 0) return;
            int row = _hotRow < 0 ? (delta > 0 ? 0 : _matches.Count - 1) : _hotRow + delta;
            SetHotRow(Math.Max(0, Math.Min(_matches.Count - 1, row)), true);
        }

        private void SetHotRow(int row, bool scrollIntoView)
        {
            if (row == _hotRow) return;
            int old = _hotRow;
            _hotRow = row;
            if (scrollIntoView) EnsureRowVisible(row);
            InvalidateRow(old);
            InvalidateRow(row);
        }

        private void EnsureRowVisible(int row)
        {
            if (row < 0 || _popup == null) return;
            var list = _popup.List;
            int visible = Math.Max(1, list.ClientSize.Height / list.ItemHeight);
            if (row < list.TopIndex)
                list.TopIndex = row;
            else if (row >= list.TopIndex + visible)
                list.TopIndex = row - visible + 1;
        }

        private void InvalidateRow(int row)
        {
            var list = _popup?.List;
            if (list == null || row < 0 || row >= list.Items.Count) return;
            list.Invalidate(list.GetItemRectangle(row));
        }

        private void CommitRow(int row)
        {
            if (row < 0 || row >= _matches.Count) return;
            int index = _matches[row];
            ClosePopup();
            _combo.SelectedIndex = index;
            SyncEditorText();
            _inputBorder.Invalidate();
        }

        private void OnPopupItemClicked(object sender, int row) => CommitRow(row);

        private void OnPopupMouseMove(object sender, MouseEventArgs e)
        {
            int row = _popup.List.IndexFromPoint(e.Location);
            if (row >= 0 && row < _matches.Count) SetHotRow(row, false);
        }

        /// <summary>Called by the message filter while the popup is open.</summary>
        private bool FilterPopupMessage(ref Message m)
        {
            if (!IsPopupVisible) return false;

            switch (m.Msg)
            {
                case NativeMethods.WM_LBUTTONDOWN:
                case NativeMethods.WM_RBUTTONDOWN:
                case NativeMethods.WM_MBUTTONDOWN:
                case NativeMethods.WM_NCLBUTTONDOWN:
                case NativeMethods.WM_NCRBUTTONDOWN:
                    if (!IsOwnHandle(m.HWnd)) ClosePopup();
                    break;

                case NativeMethods.WM_MOUSEWHEEL:
                    // Scroll the list regardless of which window has focus
                    int delta = (short)(((long)m.WParam >> 16) & 0xFFFF);
                    int lines = SystemInformation.MouseWheelScrollLines;
                    if (lines <= 0) lines = 3;
                    var list = _popup.List;
                    list.TopIndex = Math.Max(0, list.TopIndex - Math.Sign(delta) * lines);
                    return true;
            }
            return false;
        }

        private bool IsOwnHandle(IntPtr hwnd)
        {
            for (var c = Control.FromChildHandle(hwnd); c != null; c = c.Parent)
                if (c == this || c == _popup) return true;
            return false;
        }

        // ── Owner form tracking (close popup on move/resize/deactivate) ──

        private void HookOwnerForm(Form form)
        {
            if (_ownerForm == form) return;
            UnhookOwnerForm();
            _ownerForm = form;
            _ownerForm.Move       += OnOwnerFormChanged;
            _ownerForm.Resize     += OnOwnerFormChanged;
            _ownerForm.Deactivate += OnOwnerFormChanged;
        }

        private void UnhookOwnerForm()
        {
            if (_ownerForm == null) return;
            _ownerForm.Move       -= OnOwnerFormChanged;
            _ownerForm.Resize     -= OnOwnerFormChanged;
            _ownerForm.Deactivate -= OnOwnerFormChanged;
            _ownerForm = null;
        }

        private void OnOwnerFormChanged(object sender, EventArgs e) => ClosePopup();

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            ClosePopup();
            UnhookOwnerForm();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (!Visible) ClosePopup();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            ClosePopup();
            base.OnHandleDestroyed(e);
        }

        // ── Enable/disable ───────────────────────────────────────

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            if (!Enabled) ClosePopup();
            _combo.Enabled = Enabled;
            UpdateEditorState();
            Cursor = Enabled ? Cursors.Hand : Cursors.Default;
            Invalidate();
            _inputBorder.Invalidate();
        }

        // ── Layout ───────────────────────────────────────────────

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ClosePopup();
            LayoutInternals();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            if (_combo == null) return;

            _boldFont?.Dispose();   _boldFont = null;
            _italicFont?.Dispose(); _italicFont = null;

            _combo.Font  = Font;
            _editor.Font = Font;
            if (_popup != null && !_popup.IsDisposed)
            {
                _popup.List.Font       = Font;
                _popup.List.ItemHeight = RowHeight;
            }
            LayoutInternals();
        }

        private void LayoutInternals()
        {
            if (_inputBorder == null || _combo == null || _editor == null) return;

            int borderTop = _showLabel ? LabelHeight + LabelGap : 0;
            int borderH   = Height - borderTop;

            // Input border covers the full input area
            _inputBorder.SetBounds(0, borderTop, Width, borderH);

            // Native combo sits at the same position but behind the panel
            _combo.SetBounds(0, borderTop, Width, borderH);

            // Editor sits inside the border, between left padding and chevron
            int textH = _editor.PreferredHeight;
            _editor.SetBounds(InputPadH, Math.Max(0, (borderH - textH) / 2),
                Math.Max(0, Width - InputPadH - ChevronWidth), textH);

            _inputBorder.BringToFront();
        }

        // ── Paint label ──────────────────────────────────────────

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            if (!_showLabel) return;

            Color labelColor = !Enabled ? LabelDisabled : LabelNormal;

            var labelRect = new Rectangle(0, 0, Width, LabelHeight);
            TextRenderer.DrawText(g, _labelText, WerTheme.LabelFont, labelRect, labelColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);

            if (_required && Enabled)
            {
                int lw = TextRenderer.MeasureText(g, _labelText, Font).Width;
                TextRenderer.DrawText(g, "*", Font, new Rectangle(lw + 2, 0, 12, LabelHeight),
                    RequiredStar, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            }
        }

        // ── Paint input area ─────────────────────────────────────

        private void OnBorderPaint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode     = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            var panel = (Panel)sender;
            var rect  = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
            bool inactive = !Enabled || _readOnly;

            // Background fill
            using (var path = RoundedRect(rect, BorderRadius))
            using (var brush = new SolidBrush(inactive ? BgDisabled : BgNormal))
                g.FillPath(brush, path);

            // Border
            if (Enabled && !_readOnly)
            {
                var borderColor = _hasFocus ? BorderFocus : BorderNormal;
                using (var path = RoundedRect(rect, BorderRadius))
                using (var pen  = new Pen(borderColor, _hasFocus ? 1.5f : 1f))
                    g.DrawPath(pen, path);
            }
            else if (_readOnly)
            {
                using (var path = RoundedRect(rect, BorderRadius))
                using (var pen  = new Pen(BorderNormal, 1f))
                    g.DrawPath(pen, path);
            }

            // Text: selected value or placeholder (editor draws its own text when typing is enabled)
            if (!_allowTyping)
            {
                var textRect = new Rectangle(InputPadH, 0, panel.Width - InputPadH - ChevronWidth, panel.Height);

                if (_combo.SelectedIndex >= 0)
                {
                    var textColor = !Enabled ? LabelDisabled : WerTheme.TextColor;
                    TextRenderer.DrawText(g, SelectedText, Font, textRect, textColor,
                        TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine |
                        TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis);
                }
                else
                {
                    TextRenderer.DrawText(g, _placeholder, Font, textRect, PlaceholderColor,
                        TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix);
                }
            }

            // Chevron ∨ (∧ while open)
            if (Enabled && !_readOnly)
            {
                var chevronRect = new Rectangle(panel.Width - ChevronWidth, 0, ChevronWidth, panel.Height);
                DrawChevron(g, chevronRect, IsPopupVisible, _hasFocus ? BorderFocus : WerTheme.IconColor);
            }
        }

        private static void DrawChevron(Graphics g, Rectangle bounds, bool up, Color color)
        {
            int cx = bounds.X + bounds.Width / 2;
            int cy = bounds.Y + bounds.Height / 2;
            int w = 5, h = up ? -3 : 3;

            using (var pen = new Pen(color, 1.8f) { LineJoin = LineJoin.Round })
            {
                g.DrawLines(pen, new[]
                {
                    new PointF(cx - w, cy - h),
                    new PointF(cx, cy + h),
                    new PointF(cx + w, cy - h)
                });
            }
        }

        // ── Owner-draw popup rows ────────────────────────────────

        private void OnPopupDrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            var g = e.Graphics;
            g.SmoothingMode     = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            using (var bg = new SolidBrush(BgNormal))
                g.FillRectangle(bg, e.Bounds);

            var textRect = new Rectangle(e.Bounds.X + InputPadH, e.Bounds.Y,
                e.Bounds.Width - InputPadH * 2, e.Bounds.Height);

            if (_matches.Count == 0)
            {
                TextRenderer.DrawText(g, NoMatchesText, ItalicFont, textRect, WerTheme.MutedColor, RowTextFlags);
                return;
            }
            if (e.Index >= _matches.Count) return;

            if (e.Index == _hotRow)
            {
                var hover = new Rectangle(e.Bounds.X + 2, e.Bounds.Y + 2, e.Bounds.Width - 4, e.Bounds.Height - 4);
                using (var path  = RoundedRect(hover, 6))
                using (var brush = new SolidBrush(RowHoverBg))
                    g.FillPath(brush, path);
            }

            int index   = _matches[e.Index];
            string text = ItemText(index);
            Color color = index == _combo.SelectedIndex ? WerTheme.PrimaryColor : WerTheme.TextColor;

            int pos = _filterText.Length > 0 ? text.IndexOf(_filterText, StringComparison.OrdinalIgnoreCase) : -1;
            if (pos < 0)
            {
                TextRenderer.DrawText(g, text, Font, textRect, color, RowTextFlags | TextFormatFlags.EndEllipsis);
                return;
            }

            // Draw matched part in bold: [before][match][after]
            int x = textRect.X;
            x = DrawSegment(g, text.Substring(0, pos), Font, x, textRect, color);
            x = DrawSegment(g, text.Substring(pos, _filterText.Length), BoldFont, x, textRect, color);
            DrawSegment(g, text.Substring(pos + _filterText.Length), Font, x, textRect, color);
        }

        private static int DrawSegment(Graphics g, string segment, Font font, int x, Rectangle bounds, Color color)
        {
            if (segment.Length == 0 || x >= bounds.Right) return x;
            var segRect = new Rectangle(x, bounds.Y, bounds.Right - x, bounds.Height);
            TextRenderer.DrawText(g, segment, font, segRect, color, RowTextFlags | TextFormatFlags.EndEllipsis);
            return x + TextRenderer.MeasureText(g, segment, font, new Size(int.MaxValue, bounds.Height), RowTextFlags).Width;
        }

        // ── Dispose ──────────────────────────────────────────────

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ClosePopup();
                Application.RemoveMessageFilter(_messageFilter);
                UnhookOwnerForm();

                if (_popup != null)
                {
                    _popup.List.DrawItem    -= OnPopupDrawItem;
                    _popup.List.MouseMove   -= OnPopupMouseMove;
                    _popup.List.ItemClicked -= OnPopupItemClicked;
                    _popup.Dispose();
                    _popup = null;
                }

                _boldFont?.Dispose();   _boldFont = null;
                _italicFont?.Dispose(); _italicFont = null;
            }
            base.Dispose(disposing);
        }

        // ── Helpers ──────────────────────────────────────────────

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

        private static class NativeMethods
        {
            public const int WM_PAINT         = 0x000F;
            public const int WM_MOUSEACTIVATE = 0x0021;
            public const int WM_NCLBUTTONDOWN = 0x00A1;
            public const int WM_NCRBUTTONDOWN = 0x00A4;
            public const int WM_LBUTTONDOWN   = 0x0201;
            public const int WM_LBUTTONDBLCLK = 0x0203;
            public const int WM_RBUTTONDOWN   = 0x0204;
            public const int WM_MBUTTONDOWN   = 0x0207;
            public const int WM_MOUSEWHEEL    = 0x020A;
            public const int MA_NOACTIVATE    = 3;
            public const int WS_EX_TOOLWINDOW = 0x00000080;
            public const int WS_EX_NOACTIVATE = 0x08000000;
            public const int CS_DROPSHADOW    = 0x00020000;
        }

        /// <summary>Panel with double buffering enabled to prevent flicker.</summary>
        private class DoubleBufferedPanel : Panel
        {
            public DoubleBufferedPanel()
            {
                SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                         ControlStyles.OptimizedDoubleBuffer, true);
            }
        }

        /// <summary>Borderless TextBox that paints a placeholder when empty and unfocused.</summary>
        private sealed class PlaceholderTextBox : TextBox
        {
            public string Placeholder { get; set; } = "";

            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);
                if (m.Msg != NativeMethods.WM_PAINT || TextLength > 0 || Focused || string.IsNullOrEmpty(Placeholder))
                    return;

                using (var g = Graphics.FromHwnd(Handle))
                    TextRenderer.DrawText(g, Placeholder, Font, ClientRectangle, PlaceholderColor,
                        RowTextFlags | TextFormatFlags.EndEllipsis);
            }

            protected override void OnGotFocus(EventArgs e)  { base.OnGotFocus(e);  Invalidate(); }
            protected override void OnLostFocus(EventArgs e) { base.OnLostFocus(e); Invalidate(); }
        }

        /// <summary>Non-activating rounded dropdown window with drop shadow. Keeps focus in the field.</summary>
        private sealed class WerComboPopup : Form
        {
            private readonly Color _borderColor;
            public readonly PopupListBox List;

            public WerComboPopup(Color borderColor)
            {
                _borderColor    = borderColor;
                FormBorderStyle = FormBorderStyle.None;
                ShowInTaskbar   = false;
                StartPosition   = FormStartPosition.Manual;
                AutoScaleMode   = AutoScaleMode.None;
                BackColor       = BgNormal;
                Padding         = new Padding(4);
                SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

                List = new PopupListBox
                {
                    Dock           = DockStyle.Fill,
                    BorderStyle    = BorderStyle.None,
                    DrawMode       = DrawMode.OwnerDrawFixed,
                    IntegralHeight = false,
                    SelectionMode  = SelectionMode.None,
                    BackColor      = BgNormal,
                    TabStop        = false,
                };
                Controls.Add(List);
            }

            protected override bool ShowWithoutActivation => true;

            protected override CreateParams CreateParams
            {
                get
                {
                    var cp = base.CreateParams;
                    cp.ExStyle    |= NativeMethods.WS_EX_NOACTIVATE | NativeMethods.WS_EX_TOOLWINDOW;
                    cp.ClassStyle |= NativeMethods.CS_DROPSHADOW;
                    return cp;
                }
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == NativeMethods.WM_MOUSEACTIVATE)
                {
                    m.Result = (IntPtr)NativeMethods.MA_NOACTIVATE;
                    return;
                }
                base.WndProc(ref m);
            }

            protected override void OnSizeChanged(EventArgs e)
            {
                base.OnSizeChanged(e);
                var old = Region;
                using (var path = RoundedRect(new Rectangle(0, 0, Width, Height), BorderRadius))
                    Region = new Region(path);
                old?.Dispose();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = RoundedRect(new Rectangle(0, 0, Width - 1, Height - 1), BorderRadius))
                using (var pen  = new Pen(_borderColor, 1f))
                    e.Graphics.DrawPath(pen, path);
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    var region = Region;
                    Region = null;
                    region?.Dispose();
                }
                base.Dispose(disposing);
            }
        }

        /// <summary>ListBox that reports clicks without taking focus or activating its window.</summary>
        private sealed class PopupListBox : ListBox
        {
            public event EventHandler<int> ItemClicked;

            protected override void WndProc(ref Message m)
            {
                switch (m.Msg)
                {
                    case NativeMethods.WM_MOUSEACTIVATE:
                        m.Result = (IntPtr)NativeMethods.MA_NOACTIVATE;
                        return;

                    case NativeMethods.WM_LBUTTONDOWN:
                    case NativeMethods.WM_LBUTTONDBLCLK:
                        // Skip base so the list never steals focus from the field
                        long lp = (long)m.LParam;
                        var pt  = new Point((short)(lp & 0xFFFF), (short)((lp >> 16) & 0xFFFF));
                        int row = IndexFromPoint(pt);
                        if (row >= 0 && row < Items.Count) ItemClicked?.Invoke(this, row);
                        return;
                }
                base.WndProc(ref m);
            }
        }

        private sealed class PopupMessageFilter : IMessageFilter
        {
            private readonly WerComboBox _owner;
            public PopupMessageFilter(WerComboBox owner) { _owner = owner; }
            public bool PreFilterMessage(ref Message m) => _owner.FilterPopupMessage(ref m);
        }
    }
}
