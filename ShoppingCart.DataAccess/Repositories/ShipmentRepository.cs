using Microsoft.EntityFrameworkCore;
using ShoppingCart.DataAccess.Interfaces;
using ShoppingCart.Models;

namespace ShoppingCart.DataAccess.Repositories;

public class ShipmentRepository : Repository<Shipment>, IShipmentRepository
{

    public ShoppingCartContext context
    {
        get
        {
            return Context as ShoppingCartContext;
        }
    }
    public ShipmentRepository(ShoppingCartContext context) : base(context)
    {
    }
}