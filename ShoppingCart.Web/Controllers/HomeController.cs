using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Web.Models;
using ShoppingCart.DataAccess.Interfaces;
using ShoppingCart.DataAccess;
using ShoppingCart.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ShoppingCart.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<ApplicationUser> _UserManager;
    public HomeController(ILogger<HomeController> logger,UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _UserManager = userManager;
    }
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        var user = User;
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

