using System.Linq.Expressions;
using ShoppingCart.Models;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.DataAccess.Interfaces;
using ShoppingCart.DataAccess;
namespace ShoppingCart.DataAccess.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {


        public Product GetForEdit(int id)
        {
            var product = context.Set<Product>()
            .Include(p => p.Images)
            .Include(p=>p.Tags)
            .FirstOrDefault(p => p.ProductId == id);

            return product;
        }
        public ShoppingCartContext context
        {
            get
            {
                return Context as ShoppingCartContext;
            }
        }
        public ProductRepository(ShoppingCartContext context) : base(context)
        {

        }
    }
}