using System;
using System.ComponentModel.DataAnnotations;
namespace ShoppingCart.Models
{
    public class Cart
    {
        [Key]
        public Guid CartId {get; set; }

        public int? ApplicationUserId { get; set; }



        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; }

    }
}

