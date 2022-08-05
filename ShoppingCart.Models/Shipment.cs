using System;
namespace ShoppingCart.Models
{
    public class Shipment
    {
        public int ShipmentId { get; set; }

        public int OrderId { get; set; }

        public int ShippingPrice { get; set; }

        public int ShippingServiceId { get; set; }

        public string ShippingAddress { get; set; }

        public string? TrackingNumber { get; set; }

        public string Status { get; set; } = "Pending";



        public virtual Order Order { get; set; }

        public virtual ShippingService ShippingService { get; set; }
    }
}

