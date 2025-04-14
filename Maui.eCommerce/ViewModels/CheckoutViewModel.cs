using Library.eCommerce.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Maui.eCommerce.ViewModels
{
    public class CheckoutViewModel : INotifyPropertyChanged
    {
        private CartServiceProxy _cartSvc = CartServiceProxy.Current;

        public string ReceiptText
        {
            get
            {
                var sb = new StringBuilder();
                sb.AppendLine("===== RECEIPT =====");
                sb.AppendLine($"Date: {DateTime.Now}");
                sb.AppendLine();

                foreach (var item in _cartSvc.CartItems)
                {
                    sb.AppendLine($"{item.Product?.Name} x{item.Quantity}");
                    sb.AppendLine($"  ${item.Product?.Price:F2} each = ${item.ItemTotal:F2}");
                }

                sb.AppendLine();
                sb.AppendLine($"Subtotal: ${_cartSvc.GetSubtotal():F2}");
                sb.AppendLine($"Tax (7%): ${_cartSvc.GetTax():F2}");
                sb.AppendLine($"TOTAL: ${_cartSvc.GetTotal():F2}");
                sb.AppendLine();
                sb.AppendLine("Thank you for shopping with us!");
                sb.AppendLine("====================");

                return sb.ToString();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void CompleteCheckout()
        {
            _cartSvc.ClearCart();
        }
    }
}