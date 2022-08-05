using System;
namespace ShoppingCart.Models
{
    public class Cart
    {
        public int CartId {get; set;}

        public int ApplicationUserId { get; set; }



        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; }

    }
}

