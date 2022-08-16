using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.DataAccess;
using ShoppingCart.DataAccess.Interfaces;
using ShoppingCart.Models;
using ShoppingCart.Web.Services;
using ShoppingCart.Web.ViewModels.Checkout;
using Stripe;
using Address = ShoppingCart.Models.Address;
using Coupon = ShoppingCart.Models.Coupon;

namespace ShoppingCart.Web.Controllers;

[Authorize]
public class CheckOut : Controller
{
    private IUnitOfWork _unitOfWork;
    private UserManager<ApplicationUser> _userManager;

    public CheckOut(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    [HttpGet("/checkout")]
    public IActionResult Index()
    {
        CheckOutViewModel model = new CheckOutViewModel();
        var user = _userManager.GetUserAsync(User).Result;
        var cart = _unitOfWork.Cart.GetWith(c => c.ApplicationUserId == user.Id);
        if (cart != null)
        {
            var addresses = _unitOfWork.Address.Find(a => a.ApplicationUserId == user.Id);
            var shippingServices = _unitOfWork.ShippingServices.GetAll();
            var timeForLastValidation = DateTime.Now - cart.LastValidationAt;
            TimeSpan maximumAllowedValidationAge = TimeSpan.FromMinutes(5);
            if (timeForLastValidation > maximumAllowedValidationAge)
            {
                return RedirectToAction("Index", "Cart");
            }

            model.Addresses = addresses;
            model.ShippingServices = shippingServices;
            model.Total = Math.Round(cart.Total / 100.0, 2);
            model.Subtotal = Math.Round(cart.SubtTotal / 100.0, 2);
            model.Discount = Math.Round(cart.Discount / 100.0, 2);
            return View(model);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PlaceOrder(CheckOutViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (model.ShippingServiceId == null)
            {
                return RedirectToAction("Index", "CheckOut");
            }

            string cartId;
            Request.Cookies.TryGetValue("Cart", out cartId);
            if (cartId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var cart = _unitOfWork.Cart.GetWith(c => c.CartId == Guid.Parse(cartId),"Coupon");
                if (cart != null)
                {
                    var timeForLastValidation = DateTime.Now - cart.LastValidationAt;
                    TimeSpan maximumAllowedValidationAge = TimeSpan.FromMinutes(6);
                    if (timeForLastValidation > maximumAllowedValidationAge)
                    {
                        Response.StatusCode = 400;
                        Response.WriteAsJsonAsync(new
                            { result = "redirect", redirect = Url.Action("Index", "CheckOut") });
                        return new EmptyResult();
                    }

                    var shippingService = _unitOfWork.ShippingServices.Get(model.ShippingServiceId);
                    int finalCaptureValue = shippingService.Price + cart.Total;

                    var result = PaymentServices
                        .PayAsync(model.CardNumber, model.Month, model.Year, model.Cvc, finalCaptureValue).Result;
                    var x = result;

                    //if charge paid 
                    if (result.Status == PaymentStatus.Paid)
                    {
                        if (result.Charge.Paid)
                        {
                            //create new payment
                            var user = await _userManager.GetUserAsync(User);
                            
                            
                            List<OrderItem> orderItems = new List<OrderItem>();
                            IEnumerable<CartItem> cartItems =
                                _unitOfWork.CartItem.Find(i => i.CartId == cart.CartId, "Product");
                            Address billingAddress = _unitOfWork.Address.Get(model.BillingAddressId);
                            Address shippingAddress = _unitOfWork.Address.Get(model.ShippingAddressId);
                            
                            
                            Payment payment = new Payment(result.Charge,billingAddress,user);
                            // payment.Amount = result.Charge.Amount;
                            // payment.GatewayId = result.Charge.Id;
                            // payment.Status = result.Charge.Status;
                            // payment.CardBrand = result.Charge.PaymentMethodDetails.Card.Brand;
                            // payment.CvcCheck = result.Charge.PaymentMethodDetails.Card.Checks.CvcCheck;
                            // payment.Country = result.Charge.PaymentMethodDetails.Card.Country;
                            // payment.ExpireMonth = result.Charge.PaymentMethodDetails.Card.ExpMonth.ToString();
                            // payment.ExpireYear = result.Charge.PaymentMethodDetails.Card.ExpYear.ToString();
                            // payment.LastFourDigits = result.Charge.PaymentMethodDetails.Card.Last4;
                            // payment.StreetAddress1 = billingAddress.StreetAddress1;
                            // payment.StreetAddress2 = billingAddress.StreetAddress2;
                            // payment.City = billingAddress.City;
                            // payment.State = billingAddress.State;
                            // payment.Zip = billingAddress.Zip;
                            // payment.AddressCountry = billingAddress.Country;
                            // payment.ApplicationUser = user;
                            
                            
                            Shipment shipment = new Shipment(shippingService,shippingAddress);
                            
                            // shipment.ShippingPrice = shippingService.Price;
                            // shipment.ShippingServiceId = shippingService.ShippingServiceId;
                            // shipment.FirstName = shippingAddress.FirstName;
                            // shipment.LastName = shippingAddress.LastName;
                            // shipment.StreetAddress1 = shippingAddress.StreetAddress1;
                            // shipment.StreetAddress2 = shippingAddress.StreetAddress2;
                            // shipment.City = shippingAddress.City;
                            // shipment.State = shippingAddress.State;
                            // shipment.Zip = shippingAddress.Zip;
                            // shipment.Country = shippingAddress.Country;
                            // shipment.Mobile = shippingAddress.Mobile;
                            // shipment.Email = shippingAddress.Email;


                            foreach (var item in cartItems)
                            {
                                OrderItem orderItem = new OrderItem(item);
                                // orderItem.ProductId = item.ProductId;
                                // orderItem.Quantity = item.Quantity;
                                // orderItem.Price = item.Product.Price;
                                // orderItem.DiscountedPrice = item.Product.SalesPrice;
                                orderItems.Add(orderItem);
                                _unitOfWork.CartItem.Remove(item);
                            }

                            
                            Order order = new Order(cart,orderItems,user,payment,shipment);
                            // order.Coupon = cart.Coupon != null ? cart.Coupon : null;
                            // order.SubTotal = cart.SubtTotal;
                            // order.DiscountAmount = cart.Discount;
                            // order.Total = cart.Total;
                            // order.OrderItems = orderItems;
                            // order.ApplicationUser = user;
                            // order.Payment = payment;
                            // order.Shipment = shipment;
                            order.Coupon.UsedBefore += 1;
                            _unitOfWork.Order.Add(order);
                            Response.Cookies.Delete("Cart");
                            _unitOfWork.Cart.Remove(cart);
                            var re = _unitOfWork.Complete();
                            Console.WriteLine(re);
                            Response.StatusCode = 200;
                            Response.WriteAsJsonAsync(new
                                { result = "redirect", redirect = Url.Action("ThankYou", "CheckOut") });

                            return new EmptyResult();


                            //create new order items from each cart item and delete each converted cart item
                            //create new order from cart and delete cart from database and cookie
                            //assign payment and items to order
                            //save changes to database
                            //redirect to thank you page
                        }
                        else
                        {
                            Response.StatusCode = 400;
                            Response.WriteAsJsonAsync(new { result = "paymenterror", msg = result.Charge.Status });
                            return new EmptyResult();
                        }
                    }
                    else
                    {
                        Response.StatusCode = 400;
                        Response.WriteAsJsonAsync(new { result = "paymenterror", msg = result.Exception.Message });
                        return new EmptyResult();
                    }
                }

                Response.StatusCode = 400;
                Response.WriteAsJsonAsync(new { result = "redirect", redirect = Url.Action("Index", "Home") });
                return new EmptyResult();
            }
        }

        Response.StatusCode = 400;
        var error = ModelState.ToDictionary(k => k.Key, v => v.Value.Errors.ToArray());
        Response.WriteAsJsonAsync(new { result = "notvalid", msgs = error });
        return new EmptyResult();
    }

    [HttpGet("/thanyou")]
    public IActionResult ThankYou()
    {
        return View();
    }
}