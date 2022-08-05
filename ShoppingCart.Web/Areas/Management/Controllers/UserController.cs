using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ShoppingCart.Models;
using ShoppingCart.DataAccess.Interfaces;
using ShoppingCart.Web.Areas.Management.ViewModels.User;
using Microsoft.AspNetCore.Identity;

namespace ShoppingCart.Web.Areas.Management.Controllers
{
    public class UserController : ManagementController
    {
        private IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }


        [HttpGet]
        [Authorize(Policy = "view-users")]
        public IActionResult Index()
        {
            var users = _unitOfWork.ApplicationUser.Find(a => a.IsCustomer == false);

            return View(users);
        }


        [HttpGet]
        [Authorize(Policy = "add-user")]
        public IActionResult AddUser()
        {
            return View();
        }


        [HttpPost]
        [Authorize(Policy = "add-user")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(AddUserViewModel Model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser User = new ApplicationUser
                {
                    FirstName = Model.FirstName,
                    LastName = Model.LastName,
                    Email = Model.Email,
                    UserName = Model.Email,
                    PhoneNumber = Model.CountryCode + Model.MobileNumber,
                    IsCustomer = false
                };
                var result = await _userManager.CreateAsync(User, Model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError("", err.ToString());
                    }
                }
            }

            return View(Model);


        }

        [HttpGet]
        [Authorize(Policy = "edit-user")]
        public IActionResult EditUser(int? Id)
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "edit-user")]
        [ValidateAntiForgeryToken]
        public IActionResult EditUser(EditUserViewModel model)
        {
            return RedirectToAction("Index", "User");
        }

        [HttpPost]
        [Authorize(Policy = "delete-user")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteUser(int id)
        {
            ApplicationUser User = _unitOfWork.ApplicationUser.Get(id);
            if(User != null){
               var result = _userManager.DeleteAsync(User);
            }
            return RedirectToAction("Index", "User");
        }

    }
}