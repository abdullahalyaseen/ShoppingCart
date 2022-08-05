using ShoppingCart.Models;
namespace ShoppingCart.DataAccess.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {


        Product GetForEdit(int id);
    }
}