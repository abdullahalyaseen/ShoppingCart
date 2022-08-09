using Microsoft.EntityFrameworkCore;
using ShoppingCart.DataAccess.Interfaces;
using ShoppingCart.Models;

namespace ShoppingCart.DataAccess.Repositories;

public class CartItemRepository : Repository<CartItem>, ICartItemRepository

{
    public CartItem CheckCartItemExistence(int productId, Guid CartId)
    {
        return context.CartItems.Where(i => i.ProductId == productId).Where(i => i.CartId == CartId).FirstOrDefault();
    }

    public ShoppingCartContext context
    {
        get { return Context as ShoppingCartContext; }
    }

    public CartItemRepository(ShoppingCartContext context) : base(context)
    {
    }
}