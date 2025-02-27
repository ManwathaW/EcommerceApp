using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApp.Interfaces
{
    public interface IShoppingService
    {
        Task<IEnumerable<ShoppingItem>> GetShoppingItemsAsync();
        Task<ShoppingItem> GetShoppingItemAsync(int id);
        Task<bool> UpdateStockQuantityAsync(int itemId, int quantity);
        Task<bool> CheckStockAvailabilityAsync(int itemId, int requestedQuantity);
    }
}
