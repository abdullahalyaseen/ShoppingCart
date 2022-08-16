using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Web.Models;
using ShoppingCart.DataAccess.Interfaces;
using ShoppingCart.DataAccess;
using ShoppingCart.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Stripe.Issuing;

namespace ShoppingCart.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<ApplicationUser> _UserManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ShoppingCartContext _context;

    public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork, ShoppingCartContext context)
    {
        _logger = logger;
        _UserManager = userManager;
        _unitOfWork = unitOfWork;
        _context = context;
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

    [HttpGet("/setup")]
    public IActionResult Setup()
    {
        var user = _unitOfWork.ApplicationUser.Find(u => u.IsCustomer != true);
        if (!(user.Count() > 0))
        {
            ApplicationUser admin = new ApplicationUser
            {
                FirstName = "Admin",
                LastName = "Admin",
                Email = "admin@shoppingcart.com",
                UserName = "admin@shoppingcart.com",
                PhoneNumber = "00100000000",
                IsCustomer = false
            };

            _UserManager.CreateAsync(admin, "ShoppingCartAdmin@1");
            var newuser = _context.Users.Where(u => u.FirstName == "Admin").FirstOrDefault();
            
            Role adminRole = new Role
                { Id = 1, Name = BuiltInRoles.admin, NormalizedName = BuiltInRoles.admin.ToUpper() };
            
            _context.Roles.Add(adminRole);
            
            List<RoleClaim> roleClaims = new List<RoleClaim>
            {
                new RoleClaim { Id = 1, ClaimType = "view", ClaimValue = "management", RoleId = 1 },
                new RoleClaim { Id = 2, ClaimType = "view", ClaimValue = "categories", RoleId = 1 },
                new RoleClaim { Id = 3, ClaimType = "add", ClaimValue = "category", RoleId = 1 },
                new RoleClaim { Id = 4, ClaimType = "edit", ClaimValue = "category", RoleId = 1 },
                new RoleClaim { Id = 5, ClaimType = "delete", ClaimValue = "category", RoleId = 1 },
                new RoleClaim { Id = 6, ClaimType = "view", ClaimValue = "users", RoleId = 1 },
                new RoleClaim { Id = 7, ClaimType = "add", ClaimValue = "user", RoleId = 1 },
                new RoleClaim { Id = 8, ClaimType = "edit", ClaimValue = "user", RoleId = 1 },
                new RoleClaim { Id = 9, ClaimType = "delete", ClaimValue = "user", RoleId = 1 },
                new RoleClaim { Id = 10, ClaimType = "view", ClaimValue = "products", RoleId = 1 },
                new RoleClaim { Id = 11, ClaimType = "add", ClaimValue = "product", RoleId = 1 },
                new RoleClaim { Id = 12, ClaimType = "edit", ClaimValue = "product", RoleId = 1 },
                new RoleClaim { Id = 13, ClaimType = "delete", ClaimValue = "product", RoleId = 1 },
                new RoleClaim { Id = 14, ClaimType = "view", ClaimValue = "tags", RoleId = 1 },
                new RoleClaim { Id = 15, ClaimType = "add", ClaimValue = "tag", RoleId = 1 },
                new RoleClaim { Id = 16, ClaimType = "edit", ClaimValue = "tag", RoleId = 1 },
                new RoleClaim { Id = 17, ClaimType = "delete", ClaimValue = "tag", RoleId = 1 },
            };
            _context.RoleClaims.AddRange(roleClaims);
            
            ApplicationUserRole userRole = new ApplicationUserRole { RoleId = 1, UserId = newuser.Id };
            _context.ApplicationUserRoles.Add(userRole);
            _context.SaveChanges();
            return RedirectToAction("index", "Home");
        }

        return RedirectToAction("AccessDenied", "Account");
    }
}