using ShoppingCart.Models;

namespace ShoppingCart.DataAccess.Interfaces;

public interface ICartItemRepository : IRepository<CartItem>
{
    CartItem CheckCartItemExistence(int productId, Guid CartId);
}