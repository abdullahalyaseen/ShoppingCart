using System;
namespace ShoppingCart.Models
{
    public class Coupon
    {
        public int CouponId { get; set; }

        public string Code { get; set; }

        public string Type { get; set; }

        public bool SingleUse { get; set; }

        public int Amount { get; set; }

        public int Threshold { get; set; }

        public int MaxDiscount { get; set; }


        public virtual ICollection<Order> Orders { get; set; }
    }
}

