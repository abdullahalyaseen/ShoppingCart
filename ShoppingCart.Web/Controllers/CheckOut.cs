using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.DataAccess;
using ShoppingCart.DataAccess.Interfaces;
using ShoppingCart.Models;
using ShoppingCart.Web.Services;
using ShoppingCart.Web.ViewModels.Checkout;

namespace ShoppingCart.Web.Controllers;

[Authorize]
public class CheckOut : Controller
{
    private IUnitOfWork _unitOfWork;
    private UserManager<ApplicationUser> _userManager;

    public CheckOut(IUnitOfWork unitOfWork,UserManager<ApplicationUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    [HttpGet("/checkout")]
    public IActionResult Index()
    {
        CheckOutViewModel model = new CheckOutViewModel();
        var user = _userManager.GetUserAsync(User).Result;
        var addresses = _unitOfWork.Address.Find(a => a.ApplicationUserId == user.Id);
        var shippingServices = _unitOfWork.ShippingServices.GetAll();
        var cart = _unitOfWork.Cart.GetWith(c => c.ApplicationUserId == user.Id);
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult PlaceOrder(CheckOutViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (model.ShippingServiceId == null)
            {
                return RedirectToAction("Index", "CheckOut");
            }
            string cartId;
            Request.Cookies.TryGetValue("Cart",out cartId);
            if (cartId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var cart = _unitOfWork.Cart.Get(Guid.Parse(cartId));
                if (cart != null)
                {
                    var timeForLastValidation = DateTime.Now - cart.LastValidationAt;
                    TimeSpan maximumAllowedValidationAge = TimeSpan.FromMinutes(5);
                    if (timeForLastValidation > maximumAllowedValidationAge)
                    {
                        return RedirectToAction("Index", "Cart");
                    }
                    var shippingService = _unitOfWork.ShippingServices.Get(model.ShippingServiceId);
                    int finalCaptureValue = shippingService.Price + cart.Total;

                    var result = PaymentServices.PayAsync(model.CardNumber, model.Month, model.Year, model.Cvc, finalCaptureValue).Result;
                    return Content(result);
                }
                return RedirectToAction("Index", "Home");
            }
            
        }

        return RedirectToAction("Index","CheckOut");
    }
}