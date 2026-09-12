using System;

namespace Wer.Winforms.Toolkit.Controls
{
    public class WerDataGridEditEventArgs : EventArgs
    {
        public object PrimaryKey { get; }
        public int RowIndex { get; }

        public WerDataGridEditEventArgs(object primaryKey, int rowIndex)
        {
            PrimaryKey = primaryKey;
            RowIndex = rowIndex;
        }
    }
}
