using Library.eCommerce.Services;
using Spring2025_Samples.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Maui.eCommerce.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        private Product? _model;

        public Product? Model
        {
            get => _model;
            set
            {
                _model = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(Name));
                NotifyPropertyChanged(nameof(Price));
                NotifyPropertyChanged(nameof(Quantity));
            }
        }

        public string? Name
        {
            get => Model?.Name ?? string.Empty;
            set
            {
                if (Model != null && Model.Name != value)
                {
                    Model.Name = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Price
        {
            get => Model?.Price.ToString("F2") ?? "0.00";
            set
            {
                if (Model != null && decimal.TryParse(value, out decimal price))
                {
                    Model.Price = price;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Quantity
        {
            get => Model?.Quantity.ToString() ?? "0";
            set
            {
                if (Model != null && int.TryParse(value, out int quantity))
                {
                    Model.Quantity = quantity;
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

        public void AddOrUpdate()
        {
            if (Model != null)
            {
                ProductServiceProxy.Current.AddOrUpdate(Model);
            }
        }

        public ProductViewModel()
        {
            Model = new Product();
        }

        public ProductViewModel(Product? model)
        {
            Model = model ?? new Product();
        }
    }
}