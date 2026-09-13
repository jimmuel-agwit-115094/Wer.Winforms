using System;

namespace Wer.Winforms.Demo
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public string Supplier { get; set; }
        public string Warehouse { get; set; }
        public string PaymentMethod { get; set; }
        public string SalesRep { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public string Priority { get; set; }
    }
}