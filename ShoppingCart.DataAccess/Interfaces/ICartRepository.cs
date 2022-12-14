using System;
using ShoppingCart.Models;
namespace ShoppingCart.DataAccess.Interfaces
{
    public interface ICartRepository : IRepository<Cart>
    {
        public Cart Get(Guid id);
    }
}

