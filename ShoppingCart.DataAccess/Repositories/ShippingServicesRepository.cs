using Microsoft.EntityFrameworkCore;
using ShoppingCart.DataAccess.Interfaces;
using ShoppingCart.Models;

namespace ShoppingCart.DataAccess.Repositories;

public class ShippingServicesRepository : Repository<ShippingService>, IShippingServicesRepository
{
    ShoppingCartContext context
    {
        get
        {
            return Context as ShoppingCartContext;
            
        }
    }
    
    public ShippingServicesRepository(ShoppingCartContext context) : base(context)
    {
    }
}