using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spring2025_Samples.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }

        public string? Display
        {
            get
            {
                return $"{Product?.Name} - Quantity: {Quantity}";
            }
        }

        public decimal ItemTotal
        {
            get
            {
                return (Product?.Price ?? 0) * Quantity;
            }
        }
    }
}