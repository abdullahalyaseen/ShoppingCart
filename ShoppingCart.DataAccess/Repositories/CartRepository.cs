using System;
using ShoppingCart.DataAccess.Interfaces;
using ShoppingCart.DataAccess.Repositories;
using ShoppingCart.Models;
namespace ShoppingCart.DataAccess.Repositories
{
    public class CartRepository : Repository<Cart>,ICartRepository
    {
        public Cart Get(Guid id)
        {
            return context.Carts.Find(id);
        }
        public ShoppingCartContext context
        {
            get
            {
                return Context as ShoppingCartContext;
            }
        }

        public CartRepository(ShoppingCartContext context) : base(context)
        {
        }
    }
}

