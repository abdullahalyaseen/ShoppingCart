using System;
namespace ShoppingCart.Models
{
    public class Product
    {
        
        public int ProductId { get; set; }

        public string Title { get; set; }

        public int Price { get; set; }

        public int SalesPrice { get; set; }

        public int CategoryId { get; set; }

        public string MainImage {get; set;}

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public string Sku { get; set; }

        public int Quantity { get; set; }

        public bool Featured { get; set; }

        public bool Archived { get; set; }

        public DateTime AddAt { get; set; }

        public DateTime ModifiedAt { get; set; }


        public virtual ICollection<CustomerProductVisit> CustomerProductVisits { get; set; }

        public virtual ICollection<WishlistItem> WishlistItems { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public virtual ICollection<Image> Images { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }

        public virtual ICollection<ProductUserLog> ProductUserLogs { get; set; }

    }
}

