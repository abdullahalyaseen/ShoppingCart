using System;
namespace ShoppingCart.DataAccess.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IApplicationUserRepository ApplicationUser { get; }
        ICartRepository Cart { get; }
        IWishListItemRepository WishListItem { get; }
        IAddressRepository Address { get; }
        ICategoryRepository Category { get; }
        IProductRepository Product {get;}
        ITagRepository Tag {get; }
        IImageRepository Image {get;}
        int Complete();
    }
}

