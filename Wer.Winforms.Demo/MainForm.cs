using System.Data;
using System.Windows.Forms;
using Wer.Winforms.Toolkit.Controls;

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

            SetupDataGrid();
        }

        private void SetupDataGrid()
        {
            var grid = new WerDataGrid();
            grid.Dock = DockStyle.Fill;

            grid.SetColumns(new[]
            {
                new WerDataGridColumn("Title", "Title"),
                new WerDataGridColumn("Available", "Available", 90, HorizontalAlignment.Center),
                new WerDataGridColumn("Format", "Format"),
                new WerDataGridColumn("Label", "Label"),
                new WerDataGridColumn("Country", "Country", 100),
                new WerDataGridColumn("CatNo", "Cat#", 100),
                new WerDataGridColumn("Year", "Year", 70, HorizontalAlignment.Center),
            });

            var dt = new DataTable();
            dt.Columns.Add("Title");
            dt.Columns.Add("Available", typeof(int));
            dt.Columns.Add("Format");
            dt.Columns.Add("Label");
            dt.Columns.Add("Country");
            dt.Columns.Add("CatNo");
            dt.Columns.Add("Year");

            dt.Rows.Add("Dreams of You", 5, "LP, Album, Reissue", "Amberline Records", "US", "AMB-2201", "2023");
            dt.Rows.Add("Dreams of You", 3, "CD, Album, Remastered", "Amberline Records", "US", "AMB-2202", "2023");
            dt.Rows.Add("Electric Sunrise", 12, "LP, Album", "Neon Wave Music", "UK", "NWM-1105", "2022");
            dt.Rows.Add("Midnight Harbor", 8, "CD, Album", "Coastal Sounds", "JP", "CS-0441", "2024");
            dt.Rows.Add("Silver Lining", 2, "LP, Limited Edition", "Horizon Press", "DE", "HP-3302", "2021");
            dt.Rows.Add("Velocity", 15, "CD, Album, Deluxe", "Apex Audio", "US", "APX-7710", "2023");
            dt.Rows.Add("Wanderlust", 6, "LP, Album", "Drift Records", "AU", "DFT-0089", "2022");
            dt.Rows.Add("Neon Bloom", 4, "Vinyl, LP, Album", "Synth City Label", "CA", "SCL-5501", "2024");

            grid.PrimaryKeyColumn = "Title";
            grid.ShowEditColumn = true;
            grid.DataSource = dt;

            grid.EditClicked += (s, args) =>
                MessageBox.Show("Edit: " + args.PrimaryKey, "Edit Clicked");

            tabPage6.Controls.Add(grid);
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
