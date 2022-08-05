using System;
using ShoppingCart.Models;
namespace ShoppingCart.Web.ViewModels.Product{
    public class ProductViewModel{
        public int Id{get; set;}

        public string Title {get; set;}

        public double Price {get; set;}

        public double SalesPrice {get; set;}

        public string MainImage {get; set;}

        public IEnumerable<Image> Gallery{ get; set;}

        public IEnumerable<Tag> Tags {get; set;}

        public string Description {get; set;}

        public string ShortDescription {get; set;}

        public string Sku {get; set;}

        public IEnumerable<Review> Reviews{ get ;set;}


    }
}