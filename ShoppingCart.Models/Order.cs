using System;
namespace ShoppingCart.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public int ApplicationUserId { get; set; }

        public string? Note { get; set; }

        public int? CouponId { get; set; }

        public int SubTotal { get; set; }

        public int DiscountAmount { get; set; }

        public int Total { get; set; }




        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public virtual Coupon? Coupon { get; set; }

        public virtual Payment Payment { get; set; }

        public virtual Shipment Shipment { get; set; }
    }
}

