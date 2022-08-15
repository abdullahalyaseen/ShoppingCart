using Microsoft.EntityFrameworkCore;
using ShoppingCart.DataAccess.Interfaces;
using ShoppingCart.Models;

namespace ShoppingCart.DataAccess.Repositories;

public class CouponRepository : Repository<Coupon> , ICoupinRepository
{

    public ShoppingCartContext context
    {
        get
        {
            return Context as ShoppingCartContext;
            
        }
    }
    public CouponRepository(ShoppingCartContext context) : base(context)
    {
        
    }
}