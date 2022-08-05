using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ShoppingCart.Models;
using ShoppingCart.DataAccess.Interfaces;
using ShoppingCart.Web.Areas.Management.ViewModels.Tags;
namespace ShoppingCart.Web.Areas.Management.Controllers
{
    public class TagController : ManagementController
    {
        private IUnitOfWork _unitOfWork;

        public TagController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Authorize(Policy = "view-tags")]
        public IActionResult Index()
        {
            var Tags = _unitOfWork.Tag.GetAll();

            return View(Tags);
        }
        [HttpGet]
        [Authorize(Policy = "add-tag")]
        public IActionResult AddTag()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Policy = "add-tag")]
        public IActionResult AddTag(AddTagViewModel Model)
        {

            if (ModelState.IsValid)
            {
                var checkDuplication = _unitOfWork.Tag.Find(t => t.Name == Model.Name).FirstOrDefault();
                if (checkDuplication == null)
                {
                    Tag tag = new Tag
                    {
                        Name = Model.Name
                    };
                    _unitOfWork.Tag.Add(tag);
                    _unitOfWork.Complete();
                    return RedirectToAction("Index", "Tag", new { area = "Management" });
                }
                else
                {
                    ModelState.AddModelError("Name", "Duplicate Tag Name");
                    return View(Model);
                }

            }
            return View(Model);
        }

        [HttpGet]
        [Authorize(Policy = "edit-tag")]
        public IActionResult EditTag()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Policy = "edit-tag")]
        public IActionResult EditTag(int Id)
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "delete-tag")]
        public IActionResult DeleteTag()
        {
            return View();
        }

    }
}