using System;
namespace ShoppingCart.Models
{
    public class OrderItem
    {
        public OrderItem()
        {
        }

        public int OrderItemId { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }

        public int DiscountedPrice { get; set; }



        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
        
        
        public OrderItem(CartItem item)
        {
            ProductId = item.ProductId;
            Quantity = item.Quantity;
            Price = item.Product.Price;
            DiscountedPrice = item.Product.SalesPrice;
        }
    }
}

