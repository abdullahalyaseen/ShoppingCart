using Microsoft.AspNetCore.Mvc;
using ShoppingCart.DataAccess.Interfaces;
using ShoppingCart.Models;
using ShoppingCart.Web.Areas.Management.ViewModels.ShippingService;

namespace ShoppingCart.Web.Areas.Management.Controllers;

public class ShippingServiceController : ManagementController
{
    private IUnitOfWork _unitOfWork;

    public ShippingServiceController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult AddShippingService()
    {
        ShippingServiceViewModel model = new ShippingServiceViewModel();
        return View(model);
    }

    [HttpPost]
    public IActionResult AddShippingService(ShippingServiceViewModel model)
    {
        if (ModelState.IsValid)
        {
            ShippingService service = new ShippingService
            {
                Name = model.Name,
                Price = (int)(model.Price * 100),
                Duration = model.Duration,
                Descrption = model.Description
            };
            _unitOfWork.ShippingServices.Add(service);
            _unitOfWork.Complete();
            return RedirectToAction("Index", "ShippingService", new { area = "Management" });

        }

        return View(model);
    }
}