using ShoppingCart.Models;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Web.ViewModels.Checkout;

public class CheckOutViewModel
{
    public IEnumerable<Address>? Addresses { get; set; }
    [Display(Name = "Shipping Address")]
    public int ShippingAddressId { get; set; }
    [Display(Name = "Billing Address")]
    public int BillingAddressId { get; set; }

    public double Total { get; set; }
    public double Subtotal { get; set; }
    public double Discount { get; set; }
    [MinLength(16,ErrorMessage = "Card Number length must be 16")]
    [MaxLength(16,ErrorMessage = "Card Number length must be 16")]
    [Display(Name = "Card Number")]
    public string CardNumber { get; set; }
    [Range(typeof(int),"1","12")]
    [Display(Name = "Month")]
    public string Month { get; set; }
    [Range(typeof(int),"2022","2050")]
    [Display(Name = "Year")]
    public string Year { get; set; }
    [Range(typeof(int),"001","999")]
    [Display(Name = "CVC")]
    public string Cvc { get; set; }
    public IEnumerable<ShippingService>? ShippingServices { get; set; }
    [Display(Name = "Shipping Service")]
    public int ShippingServiceId { get; set; }

    
}