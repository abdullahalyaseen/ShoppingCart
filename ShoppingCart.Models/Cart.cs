using System;
using System.ComponentModel.DataAnnotations;
namespace ShoppingCart.Models
{
    public class Cart
    {
        [Key]
        public Guid CartId {get; set; }

        public int? ApplicationUserId { get; set; }
        
        public int? CouponId { get; set; }

        public int Total { get; set; }

        public int SubtTotal { get; set; }

        public int Discount { get; set; }
        
        public DateTime? LastValidationAt { get; set; }
        
        



        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; }
        
        public virtual Coupon Coupon { get; set; }

    }
}

