using Microsoft.EntityFrameworkCore;
using ShoppingCart.DataAccess.Interfaces;
using ShoppingCart.Models;

namespace ShoppingCart.DataAccess.Repositories;

public class OrderRepository : Repository<Order> , IOrderRepository
{
    public ShoppingCartContext context
    {
        get
        {
            return Context as ShoppingCartContext;
        }
    }
    
    public OrderRepository(ShoppingCartContext context) : base(context)
    {
    }
}