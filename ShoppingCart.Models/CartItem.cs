using System;
using System.ComponentModel.DataAnnotations;
namespace ShoppingCart.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }

        public Guid CartId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }


        public virtual Cart Cart { get; set; }
        public virtual Product Product { get; set; }
    }
}