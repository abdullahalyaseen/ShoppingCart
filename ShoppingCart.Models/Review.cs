using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.Models
{
    public class Review
    {
        public int ReviewId { get; set; }

        [ForeignKey("ApplicationUser")]
        public int ApplicationUserId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        
        public DateTime AddedAt { get; set; }

        public short Rate { get; set; }

        public string? Comment { get; set; }


        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual Product Product { get; set; }
    }
}

