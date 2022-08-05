using System;
using ShoppingCart.Models;
namespace ShoppingCart.Web.Areas.Management.ViewModels.Products{
    public class ProductIndexViewModel{
        public IEnumerable<Product> Products {get; set;}
    }
}