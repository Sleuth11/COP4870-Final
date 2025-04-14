using Maui.eCommerce.ViewModels;

namespace Maui.eCommerce
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }

        private async void ShopClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//ShopView");
        }

        private async void CartClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//CartView");
        }

        private async void InventoryClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//InventoryManagement");
        }
    }
}