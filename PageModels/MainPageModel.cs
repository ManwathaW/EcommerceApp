using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EcommerceApp.Models;
using EcommerceApp.Services;
using Microsoft.Maui.Storage;
using Microsoft.Maui.ApplicationModel;
using System.Text.RegularExpressions;
using EcommerceApp.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApp.PageModels
{
    public partial class MainPageModel : BasePageModel
    {
        private readonly INavigation _navigation;
     

        [ObservableProperty]
        private ObservableCollection<ShoppingItem> shoppingItems;

        [ObservableProperty]
        private ShoppingItem selectedItem;

        public MainPageModel(DatabaseContext database)
     : base(database)
        {
            Title = "Shopping";
        }



        public override async Task InitializeAsync()
        {
            await LoadItemsAsync();
        }

        private async Task LoadItemsAsync()
        {
            await ExecuteBusyActionAsync(async () =>
            {
                var items = await Database.GetShoppingItemsAsync();
                ShoppingItems = new ObservableCollection<ShoppingItem>(items);
            });
        }

        [RelayCommand]
        private async Task AddToCart(ShoppingItem item)
        {
            if (item == null) return;

            await ExecuteBusyActionAsync(async () =>
            {
                try
                {
                    var profiles = await Database.GetProfilesAsync();
                    var profileId = profiles.FirstOrDefault()?.ProfileId ?? 1;

                    var cart = await Database.GetActiveCartAsync(profileId);
                    if (cart == null)
                    {
                        cart = new ShoppingCart
                        {
                            ProfileId = profileId,
                            CreatedDate = DateTime.Now
                        };
                        await Database.SaveCartAsync(cart);
                    }

                    // Check stock availability
                    var currentCartItems = await Database.GetCartItemsAsync(cart.CartId);
                    var existingCartItem = currentCartItems.FirstOrDefault(ci => ci.ShoppingItemId == item.ShoppingItemId);
                    var requestedQuantity = (existingCartItem?.Quantity ?? 0) + 1;

                    if (requestedQuantity > item.StockQuantity)
                    {
                        await Shell.Current.DisplayAlert("Error",
                            "Not enough items in stock", "OK");
                        return;
                    }

                    var cartItem = new CartItem
                    {
                        CartId = cart.CartId,
                        ShoppingItemId = item.ShoppingItemId,
                        Quantity = 1
                    };

                    await Database.SaveCartItemAsync(cartItem);
                    await Shell.Current.DisplayAlert("Success",
                        "Item added to cart!", "OK");
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert("Error",
                        $"Failed to add item: {ex.Message}", "OK");
                }
            });
        }

        [RelayCommand]
        private async Task ItemSelected(ShoppingItem item)
        {
            if (item == null)
                return;

            SelectedItem = null; // Reset selection
            await Shell.Current.DisplayAlert(item.Name,
                $"Price: ${item.Price}\n{item.Description}", "Close");
        }

        [RelayCommand]
        private async Task NavigateToCart()
        {
            await Shell.Current.GoToAsync("//cart");
        }

        [RelayCommand]
        private async Task NavigateToProfile()
        {
            await Shell.Current.GoToAsync("//profile");
        }

        [RelayCommand]
        private async Task Refresh()
        {
            IsRefreshing = true;
            await LoadItemsAsync();
            IsRefreshing = false;
        }
    }
}
