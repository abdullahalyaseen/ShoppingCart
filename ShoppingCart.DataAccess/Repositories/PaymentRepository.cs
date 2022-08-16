using Microsoft.EntityFrameworkCore;
using ShoppingCart.DataAccess.Interfaces;
using ShoppingCart.Models;

namespace ShoppingCart.DataAccess.Repositories;

public class PaymentRepository : Repository<Payment>, IPaymentRepository
{
    public ShoppingCartContext context
    {
        get
        {
            return Context as ShoppingCartContext;
        }
    }
    
    public PaymentRepository(ShoppingCartContext context) : base(context)
    {
    }
}