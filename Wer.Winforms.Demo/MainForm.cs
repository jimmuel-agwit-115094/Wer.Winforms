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

            werLeftNavMenu1.ContentPanel = panel1;
            werLeftNavMenu1.LogoText = "MyApp";
            werTopNav1.UserName = "Agwit Jim";
            werTopNav1.LogoutClicked += (s, e) => Application.Exit();
            SetupDataGrid();
        }

        private void SetupDataGrid()
        {
//            // Simulated DB result — just set DataSource, columns auto-generate from properties
//            var products = new List<Product>
//            {
//             new Product { Id = 1, Name = "MacBook Pro 16\"", Category = "Electronics", Qty = 2, Price = 125999.50m, Total = 251999.00m, OrderDate = new DateTime(2025, 9, 1), Status = "Delivered", Supplier = "Apple Inc.", Warehouse = "Manila", PaymentMethod = "Bank Transfer", SalesRep = "Juan Dela Cruz", Discount = 5000.00m, Tax = 30239.94m, Priority = "High" },
//new Product { Id = 2, Name = "Office Chair", Category = "Furniture", Qty = 10, Price = 8450.00m, Total = 84500.00m, OrderDate = new DateTime(2025, 8, 15), Status = "Shipped", Supplier = "ErgoWorks", Warehouse = "Cebu", PaymentMethod = "Credit Card", SalesRep = "Maria Santos", Discount = 2500.00m, Tax = 10140.00m, Priority = "Normal" },
//new Product { Id = 3, Name = "Wireless Mouse", Category = "Accessories", Qty = 50, Price = 350.75m, Total = 17537.50m, OrderDate = new DateTime(2025, 7, 22), Status = "Delivered", Supplier = "Logitech", Warehouse = "Manila", PaymentMethod = "Cash", SalesRep = "Pedro Reyes", Discount = 500.00m, Tax = 2104.50m, Priority = "Low" },
//new Product { Id = 4, Name = "Standing Desk", Category = "Furniture", Qty = 3, Price = 24000.00m, Total = 72000.00m, OrderDate = new DateTime(2025, 9, 10), Status = "Pending", Supplier = "FlexiSpot", Warehouse = "Davao", PaymentMethod = "Bank Transfer", SalesRep = "Ana Garcia", Discount = 3000.00m, Tax = 8280.00m, Priority = "High" },
//new Product { Id = 5, Name = "USB-C Hub", Category = "Accessories", Qty = 25, Price = 1200.00m, Total = 30000.00m, OrderDate = new DateTime(2025, 6, 5), Status = "Delivered", Supplier = "Anker", Warehouse = "Manila", PaymentMethod = "GCash", SalesRep = "Carlos Mendoza", Discount = 750.00m, Tax = 3510.00m, Priority = "Normal" },
//new Product { Id = 6, Name = "Monitor 27\" 4K", Category = "Electronics", Qty = 5, Price = 18500.00m, Total = 92500.00m, OrderDate = new DateTime(2025, 9, 12), Status = "Processing", Supplier = "Dell", Warehouse = "Cebu", PaymentMethod = "Credit Card", SalesRep = "Sofia Cruz", Discount = 4500.00m, Tax = 10560.00m, Priority = "High" },
//new Product { Id = 7, Name = "Keyboard Mech.", Category = "Accessories", Qty = 15, Price = 4250.00m, Total = 63750.00m, OrderDate = new DateTime(2025, 5, 30), Status = "Delivered", Supplier = "Keychron", Warehouse = "Manila", PaymentMethod = "Bank Transfer", SalesRep = "Miguel Torres", Discount = 1500.00m, Tax = 7650.00m, Priority = "Normal" },
//new Product { Id = 8, Name = "Webcam HD", Category = "Electronics", Qty = 8, Price = 2100.50m, Total = 16804.00m, OrderDate = new DateTime(2025, 8, 1), Status = "Shipped", Supplier = "Logitech", Warehouse = "Davao", PaymentMethod = "GCash", SalesRep = "Laura Ramos", Discount = 300.00m, Tax = 1951.68m, Priority = "Low" },
//new Product { Id = 9, Name = "Laser Printer", Category = "Electronics", Qty = 4, Price = 28900.00m, Total = 115600.00m, OrderDate = new DateTime(2025, 8, 25), Status = "Delivered", Supplier = "HP", Warehouse = "Manila", PaymentMethod = "Bank Transfer", SalesRep = "Daniel Flores", Discount = 6000.00m, Tax = 13152.00m, Priority = "High" },
//new Product { Id = 10, Name = "Filing Cabinet", Category = "Furniture", Qty = 6, Price = 6500.00m, Total = 39000.00m, OrderDate = new DateTime(2025, 7, 18), Status = "Processing", Supplier = "OfficeMate", Warehouse = "Cebu", PaymentMethod = "Credit Card", SalesRep = "Rachel Tan", Discount = 1200.00m, Tax = 4536.00m, Priority = "Normal" },
//new Product { Id = 11, Name = "External SSD 2TB", Category = "Storage", Qty = 12, Price = 7200.00m, Total = 86400.00m, OrderDate = new DateTime(2025, 9, 5), Status = "Pending", Supplier = "Samsung", Warehouse = "Davao", PaymentMethod = "Bank Transfer", SalesRep = "Kevin Lim", Discount = 2200.00m, Tax = 10104.00m, Priority = "High" },
//new Product { Id = 12, Name = "Network Switch", Category = "Networking", Qty = 7, Price = 9800.00m, Total = 68600.00m, OrderDate = new DateTime(2025, 6, 28), Status = "Delivered", Supplier = "TP-Link", Warehouse = "Manila", PaymentMethod = "Cash", SalesRep = "John Bautista", Discount = 1750.00m, Tax = 8022.00m, Priority = "Normal" },
//new Product { Id = 13, Name = "Office Laptop", Category = "Electronics", Qty = 6, Price = 54999.00m, Total = 329994.00m, OrderDate = new DateTime(2025, 9, 8), Status = "Shipped", Supplier = "Lenovo", Warehouse = "Cebu", PaymentMethod = "Bank Transfer", SalesRep = "Angela Villanueva", Discount = 10000.00m, Tax = 38399.28m, Priority = "High" },

//            };

//            werDataGrid1.PrimaryKeyColumn = "Id";
//            werDataGrid1.ShowEditColumn = true;
//            werDataGrid1.FieldOptions = new[] { "Name", "Category", "Qty", "Price", "Total", "OrderDate", "Status" };
//            werDataGrid1.TotalAmountColumn = "Total";
//            werDataGrid1.PageSize = 5;
//            werDataGrid1.DataSource = products;
//            werDataGrid1.ShowHorizontalScroll = true;
//            werDataGrid1.EditClicked += (s, args) =>
//                MessageBox.Show("Edit: " + args.PrimaryKey, "Edit Clicked");

//            tabPage6.Controls.Add(werDataGrid1);
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

        private void werDataGrid1_Load(object sender, EventArgs e)
        {

        }

        private void werButtonSuccess2_Click(object sender, EventArgs e)
        {
           
        }

        private void werButtonSuccess3_Click(object sender, EventArgs e)
        {
         
        }

        private void werButtonSuccess4_Click(object sender, EventArgs e)
        {
          
        }

        private void werButtonSuccess5_Click(object sender, EventArgs e)
        {
           
        }

        private void werButtonSuccess6_Click(object sender, EventArgs e)
        {
        }

        private void werButtonSuccess7_Click(object sender, EventArgs e)
        {
            
        }

        private void werToggle1_CheckedChanged(object sender, EventArgs e)
        {

         

        }

        private void werDateRange1_RangeChanged(object sender, EventArgs e)
        {
          
        }

        private void werButtonPrimary2_Click(object sender, EventArgs e)
        {
        }

        private void werButtonOrange1_Click(object sender, EventArgs e)
        {
            WerMessageBox.Error.Show("Something went wrong.");
        }

        private void werButtonPrimary1_Click_1(object sender, EventArgs e)
        {
            WerMessageBox.Error.Show("Something went wrong.", "Oops");
        }

        private void werButtonSuccess1_Click(object sender, EventArgs e)
        {
            WerMessageBox.Warning.Show("Continue?", "Confirm", WerMessageButtons.OKCancel);
        }

        private void werButtonWarning1_Click(object sender, EventArgs e)
        {
            WerMessageBox.Success.Show("Record saved successfully.");
        }

        private void werMenuButton1_Click(object sender, EventArgs e)
        {
        }

        private void werMenuButton3_Click(object sender, EventArgs e)
        {
        }

        private void werMenuButton2_Click(object sender, EventArgs e)
        {
        }
    }
}
