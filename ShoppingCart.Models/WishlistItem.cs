using System;
namespace ShoppingCart.Models
{
    public class WishlistItem
    {
        public int WishlistItemId { get; set; }

        public int ApplicationUserId { get; set; }

        public int ProductId { get; set; }


        
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Product Product { get; set; }
    }
}

