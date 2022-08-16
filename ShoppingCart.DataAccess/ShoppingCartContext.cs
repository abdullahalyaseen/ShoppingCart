using Microsoft.EntityFrameworkCore;
using System.Linq;
using ShoppingCart.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace ShoppingCart.DataAccess
{

    public class ShoppingCartContext : IdentityDbContext<ApplicationUser, Role, int>
    {
        Microsoft.AspNetCore.Identity.PasswordHasher<ApplicationUser> hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<ApplicationUser>();


        public ShoppingCartContext(DbContextOptions<ShoppingCartContext> options) : base(options)
        {


        }


        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<CartItem> CartItems { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Coupon> Coupons { get; set; }
        public virtual DbSet<ApplicationUser> ApplicationUser { get; set; }
        public virtual DbSet<CustomerProductVisit> CustomerProductVisits { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<RoleClaim> RoleClaims { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductUserLog> ProductUserLogs { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Shipment> Shipments { get; set; }
        public virtual DbSet<ShippingService> ShippingServices { get; set; }
        public virtual DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<WishlistItem> WishlistItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Product>().HasMany(p => p.Tags).WithMany(t => t.Products);
            builder.Entity<Product>().HasMany(p => p.CartItems);
            base.OnModelCreating(builder);
        }
    }


}