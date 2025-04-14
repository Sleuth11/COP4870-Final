using Maui.eCommerce.ViewModels;

namespace Maui.eCommerce.Views;

public partial class ShopView : ContentPage
{
    public ShopView()
    {
        InitializeComponent();
        BindingContext = new ShopViewModel();
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        (BindingContext as ShopViewModel)?.RefreshProductList();
    }

    private void SearchClicked(object sender, EventArgs e)
    {
        (BindingContext as ShopViewModel)?.RefreshProductList();
    }

    private async void AddToCartClicked(object sender, EventArgs e)
    {
        var viewModel = BindingContext as ShopViewModel;
        if (viewModel?.SelectedProduct == null)
        {
            await DisplayAlert("Selection Required", "Please select a product first.", "OK");
            return;
        }

        bool success = viewModel.AddToCart();
        if (success)
        {
            await DisplayAlert("Success", "Item added to cart!", "OK");
            viewModel.RefreshProductList();
        }
        else
        {
            await DisplayAlert("Error", "Not enough items in stock.", "OK");
        }
    }

    private async void ViewCartClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//CartView");
    }

    private async void MainMenuClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
}