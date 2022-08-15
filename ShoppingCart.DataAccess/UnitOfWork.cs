using System;
using System.Reflection;
using ShoppingCart.DataAccess.Interfaces;
using ShoppingCart.DataAccess.Repositories;
using ShoppingCart.DataAccess.DbChangeTracking;
namespace ShoppingCart.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ShoppingCartContext _Context;
        private IDbTracker _DbTracker;
        public UnitOfWork(ShoppingCartContext context, IDbTracker DbTracker)
        {
            
            _Context = context;
            _DbTracker = DbTracker;
            ApplicationUser = new ApplicationUserRepository(_Context);
            Cart = new CartRepository(_Context);
            WishListItem = new WishListItemRepository(_Context);
            Address = new AddressRepository(_Context);
            Category = new CategoryRepository(_Context);
            Product = new ProductRepository(_Context);
            Tag = new TagRepository(_Context);
            Image = new ImageRepository(_Context);
            CartItem = new CartItemRepository(_Context);
            Coupon = new CouponRepository(_Context);
            ShippingServices = new ShippingServicesRepository(_Context);

        }

        public virtual IApplicationUserRepository ApplicationUser { get; private set; }
        public virtual ICartRepository Cart { get; private set; }
        public virtual IWishListItemRepository WishListItem { get; private set; }
        public virtual IAddressRepository Address { get; private set; }
        public virtual ICategoryRepository Category { get; private set; }
        public virtual IProductRepository Product { get; private set; }
        public virtual ITagRepository Tag { get; private set; }
        public virtual IImageRepository Image { get; private set; }
        public virtual ICartItemRepository CartItem { get; private set; }
        public virtual ICoupinRepository Coupon { get; private set; }
        public virtual IShippingServicesRepository ShippingServices { get; private set; }
        public virtual int Complete()
        {
            _Context.SavingChanges += _DbTracker.SavingChanges;
            return _Context.SaveChanges();
        }

        public void Dispose()
        {
            _Context.Dispose();
        }



    }
}

