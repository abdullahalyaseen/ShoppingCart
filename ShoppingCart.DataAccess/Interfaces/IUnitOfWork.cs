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
        ICartItemRepository CartItem { get; }
        ICouponRepository Coupon { get; }
        IShippingServicesRepository ShippingServices { get; }
        
        IPaymentRepository Payment { get; }
        IOrderRepository Order { get; }
        IOrderItemRepository OrderItem { get; }
        IShipmentRepository Shipment { get; }
        int Complete();
    }
}

