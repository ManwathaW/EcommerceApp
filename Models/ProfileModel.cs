using SQLite;

namespace EcommerceApp.Models
{
    [Table("Profiles")]
    public class Profile
    {
        [PrimaryKey, AutoIncrement]
        public int ProfileId { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Surname { get; set; } = string.Empty;

        [MaxLength(150)]
        public string EmailAddress { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Bio { get; set; } = string.Empty;

        public string ProfileImagePath { get; set; } = string.Empty;

        [Ignore]
        public string FullName => $"{Name} {Surname}";
    }

    [Table("ShoppingItems")]
    public class ShoppingItem
    {
        [PrimaryKey, AutoIncrement]
        public int ShoppingItemId { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

        [Ignore]
        public bool IsAvailable => StockQuantity > 0;
    }

    [Table("ShoppingCarts")]
    public class ShoppingCart
    {
        [PrimaryKey, AutoIncrement]
        public int CartId { get; set; }

        [Indexed]
        public int ProfileId { get; set; }

        public DateTime CreatedDate { get; set; }

        [Ignore]
        public decimal TotalAmount { get; set; }

        [Ignore]
        public List<CartItem> Items { get; set; } = new List<CartItem>();
    }

    [Table("CartItems")]
    public class CartItem
    {
        [PrimaryKey, AutoIncrement]
        public int CartItemId { get; set; }

        [Indexed]
        public int CartId { get; set; }

        [Indexed]
        public int ShoppingItemId { get; set; }

        public int Quantity { get; set; }

        [Ignore]
        public ShoppingItem Item { get; set; }

        [Ignore]
        public decimal Subtotal => Item?.Price * Quantity ?? 0;
    }

    public class CartItemView
    {
        public int CartItemId { get; set; }
        public ShoppingItem Item { get; set; } = new();
        public int Quantity { get; set; }
        public decimal Subtotal => Item?.Price * Quantity ?? 0;
    }
}