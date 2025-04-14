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
    public class ShopViewModel : INotifyPropertyChanged
    {
        public Product? SelectedProduct { get; set; }
        public string? Query { get; set; }
        private ProductServiceProxy _productSvc = ProductServiceProxy.Current;
        private CartServiceProxy _cartSvc = CartServiceProxy.Current;

        private int _quantity = 1;
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = Math.Max(1, value);  // Ensure quantity is at least 1
                    NotifyPropertyChanged();
                }
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

        public void RefreshProductList()
        {
            NotifyPropertyChanged(nameof(Products));
        }

        public ObservableCollection<Product?> Products
        {
            get
            {
                var filteredList = _productSvc.Products
                    .Where(p => (p?.Name?.ToLower().Contains(Query?.ToLower() ?? string.Empty) ?? false)
                             && (p?.Quantity > 0));
                return new ObservableCollection<Product?>(filteredList);
            }
        }

        public bool AddToCart()
        {
            if (SelectedProduct == null || Quantity <= 0 || SelectedProduct.Quantity < Quantity)
                return false;

            _cartSvc.AddToCart(SelectedProduct, Quantity);
            Quantity = 1;  // Reset quantity
            NotifyPropertyChanged(nameof(Products));  // Refresh the list to show updated quantities
            return true;
        }
    }
}