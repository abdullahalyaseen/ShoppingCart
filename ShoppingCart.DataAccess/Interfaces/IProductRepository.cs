using ShoppingCart.Models;
namespace ShoppingCart.DataAccess.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {


        Product GetForEdit(int id);
        public KeyValuePair<bool,int> CheckProductAvailability(int productId, int quantity);
    }
}