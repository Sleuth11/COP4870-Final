using Maui.eCommerce.ViewModels;

namespace Maui.eCommerce.Views;

public partial class CheckoutView : ContentPage
{
    public CheckoutView()
    {
        InitializeComponent();
        BindingContext = new CheckoutViewModel();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Complete the checkout process
        (BindingContext as CheckoutViewModel)?.CompleteCheckout();
    }

    private async void ReturnToMainClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
}