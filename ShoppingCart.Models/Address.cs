using System;
namespace ShoppingCart.Models
{
    public class Address
    {
        public int AddressId { get; set; }

        public int ApplicationUserId { get; set; }

        public string AddressLabel { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string StreetAddress1 { get; set; }

        public string StreetAddress2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public string Country { get; set; }




        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}

