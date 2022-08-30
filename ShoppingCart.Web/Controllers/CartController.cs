using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.DataAccess.Interfaces;
using ShoppingCart.Models;
using ShoppingCart.Web.ViewModels.Cart;


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

    [HttpGet("/cart")]
    public IActionResult Index()
    {
        //Getting cart id from cookie 
        string cartId;
        Request.Cookies.TryGetValue("Cart", out cartId);
        if (cartId != null)
        {
            
            var cartItems =
                _unitOfWork.CartItem.Find(i => i.CartId == Guid.Parse(cartId), includeProperties: "Product");
            if (cartItems.Any())
            {

                // _unitOfWork.Complete();
                string coupon = null;
                var cart = _unitOfWork.Cart.GetWith(c=>c.CartId == Guid.Parse(cartId),"Coupon");
                if (cart.Coupon != null)
                {
                    coupon = cart.Coupon.Code;
                }
                CartViewModel cartViewModel = new CartViewModel();
                cartViewModel.CartItems = cartItems;
                double subtotal;
                double total;
                double discount;
                var result = CalculateCart(cartItems, out subtotal, out total, out discount, coupon);
                cartViewModel.CouponCode = coupon;
                cartViewModel.Total = total;
                cartViewModel.SubTotal = subtotal;
                cartViewModel.Discount = discount;
                return View(cartViewModel);
            }
            //check if there is cart cookie but no items in database
            if (cartId.Any() && !cartItems.Any())
            {
                Response.Cookies.Delete("Cart");
            }
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddCartItem([FromForm] int productId, [FromForm] int quantity)
    {
        //Add Product To Cart
        CartItemMessaging result = _AddCartItem(productId, quantity);


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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Increase(int CartItemId)
    {
        CartItemMessaging result = IncreaseCartItemQuantity(CartItemId, 1);
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Decrease(int CartItemId)
    {
        CartItemMessaging result = DecreaseCartItemQuantity(CartItemId, 1);
        if (result.Status == MessagingStatus.Ok)
        {
            Response.StatusCode = 202;
            Response.WriteAsJsonAsync(new { Result = "Removed" });
            return new EmptyResult();
        }

        Response.StatusCode = 400;
        return new EmptyResult();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteCartItem(int cartItemId)
    {
        string result = RemoveCartItem(cartItemId);
        if (result != "")
        {
            Response.StatusCode = 202;
            Response.WriteAsJsonAsync(new { Result = "Ok!" });
            return new EmptyResult();
        }

        Response.StatusCode = 400;
        return new EmptyResult();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ReCalculateCart(string CouponCode)
    {
        string CartId;
        Request.Cookies.TryGetValue("Cart", out CartId);
        if (CartId != null)
        {
            IEnumerable<CartItem> cartItems = _unitOfWork.CartItem.Find(i => i.CartId == Guid.Parse(CartId), "Product");
            if (cartItems != null)
            {
                double total;
                double subtotal;
                double discount;
                string CouponMsg;
                CouponMessaging result = CalculateCart(cartItems, out subtotal, out total, out discount, CouponCode);
                var cart = _unitOfWork.Cart.GetWith(c=>c.CartId == Guid.Parse(CartId), "Coupon");
                if (cart.CartId != null && result.Status == CouponMessagingStatus.NoCoupon)
                {
                    cart.CouponId = null;
                    _unitOfWork.Complete();
                }
                if (result.Status == CouponMessagingStatus.Ok)
                {
                    cart.Coupon = result.Coupon;
                    _unitOfWork.Complete();
                }
                switch (result.Status)
                {
                    case CouponMessagingStatus.Expired:
                        CouponMsg = "Coupon code has expired";
                        break;
                    case CouponMessagingStatus.Invalid:
                        CouponMsg = "Invalid coupon code";
                        break;
                    case CouponMessagingStatus.UnderThreshold:
                        CouponMsg = "Minimum order amount must be $ " + result.Threshold;
                        break;
                    default:
                        CouponMsg = "ok";
                        break;
                }

                Response.StatusCode = 202;
                Response.WriteAsJsonAsync(new
                    { total = total, subtotal = subtotal, discount = discount, CouponMsg = CouponMsg });
                return new EmptyResult();
            }
        }

        Response.StatusCode = 400;
        return new EmptyResult();
    }

    [HttpGet("/cart-validation")]
    [Authorize]
    public IActionResult CartValidation()
    {
        string CartId;
        Request.Cookies.TryGetValue("Cart", out CartId);
        if (CartId != null)
        {
            var cart = _unitOfWork.Cart.GetWith(c=>c.CartId == Guid.Parse(CartId),"Coupon");
            var cartItems = _unitOfWork.CartItem.Find(i => i.CartId == Guid.Parse(CartId), "Product");
            if (cart != null && cartItems != null)
            {
                bool allProductsAvailable = true;
                //check products availability 
                foreach (var item in cartItems)
                {
                    var checkResult =  _unitOfWork.Product.CheckProductAvailability(item.ProductId, item.Quantity);
                    if (checkResult.Key == false)
                    {
                        allProductsAvailable = false;
                        //if product not found or the product quantity is 0 then remove the cartItem
                        if (checkResult.Value == -1 || checkResult.Value == 0)
                        {
                            _unitOfWork.CartItem.Remove(item);
                        }
                        //if the product quantity id less than cartItem quantity than decrease the cart item quantity to tha maximum available quantity
                        if (checkResult.Value > 0)
                        {
                            item.Quantity = checkResult.Value;
                        }
                    }
                }

                if (!allProductsAvailable)
                {
                    _unitOfWork.Complete();
                    return RedirectToAction("Index", "Cart");
                }
                
                double subtotal;
                double total;
                double discount;
                string couponCode = null;
                if (cart.Coupon != null)
                {
                    couponCode = cart.Coupon.Code;
                }
                
                
                CouponMessaging result = CalculateCart(cartItems, out subtotal, out total, out discount, couponCode);

                cart.SubtTotal = (int)(subtotal * 100);
                cart.Total = (int)(total * 100);
                cart.Discount = (int)(discount * 100);
                cart.LastValidationAt =  DateTime.Now;


                    _unitOfWork.Complete();
                    return RedirectToAction("Index", "CheckOut");

            }
        }

        return RedirectToAction("Index", "Home");
    }
    
    


    private CouponMessaging CalculateCart(IEnumerable<CartItem> cartItems, out double subTotal, out double total,
        out double discount, string CouponCode = null)
    {
        CouponMessaging messenger = new CouponMessaging();
        int SubTotal = 0;
        int Total = 0;

        foreach (var item in cartItems)
        {
            SubTotal = SubTotal + (item.Product.Price * item.Quantity);
            if (item.Product.SalesPrice > 0)
            {
                Total = Total + (item.Product.SalesPrice * item.Quantity);
            }
            else
            {
                Total = Total + (item.Product.Price * item.Quantity);
            }
        }

        subTotal = Math.Round(SubTotal / 100.0,2);
        total = Math.Round(Total / 100.0,2);
        messenger.Status = CouponMessagingStatus.NoCoupon;
        if (CouponCode != null)
        {
            Coupon coupon = _unitOfWork.Coupon.GetWith(c => c.Code == CouponCode);
            if (coupon != null)
            {
                if (!(coupon.SingleUse && coupon.UsedBefore > 0))
                {
                    if (subTotal >= coupon.Threshold)
                    {
                        messenger.Status = CouponMessagingStatus.Ok;
                        messenger.Coupon = coupon;
                        if (coupon.Type == "Fixed")
                        {
                            total = Math.Round(total - coupon.Amount,2);
                        }

                        if (coupon.Type == "Percentage")
                        {
                            if (total * (coupon.Amount / 100.0) <= coupon.MaxDiscount)
                            {
                                total = Math.Round(total - total * (coupon.Amount / 100.0),2);
                            }
                            else
                            {
                                total = total - coupon.MaxDiscount;
                            }
                        }
                    }
                    else
                    {
                        messenger.Status = CouponMessagingStatus.UnderThreshold;
                        messenger.Threshold = coupon.Threshold;
                    }
                }
                else
                {
                    messenger.Status = CouponMessagingStatus.Expired;
                }
            }
            else
            {
                messenger.Status = CouponMessagingStatus.Invalid;
            }
        }

        discount = Math.Round(((subTotal - total) / subTotal) * 100, 2);
        return messenger;
    }

    private CartItemMessaging _AddCartItem(int productId, int quantity)
    {
        //instantiate new CartItemMessaging object
        CartItemMessaging massenger = new CartItemMessaging();

        {
            //if null means no cartitem for this product & this cart
            //instantiate cart item for the product
            var productStatus = _unitOfWork.Product.CheckProductAvailability(productId, quantity);
            if (productStatus.Key.Equals(true))
            {
                //Get Cart Id
                Guid CartId = GetCustomerCartId();
                //TODO: write code if this cart contains same product just increase quantity
                //Check if there is a cartitem for this product for this cart
                CartItem cartItem = _unitOfWork.CartItem.CheckCartItemExistence(productId, CartId);
                if ((cartItem == null))
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
                else
                {
                    //TODO create increase method and use it here
                    return IncreaseCartItemQuantity(cartItem.CartItemId, quantity);
                }
            }

            if (productStatus.Key.Equals(false))
            {
                if (productStatus.Value.Equals(0))
                {
                    //TODO remove CartItem if exist in database
                    string cartId;
                    Request.Cookies.TryGetValue("Cart", out cartId);
                    if (cartId != null)
                    {
                        CartItem cartItem = _unitOfWork.CartItem.CheckCartItemExistence(productId, Guid.Parse(cartId));
                        if (cartItem != null)
                        {
                            RemoveCartItem(cartItem.CartItemId);
                        }
                    }

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
                    RemoveCartItem(cartItemId);
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

    private CartItemMessaging DecreaseCartItemQuantity(int cartItemId, int decrement)
    {
        //get the cart item 
        CartItem cartItem = _unitOfWork.CartItem.GetWith(i => i.CartItemId == cartItemId, "Product");
        //check it the cart item exist in database
        if (cartItem != null)
        {
            //if the cart item exist:
            CartItemMessaging messenger = new CartItemMessaging();
            //calculate the final quantity
            int totalQuantity = cartItem.Quantity - decrement;
            //check the quantity more than ZERO

            if (totalQuantity > 0)
            {
                //Then Decrease the quantity
                cartItem.Quantity = totalQuantity;
                _unitOfWork.Complete();
                messenger.Status = MessagingStatus.Ok;
                return messenger;
            }
            else
            {
                RemoveCartItem(cartItemId);
                messenger.Status = MessagingStatus.Ok;
                return messenger;
            }
        }

        return null;
    }


    private string RemoveCartItem(int cartItemId)
    {
        CartItem cartItem = _unitOfWork.CartItem.Get(cartItemId);
        if (cartItem != null)
        {
            Cart cart = _unitOfWork.Cart.GetWith(c => c.CartId == cartItem.CartId, "CartItems");
            if (cart.CartItems.Count == 1)
            {
                DeleteCart(cart.CartId);
                return "WithCart";
            }
            else
            {
                _unitOfWork.CartItem.Remove(cartItem);
                _unitOfWork.Complete();
                return "WithOutCart";
            }
        }

        return "";
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


    private bool DeleteCart(Guid cartId)
    {
        Cart cart = _unitOfWork.Cart.GetWith(c => c.CartId == cartId, "CartItems");
        if (cart != null)
        {
            foreach (var item in cart.CartItems)
            {
                _unitOfWork.CartItem.Remove(item);
            }

            _unitOfWork.Cart.Remove(cart);
            _unitOfWork.Complete();
            Response.Cookies.Delete("Cart");
            return true;
        }

        return false;
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

    private class CouponMessaging
    {
        public CouponMessagingStatus Status { get; set; }
        public int? Threshold { get; set; }

        public Coupon Coupon;
    }

    private enum CouponMessagingStatus
    {
        Ok,
        Expired,
        UnderThreshold,
        Invalid,
        NoCoupon
    }
}