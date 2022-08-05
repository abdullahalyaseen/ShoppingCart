using System;
namespace ShoppingCart.Models
{
    public class Review
    {
        public int ReviewId { get; set; }

        public int ApplicationUserId { get; set; }

        public int ProductId { get; set; }

        public short Rate { get; set; }

        public string Comment { get; set; }


        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual Product Product { get; set; }
    }
}

