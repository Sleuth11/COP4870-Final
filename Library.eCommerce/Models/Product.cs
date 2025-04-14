using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spring2025_Samples.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string? Display
        {
            get
            {
                return $"{Id}. {Name} - ${Price:F2} ({Quantity} in stock)";
            }
        }

        public Product()
        {
            Name = string.Empty;
            Price = 0.00m;
            Quantity = 0;
        }

        public override string ToString()
        {
            return Display ?? string.Empty;
        }
    }
}