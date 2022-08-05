using System;
using ShoppingCart.Models;
using ShoppingCart.DataAccess.Interfaces;
namespace ShoppingCart.DataAccess.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {


        public ShoppingCartContext shoppingCartContext
        {
            get
            {
                return Context as ShoppingCartContext;
            }
        }
        public CategoryRepository(ShoppingCartContext context) : base(context)
        {
        }
    }
}

