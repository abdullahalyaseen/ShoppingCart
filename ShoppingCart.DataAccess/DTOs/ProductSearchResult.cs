using ShoppingCart.Models;
namespace ShoppingCart.DataAccess.DTOs
{
    public class ProductSearchResult
    {
        public IEnumerable<ProductSearchCategories> Categories { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}

