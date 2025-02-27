using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EcommerceApp.Services;
using Microsoft.Maui.Storage;
using Microsoft.Maui.ApplicationModel;
using System.Text.RegularExpressions;
using EcommerceApp.Extensions;
using EcommerceApp.Models;

using System.Collections.ObjectModel;
using SQLite;

namespace EcommerceApp.PageModels
{
    public partial class CartPageModel : BasePageModel
    {
        [ObservableProperty]
        private ObservableCollection<CartItemView> cartItems;

        [ObservableProperty]
        private decimal totalAmount;

        [ObservableProperty]
        private bool isCartEmpty;

        public CartPageModel(DatabaseContext database)
     : base(database)
        {
            Title = "Shopping Cart";
        }

        public override async Task InitializeAsync()
        {
            await LoadCartAsync();
        }

        private async Task LoadCartAsync()
        {
            await ExecuteBusyActionAsync(async () =>
            {
                try
                {
                    var profiles = await Database.GetProfilesAsync();
                    var profileId = profiles.FirstOrDefault()?.ProfileId ?? 1;

                    var cart = await Database.GetActiveCartAsync(profileId);
                    if (cart != null)
                    {
                        var items = await Database.GetCartItemsAsync(cart.CartId);
                        var cartItemViews = new List<CartItemView>();

                        foreach (var item in items)
                        {
                            var shoppingItem = await Database.GetShoppingItemAsync(item.ShoppingItemId);
                            if (shoppingItem != null)
                            {
                                cartItemViews.Add(new CartItemView
                                {
                                    CartItemId = item.CartItemId,
                                    Item = shoppingItem,
                                    Quantity = item.Quantity
                                });
                            }
                        }

                        CartItems = new ObservableCollection<CartItemView>(cartItemViews);
                        CalculateTotal();
                    }
                    else
                    {
                        CartItems = new ObservableCollection<CartItemView>();
                        TotalAmount = 0;
                    }

                    IsCartEmpty = !CartItems.Any();
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert("Error",
                        $"Failed to load cart: {ex.Message}", "OK");
                }
            });
        }

        private void CalculateTotal()
        {
            TotalAmount = CartItems?.Sum(ci => ci.Subtotal) ?? 0;
        }

        [RelayCommand]
        private async Task RemoveFromCart(CartItemView cartItem)
        {
            if (cartItem == null) return;

            var result = await Shell.Current.DisplayAlert("Confirm",
                "Remove this item from cart?", "Yes", "No");

            if (result)
            {
                await ExecuteBusyActionAsync(async () =>
                {
                    try
                    {
                        await Database.DeleteCartItemAsync(cartItem.CartItemId);
                        CartItems.Remove(cartItem);
                        CalculateTotal();
                        IsCartEmpty = !CartItems.Any();

                        await Shell.Current.DisplayAlert("Success",
                            "Item removed from cart!", "OK");
                    }
                    catch (Exception ex)
                    {
                        await Shell.Current.DisplayAlert("Error",
                            $"Failed to remove item: {ex.Message}", "OK");
                    }
                });
            }
        }

        [RelayCommand]
        private async Task UpdateQuantity(CartItemView cartItem)
        {
            if (cartItem == null) return;

            string result = await Shell.Current.DisplayPromptAsync("Update Quantity",
                "Enter new quantity:",
                initialValue: cartItem.Quantity.ToString(),
                keyboard: Keyboard.Numeric);

            if (int.TryParse(result, out int newQuantity) && newQuantity > 0)
            {
                await ExecuteBusyActionAsync(async () =>
                {
                    try
                    {
                        // Check stock availability
                        if (newQuantity > cartItem.Item.StockQuantity)
                        {
                            await Shell.Current.DisplayAlert("Error",
                                "Not enough items in stock", "OK");
                            return;
                        }

                        var dbCartItem = new CartItem
                        {
                            CartItemId = cartItem.CartItemId,
                            Quantity = newQuantity
                        };

                        await Database.UpdateCartItemAsync(dbCartItem);
                        cartItem.Quantity = newQuantity;
                        CalculateTotal();
                    }
                    catch (Exception ex)
                    {
                        await Shell.Current.DisplayAlert("Error",
                            $"Failed to update quantity: {ex.Message}", "OK");
                    }
                });
            }
        }

        [RelayCommand]
        private async Task Checkout()
        {
            if (IsCartEmpty)
            {
                await Shell.Current.DisplayAlert("Error",
                    "Your cart is empty", "OK");
                return;
            }

            var result = await Shell.Current.DisplayAlert("Checkout",
                $"Total amount: ${TotalAmount:F2}\nProceed with purchase?",
                "Yes", "No");

            if (result)
            {
                await ExecuteBusyActionAsync(async () =>
                {
                    try
                    {
                        // Here you would typically integrate with a payment service
                        // For now, we'll just clear the cart
                        foreach (var item in CartItems)
                        {
                            await Database.DeleteCartItemAsync(item.CartItemId);
                        }

                        CartItems.Clear();
                        TotalAmount = 0;
                        IsCartEmpty = true;

                        await Shell.Current.DisplayAlert("Success",
                            "Purchase completed successfully!", "OK");
                    }
                    catch (Exception ex)
                    {
                        await Shell.Current.DisplayAlert("Error",
                            $"Checkout failed: {ex.Message}", "OK");
                    }
                });
            }
        }

        [RelayCommand]
        private async Task Refresh()
        {
            IsRefreshing = true;
            await LoadCartAsync();
            IsRefreshing = false;
        }
    }
}

