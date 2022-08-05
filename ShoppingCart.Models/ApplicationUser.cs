using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
namespace ShoppingCart.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsCustomer {get; set;}



        public virtual ICollection<Address> Addresses { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual Cart Cart { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<WishlistItem> WishlistItems { get; set; }

        public virtual ICollection<CustomerProductVisit> CustomerProductVisits { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }

        public virtual ICollection<ProductUserLog> ProductUserLogs {get; set;}




    }
}

