using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFinalProject.Models
{
    internal class SellHistory
    {
        public required string Username { get; set; }
        public DateTime PurchaseTime { get; set; }
        public List<Product>? ProductList { get; set; }
        public double TotalAmount { get; set; }
    }
}
