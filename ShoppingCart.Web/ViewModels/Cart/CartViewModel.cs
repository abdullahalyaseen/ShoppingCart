using ShoppingCart.Models;

namespace ShoppingCart.Web.ViewModels.Cart;

public class CartViewModel
{
    public IEnumerable<CartItem>? CartItems { get; set; }
    
    public double? Total { get; set; }
    
    public double? SubTotal { get; set; }
    
    public double? Discount { get; set; }
    
    public string? CouponCode { get; set; }
}