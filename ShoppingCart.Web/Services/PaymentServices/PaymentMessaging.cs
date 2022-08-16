using Stripe;
namespace ShoppingCart.Web.Services
{

    public class PaymentMessaging
    {
        public PaymentStatus Status { get; set; }
        public Charge Charge { get; set; }
        public Exception Exception { get; set; }

    }

    public enum PaymentStatus
    {
        Paid,
        Failed
    }
}