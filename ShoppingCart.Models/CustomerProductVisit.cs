using System;
namespace ShoppingCart.Models
{
    public class CustomerProductVisit
    {
        public int CustomerProductVisitId { get; set; }

        public int ProductId { get; set; }

        public int ApplicationUserId { get; set; }

        public DateTime VisitTime { get; set; }



        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual Product Product { get; set; }
    }
}

