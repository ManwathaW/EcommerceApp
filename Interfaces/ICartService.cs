using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApp.Interfaces
{
    public interface ICartService
    {
        Task<ShoppingCart> GetActiveCartAsync(int profileId);
        Task<IEnumerable<CartItem>> GetCartItemsAsync(int cartId);
        Task<bool> AddToCartAsync(int profileId, int itemId, int quantity);
        Task<bool> RemoveFromCartAsync(int cartItemId);
        Task<bool> UpdateCartItemQuantityAsync(int cartItemId, int quantity);
        Task<decimal> GetCartTotalAsync(int cartId);
    }
}
