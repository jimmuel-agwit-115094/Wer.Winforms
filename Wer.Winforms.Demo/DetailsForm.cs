using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Wer.Winforms.Toolkit.Controls;

namespace Wer.Winforms.Demo
{
    public partial class DetailsForm : WerForm
    {
        public DetailsForm()
        {
            InitializeComponent();
        }

        private void DetailsForm_Load(object sender, EventArgs e)
        {
            werDataGrid1.FieldOptions = new[] { "Name", "Category", "Qty", "Price", "Total", "OrderDate", "Status" };
            werDataGrid1.PrimaryKeyColumn = "Id";
            werDataGrid1.TotalAmountColumn = "Total";

            werDataGrid1.DataSource = new List<Product>
            {
                new Product { Id = 1,  Name = "Wireless Mouse",       Category = "Electronics", Qty = 25,  Price = 29.99m,   Total = 749.75m,   OrderDate = new DateTime(2026, 9, 1),  Status = "Delivered" },
                new Product { Id = 2,  Name = "USB-C Hub",            Category = "Electronics", Qty = 15,  Price = 49.95m,   Total = 749.25m,   OrderDate = new DateTime(2026, 9, 3),  Status = "Delivered" },
                new Product { Id = 3,  Name = "Standing Desk",        Category = "Furniture",   Qty = 5,   Price = 399.00m,  Total = 1995.00m,  OrderDate = new DateTime(2026, 9, 5),  Status = "Shipped" },
                new Product { Id = 4,  Name = "Mechanical Keyboard",  Category = "Electronics", Qty = 30,  Price = 89.99m,   Total = 2699.70m,  OrderDate = new DateTime(2026, 9, 7),  Status = "Delivered" },
                new Product { Id = 5,  Name = "Monitor Arm",          Category = "Accessories", Qty = 12,  Price = 65.00m,   Total = 780.00m,   OrderDate = new DateTime(2026, 9, 8),  Status = "Processing" },
                new Product { Id = 6,  Name = "Noise Cancelling Headphones", Category = "Audio", Qty = 8,  Price = 249.99m,  Total = 1999.92m,  OrderDate = new DateTime(2026, 9, 10), Status = "Delivered" },
                new Product { Id = 7,  Name = "Webcam 4K",            Category = "Electronics", Qty = 20,  Price = 129.00m,  Total = 2580.00m,  OrderDate = new DateTime(2026, 9, 11), Status = "Shipped" },
                new Product { Id = 8,  Name = "Desk Lamp LED",        Category = "Lighting",    Qty = 40,  Price = 34.50m,   Total = 1380.00m,  OrderDate = new DateTime(2026, 9, 12), Status = "Delivered" },
                new Product { Id = 9,  Name = "Laptop Stand",         Category = "Accessories", Qty = 18,  Price = 45.00m,   Total = 810.00m,   OrderDate = new DateTime(2026, 9, 14), Status = "Processing" },
                new Product { Id = 10, Name = "Cable Management Kit", Category = "Accessories", Qty = 50,  Price = 19.99m,   Total = 999.50m,   OrderDate = new DateTime(2026, 9, 15), Status = "Delivered" },
                new Product { Id = 11, Name = "Ergonomic Chair",      Category = "Furniture",   Qty = 3,   Price = 599.00m,  Total = 1797.00m,  OrderDate = new DateTime(2026, 9, 16), Status = "Shipped" },
                new Product { Id = 12, Name = "Portable SSD 1TB",     Category = "Storage",     Qty = 22,  Price = 109.99m,  Total = 2419.78m,  OrderDate = new DateTime(2026, 9, 17), Status = "Delivered" },
                new Product { Id = 13, Name = "Wireless Charger",     Category = "Electronics", Qty = 35,  Price = 24.99m,   Total = 874.65m,   OrderDate = new DateTime(2026, 9, 18), Status = "Delivered" },
                new Product { Id = 14, Name = "Whiteboard 48x36",     Category = "Office",      Qty = 6,   Price = 85.00m,   Total = 510.00m,   OrderDate = new DateTime(2026, 9, 19), Status = "Processing" },
                new Product { Id = 15, Name = "Surge Protector",      Category = "Electronics", Qty = 45,  Price = 32.00m,   Total = 1440.00m,  OrderDate = new DateTime(2026, 9, 20), Status = "Delivered" },
                new Product { Id = 16, Name = "Desk Organizer",       Category = "Office",      Qty = 28,  Price = 22.50m,   Total = 630.00m,   OrderDate = new DateTime(2026, 9, 21), Status = "Shipped" },
                new Product { Id = 17, Name = "Blue Light Glasses",   Category = "Accessories", Qty = 60,  Price = 15.99m,   Total = 959.40m,   OrderDate = new DateTime(2026, 9, 22), Status = "Delivered" },
                new Product { Id = 18, Name = "Docking Station",      Category = "Electronics", Qty = 10,  Price = 179.00m,  Total = 1790.00m,  OrderDate = new DateTime(2026, 9, 23), Status = "Processing" },
                new Product { Id = 19, Name = "Mouse Pad XL",         Category = "Accessories", Qty = 55,  Price = 18.00m,   Total = 990.00m,   OrderDate = new DateTime(2026, 9, 24), Status = "Delivered" },
                new Product { Id = 20, Name = "Bluetooth Speaker",    Category = "Audio",       Qty = 14,  Price = 59.99m,   Total = 839.86m,   OrderDate = new DateTime(2026, 9, 25), Status = "Shipped" },
                new Product { Id = 21, Name = "HDMI Cable 6ft",       Category = "Cables",      Qty = 100, Price = 8.99m,    Total = 899.00m,   OrderDate = new DateTime(2026, 9, 26), Status = "Delivered" },
                new Product { Id = 22, Name = "Webcam Ring Light",    Category = "Lighting",    Qty = 16,  Price = 28.00m,   Total = 448.00m,   OrderDate = new DateTime(2026, 9, 27), Status = "Delivered" },
                new Product { Id = 23, Name = "USB Flash Drive 64GB", Category = "Storage",     Qty = 75,  Price = 12.50m,   Total = 937.50m,   OrderDate = new DateTime(2026, 9, 28), Status = "Processing" },
                new Product { Id = 24, Name = "Privacy Screen Filter",Category = "Accessories", Qty = 9,   Price = 42.00m,   Total = 378.00m,   OrderDate = new DateTime(2026, 9, 29), Status = "Shipped" },
                new Product { Id = 25, Name = "Ethernet Cable 25ft",  Category = "Cables",      Qty = 80,  Price = 14.99m,   Total = 1199.20m,  OrderDate = new DateTime(2026, 9, 30), Status = "Delivered" },
                new Product { Id = 26, Name = "Portable Monitor 15in",Category = "Electronics", Qty = 7,   Price = 219.00m,  Total = 1533.00m,  OrderDate = new DateTime(2026, 10, 1), Status = "Processing" },
                new Product { Id = 27, Name = "Wrist Rest Gel",       Category = "Accessories", Qty = 38,  Price = 16.50m,   Total = 627.00m,   OrderDate = new DateTime(2026, 10, 1), Status = "Delivered" },
            };
        }
    }
}
