using System;
using Stripe;

namespace ShoppingCart.Models
{

    public class Payment
    {
        public Payment()
        {
            
        }


        public int PaymentId { get; set; }

        public int OrderId { get; set; }

        public int ApplicationUserId { get; set; }

        public string GatewayId {get; set;}

        public long Amount { get; set; }

        public string StreetAddress1 { get; set; }

        public string StreetAddress2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public string AddressCountry { get; set; }

        public string CardBrand { get; set; }

        public string CvcCheck { get; set; }

        public string Country { get; set; }

        public string ExpireMonth { get; set; }

        public string ExpireYear { get; set; }

        public string LastFourDigits { get; set; }

        public string Status { get; set; }


        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual Order Order { get; set; }



        public Payment(Charge charge, Address billingAddress, ApplicationUser user)
        {
            Amount = charge.Amount;
            GatewayId = charge.Id;
            Status = charge.Status;
            CardBrand = charge.PaymentMethodDetails.Card.Brand;
            CvcCheck = charge.PaymentMethodDetails.Card.Checks.CvcCheck;
            Country = charge.PaymentMethodDetails.Card.Country;
            ExpireMonth = charge.PaymentMethodDetails.Card.ExpMonth.ToString();
            ExpireYear = charge.PaymentMethodDetails.Card.ExpYear.ToString();
            LastFourDigits = charge.PaymentMethodDetails.Card.Last4;
            StreetAddress1 = billingAddress.StreetAddress1;
            StreetAddress2 = billingAddress.StreetAddress2;
            City = billingAddress.City;
            State = billingAddress.State;
            Zip = billingAddress.Zip;
            AddressCountry = billingAddress.Country;
            ApplicationUser = user;
        }
    }
}

