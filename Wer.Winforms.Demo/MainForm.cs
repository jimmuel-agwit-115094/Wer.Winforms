using System.Windows.Forms;

namespace Wer.Winforms.Demo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            // Populate combobox
            werComboBox1.Items.AddRange(new[]
            {
                "Combobox item",
                "Combobox item",
                "Combobox item",
                "Combobox item"
            });
        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
        }

        private void werTabControl1_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }

        private void werButtonPrimary1_Click(object sender, System.EventArgs e)
        {
        }

        private void werTabControl1_SelectedIndexChanged_1(object sender, System.EventArgs e)
        {

        }
    }
}
