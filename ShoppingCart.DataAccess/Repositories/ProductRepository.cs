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

        public KeyValuePair<bool,int> CheckProductAvailability(int productId, int quantity)
        {
            var product = context.Products.Find(productId);
            if (product != null)
            {
                if (product.Quantity >= quantity)
                {
                    return new KeyValuePair<bool, int>(key:true,product.Quantity);
                }
                if (product.Quantity == 0)
                {
                    return new KeyValuePair<bool, int>(false, 0);
                }
                if(product.Quantity < quantity)
                {
                    return  new KeyValuePair<bool, int>(false, product.Quantity);
                }
            }
            return new KeyValuePair<bool, int>(false, -1);
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