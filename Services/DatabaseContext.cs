using SQLite;
using EcommerceApp.Models;
using Microsoft.Maui.Storage;
using Microsoft.Maui.ApplicationModel;
using SQLite;
using System.IO;
using Microsoft.Maui.Graphics;

namespace EcommerceApp.Services
{
    public class DatabaseContext
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseContext()
        {
            try
            {
                var databasePath = Path.Combine(FileSystem.AppDataDirectory, "ecommerce.db");
            _database = new SQLiteAsyncConnection(databasePath);
            InitializeAsync().Wait();
        }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database initialization error: {ex.Message}");
            }
        }

        private async Task InitializeAsync()
        {
            // Create tables one by one to ensure proper initialization
            await _database.CreateTableAsync<Profile>();
            await _database.CreateTableAsync<ShoppingItem>();
            await _database.CreateTableAsync<ShoppingCart>();
            await _database.CreateTableAsync<CartItem>();

            await SeedDataAsync();
        }

        private async Task SeedDataAsync()
        {
            var count = await _database.Table<ShoppingItem>().CountAsync();
            if (count == 0)
            {
                var items = new List<ShoppingItem>
                {
                    new ShoppingItem
                    {
                        Name = "Premium Savings Account",
                        Description = "High-yield savings account with premium benefits",
                        Price = 0,
                        StockQuantity = 100,
                        ImageUrl = "savings_account.png"
                    },
                    new ShoppingItem
                    {
                        Name = "Investment Portfolio",
                        Description = "Diversified investment package for growth",
                        Price = 1000,
                        StockQuantity = 50,
                        ImageUrl = "investment.png"
                    }
                };

                foreach (var item in items)
                {
                    await _database.InsertAsync(item);
                }
            }
        }

        // Profile Methods
        public async Task<List<Profile>> GetProfilesAsync() =>
            await _database.Table<Profile>().ToListAsync();

        public async Task<Profile> GetProfileAsync(int id) =>
            await _database.GetAsync<Profile>(id);

        public async Task<int> SaveProfileAsync(Profile profile)
        {
            if (profile.ProfileId != 0)
                return await _database.UpdateAsync(profile);
            return await _database.InsertAsync(profile);
        }

        // Shopping Item Methods
        public async Task<List<ShoppingItem>> GetShoppingItemsAsync() =>
            await _database.Table<ShoppingItem>().ToListAsync();

        public async Task<ShoppingItem> GetShoppingItemAsync(int id) =>
            await _database.GetAsync<ShoppingItem>(id);

        public async Task<int> UpdateShoppingItemAsync(ShoppingItem item) =>
            await _database.UpdateAsync(item);

        // Shopping Cart Methods
        public async Task<ShoppingCart> GetActiveCartAsync(int profileId) =>
            await _database.Table<ShoppingCart>()
                .Where(c => c.ProfileId == profileId)
                .OrderByDescending(c => c.CreatedDate)
                .FirstOrDefaultAsync();

        public async Task<int> SaveCartAsync(ShoppingCart cart)
        {
            if (cart.CartId != 0)
                return await _database.UpdateAsync(cart);
            return await _database.InsertAsync(cart);
        }

        // Cart Item Methods
        public async Task<List<CartItem>> GetCartItemsAsync(int cartId) =>
            await _database.Table<CartItem>()
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();

        public async Task<int> SaveCartItemAsync(CartItem item)
        {
            if (item.CartItemId != 0)
                return await _database.UpdateAsync(item);
            return await _database.InsertAsync(item);
        }

        public async Task<int> UpdateCartItemAsync(CartItem item) =>
            await _database.UpdateAsync(item);

        public async Task<int> DeleteCartItemAsync(int cartItemId) =>
            await _database.DeleteAsync<CartItem>(cartItemId);
    }
}