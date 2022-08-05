using System;
namespace ShoppingCart.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }

        public int OrderId { get; set; }

        public int ApplicationUserId { get; set; }

        public int GatewayId {get; set;}

        public int Amount { get; set; }

        public string ShippingAddress { get; set; }

        public string CardBrand { get; set; }

        public string CvcCheck { get; set; }

        public string Country { get; set; }

        public string ExpireMonth { get; set; }

        public string ExpireYear { get; set; }

        public string LastFourDigits { get; set; }

        public string Status { get; set; }


        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual Order Order { get; set; }

        
    }
}

