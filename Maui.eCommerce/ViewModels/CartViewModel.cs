using Library.eCommerce.Services;
using Spring2025_Samples.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Maui.eCommerce.ViewModels
{
    public class CartViewModel : INotifyPropertyChanged
    {
        private CartServiceProxy _cartSvc = CartServiceProxy.Current;

        public CartItem? SelectedCartItem { get; set; }

        private int _updateQuantity = 1;
        public int UpdateQuantity
        {
            get => _updateQuantity;
            set
            {
                if (_updateQuantity != value)
                {
                    _updateQuantity = Math.Max(0, value);  // Allow 0 to remove item
                    NotifyPropertyChanged();
                }
            }
        }

        public ObservableCollection<CartItem> CartItems
        {
            get
            {
                return new ObservableCollection<CartItem>(_cartSvc.CartItems);
            }
        }

        public string SubtotalDisplay => $"Subtotal: ${_cartSvc.GetSubtotal():F2}";
        public string TaxDisplay => $"Tax (7%): ${_cartSvc.GetTax():F2}";
        public string TotalDisplay => $"Total: ${_cartSvc.GetTotal():F2}";

        public string CheckoutSummary
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

        public void RefreshCart()
        {
            NotifyPropertyChanged(nameof(CartItems));
            NotifyPropertyChanged(nameof(SubtotalDisplay));
            NotifyPropertyChanged(nameof(TaxDisplay));
            NotifyPropertyChanged(nameof(TotalDisplay));
        }

        public bool UpdateItemQuantity()
        {
            if (SelectedCartItem == null)
                return false;

            bool result = _cartSvc.UpdateCartItemQuantity(SelectedCartItem.Id, UpdateQuantity);
            RefreshCart();
            UpdateQuantity = 1;  // Reset quantity input
            return result;
        }

        public bool RemoveItem()
        {
            if (SelectedCartItem == null)
                return false;

            bool result = _cartSvc.RemoveFromCart(SelectedCartItem.Id);
            RefreshCart();
            return result;
        }

        public void Checkout()
        {
            // We'll just clear the cart after checkout
            _cartSvc.ClearCart();
            RefreshCart();
        }
    }
}