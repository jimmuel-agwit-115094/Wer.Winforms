using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    [ToolboxItem(true)]
    public class WerDataGrid : UserControl
    {
        // --- Colors ---
        private static readonly Color HeaderBg = Color.FromArgb(248, 249, 250);
        private static readonly Color HeaderText = Color.FromArgb(55, 65, 81);
        private static readonly Color HeaderSep = Color.FromArgb(228, 231, 235);
        private static readonly Color RowSep = Color.FromArgb(238, 240, 243);
        private static readonly Color SelectedBg = Color.FromArgb(234, 245, 252);
        private static readonly Color ContainerBorder = Color.FromArgb(218, 222, 228);
        private static readonly Color DataText = Color.FromArgb(33, 37, 41);
        private static readonly Color EditBtnBorder = Color.FromArgb(12, 124, 146);
        private static readonly Color EditBtnText = Color.FromArgb(12, 124, 146);
        private static readonly Color EditBtnHover = Color.FromArgb(240, 250, 252);

        // --- Metrics ---
        private const int CornerRadius = 8;
        private const int HeaderHeight = 44;
        private const int RowHeight = 42;
        private const int CellPadding = 14;
        private const int EditBtnWidth = 60;
        private const int EditBtnHeight = 28;
        private const int EditBtnRadius = 6;
        private const int SelectionRadius = 7;

        // --- State ---
        private DataTable _dataTable;
        private List<WerDataGridColumn> _columns = new List<WerDataGridColumn>();
        private string _primaryKeyColumn;
        private bool _showEditColumn;
        private bool _allowSorting = true;
        private int _selectedRowIndex = 0;
        private int _hoverRowIndex = -1;
        private int _hoverEditRow = -1;
        private int _scrollOffset;
        private int _sortColumnIndex = -1;
        private bool _sortAscending = true;
        private int _hoverHeaderCol = -1;
        private int[] _userColumnWidths;
        private int _resizingCol = -1;
        private int _resizeStartX;
        private int _resizeStartWidth;

        // --- Tabs ---
        private string[] _tabOptions;
        private int _tabSelected = 0;
        private int _hoverTab = -1;

        // --- Pagination ---
        private int _pageSize = 25;
        private int _currentPage = 0;
        private string _totalAmountColumn;
        private int _hoverNavBtn = -1;
        private const int FooterHeight = 44;
        private const int SearchBarHeight = 42;
        private const int TabHeight = 30;
        private const int TabPadH = 14;
        private const int TabGap = 2;
        private static readonly string[] NavLabels = { "|<", "<", ">", ">|" };

        private VScrollBar _vScroll;
        private HScrollBar _hScroll;
        private bool _horizontalScroll;
        private int _hScrollOffset;
        private WerSearchField _searchBox;
        private WerButtonPrimary _addButton;
        private bool _showAddButton;
        private string _searchText = "";
        private DataTable _filteredTable;
        private Font _headerFont;
        private Font _dataFont;
        private Font _editFont;
        private Font _footerFont;
        private Font _addButtonFont;

        public event EventHandler<WerDataGridEditEventArgs> EditClicked;

        /// <summary>
        /// Fired when the selected row changes (keyboard or mouse).
        /// The second argument is the PrimaryKeyColumn value of the newly selected row.
        /// Usage: werDataGrid1.SelectedRowChanged += (s, id) => lblSelected.Text = id?.ToString();
        /// </summary>
        public event EventHandler<object> SelectedRowChanged;

        /// <summary>Fired when a tab is clicked.</summary>
        public event EventHandler TabChanged;

        /// <summary>Fired when the Add button is clicked.</summary>
        public event EventHandler AddClicked;

        /// <summary>Show an Add button to the left of the search bar.</summary>
        [Category("Wer Data")]
        [DefaultValue(false)]
        [Description("Show an Add button to the left of the search bar.")]
        public bool ShowAddButton
        {
            get => _showAddButton;
            set
            {
                _showAddButton = value;
                if (_addButton != null) _addButton.Visible = value;
                PositionSearchBox();
                InvalidateGrid();
            }
        }

        /// <summary>
        /// Tab labels shown at the top-left of the grid.
        /// Set via Properties panel (collection editor) or code.
        /// Usage: dgUsers.TabOptions = new[] { "All", "Active", "On Hold" };
        /// null or empty = no tabs shown.
        /// </summary>
        [Category("Wer Data")]
        [Description("Tab labels displayed at the top-left of the grid.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor("System.Windows.Forms.Design.StringArrayEditor, System.Design", typeof(System.Drawing.Design.UITypeEditor))]
        public string[] TabOptions
        {
            get => _tabOptions ?? new string[0];
            set { _tabOptions = (value != null && value.Length > 0) ? value : null; _tabSelected = 0; InvalidateGrid(); }
        }

        /// <summary>
        /// Index of the currently selected tab (0-based).
        /// Usage: if (dgUsers.TabSelected == 0) { /* All */ }
        /// </summary>
        [Category("Wer Data")]
        [DefaultValue(0)]
        public int TabSelected
        {
            get => _tabSelected;
            set
            {
                if (_tabOptions == null || value < 0 || value >= _tabOptions.Length) return;
                _tabSelected = value;
                InvalidateGrid();
                TabChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>The text of the currently selected tab.</summary>
        [Browsable(false)]
        public string TabSelectedText => _tabOptions != null && _tabSelected < _tabOptions.Length ? _tabOptions[_tabSelected] : "";

        [Category("Wer Data")]
        public string PrimaryKeyColumn
        {
            get => _primaryKeyColumn;
            set => _primaryKeyColumn = value;
        }

        /// <summary>
        /// The primary key value of the currently selected row.
        /// Returns null if no row is selected, PrimaryKeyColumn is not set, or no data is loaded.
        /// </summary>
        [Browsable(false)]
        public object SelectedRowId
        {
            get
            {
                if (_primaryKeyColumn == null || ActiveTable == null) return null;
                var view = GetSortedView();
                if (_selectedRowIndex < 0 || _selectedRowIndex >= view.Length) return null;
                return view[_selectedRowIndex][_primaryKeyColumn];
            }
        }

        [Category("Wer Data")]
        [DefaultValue(false)]
        public bool ShowEditColumn
        {
            get => _showEditColumn;
            set { _showEditColumn = value; RecalcLayout(); InvalidateGrid(); }
        }

        [Category("Wer Data")]
        [DefaultValue(true)]
        public bool AllowSorting
        {
            get => _allowSorting;
            set => _allowSorting = value;
        }

        /// <summary>
        /// Enable horizontal scrollbar when columns exceed control width.
        /// </summary>
        [Category("Wer Data")]
        [DefaultValue(false)]
        [Description("Enable horizontal scroll when columns exceed control width.")]
        public bool ShowHorizontalScroll
        {
            get => _horizontalScroll;
            set
            {
                _horizontalScroll = value;
                _hScroll.Visible = value;
                _hScrollOffset = 0;
                RecalcLayout();
                InvalidateGrid();
            }
        }

        /// <summary>
        /// Property names to display. null = show all. Empty = show none.
        /// </summary>
        [Category("Wer Data")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public string[] FieldOptions { get; set; }

        /// <summary>
        /// Rows per page. Default 25.
        /// </summary>
        [Category("Wer Data")]
        [DefaultValue(25)]
        public int PageSize
        {
            get => _pageSize;
            set { _pageSize = Math.Max(1, value); _currentPage = 0; RecalcLayout(); InvalidateGrid(); }
        }

        /// <summary>
        /// Property name to sum for "Total Amount" display. Must be numeric (int/decimal/double/float).
        /// null = hidden. String columns are ignored.
        /// </summary>
        [Category("Wer Data")]
        [DefaultValue(null)]
        public string TotalAmountColumn
        {
            get => _totalAmountColumn;
            set { _totalAmountColumn = value; InvalidateGrid(); }
        }

        [Browsable(false)]
        public object DataSource
        {
            get => _dataTable;
            set
            {
                if (value is DataTable dt)
                    _dataTable = dt;
                else if (value is IEnumerable enumerable)
                    _dataTable = ObjectListToDataTable(enumerable);
                else
                    _dataTable = null;

                if (_dataTable != null && _columns.Count == 0)
                    AutoGenerateColumns();

                _selectedRowIndex = 0;
                _scrollOffset = 0;
                _sortColumnIndex = -1;
                _userColumnWidths = null;
                RecalcLayout();
                InvalidateGrid();
            }
        }

        private DataTable ObjectListToDataTable(IEnumerable items)
        {
            var table = new DataTable();
            PropertyInfo[] props = null;

            foreach (var item in items)
            {
                if (props == null)
                {
                    props = item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    // If columns are defined, only include those properties
                    if (_columns.Count > 0)
                    {
                        var colNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                        foreach (var c in _columns)
                            colNames.Add(c.PropertyName);
                        if (_primaryKeyColumn != null)
                            colNames.Add(_primaryKeyColumn);

                        foreach (var p in props)
                        {
                            if (colNames.Contains(p.Name))
                            {
                                var colType = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                                table.Columns.Add(p.Name, colType);
                            }
                        }
                    }
                    else
                    {
                        foreach (var p in props)
                        {
                            var colType = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                            table.Columns.Add(p.Name, colType);
                        }
                    }
                }

                var row = table.NewRow();
                foreach (DataColumn col in table.Columns)
                {
                    var prop = item.GetType().GetProperty(col.ColumnName, BindingFlags.Public | BindingFlags.Instance);
                    if (prop != null)
                    {
                        var val = prop.GetValue(item);
                        row[col.ColumnName] = val ?? DBNull.Value;
                    }
                }
                table.Rows.Add(row);
            }

            return table;
        }

        public WerDataGrid()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);

            _headerFont = new Font(WerTheme.FontFamily, 9.75f, FontStyle.Regular);
            _dataFont = new Font(WerTheme.FontFamily, 9.75f, FontStyle.Regular);
            _editFont = new Font(WerTheme.FontFamily, 8.5f, FontStyle.Regular);
            _footerFont = new Font(WerTheme.FontFamily, 9f, FontStyle.Regular);

            _vScroll = new VScrollBar();
            _vScroll.Visible = false;
            _vScroll.Anchor = AnchorStyles.None; // positioned manually
            _vScroll.TabStop = false;
            _vScroll.ValueChanged += (s, e) => { _scrollOffset = _vScroll.Value; InvalidateGrid(); };
            Controls.Add(_vScroll);

            _hScroll = new HScrollBar();
            _hScroll.Dock = DockStyle.Bottom;
            _hScroll.Visible = false;
            _hScroll.ValueChanged += (s, e) => { _hScrollOffset = _hScroll.Value; InvalidateGrid(); };
            Controls.Add(_hScroll);

            _searchBox = new WerSearchField();
            _searchBox.Size = new Size(260, 30);
            _searchBox.Text = "";
            _searchBox.TextChanged += (s, ev) =>
            {
                _searchText = _searchBox.Text.Trim();
                ApplySearch();
                _currentPage = 0;
                _selectedRowIndex = 0;
                RecalcLayout();
                InvalidateGrid();
            };
            Controls.Add(_searchBox);

            _addButton = new WerButtonPrimary();
            _addButton.Text = "Add";
            _addButtonFont = new Font(WerTheme.FontFamily, 9.75f, FontStyle.Bold);
            _addButton.Font = _addButtonFont;
            _addButton.Size = new Size(80, 30);
            _addButton.Visible = false;
            _addButton.Click += (s, ev) => AddClicked?.Invoke(this, EventArgs.Empty);
            Controls.Add(_addButton);

            BackColor = Color.White;
            PositionSearchBox();

            // Keyboard navigation
            SetStyle(ControlStyles.Selectable, true);
            TabStop = true;
        }

        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Up || keyData == Keys.Down ||
                keyData == Keys.PageUp || keyData == Keys.PageDown)
                return true;
            return base.IsInputKey(keyData);
        }

        // Intercept arrow/page keys before any child control (VScrollBar) gets them
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.PageUp:
                case Keys.PageDown:
                    OnKeyDown(new KeyEventArgs(keyData));
                    return true; // consumed — scrollbar never sees it
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            // Handle wheel here — don't let VScrollBar steal it
            if (_vScroll.Visible)
            {
                int delta = e.Delta > 0 ? -3 : 3;
                int newVal = Math.Max(_vScroll.Minimum,
                    Math.Min(_vScroll.Maximum - _vScroll.LargeChange + 1, _vScroll.Value + delta));
                _vScroll.Value = newVal;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            int startRow = PageStartRow;
            int endRow = PageEndRow;
            int pageRows = endRow - startRow;

            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (_selectedRowIndex < endRow - 1)
                    {
                        _selectedRowIndex++;
                        EnsureRowVisible(_selectedRowIndex);
                        InvalidateGrid();
                        SelectedRowChanged?.Invoke(this, SelectedRowId);
                    }
                    e.Handled = true;
                    break;

                case Keys.Up:
                    if (_selectedRowIndex > startRow)
                    {
                        _selectedRowIndex--;
                        EnsureRowVisible(_selectedRowIndex);
                        InvalidateGrid();
                        SelectedRowChanged?.Invoke(this, SelectedRowId);
                    }
                    e.Handled = true;
                    break;

                case Keys.PageDown:
                    if (_currentPage < TotalPages - 1)
                    {
                        _currentPage++;
                        _selectedRowIndex = PageStartRow;
                        _scrollOffset = 0;
                        if (_vScroll.Visible) _vScroll.Value = 0;
                        RecalcLayout();
                        InvalidateGrid();
                    }
                    e.Handled = true;
                    break;

                case Keys.PageUp:
                    if (_currentPage > 0)
                    {
                        _currentPage--;
                        _selectedRowIndex = PageStartRow;
                        _scrollOffset = 0;
                        if (_vScroll.Visible) _vScroll.Value = 0;
                        RecalcLayout();
                        InvalidateGrid();
                    }
                    e.Handled = true;
                    break;
            }
        }

        private void EnsureRowVisible(int rowIndex)
        {
            int visualRow = rowIndex - PageStartRow - _scrollOffset;
            int visibleRows = VisibleRowCount;

            if (visualRow < 0)
            {
                // Scroll up
                _scrollOffset = Math.Max(0, rowIndex - PageStartRow);
                if (_vScroll.Visible) _vScroll.Value = Math.Min(_scrollOffset, _vScroll.Maximum - _vScroll.LargeChange + 1);
            }
            else if (visualRow >= visibleRows)
            {
                // Scroll down
                _scrollOffset = rowIndex - PageStartRow - visibleRows + 1;
                if (_vScroll.Visible) _vScroll.Value = Math.Min(_scrollOffset, _vScroll.Maximum - _vScroll.LargeChange + 1);
            }
        }

        public void SetColumns(WerDataGridColumn[] columns)
        {
            _columns = new List<WerDataGridColumn>(columns);
            RecalcLayout();
            InvalidateGrid();
        }

        private void AutoGenerateColumns()
        {
            _columns.Clear();
            if (_dataTable == null) return;

            if (FieldOptions != null)
            {
                // Only include specified fields, in order
                foreach (var field in FieldOptions)
                {
                    if (_dataTable.Columns.Contains(field))
                        _columns.Add(new WerDataGridColumn(field, field));
                }
            }
            else
            {
                foreach (DataColumn dc in _dataTable.Columns)
                    _columns.Add(new WerDataGridColumn(dc.ColumnName, dc.ColumnName));
            }
        }

        private DataTable ActiveTable => _filteredTable ?? _dataTable;

        private void ApplySearch()
        {
            if (_dataTable == null || string.IsNullOrEmpty(_searchText))
            {
                _filteredTable = null;
                return;
            }

            var filtered = _dataTable.Clone(); // schema only
            string search = _searchText.ToLowerInvariant();

            foreach (DataRow dr in _dataTable.Rows)
            {
                foreach (var col in _columns)
                {
                    var val = dr[col.PropertyName];
                    if (val != null && val != DBNull.Value)
                    {
                        string text = FormatValue(val).ToLowerInvariant();
                        if (text.Contains(search))
                        {
                            filtered.ImportRow(dr);
                            break;
                        }
                    }
                }
            }

            _filteredTable = filtered;
        }

        private int RowCount => ActiveTable?.Rows.Count ?? 0;

        private bool ShowFooter => (_dataTable?.Rows.Count ?? 0) > 0;

        private int ContentAreaTop => CornerRadius + HeaderHeight + SearchBarHeight;

        private int FooterTop
        {
            get
            {
                int hScrollH = (_hScroll != null && _hScroll.Visible) ? _hScroll.Height : 0;
                return Height - hScrollH - (ShowFooter ? FooterHeight + CornerRadius + 2 : CornerRadius + 2);
            }
        }

        private int TotalPages => RowCount > 0 ? (int)Math.Ceiling((double)RowCount / _pageSize) : 1;

        private int PageStartRow => _currentPage * _pageSize;

        private int PageEndRow => Math.Min(PageStartRow + _pageSize, RowCount);

        private int VisibleRowCount
        {
            get
            {
                int bottomReserve = (ShowFooter ? FooterHeight : 0) + CornerRadius + 4;
                int available = Height - ContentAreaTop - bottomReserve;
                return Math.Max(1, available / RowHeight);
            }
        }

        private void RecalcLayout()
        {
            if (_currentPage >= TotalPages)
                _currentPage = Math.Max(0, TotalPages - 1);

            // Vertical scroll: when page rows exceed visible pixel rows
            int pageRows = PageEndRow - PageStartRow;
            int visibleRows = VisibleRowCount;
            if (pageRows > visibleRows)
            {
                _vScroll.Visible = true;
                _vScroll.Minimum = 0;
                _vScroll.Maximum = pageRows - 1;
                _vScroll.LargeChange = Math.Max(1, visibleRows);
                _vScroll.SmallChange = 1;
            }
            else
            {
                _vScroll.Visible = false;
                _scrollOffset = 0;
            }

            // Horizontal scroll
            if (_horizontalScroll && _columns.Count > 0)
            {
                var widths = GetColumnWidths();
                int totalColW = 0;
                for (int i = 0; i < widths.Length; i++) totalColW += widths[i];

                int scrollW = _vScroll.Visible ? _vScroll.Width : 0;
                int viewW = Width - scrollW - 2;

                if (totalColW > viewW)
                {
                    _hScroll.Visible = true;
                    _hScroll.Minimum = 0;
                    _hScroll.Maximum = totalColW;
                    _hScroll.LargeChange = Math.Max(1, viewW);
                    _hScroll.SmallChange = 30;
                    if (_hScrollOffset > _hScroll.Maximum - _hScroll.LargeChange)
                        _hScrollOffset = Math.Max(0, _hScroll.Maximum - _hScroll.LargeChange);
                }
                else
                {
                    _hScroll.Visible = false;
                    _hScrollOffset = 0;
                }
            }
            else
            {
                if (_hScroll != null) _hScroll.Visible = false;
                _hScrollOffset = 0;
            }

            PositionSearchBox();
        }

        private int[] GetColumnWidths()
        {
            if (_columns.Count == 0) return new int[0];

            int scrollW = _vScroll.Visible ? _vScroll.Width : 0;
            int totalW = Width - scrollW - 2;
            int editColW = _showEditColumn ? 90 : 0;
            int available = totalW - editColW;
            int colCount = _columns.Count;

            int[] widths = new int[colCount + (_showEditColumn ? 1 : 0)];

            // If user has resized, use those widths
            if (_userColumnWidths != null && _userColumnWidths.Length == colCount)
            {
                Array.Copy(_userColumnWidths, widths, colCount);
                if (_showEditColumn)
                    widths[colCount] = editColW;
                return widths;
            }

            // Auto-fit: measure header + data content
            using (var bmp = new Bitmap(1, 1))
            using (var g = Graphics.FromImage(bmp))
            {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                for (int i = 0; i < colCount; i++)
                {
                    // Measure header
                    string header = _columns[i].HeaderText ?? _columns[i].PropertyName;
                    if (_sortColumnIndex == i)
                        header += "  \u25B2";
                    int maxW = TextRenderer.MeasureText(g, header, _headerFont).Width + CellPadding * 2 + 4;

                    // Measure data (sample up to 50 rows)
                    var measureTable = ActiveTable;
                    if (measureTable != null)
                    {
                        int sampleCount = Math.Min(measureTable.Rows.Count, 50);
                        for (int r = 0; r < sampleCount; r++)
                        {
                            var val = measureTable.Rows[r][_columns[i].PropertyName];
                            string text = FormatValue(val);
                            int tw = TextRenderer.MeasureText(g, text, _dataFont).Width + CellPadding * 2 + 4;
                            if (tw > maxW) maxW = tw;
                        }
                    }

                    widths[i] = Math.Max(50, maxW);
                }
            }

            // Scale to fit available width (only when horizontal scroll is off)
            int totalMeasured = 0;
            for (int i = 0; i < colCount; i++)
                totalMeasured += widths[i];

            if (totalMeasured < available)
            {
                int extra = available - totalMeasured;
                for (int i = 0; i < colCount; i++)
                {
                    int share = (int)((double)widths[i] / totalMeasured * extra);
                    widths[i] += share;
                }
                int sum = 0;
                for (int i = 0; i < colCount; i++) sum += widths[i];
                widths[colCount - 1] += available - sum;
            }

            if (_showEditColumn)
                widths[colCount] = editColW;

            return widths;
        }

        /// <summary>Invalidate only the grid area (below search bar) to avoid flickering the search box.</summary>
        private void InvalidateGrid()
        {
            // Tab area (left side of search bar row)
            int searchLeft = _searchBox != null ? _searchBox.Left - 4 : Width;
            Invalidate(new Rectangle(0, 0, searchLeft, SearchBarHeight));

            // Grid area (below search bar)
            Invalidate(new Rectangle(0, SearchBarHeight, Width, Height - SearchBarHeight));
        }

        private static string FormatValue(object val)
        {
            if (val == null || val == DBNull.Value) return "";
            if (val is decimal decVal) return decVal.ToString("#,##0.00");
            if (val is double dblVal) return dblVal.ToString("#,##0.00");
            if (val is float fltVal) return fltVal.ToString("#,##0.00");
            if (val is DateTime dtVal) return dtVal.ToString("MMM. dd, yyyy");
            return val.ToString();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            int scrollW = _vScroll.Visible ? _vScroll.Width : 0;
            int w = Width - scrollW;
            int h = Height;

            // White background above grid (search + tabs area)
            using (var brush = new SolidBrush(Color.White))
                g.FillRectangle(brush, 0, 0, w, SearchBarHeight);

            // --- Tabs ---
            if (_tabOptions != null && _tabOptions.Length > 0)
                DrawTabs(g);

            // --- Outer rounded container (below search) ---
            using (var path = RoundedRect(0, SearchBarHeight, w - 1, h - SearchBarHeight - 1, CornerRadius))
            {
                g.SetClip(path);

                // White background
                using (var brush = new SolidBrush(Color.White))
                    g.FillRectangle(brush, 0, SearchBarHeight, w, h - SearchBarHeight);

                // --- Header ---
                DrawHeader(g, w);

                // --- Data rows ---
                if (RowCount == 0)
                    DrawNoRecords(g, w, h);
                else
                    DrawRows(g, w, h);

                // --- Footer (pagination + total) ---
                if (ShowFooter)
                    DrawFooter(g, w, h);

                g.ResetClip();

                // Container border
                using (var pen = new Pen(ContainerBorder, 1f))
                    g.DrawPath(pen, path);
            }
        }

        private void DrawTabs(Graphics g)
        {
            int containerY = (SearchBarHeight - TabHeight) / 2;
            int innerPad = 4;

            // Measure total width of all tabs
            using (var measureFont = new Font(WerTheme.FontFamily, 9.75f, FontStyle.Regular))
            {
                int totalTabsW = innerPad;
                var tabWidths = new int[_tabOptions.Length];
                for (int i = 0; i < _tabOptions.Length; i++)
                {
                    tabWidths[i] = TextRenderer.MeasureText(g, _tabOptions[i], measureFont).Width + TabPadH * 2;
                    totalTabsW += tabWidths[i];
                }
                totalTabsW += innerPad;

                // Outer rounded container — aligned to grid left edge
                var containerRect = new Rectangle(0, containerY, totalTabsW, TabHeight);
                using (var path = RoundedRect(containerRect.X, containerRect.Y, containerRect.Width, containerRect.Height, 8))
                {
                    using (var brush = new SolidBrush(Color.White))
                        g.FillPath(brush, path);
                    using (var pen = new Pen(Color.FromArgb(210, 215, 222), 1f))
                        g.DrawPath(pen, path);
                }

                // Draw each tab inside
                int x = containerRect.X + innerPad;
                for (int i = 0; i < _tabOptions.Length; i++)
                {
                    string text = _tabOptions[i];
                    int tabW = tabWidths[i];
                    var tabRect = new Rectangle(x, containerY + 2, tabW, TabHeight - 4);

                    bool selected = i == _tabSelected;
                    bool hovered = i == _hoverTab && !selected;

                    if (selected)
                    {
                        using (var path = RoundedRect(tabRect.X, tabRect.Y, tabRect.Width, tabRect.Height, 6))
                        using (var brush = new SolidBrush(Color.FromArgb(245, 247, 250)))
                            g.FillPath(brush, path);
                    }
                    else if (hovered)
                    {
                        using (var path = RoundedRect(tabRect.X, tabRect.Y, tabRect.Width, tabRect.Height, 6))
                        using (var brush = new SolidBrush(Color.FromArgb(250, 251, 252)))
                            g.FillPath(brush, path);
                    }

                    var textColor = selected ? Color.FromArgb(12, 124, 146)
                                  : hovered ? Color.FromArgb(60, 65, 75)
                                  : Color.FromArgb(120, 130, 140);

                    var fontStyle = selected ? FontStyle.Bold : FontStyle.Regular;
                    using (var font = new Font(WerTheme.FontFamily, 9.75f, fontStyle))
                        TextRenderer.DrawText(g, text, font, tabRect, textColor,
                            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

                    x += tabW;
                }
            }
        }

        private void DrawHeader(Graphics g, int totalWidth)
        {
            int headerY = SearchBarHeight;

            // Header background with rounded top corners
            using (var path = RoundedRectTop(1, headerY, totalWidth - 2, HeaderHeight + CornerRadius, CornerRadius))
            using (var brush = new SolidBrush(HeaderBg))
                g.FillPath(brush, path);

            // Header bottom line
            using (var pen = new Pen(HeaderSep, 1f))
                g.DrawLine(pen, 1, headerY + HeaderHeight, totalWidth - 2, headerY + HeaderHeight);

            var widths = GetColumnWidths();
            int x = 1 - _hScrollOffset;

            for (int i = 0; i < _columns.Count; i++)
            {
                int colW = widths[i];
                var textRect = new Rectangle(x + CellPadding, headerY, colW - CellPadding * 2, HeaderHeight);

                // Header text
                var headerText = _columns[i].HeaderText ?? _columns[i].PropertyName;
                if (_sortColumnIndex == i)
                    headerText += _sortAscending ? "  \u25B2" : "  \u25BC";

                TextRenderer.DrawText(g, headerText, _headerFont, textRect, HeaderText,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);

                // Vertical separator (subtle, only in header)
                if (i < _columns.Count - 1 || _showEditColumn)
                {
                    using (var pen = new Pen(HeaderSep, 1f))
                        g.DrawLine(pen, x + colW, headerY + 10, x + colW, headerY + HeaderHeight - 10);
                }

                x += colW;
            }

            // "Actions" header if edit column
            if (_showEditColumn && widths.Length > _columns.Count)
            {
                int editW = widths[_columns.Count];
                var textRect = new Rectangle(x + CellPadding, headerY, editW - CellPadding * 2, HeaderHeight);
                TextRenderer.DrawText(g, "Actions", _headerFont, textRect, HeaderText,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
            }
        }

        private void DrawNoRecords(Graphics g, int totalWidth, int totalHeight)
        {
            int maxY = ShowFooter ? FooterTop : (totalHeight - CornerRadius);
            int areaHeight = maxY - ContentAreaTop;
            var rect = new Rectangle(0, ContentAreaTop, totalWidth, areaHeight);
            using (var font = new Font(WerTheme.FontFamily, 12f, FontStyle.Italic))
                TextRenderer.DrawText(g, "No Records Found", font, rect,
                    Color.FromArgb(180, 180, 180),
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
        }

        private void DrawRows(Graphics g, int totalWidth, int totalHeight)
        {
            if (ActiveTable == null || _columns.Count == 0) return;

            var widths = GetColumnWidths();
            int y = ContentAreaTop;
            int maxY = ShowFooter ? FooterTop : (totalHeight - CornerRadius);
            var view = GetSortedView();

            int startRow = PageStartRow + _scrollOffset;
            int endRow = Math.Min(PageEndRow, view.Length);

            for (int vi = startRow; vi < endRow && y + RowHeight <= maxY; vi++)
            {
                var row = view[vi];
                bool selected = vi == _selectedRowIndex;
                bool hovered = vi == _hoverRowIndex && !selected;

                // Selected row: rounded highlight
                if (selected)
                {
                    using (var path = RoundedRect(3, y + 1, totalWidth - 6, RowHeight - 2, SelectionRadius))
                    using (var brush = new SolidBrush(SelectedBg))
                        g.FillPath(brush, path);
                }
                else if (hovered)
                {
                    using (var brush = new SolidBrush(Color.FromArgb(248, 250, 252)))
                        g.FillRectangle(brush, 3, y, totalWidth - 6, RowHeight);
                }

                // Cell text
                int x = 1 - _hScrollOffset;
                for (int i = 0; i < _columns.Count; i++)
                {
                    int colW = widths[i];
                    var textRect = new Rectangle(x + CellPadding, y, colW - CellPadding * 2, RowHeight);

                    var val = row[_columns[i].PropertyName];
                    string text = FormatValue(val);
                    var colAlign = _columns[i].Alignment;

                    // Auto right-align numeric types
                    if (colAlign == HorizontalAlignment.Left &&
                        (val is decimal || val is double || val is float))
                        colAlign = HorizontalAlignment.Right;

                    var align = colAlign == HorizontalAlignment.Right
                        ? TextFormatFlags.Right
                        : colAlign == HorizontalAlignment.Center
                            ? TextFormatFlags.HorizontalCenter
                            : TextFormatFlags.Left;

                    TextRenderer.DrawText(g, text, _dataFont, textRect, DataText,
                        align | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);

                    x += colW;
                }

                // Edit button
                if (_showEditColumn && widths.Length > _columns.Count)
                {
                    int editColW = widths[_columns.Count];
                    int btnX = x + (editColW - EditBtnWidth) / 2;
                    int btnY = y + (RowHeight - EditBtnHeight) / 2;

                    bool editHover = vi == _hoverEditRow;

                    using (var path = RoundedRect(btnX, btnY, EditBtnWidth, EditBtnHeight, EditBtnRadius))
                    {
                        if (editHover)
                        {
                            using (var brush = new SolidBrush(EditBtnHover))
                                g.FillPath(brush, path);
                        }
                        using (var pen = new Pen(EditBtnBorder, 1.5f))
                            g.DrawPath(pen, path);
                    }

                    var btnRect = new Rectangle(btnX, btnY, EditBtnWidth, EditBtnHeight);
                    TextRenderer.DrawText(g, "Edit", _editFont, btnRect, EditBtnText,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
                }

                // Row separator (below each row, except last visible)
                int nextY = y + RowHeight;
                if (nextY < maxY && vi < view.Length - 1)
                {
                    using (var pen = new Pen(RowSep, 1f))
                        g.DrawLine(pen, CellPadding, nextY, totalWidth - CellPadding, nextY);
                }

                y += RowHeight;
            }
        }

        private void DrawFooter(Graphics g, int totalWidth, int totalHeight)
        {
            int footerY = FooterTop;

            // Separator line
            using (var pen = new Pen(HeaderSep, 1f))
                g.DrawLine(pen, 1, footerY, totalWidth - 2, footerY);

            // "1 – 25 of 229" on left
            int from = PageStartRow + 1;
            int to = PageEndRow;
            int total = RowCount;
            string pageText = from + " \u2013 " + to + " of " + total;

            var pageRect = new Rectangle(CellPadding, footerY, 200, FooterHeight);
            TextRenderer.DrawText(g, pageText, _footerFont, pageRect, DataText,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            // Nav buttons: |<  <  >  >|  — centered
            int btnW = 30;
            int btnH = 28;
            int btnGap = 6;
            int navTotalW = btnW * 4 + btnGap * 3;
            int navX = (totalWidth - navTotalW) / 2;
            int navY = footerY + (FooterHeight - btnH) / 2;

            for (int i = 0; i < 4; i++)
            {
                int bx = navX + i * (btnW + btnGap);
                var btnRect = new Rectangle(bx, navY, btnW, btnH);

                bool enabled = (i < 2) ? _currentPage > 0 : _currentPage < TotalPages - 1;
                bool hovered = _hoverNavBtn == i && enabled;

                using (var path = RoundedRect(bx, navY, btnW, btnH, 4))
                {
                    if (hovered)
                    {
                        using (var brush = new SolidBrush(Color.FromArgb(240, 242, 245)))
                            g.FillPath(brush, path);
                    }
                    using (var pen = new Pen(enabled ? ContainerBorder : Color.FromArgb(230, 230, 230), 1f))
                        g.DrawPath(pen, path);
                }

                var textColor = enabled ? DataText : Color.FromArgb(190, 190, 190);
                TextRenderer.DrawText(g, NavLabels[i], _footerFont, btnRect, textColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
            }

            // Total amount on right (if configured and numeric)
            var amountTable = ActiveTable;
            if (!string.IsNullOrEmpty(_totalAmountColumn) && amountTable != null && amountTable.Columns.Contains(_totalAmountColumn))
            {
                var colType = amountTable.Columns[_totalAmountColumn].DataType;
                colType = Nullable.GetUnderlyingType(colType) ?? colType;

                if (colType == typeof(decimal) || colType == typeof(double) || colType == typeof(float) ||
                    colType == typeof(int) || colType == typeof(long) || colType == typeof(short))
                {
                    decimal sum = 0;
                    foreach (DataRow dr in amountTable.Rows)
                    {
                        var v = dr[_totalAmountColumn];
                        if (v != null && v != DBNull.Value)
                            sum += Convert.ToDecimal(v);
                    }

                    string totalLabel = "Total Amount";
                    string totalValue = "$" + sum.ToString("#,##0.00");

                    var labelRect = new Rectangle(totalWidth - 220, footerY + 2, 200, FooterHeight / 2);
                    var valueRect = new Rectangle(totalWidth - 220, footerY + FooterHeight / 2 - 2, 200, FooterHeight / 2);

                    TextRenderer.DrawText(g, totalLabel, _footerFont, labelRect, DataText,
                        TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

                    using (var boldFont = new Font(WerTheme.FontFamily, 10f, FontStyle.Bold))
                        TextRenderer.DrawText(g, totalValue, boldFont, valueRect, DataText,
                            TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
                }
            }
        }

        private DataRowView[] GetSortedView()
        {
            var table = ActiveTable;
            if (table == null) return new DataRowView[0];

            var dv = table.DefaultView;
            if (_sortColumnIndex >= 0 && _sortColumnIndex < _columns.Count)
            {
                string col = _columns[_sortColumnIndex].PropertyName;
                dv.Sort = col + (_sortAscending ? " ASC" : " DESC");
            }
            else
            {
                dv.Sort = "";
            }

            var rows = new DataRowView[dv.Count];
            for (int i = 0; i < dv.Count; i++)
                rows[i] = dv[i];
            return rows;
        }

        // --- Mouse handling ---

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            // Active resize drag
            if (_resizingCol >= 0)
            {
                int delta = e.X - _resizeStartX;
                int newWidth = Math.Max(40, _resizeStartWidth + delta);
                if (_userColumnWidths == null)
                    _userColumnWidths = GetColumnWidthsSnapshot();
                _userColumnWidths[_resizingCol] = newWidth;
                InvalidateGrid();
                return;
            }

            int oldHoverRow = _hoverRowIndex;
            int oldHoverEdit = _hoverEditRow;
            int oldHoverHeader = _hoverHeaderCol;

            _hoverRowIndex = -1;
            _hoverEditRow = -1;
            _hoverHeaderCol = -1;

            // Tab hover (in search bar area, left side)
            if (e.Y < SearchBarHeight && _tabOptions != null && _tabOptions.Length > 0)
            {
                int oldHoverTab = _hoverTab;
                _hoverTab = GetTabAtX(e.X, e.Y);
                Cursor = _hoverTab >= 0 ? Cursors.Hand : Cursors.Default;
                if (oldHoverTab != _hoverTab) InvalidateGrid();
                return;
            }
            else
            {
                _hoverTab = -1;
            }

            if (e.Y >= SearchBarHeight && e.Y < SearchBarHeight + HeaderHeight)
            {
                // Check if near a column border for resize
                int resizeCol = GetResizeBorderAtX(e.X);
                if (resizeCol >= 0)
                {
                    Cursor = Cursors.SizeWE;
                }
                else if (_allowSorting)
                {
                    int col = GetColumnAtX(e.X);
                    if (col >= 0 && col < _columns.Count)
                        _hoverHeaderCol = col;
                    Cursor = _hoverHeaderCol >= 0 ? Cursors.Hand : Cursors.Default;
                }
            }
            else if (ShowFooter && e.Y >= FooterTop)
            {
                // Footer area — check nav buttons
                int oldNav = _hoverNavBtn;
                _hoverNavBtn = GetNavButtonAt(e.X, e.Y);
                Cursor = _hoverNavBtn >= 0 ? Cursors.Hand : Cursors.Default;
                if (oldNav != _hoverNavBtn) InvalidateGrid();
            }
            else
            {
                _hoverNavBtn = -1;
                int row = GetRowAtY(e.Y);
                if (row >= 0 && row < RowCount)
                {
                    _hoverRowIndex = row;

                    if (_showEditColumn && IsOverEditButton(e.X, e.Y, row))
                        _hoverEditRow = row;
                }
                Cursor = _hoverEditRow >= 0 ? Cursors.Hand : Cursors.Default;
            }

            if (oldHoverRow != _hoverRowIndex || oldHoverEdit != _hoverEditRow || oldHoverHeader != _hoverHeaderCol)
                InvalidateGrid();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            bool dirty = _hoverRowIndex != -1 || _hoverEditRow != -1 || _hoverHeaderCol != -1 || _hoverNavBtn != -1 || _hoverTab != -1;
            _hoverRowIndex = -1;
            _hoverEditRow = -1;
            _hoverHeaderCol = -1;
            _hoverNavBtn = -1;
            _hoverTab = -1;
            Cursor = Cursors.Default;
            if (dirty) InvalidateGrid();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (_resizingCol >= 0)
            {
                _resizingCol = -1;
                Cursor = Cursors.Default;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left) return;

            // Tab click?
            if (e.Y < SearchBarHeight && _tabOptions != null && _tabOptions.Length > 0)
            {
                int tab = GetTabAtX(e.X, e.Y);
                if (tab >= 0 && tab != _tabSelected)
                {
                    _tabSelected = tab;
                    _currentPage = 0;
                    _selectedRowIndex = 0;

                    // Clear search on tab change
                    if (_searchBox != null && !string.IsNullOrEmpty(_searchBox.Text))
                    {
                        _searchBox.Text = "";
                        _searchText = "";
                        _filteredTable = null;
                    }

                    InvalidateGrid();
                    TabChanged?.Invoke(this, EventArgs.Empty);
                }
                return;
            }

            // Footer nav click?
            if (ShowFooter && e.Y >= FooterTop)
            {
                int btn = GetNavButtonAt(e.X, e.Y);
                if (btn == 0 && _currentPage > 0) { _currentPage = 0; _selectedRowIndex = PageStartRow; InvalidateGrid(); }
                else if (btn == 1 && _currentPage > 0) { _currentPage--; _selectedRowIndex = PageStartRow; InvalidateGrid(); }
                else if (btn == 2 && _currentPage < TotalPages - 1) { _currentPage++; _selectedRowIndex = PageStartRow; InvalidateGrid(); }
                else if (btn == 3 && _currentPage < TotalPages - 1) { _currentPage = TotalPages - 1; _selectedRowIndex = PageStartRow; InvalidateGrid(); }
                return;
            }

            // Take focus so keyboard events come to us, not the scrollbar
            this.Focus();

            // Start column resize?
            if (e.Y >= SearchBarHeight && e.Y < SearchBarHeight + HeaderHeight)
            {
                int resizeCol = GetResizeBorderAtX(e.X);
                if (resizeCol >= 0)
                {
                    _resizingCol = resizeCol;
                    _resizeStartX = e.X;
                    var widths = GetColumnWidths();
                    _resizeStartWidth = widths[resizeCol];
                    return;
                }
            }

            // Header click → sort
            if (e.Y >= SearchBarHeight && e.Y < SearchBarHeight + HeaderHeight && _allowSorting)
            {
                int col = GetColumnAtX(e.X);
                if (col >= 0 && col < _columns.Count)
                {
                    if (_sortColumnIndex == col)
                        _sortAscending = !_sortAscending;
                    else
                    {
                        _sortColumnIndex = col;
                        _sortAscending = true;
                    }
                    InvalidateGrid();
                }
                return;
            }

            // Data row click
            int row = GetRowAtY(e.Y);
            if (row >= 0 && row < RowCount)
            {
                // Edit button?
                if (_showEditColumn && IsOverEditButton(e.X, e.Y, row))
                {
                    _selectedRowIndex = row;
                    InvalidateGrid();
                    var view = GetSortedView();
                    if (row < view.Length && _primaryKeyColumn != null)
                    {
                        var pk = view[row][_primaryKeyColumn];
                        EditClicked?.Invoke(this, new WerDataGridEditEventArgs(pk, row));
                    }
                    return;
                }

                if (_selectedRowIndex != row)
                {
                    _selectedRowIndex = row;
                    InvalidateGrid();
                    SelectedRowChanged?.Invoke(this, SelectedRowId);
                }
            }
        }


        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            PositionSearchBox();
            RecalcLayout();
            InvalidateGrid();
        }

        private void PositionSearchBox()
        {
            if (_searchBox == null) return;
            int scrollW = _vScroll != null && _vScroll.Visible ? _vScroll.Width : 0;
            int searchY = (SearchBarHeight - _searchBox.Height) / 2;
            _searchBox.Location = new Point(Width - scrollW - _searchBox.Width - 2, Math.Max(0, searchY));
            _searchBox.BringToFront();

            // Add button to the left of search bar
            if (_addButton != null)
            {
                _addButton.Visible = _showAddButton;
                if (_showAddButton)
                {
                    int btnY = (SearchBarHeight - _addButton.Height) / 2;
                    _addButton.Location = new Point(_searchBox.Left - _addButton.Width - 8, Math.Max(0, btnY));
                    _addButton.BringToFront();
                }
            }

            // Position vScroll below search bar
            if (_vScroll != null && _vScroll.Visible)
            {
                int hScrollH = (_hScroll != null && _hScroll.Visible) ? _hScroll.Height : 0;
                _vScroll.SetBounds(Width - _vScroll.Width, SearchBarHeight, _vScroll.Width, Height - SearchBarHeight - hScrollH);
                _vScroll.BringToFront();
            }
        }

        // --- Hit testing ---

        private int GetRowAtY(int y)
        {
            if (y <= ContentAreaTop) return -1;
            int relY = y - ContentAreaTop;
            return _scrollOffset + relY / RowHeight;
        }

        private int GetColumnAtX(int x)
        {
            var widths = GetColumnWidths();
            int cx = 1 - _hScrollOffset;
            for (int i = 0; i < _columns.Count; i++)
            {
                if (x >= cx && x < cx + widths[i])
                    return i;
                cx += widths[i];
            }
            return -1;
        }

        private bool IsOverEditButton(int mx, int my, int rowIndex)
        {
            var widths = GetColumnWidths();
            if (widths.Length <= _columns.Count) return false;

            int x = 1 - _hScrollOffset;
            for (int i = 0; i < _columns.Count; i++)
                x += widths[i];

            int editColW = widths[_columns.Count];
            int visualRow = rowIndex - PageStartRow - _scrollOffset;
            int rowY = ContentAreaTop + visualRow * RowHeight;

            int btnX = x + (editColW - EditBtnWidth) / 2;
            int btnY = rowY + (RowHeight - EditBtnHeight) / 2;

            return mx >= btnX && mx <= btnX + EditBtnWidth && my >= btnY && my <= btnY + EditBtnHeight;
        }

        /// <summary>Returns column index if x is near a column right edge (resize zone), or -1.</summary>
        private int GetResizeBorderAtX(int x)
        {
            const int hitZone = 5;
            var widths = GetColumnWidths();
            int cx = 1 - _hScrollOffset;
            for (int i = 0; i < _columns.Count; i++)
            {
                cx += widths[i];
                if (Math.Abs(x - cx) <= hitZone)
                    return i;
            }
            return -1;
        }

        /// <summary>Snapshot current computed widths (for starting a resize drag).</summary>
        private int[] GetColumnWidthsSnapshot()
        {
            var widths = GetColumnWidths();
            var snap = new int[_columns.Count];
            Array.Copy(widths, snap, _columns.Count);
            return snap;
        }

        private int GetTabAtX(int mx, int my)
        {
            if (_tabOptions == null || _tabOptions.Length == 0) return -1;
            int y = (SearchBarHeight - TabHeight) / 2;
            if (my < y || my > y + TabHeight) return -1;

            using (var bmp = new Bitmap(1, 1))
            using (var g = Graphics.FromImage(bmp))
            {
                int x = 4;
                for (int i = 0; i < _tabOptions.Length; i++)
                {
                    int textW = TextRenderer.MeasureText(g, _tabOptions[i], _dataFont).Width + TabPadH * 2;
                    if (mx >= x && mx < x + textW) return i;
                    x += textW + TabGap;
                }
            }
            return -1;
        }

        private int GetNavButtonAt(int mx, int my)
        {
            if (!ShowFooter || my < FooterTop) return -1;
            int scrollW = _vScroll.Visible ? _vScroll.Width : 0;
            int totalWidth = Width - scrollW;
            int btnW = 30;
            int btnH = 28;
            int btnGap = 6;
            int navTotalW = btnW * 4 + btnGap * 3;
            int navX = (totalWidth - navTotalW) / 2;
            int navY = FooterTop + (FooterHeight - btnH) / 2;

            if (my < navY || my > navY + btnH) return -1;

            for (int i = 0; i < 4; i++)
            {
                int bx = navX + i * (btnW + btnGap);
                if (mx >= bx && mx <= bx + btnW) return i;
            }
            return -1;
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

        private static void SetCueBanner(TextBox textBox, string text)
        {
            SendMessage(textBox.Handle, 0x1501, (IntPtr)1, text);
        }

        // --- GraphicsPath helpers ---

        private static GraphicsPath RoundedRect(int x, int y, int w, int h, int r)
        {
            var gp = new GraphicsPath();
            int d = r * 2;
            gp.AddArc(x, y, d, d, 180, 90);
            gp.AddArc(x + w - d, y, d, d, 270, 90);
            gp.AddArc(x + w - d, y + h - d, d, d, 0, 90);
            gp.AddArc(x, y + h - d, d, d, 90, 90);
            gp.CloseFigure();
            return gp;
        }

        private static GraphicsPath RoundedRectTop(int x, int y, int w, int h, int r)
        {
            var gp = new GraphicsPath();
            int d = r * 2;
            gp.AddArc(x, y, d, d, 180, 90);
            gp.AddArc(x + w - d, y, d, d, 270, 90);
            gp.AddLine(x + w, y + h, x, y + h);
            gp.CloseFigure();
            return gp;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _headerFont?.Dispose();
                _dataFont?.Dispose();
                _editFont?.Dispose();
                _footerFont?.Dispose();
                _addButtonFont?.Dispose();
                _searchBox?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
