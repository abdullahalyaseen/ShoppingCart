using ShoppingCart.Models;

namespace ShoppingCart.DataAccess.DTOs
{
    public class ProductSearchCategories
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        
        public int ProductsCount { get; set; }
    }
}