using System;
namespace ShoppingCart.Models
{
    public class Shipment
    {
        public Shipment()
        {
        }

        public int ShipmentId { get; set; }

        public int OrderId { get; set; }

        public int ShippingPrice { get; set; }

        public int ShippingServiceId { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string StreetAddress1 { get; set; }

        public string StreetAddress2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public string Country { get; set; }
        
        public string Mobile { get; set; }
        
        public string Email { get; set; }

        public string? TrackingNumber { get; set; }

        public string Status { get; set; } = "Pending";



        public virtual Order Order { get; set; }

        public virtual ShippingService ShippingService { get; set; }
        
        
        
        public Shipment(ShippingService shippingService, Address shippingAddress)
        {
            
            ShippingPrice = shippingService.Price;
            ShippingServiceId = shippingService.ShippingServiceId;
            FirstName = shippingAddress.FirstName;
            LastName = shippingAddress.LastName;
            StreetAddress1 = shippingAddress.StreetAddress1;
            StreetAddress2 = shippingAddress.StreetAddress2;
            City = shippingAddress.City;
            State = shippingAddress.State;
            Zip = shippingAddress.Zip;
            Country = shippingAddress.Country;
            Mobile = shippingAddress.Mobile;
            Email = shippingAddress.Email;
        }
    }
}

