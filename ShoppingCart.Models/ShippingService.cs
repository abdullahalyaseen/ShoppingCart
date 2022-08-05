using System;
namespace ShoppingCart.Models
{
    public class ShippingService
    {
        public int ShippingServiceId { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public int Duration { get; set; }

        public string Descrption { get; set; }


        public virtual ICollection<Shipment> Shipments { get; set; }
    }
}

