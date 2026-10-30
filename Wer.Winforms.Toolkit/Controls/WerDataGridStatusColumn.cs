using System;
using System.Collections.Generic;

namespace Wer.Winforms.Toolkit.Controls
{
    /// <summary>
    /// Visual style of a status badge in a WerDataGrid status column.
    /// </summary>
    public enum WerStatusStyle
    {
        /// <summary>Tinted background + dot + colored text (default).</summary>
        Pill,
        /// <summary>White background, colored border and text, no dot.</summary>
        Outline,
        /// <summary>Filled pill in the status color with white text.</summary>
        Solid,
        /// <summary>Colored dot + normal text, no background.</summary>
        Dot
    }

    /// <summary>
    /// Status column definition: which column, how values map to WerStatus, and the badge style.
    /// </summary>
    public class WerDataGridStatusColumn
    {
        public WerDataGridStatusColumn(string columnName, WerStatusStyle style, IDictionary<string, WerStatus> map)
        {
            ColumnName = columnName;
            Style = style;
            Map = map == null
                ? new Dictionary<string, WerStatus>(StringComparer.OrdinalIgnoreCase)
                : new Dictionary<string, WerStatus>(map, StringComparer.OrdinalIgnoreCase);
        }

        public string ColumnName { get; }
        public WerStatusStyle Style { get; }
        public Dictionary<string, WerStatus> Map { get; }
    }
}
