using ShoppingCart.Models;
namespace ShoppingCart.Web.ViewModels.Products
{
    public class SearchViewModel
    {
        public string Query;
        public string? SortBy { get; set; }
        public double? MinPrice { get; set; }
        public double? MAxPrice { get; set; }
        public IEnumerable<Product>? Products { get; set; }
    }
}