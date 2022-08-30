using ShoppingCart.Models;
using ShoppingCart.DataAccess.DTOs;
namespace ShoppingCart.DataAccess.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {

        dynamic Search(string q, int categoryid = 0, int lowerPrice = 0 , int upperPrice = 0 , List<string> rating = null, string orderby = "ordernumber");
        Product GetForEdit(int id);
        public KeyValuePair<bool,int> CheckProductAvailability(int productId, int quantity);
    }
}