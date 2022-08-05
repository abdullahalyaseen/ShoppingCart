using System;
using ShoppingCart.DataAccess.Interfaces;
using ShoppingCart.Models;
namespace ShoppingCart.DataAccess.Repositories
{
    public class AddressRepository : Repository<Address> , IAddressRepository
    {
        public ShoppingCartContext ShoppingCartContext
        {
            get
            {
                return Context as ShoppingCartContext;
            }
        }

        public AddressRepository(ShoppingCartContext context) :base (context) 
        {
        }
    }
}

