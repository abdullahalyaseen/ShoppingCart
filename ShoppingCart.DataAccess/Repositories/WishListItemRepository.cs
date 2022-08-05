using System;
using ShoppingCart.DataAccess.Interfaces;
using ShoppingCart.Models;
using ShoppingCart.DataAccess.Repositories;
namespace ShoppingCart.DataAccess.Repositories
{
    public class WishListItemRepository : Repository<WishlistItem>,IWishListItemRepository
    {
        public ShoppingCartContext ShoppingCartContext
        {
            get
            {
                return Context as ShoppingCartContext;
            }
        }

        public WishListItemRepository(ShoppingCartContext context) : base (context)
        {
        }
    }
}

