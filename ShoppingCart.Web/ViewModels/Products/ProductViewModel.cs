using System;
using ShoppingCart.Models;

namespace ShoppingCart.Web.ViewModels.Products
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public double Price { get; set; }

        public double SalesPrice { get; set; }
        
        public int Quantity { get; set; }

        public List<string> Gallery { get; set; }

        public IEnumerable<Tag> Tags { get; set; }

        public List<string> Description { get; set; }

        public string ShortDescription { get; set; }

        public string Sku { get; set; }

        public IEnumerable<Review> Reviews { get; set; }

        public double Rating { get; set; }

        public int NumberOfReview { get; set; }
    }
}