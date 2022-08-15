using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ShoppingCart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ShoppingCart.Web.ViewModels;
using ShoppingCart.Utilities.Url;
using Microsoft.AspNetCore.Authorization;
using ShoppingCart.DataAccess.Interfaces;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingCart.Web.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly SignInManager<ApplicationUser> _SignInManager;
        private IUnitOfWork _unitOfWork;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUnitOfWork unitOfWork)
        {

            _UserManager = userManager;
            _SignInManager = signInManager;
            _unitOfWork = unitOfWork;
        }
        // GET: /SignUp
        [HttpGet("/signup")]
        public IActionResult SignUp()
        {
            if(User.Identity.IsAuthenticated){
                return RedirectToAction("Index","Home");
            }
            return View();
        }

        //POST: /signup
        [HttpPost("/signup")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpViewModel Model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    FirstName = Model.FirstName,
                    LastName = Model.LastName,
                    UserName = Model.Email,
                    Email = Model.Email,
                    PhoneNumber = Model.CountryCode + Model.MobileNumber,
                    IsCustomer = true
                };
                var result = await _UserManager.CreateAsync(user, Model.Password);
                if (result.Succeeded)
                {
                    await _UserManager.AddToRoleAsync(user, BuiltInRoles.customer);
                    await _SignInManager.SignInAsync(user, isPersistent: false);
                    MergeCarts(Model.Email);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var err in result.Errors)
                    {
                        /// TODO: BIND EACH ERROR WITH ITS CONTROLLER
                        ModelState.AddModelError("", err.Description);
                    }
                }
            }

            return View("SignUp", Model);


        }

        [HttpPost("/signout")]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await _SignInManager.SignOutAsync();
            Response.Cookies.Delete("Cart");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("/signin")]
        public IActionResult SignIn()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }


        [HttpPost("/signin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInViewModel Model)
        {
            if (ModelState.IsValid)
            {
                var result = await _SignInManager.PasswordSignInAsync(Model.Email, Model.Password, false, false);
                if (result.Succeeded)
                {
                    MergeCarts(Model.Email);
                    if (!string.IsNullOrEmpty(Model.ReturnUrl) && Url.IsLocalUrl(Model.ReturnUrl))
                    {
                        return LocalRedirect(Model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    
                }
                else
                {
                    ModelState.AddModelError("", "Wrong Email or Password");
                    return View("SignIn", Model);

                }

                
            }
            return View("SignIn", Model);
        }

        [HttpGet("/accessdenied")]
        public IActionResult AccessDenied(){
            return View();
        }


        private void MergeCarts(string email)
        {
            string cartIdFromCookie;
            bool IsCartInCookie = Request.Cookies.TryGetValue("Cart", out cartIdFromCookie);
            
            var cartFromCookie = IsCartInCookie ? _unitOfWork.Cart.GetWith(c=>c.CartId == Guid.Parse(cartIdFromCookie),"CartItems") : null;
            var user = _unitOfWork.ApplicationUser.GetWith(u => u.UserName == email, "Cart");
            if (cartFromCookie != null)
            {
                if (user.Cart != null)
                {
                    var CookieCartItems = _unitOfWork.CartItem.Find(i => i.CartId == Guid.Parse(cartIdFromCookie),"Product");
                    var UserCartItems = _unitOfWork.CartItem.Find(i => i.CartId == user.Cart.CartId,"Product");
                    foreach (var cookieItem in CookieCartItems)
                    {
                        foreach (var userItem in UserCartItems)
                        {
                            if (cookieItem.ProductId == userItem.ProductId)
                            {
                                cookieItem.Quantity += userItem.Quantity;
                                _unitOfWork.CartItem.Remove(userItem);
                            }
                        }
                        cookieItem.CartId = user.Cart.CartId;
                    }
                    Response.Cookies.Delete("Cart");
                    _unitOfWork.Cart.Remove(cartFromCookie);
                    CookieOptions options = new CookieOptions();
                    options.MaxAge = TimeSpan.MaxValue;
                    Response.Cookies.Append("Cart", user.Cart.CartId.ToString(), options);
                    _unitOfWork.Complete();
                }
                else
                {
                    user.Cart = cartFromCookie;
                    _unitOfWork.Complete();
                }
            }
            else
            {
                if (user.Cart != null)
                {
                    CookieOptions options = new CookieOptions();
                    options.MaxAge = TimeSpan.MaxValue;
                    Response.Cookies.Append("Cart", user.Cart.CartId.ToString(), options);
                }
            }
            
            

            
        }
    }
}

 