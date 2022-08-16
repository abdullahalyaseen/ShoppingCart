using Microsoft.EntityFrameworkCore;
using ShoppingCart.DataAccess.Interfaces;
using ShoppingCart.Models;

namespace ShoppingCart.DataAccess.Repositories;

public class OrderItemRepository : Repository<OrderItem> , IOrderItemRepository
{
    
    public ShoppingCartContext context
    {
        get
        {
            return Context as ShoppingCartContext;
        }
    }
    public OrderItemRepository(DbContext context) : base(context)
    {
    }
}