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

            builder.Entity<ApplicationUser>().HasData(new ApplicationUser { Id = 1, IsCustomer = false, FirstName = "Admin", LastName = "Admin", UserName = "admin@admin.com", NormalizedUserName = "ADMIN@ADMIN.COM", PasswordHash = hasher.HashPassword(new ApplicationUser { Id = 1, IsCustomer = false, FirstName = "Admin", LastName = "Admin", UserName = "admin@admin.com", NormalizedUserName = "ADMIN@ADMIN.COM" }, "Abudtoni@92") });
            builder.Entity<ApplicationUser>().HasData(new ApplicationUser { Id = 2, IsCustomer = true, FirstName = "Abdullah", LastName = "Alyaseen", UserName = "abdullahalheem@gmail.com", NormalizedUserName = "ABDULLAHALHEEM@GMAIL.COM", PasswordHash = hasher.HashPassword(new ApplicationUser { Id = 2, IsCustomer = true, FirstName = "Abdullah", LastName = "Alyaseen", UserName = "abdullahalheem@gmail.com", NormalizedUserName = "ABDULLAHALHEEM@GMAIL.COM" }, "Abudtoni@92") });
            builder.Entity<Role>().HasData(new Role { Id = 1, Name = BuiltInRoles.admin, NormalizedName = BuiltInRoles.admin.ToUpper() });
            builder.Entity<Role>().HasData(new Role { Id = 2, Name = BuiltInRoles.customer, NormalizedName = BuiltInRoles.customer.ToUpper() });
            builder.Entity<RoleClaim>().HasData(new RoleClaim { Id = 1, ClaimType = "view", ClaimValue = "management", RoleId = 1 });
            builder.Entity<RoleClaim>().HasData(new RoleClaim { Id = 2, ClaimType = "view", ClaimValue = "categories", RoleId = 1 });
            builder.Entity<RoleClaim>().HasData(new RoleClaim { Id = 3, ClaimType = "add", ClaimValue = "category", RoleId = 1 });
            builder.Entity<RoleClaim>().HasData(new RoleClaim { Id = 4, ClaimType = "edit", ClaimValue = "category", RoleId = 1 });
            builder.Entity<RoleClaim>().HasData(new RoleClaim { Id = 5, ClaimType = "delete", ClaimValue = "category", RoleId = 1 });
            builder.Entity<RoleClaim>().HasData(new RoleClaim { Id = 6, ClaimType = "view", ClaimValue = "users", RoleId = 1 });
            builder.Entity<RoleClaim>().HasData(new RoleClaim { Id = 7, ClaimType = "add", ClaimValue = "user", RoleId = 1 });
            builder.Entity<RoleClaim>().HasData(new RoleClaim { Id = 8, ClaimType = "edit", ClaimValue = "user", RoleId = 1 });
            builder.Entity<RoleClaim>().HasData(new RoleClaim { Id = 9, ClaimType = "delete", ClaimValue = "user", RoleId = 1 });
            builder.Entity<RoleClaim>().HasData(new RoleClaim { Id = 10, ClaimType = "view", ClaimValue = "products", RoleId = 1 });
            builder.Entity<RoleClaim>().HasData(new RoleClaim { Id = 11, ClaimType = "add", ClaimValue = "product", RoleId = 1 });
            builder.Entity<RoleClaim>().HasData(new RoleClaim { Id = 12, ClaimType = "edit", ClaimValue = "product", RoleId = 1 });
            builder.Entity<RoleClaim>().HasData(new RoleClaim { Id = 13, ClaimType = "delete", ClaimValue = "product", RoleId = 1 });
            builder.Entity<RoleClaim>().HasData(new RoleClaim { Id = 14, ClaimType = "view", ClaimValue = "tags", RoleId = 1 });
            builder.Entity<RoleClaim>().HasData(new RoleClaim { Id = 15, ClaimType = "add", ClaimValue = "tag", RoleId = 1 });
            builder.Entity<RoleClaim>().HasData(new RoleClaim { Id = 16, ClaimType = "edit", ClaimValue = "tag", RoleId = 1 });
            builder.Entity<RoleClaim>().HasData(new RoleClaim { Id = 17, ClaimType = "delete", ClaimValue = "tag", RoleId = 1 });
            builder.Entity<ApplicationUserRole>().HasData(new ApplicationUserRole { RoleId = 1, UserId = 1 });
            builder.Entity<ApplicationUserRole>().HasData(new ApplicationUserRole { RoleId = 2, UserId = 2 });
            base.OnModelCreating(builder);
        }
    }


}