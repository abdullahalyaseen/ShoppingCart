using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.DataAccess.Interfaces;
using ShoppingCart.Models;


namespace ShoppingCart.Web.Controllers;

public class CartController : Controller
{
    private IUnitOfWork _unitOfWork;
    private UserManager<ApplicationUser> _userManager;

    public CartController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddCartItem([FromForm] int productId, [FromForm] int quantity)
    {
        //Get Cart Id
        Guid CartId = GetCustomerCartId();
        //Add Product To Cart
        CartItemMessaging result = AddCartItem(productId, quantity, CartId);


        if (result != null)
        {
            switch (result.Status)
            {
                // if status of response id Ok:
                case MessagingStatus.Ok:
                    Response.StatusCode = 202;
                    Response.WriteAsJsonAsync(new { Result = "Added" });
                    return new EmptyResult();
                // if status of response is failed because requested quantity larger than exist:
                case MessagingStatus.MoreThanAvailable:
                    Response.StatusCode = 400;
                    Response.WriteAsJsonAsync(new { Result = "Failed", max = result.Quantity });
                    return new EmptyResult();
                // if status of response is failed because quantity became 0:
                case MessagingStatus.ProductZero:
                    Response.StatusCode = 400;
                    Response.WriteAsJsonAsync(new { Result = 0 });
                    return new EmptyResult();
            }
        }

        //Unknown Error
        Response.StatusCode = 400;
        Response.WriteAsJsonAsync(new { Result = "Unknown" });
        return new EmptyResult();
    }

    private CartItemMessaging AddCartItem(int productId, int quantity, Guid CartId)
    {
        //instantiate new CartItemMessaging object
        CartItemMessaging massenger = new CartItemMessaging();
        //TODO: write code if this cart contains same product just increase quantity
        //Check if there is a cartitem for this product for this cart
        CartItem cartItem = _unitOfWork.CartItem.CheckCartItemExistence(productId, CartId);
        if (cartItem == null)
        {
            //if null means no cartitem for this product & this cart
            //instantiate cart item for the product
            var productStatus = _unitOfWork.Product.CheckProductAvailability(productId, quantity);
            if (productStatus.Key.Equals(true))
            {
                CartItem item = new CartItem();

                item.CartId = CartId;
                item.ProductId = productId;
                item.Quantity = quantity;
                _unitOfWork.CartItem.Add(item);
                _unitOfWork.Complete();
                massenger.Status = MessagingStatus.Ok;
                return massenger;
            }

            if (productStatus.Key.Equals(false))
            {
                if (productStatus.Value.Equals(0))
                {
                    //TODO remove CartItem if exist in database
                    massenger.Status = MessagingStatus.ProductZero;
                    return massenger;
                }

                if (productStatus.Value > 0)
                {
                    massenger.Status = MessagingStatus.MoreThanAvailable;
                    massenger.Quantity = productStatus.Value;
                    return massenger;
                }
            }
        }
        //if not null means there is cart item for this product & this cart so we need to increase quantity only
        else
        {
            //TODO create increase method and use it here
            return IncreaseCartItemQuantity(cartItem.CartItemId, quantity);
        }


        return null;
    }

    private CartItemMessaging IncreaseCartItemQuantity(int cartItemId, int increment)
    {
        //get the cart item 
        CartItem cartItem = _unitOfWork.CartItem.GetWith(i => i.CartItemId == cartItemId, "Product");
        //check it the cart item exist in database
        if (cartItem != null)
        {
            //if the cart item exist:
            CartItemMessaging messenger = new CartItemMessaging();
            //calculate the final quantity
            int totalQuantity = increment + cartItem.Quantity;
            //check product availability
            var result = _unitOfWork.Product.CheckProductAvailability(cartItem.ProductId, totalQuantity);
            if (result.Key.Equals(true))
            {
                //if the required quantity is available
                cartItem.Quantity = totalQuantity;
                _unitOfWork.Complete();
                messenger.Status = MessagingStatus.Ok;
                return messenger;
            }

            //if not available
            if (result.Key.Equals(false))
            {
                if (result.Value.Equals(0))
                {
                    //if the quantity is zero
                    messenger.Status = MessagingStatus.ProductZero;
                    return messenger;
                }

                if (result.Value > 0)
                {
                    //if the quantity id not zero but less than required
                    messenger.Status = MessagingStatus.MoreThanAvailable;
                    messenger.Quantity = result.Value;
                    return messenger;
                }
            }
        }

        return null;
    }

    private Guid GetCustomerCartId()
    {
        string CartId = null;
        //check if there is cart in cookies?
        if (!Request.Cookies.ContainsKey("Cart"))
        {
            //if no cart :
            //create cart
            string NewCartId = GenerateCart().Result;
            CartId = NewCartId;
            //Append the cart to cookie
            CookieOptions options = new CookieOptions();
            options.MaxAge = TimeSpan.MaxValue;
            Response.Cookies.Append("Cart", NewCartId, options);
        }
        else
        {
            Request.Cookies.TryGetValue("Cart", out CartId);
        }

        return Guid.Parse(CartId);
    }


    private async Task<string> GenerateCart()
    {
        string CartId = null;
        if (User.Identity.IsAuthenticated)
        {
            var user = await _userManager.GetUserAsync(User);
            Cart cart = new Cart();
            cart.ApplicationUser = user;
            _unitOfWork.Cart.Add(cart);
            CartId = cart.CartId.ToString();
            _unitOfWork.Complete();
        }
        else
        {
            Cart cart = new Cart();
            _unitOfWork.Cart.Add(cart);
            CartId = cart.CartId.ToString();
            _unitOfWork.Complete();
        }

        return CartId;
    }

    private class CartItemMessaging
    {
        public MessagingStatus Status { get; set; }
        public int? Quantity { get; set; }
    }

    private enum MessagingStatus
    {
        Ok,
        MoreThanAvailable,
        ProductZero
    }
}