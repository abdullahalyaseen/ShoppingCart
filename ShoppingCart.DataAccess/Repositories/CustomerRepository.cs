using System;
using ShoppingCart.Models;
using ShoppingCart.DataAccess.Interfaces;
namespace ShoppingCart.DataAccess.Repositories
{
    public class ApplicationUserRepository : Repository<ApplicationUser> , IApplicationUserRepository
    {
        public ShoppingCartContext shoppingCartContext
        {
            get { return Context as ShoppingCartContext; }
        }
        public ApplicationUserRepository(ShoppingCartContext context) : base(context) {
            
        }

    }
}

