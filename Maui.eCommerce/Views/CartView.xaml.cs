using Maui.eCommerce.ViewModels;

namespace Maui.eCommerce.Views;

public partial class CartView : ContentPage
{
    public CartView()
    {
        InitializeComponent();
        BindingContext = new CartViewModel();
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        (BindingContext as CartViewModel)?.RefreshCart();
    }

    private async void UpdateQuantityClicked(object sender, EventArgs e)
    {
        var viewModel = BindingContext as CartViewModel;
        if (viewModel?.SelectedCartItem == null)
        {
            await DisplayAlert("Selection Required", "Please select an item to update.", "OK");
            return;
        }

        bool success = viewModel.UpdateItemQuantity();
        if (success)
        {
            await DisplayAlert("Success", "Cart updated!", "OK");
            viewModel.RefreshCart();
        }
        else
        {
            await DisplayAlert("Error", "Failed to update cart. Make sure you have enough items in stock.", "OK");
        }
    }

    private async void RemoveItemClicked(object sender, EventArgs e)
    {
        var viewModel = BindingContext as CartViewModel;
        if (viewModel?.SelectedCartItem == null)
        {
            await DisplayAlert("Selection Required", "Please select an item to remove.", "OK");
            return;
        }

        bool success = viewModel.RemoveItem();
        if (success)
        {
            await DisplayAlert("Success", "Item removed from cart!", "OK");
            viewModel.RefreshCart();
        }
    }

    private async void ContinueShoppingClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//ShopView");
    }

    private async void CheckoutClicked(object sender, EventArgs e)
    {
        var viewModel = BindingContext as CartViewModel;
        if (viewModel?.CartItems.Count == 0)
        {
            await DisplayAlert("Empty Cart", "Your cart is empty. Please add items before checkout.", "OK");
            return;
        }

        await Shell.Current.GoToAsync("//CheckoutView");
    }

    private async void MainMenuClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
}