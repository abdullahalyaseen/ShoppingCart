using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.DataAccess.Interfaces;
using ShoppingCart.DataAccess.Repositories;
using ShoppingCart.Models;
using ShoppingCart.Web.ViewModels;

namespace ShoppingCart.Web.Controllers;

[Authorize]
public class AddressController : Controller
{
    private IUnitOfWork _unitOfWork;

    private UserManager<ApplicationUser> _userManager;

    public AddressController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult AddAddress()
    {
        AddAddressViewModel model = new AddAddressViewModel();
        return View(model);
    }

    [HttpPost]
    public IActionResult AddAddress(AddAddressViewModel model)
    {
        if (ModelState.IsValid)
        {
            bool result = _AddNewAddress(model);
            if (result)
            {
                return RedirectToAction("Index", "Home");
            }
        }
        

        return View(model);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddAddressAjax(AddAddressViewModel model)
    {
        bool result = _AddNewAddress(model);
        if (result)
        {
            Response.StatusCode = 200;
            Response.WriteAsJsonAsync(new { result = "ok" });
            return new EmptyResult();
        }

        Response.StatusCode = 400;
        return new EmptyResult();
    }

    private bool _AddNewAddress(AddAddressViewModel model)
    {
        var user = _userManager.GetUserAsync(User).Result;

        Address address = new Address
        {
            AddressLabel = model.AddressLabel,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Mobile = model.Mobile,
            StreetAddress1 = model.StreetAddress1,
            StreetAddress2 = model.StreetAddress2,
            City = model.City,
            State = model.State,
            Zip = model.Zip,
            Country = model.Country
        };

        address.ApplicationUser = user;
        _unitOfWork.Address.Add(address);
        int result = _unitOfWork.Complete();
        if (result > 0)
        {
            return true;
        }

        return false;
    }
}