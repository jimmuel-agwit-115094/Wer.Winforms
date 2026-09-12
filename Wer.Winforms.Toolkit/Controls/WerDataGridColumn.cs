using System.Windows.Forms;

namespace Wer.Winforms.Toolkit.Controls
{
    public class WerDataGridColumn
    {
        public string PropertyName { get; set; }
        public string HeaderText { get; set; }
        public int Width { get; set; }
        public HorizontalAlignment Alignment { get; set; }

        public WerDataGridColumn() { }

        public WerDataGridColumn(string propertyName, string headerText, int width = 0, HorizontalAlignment alignment = HorizontalAlignment.Left)
        {
            PropertyName = propertyName;
            HeaderText = headerText;
            Width = width;
            Alignment = alignment;
        }
    }
}
