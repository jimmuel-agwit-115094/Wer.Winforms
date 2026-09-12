using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
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

        private VScrollBar _vScroll;
        private Font _headerFont;
        private Font _dataFont;
        private Font _editFont;

        public event EventHandler<WerDataGridEditEventArgs> EditClicked;

        [Category("Wer Data")]
        public string PrimaryKeyColumn
        {
            get => _primaryKeyColumn;
            set => _primaryKeyColumn = value;
        }

        [Category("Wer Data")]
        [DefaultValue(false)]
        public bool ShowEditColumn
        {
            get => _showEditColumn;
            set { _showEditColumn = value; RecalcLayout(); Invalidate(); }
        }

        [Category("Wer Data")]
        [DefaultValue(true)]
        public bool AllowSorting
        {
            get => _allowSorting;
            set => _allowSorting = value;
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
                RecalcLayout();
                Invalidate();
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

            _headerFont = new Font(WerTheme.FontFamily, 10f, FontStyle.Bold);
            _dataFont = new Font(WerTheme.FontFamily, 9.75f, FontStyle.Regular);
            _editFont = new Font(WerTheme.FontFamily, 8.5f, FontStyle.Regular);

            _vScroll = new VScrollBar();
            _vScroll.Dock = DockStyle.Right;
            _vScroll.Visible = false;
            _vScroll.ValueChanged += (s, e) => { _scrollOffset = _vScroll.Value; Invalidate(); };
            Controls.Add(_vScroll);

            BackColor = Color.White;
        }

        public void SetColumns(WerDataGridColumn[] columns)
        {
            _columns = new List<WerDataGridColumn>(columns);
            RecalcLayout();
            Invalidate();
        }

        private void AutoGenerateColumns()
        {
            _columns.Clear();
            if (_dataTable == null) return;
            foreach (DataColumn dc in _dataTable.Columns)
            {
                _columns.Add(new WerDataGridColumn(dc.ColumnName, dc.ColumnName));
            }
        }

        private int RowCount => _dataTable?.Rows.Count ?? 0;

        private int ContentAreaTop => CornerRadius + HeaderHeight;

        private int VisibleRowCount
        {
            get
            {
                int available = Height - ContentAreaTop - CornerRadius;
                return Math.Max(1, available / RowHeight);
            }
        }

        private void RecalcLayout()
        {
            int totalRows = RowCount;
            int visible = VisibleRowCount;
            if (totalRows > visible)
            {
                _vScroll.Visible = true;
                _vScroll.Minimum = 0;
                _vScroll.Maximum = totalRows - 1;
                _vScroll.LargeChange = Math.Max(1, visible);
                _vScroll.SmallChange = 1;
            }
            else
            {
                _vScroll.Visible = false;
                _scrollOffset = 0;
            }
        }

        private int[] GetColumnWidths()
        {
            if (_columns.Count == 0) return new int[0];

            int scrollW = _vScroll.Visible ? _vScroll.Width : 0;
            int totalW = Width - scrollW - 2; // 2 for border
            int editColW = _showEditColumn ? 90 : 0;
            int available = totalW - editColW;

            int fixedTotal = 0;
            int autoCount = 0;
            foreach (var c in _columns)
            {
                if (c.Width > 0) fixedTotal += c.Width;
                else autoCount++;
            }

            int autoWidth = autoCount > 0 ? Math.Max(60, (available - fixedTotal) / autoCount) : 0;
            int[] widths = new int[_columns.Count + (_showEditColumn ? 1 : 0)];
            for (int i = 0; i < _columns.Count; i++)
                widths[i] = _columns[i].Width > 0 ? _columns[i].Width : autoWidth;

            if (_showEditColumn)
                widths[_columns.Count] = editColW;

            return widths;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            int scrollW = _vScroll.Visible ? _vScroll.Width : 0;
            int w = Width - scrollW;
            int h = Height;

            // --- Outer rounded container ---
            using (var path = RoundedRect(0, 0, w, h, CornerRadius))
            {
                g.SetClip(path);

                // White background
                g.Clear(Color.White);

                // --- Header ---
                DrawHeader(g, w);

                // --- Data rows ---
                DrawRows(g, w, h);

                g.ResetClip();

                // Container border
                using (var pen = new Pen(ContainerBorder, 1f))
                    g.DrawPath(pen, path);
            }
        }

        private void DrawHeader(Graphics g, int totalWidth)
        {
            // Header background with rounded top corners
            using (var path = RoundedRectTop(1, 1, totalWidth - 2, HeaderHeight + CornerRadius, CornerRadius))
            using (var brush = new SolidBrush(HeaderBg))
            {
                g.FillPath(brush, path);
            }

            // Header bottom line
            using (var pen = new Pen(HeaderSep, 1f))
                g.DrawLine(pen, 1, HeaderHeight, totalWidth - 2, HeaderHeight);

            var widths = GetColumnWidths();
            int x = 1;

            for (int i = 0; i < _columns.Count; i++)
            {
                int colW = widths[i];
                var textRect = new Rectangle(x + CellPadding, 1, colW - CellPadding * 2, HeaderHeight);

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
                        g.DrawLine(pen, x + colW, 10, x + colW, HeaderHeight - 10);
                }

                x += colW;
            }

            // "Actions" header if edit column
            if (_showEditColumn && widths.Length > _columns.Count)
            {
                int editW = widths[_columns.Count];
                var textRect = new Rectangle(x + CellPadding, 1, editW - CellPadding * 2, HeaderHeight);
                TextRenderer.DrawText(g, "Actions", _headerFont, textRect, HeaderText,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
            }
        }

        private void DrawRows(Graphics g, int totalWidth, int totalHeight)
        {
            if (_dataTable == null || _columns.Count == 0) return;

            var widths = GetColumnWidths();
            int y = ContentAreaTop;
            int maxY = totalHeight - CornerRadius;
            var view = GetSortedView();

            for (int vi = _scrollOffset; vi < view.Length && y + RowHeight <= maxY; vi++)
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
                int x = 1;
                for (int i = 0; i < _columns.Count; i++)
                {
                    int colW = widths[i];
                    var textRect = new Rectangle(x + CellPadding, y, colW - CellPadding * 2, RowHeight);

                    var val = row[_columns[i].PropertyName];
                    string text;
                    var colAlign = _columns[i].Alignment;

                    if (val == null || val == DBNull.Value)
                    {
                        text = "";
                    }
                    else if (val is decimal decVal)
                    {
                        text = decVal.ToString("#,##0.00");
                        if (colAlign == HorizontalAlignment.Left) colAlign = HorizontalAlignment.Right;
                    }
                    else if (val is double dblVal)
                    {
                        text = dblVal.ToString("#,##0.00");
                        if (colAlign == HorizontalAlignment.Left) colAlign = HorizontalAlignment.Right;
                    }
                    else if (val is float fltVal)
                    {
                        text = fltVal.ToString("#,##0.00");
                        if (colAlign == HorizontalAlignment.Left) colAlign = HorizontalAlignment.Right;
                    }
                    else if (val is DateTime dtVal)
                    {
                        text = dtVal.ToString("MMM. dd, yyyy");
                    }
                    else
                    {
                        text = val.ToString();
                    }

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

        private DataRowView[] GetSortedView()
        {
            if (_dataTable == null) return new DataRowView[0];

            var dv = _dataTable.DefaultView;
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
            int oldHoverRow = _hoverRowIndex;
            int oldHoverEdit = _hoverEditRow;
            int oldHoverHeader = _hoverHeaderCol;

            _hoverRowIndex = -1;
            _hoverEditRow = -1;
            _hoverHeaderCol = -1;

            if (e.Y < HeaderHeight)
            {
                // Header area — detect column for sort cursor
                if (_allowSorting)
                {
                    int col = GetColumnAtX(e.X);
                    if (col >= 0 && col < _columns.Count)
                        _hoverHeaderCol = col;
                }
            }
            else
            {
                int row = GetRowAtY(e.Y);
                if (row >= 0 && row < RowCount)
                {
                    _hoverRowIndex = row;

                    if (_showEditColumn && IsOverEditButton(e.X, e.Y, row))
                        _hoverEditRow = row;
                }
            }

            Cursor = (_hoverEditRow >= 0 || _hoverHeaderCol >= 0) ? Cursors.Hand : Cursors.Default;

            if (oldHoverRow != _hoverRowIndex || oldHoverEdit != _hoverEditRow || oldHoverHeader != _hoverHeaderCol)
                Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (_hoverRowIndex != -1 || _hoverEditRow != -1 || _hoverHeaderCol != -1)
            {
                _hoverRowIndex = -1;
                _hoverEditRow = -1;
                _hoverHeaderCol = -1;
                Cursor = Cursors.Default;
                Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left) return;

            // Header click → sort
            if (e.Y < HeaderHeight && _allowSorting)
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
                    Invalidate();
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
                    Invalidate();
                }
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (!_vScroll.Visible) return;

            int delta = e.Delta > 0 ? -3 : 3;
            int newVal = Math.Max(_vScroll.Minimum, Math.Min(_vScroll.Maximum - _vScroll.LargeChange + 1, _vScroll.Value + delta));
            _vScroll.Value = newVal;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            RecalcLayout();
            Invalidate();
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
            int cx = 1;
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

            int x = 1;
            for (int i = 0; i < _columns.Count; i++)
                x += widths[i];

            int editColW = widths[_columns.Count];
            int visualRow = rowIndex - _scrollOffset;
            int rowY = ContentAreaTop + visualRow * RowHeight;

            int btnX = x + (editColW - EditBtnWidth) / 2;
            int btnY = rowY + (RowHeight - EditBtnHeight) / 2;

            return mx >= btnX && mx <= btnX + EditBtnWidth && my >= btnY && my <= btnY + EditBtnHeight;
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
            }
            base.Dispose(disposing);
        }
    }
}
