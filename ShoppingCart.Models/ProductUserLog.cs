using System;
namespace ShoppingCart.Models
{
    public class ProductUserLog
    {
        public int ProductUserLogId { get; set; }

        public int ProductId { get; set; }

        public int ApplicationUserId { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public string ChangeType { get; set; }


        public virtual Product Product { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}

