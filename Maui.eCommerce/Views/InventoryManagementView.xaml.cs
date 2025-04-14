using Library.eCommerce.Services;
using Maui.eCommerce.ViewModels;
using Spring2025_Samples.Models;

namespace Maui.eCommerce.Views;

public partial class InventoryManagementView : ContentPage
{
    private ListView _productListView;

    public InventoryManagementView()
    {
        InitializeComponent();
        BindingContext = new InventoryManagementViewModel();

        // Find the ListView control by name - make sure to set x:Name="ProductListView" in your XAML
        _productListView = this.FindByName<ListView>("ProductListView");
    }

    private async void DeleteClicked(object sender, EventArgs e)
    {
        // Get the selected product directly from the ListView if possible
        var selectedProduct = _productListView?.SelectedItem as Product;

        // Fallback to ViewModel
        if (selectedProduct == null)
        {
            selectedProduct = (BindingContext as InventoryManagementViewModel)?.SelectedProduct;
        }

        if (selectedProduct == null)
        {
            await DisplayAlert("Selection Required", "Please select a product to delete.", "OK");
            return;
        }

        bool confirm = await DisplayAlert("Confirm Delete",
            $"Are you sure you want to delete {selectedProduct.Name}?",
            "Yes", "No");

        if (confirm)
        {
            try
            {
                var productName = selectedProduct.Name;
                var productId = selectedProduct.Id;

                var result = ProductServiceProxy.Current.Delete(productId);

                if (result != null)
                {
                    
                    (BindingContext as InventoryManagementViewModel)?.RefreshProductList();

                    await DisplayAlert("Success", $"{productName} has been deleted successfully.", "OK");
                }
                else
                {
                    await DisplayAlert("Error", "Failed to delete the product.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
        }
    }

    private async void CancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }

    private async void AddClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Product");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        (BindingContext as InventoryManagementViewModel)?.RefreshProductList();
    }

    private async void EditClicked(object sender, EventArgs e)
    {
        
        var selectedProduct = _productListView?.SelectedItem as Product;

        if (selectedProduct == null)
        {
            selectedProduct = (BindingContext as InventoryManagementViewModel)?.SelectedProduct;
        }

        if (selectedProduct == null)
        {
            await DisplayAlert("Selection Required", "Please select a product to edit.", "OK");
            return;
        }

        try
        {
            await Shell.Current.GoToAsync($"//Product?productId={selectedProduct.Id}");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Navigation error: {ex.Message}", "OK");
        }
    }

    private void SearchClicked(object sender, EventArgs e)
    {
        (BindingContext as InventoryManagementViewModel)?.RefreshProductList();
    }
}