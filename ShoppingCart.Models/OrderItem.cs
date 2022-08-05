using System;
namespace ShoppingCart.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }

        public int DiscountedPrice { get; set; }

        public int Total { get; set; }

        public int DiscountedTotal { get; set; }



        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}

