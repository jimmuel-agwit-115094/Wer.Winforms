using System;
using System.Collections.Generic;
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

            // Only show these 5 columns — Id, Supplier, Warehouse are on the object but hidden
            grid.SetColumns(new[]
            {
                new WerDataGridColumn("Name", "Product"),
                new WerDataGridColumn("Qty", "Qty", 70, HorizontalAlignment.Center),
                new WerDataGridColumn("Price", "Price", 110),
                new WerDataGridColumn("Total", "Total", 120),
                new WerDataGridColumn("OrderDate", "Order Date", 130),
            });

            var products = new List<Product>
            {
                new Product { Id = 1, Name = "MacBook Pro 16\"", Category = "Electronics", Qty = 2, Price = 125999.50m, Total = 251999.00m, OrderDate = new DateTime(2025, 9, 1), Status = "Delivered", Supplier = "Apple Inc.", Warehouse = "Manila" },
                new Product { Id = 2, Name = "Office Chair", Category = "Furniture", Qty = 10, Price = 8450.00m, Total = 84500.00m, OrderDate = new DateTime(2025, 8, 15), Status = "Shipped", Supplier = "ErgoWorks", Warehouse = "Cebu" },
                new Product { Id = 3, Name = "Wireless Mouse", Category = "Accessories", Qty = 50, Price = 350.75m, Total = 17537.50m, OrderDate = new DateTime(2025, 7, 22), Status = "Delivered", Supplier = "Logitech", Warehouse = "Manila" },
                new Product { Id = 4, Name = "Standing Desk", Category = "Furniture", Qty = 3, Price = 24000.00m, Total = 72000.00m, OrderDate = new DateTime(2025, 9, 10), Status = "Pending", Supplier = "FlexiSpot", Warehouse = "Davao" },
                new Product { Id = 5, Name = "USB-C Hub", Category = "Accessories", Qty = 25, Price = 1200.00m, Total = 30000.00m, OrderDate = new DateTime(2025, 6, 5), Status = "Delivered", Supplier = "Anker", Warehouse = "Manila" },
                new Product { Id = 6, Name = "Monitor 27\" 4K", Category = "Electronics", Qty = 5, Price = 18500.00m, Total = 92500.00m, OrderDate = new DateTime(2025, 9, 12), Status = "Processing", Supplier = "Dell", Warehouse = "Cebu" },
                new Product { Id = 7, Name = "Keyboard Mech.", Category = "Accessories", Qty = 15, Price = 4250.00m, Total = 63750.00m, OrderDate = new DateTime(2025, 5, 30), Status = "Delivered", Supplier = "Keychron", Warehouse = "Manila" },
                new Product { Id = 8, Name = "Webcam HD", Category = "Electronics", Qty = 8, Price = 2100.50m, Total = 16804.00m, OrderDate = new DateTime(2025, 8, 1), Status = "Shipped", Supplier = "Logitech", Warehouse = "Davao" },
            };

            grid.PrimaryKeyColumn = "Id";
            grid.ShowEditColumn = true;
            grid.DataSource = products;

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
