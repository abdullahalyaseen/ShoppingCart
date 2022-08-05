using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Web.Areas.Management.ViewModels.Categorys;
using ShoppingCart.DataAccess.Interfaces;
using ShoppingCart.Models;
using Microsoft.AspNetCore.Authorization;
using ShoppingCart.Utilities.files;
using ShoppingCart.Utilities.Url;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingCart.Web.Areas.Management.Controllers
{
    public class CategoryController : ManagementController
    {
        private readonly IWebHostEnvironment _env;
        private FileUpload _FileUpload;
        private IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork, IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _env = env;
        }
        // GET: /<controller>/
        [HttpGet]
        [Authorize(policy: "view-categories")]
        public IActionResult Index()
        {
            var categories = _unitOfWork.Category.GetAll();
            return View(categories);
        }

        [HttpGet]
        [Authorize(policy: "add-category")]

        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(policy: "add-category")]
        public IActionResult AddCategory(AddCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                string imagepath = null;
                if (model.Image != null)
                {
                    if (model.Image.IsImage())
                    {
                        string uniqueFileName = FilePathGenerator.GeneraterPath(model.Image, "Images/categories", model.CategoryName, keepName: false);

                        using (FileUpload uploader = new FileUpload(_env, model.Image, uniqueFileName))
                        {
                            uploader.Upload();
                            imagepath = uniqueFileName;
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", "Only Images are allowd");
                        return View(model);
                    }

                }
                Category category = new Category
                {
                    Name = model.CategoryName,
                    ImageUrl = imagepath
                };
                _unitOfWork.Category.Add(category);
                _unitOfWork.Complete();
                return RedirectToAction("Index", "Category");
            }

            else
            {
                return View("AddCategory", model);
            }
        }

        [HttpGet]
        [Authorize(policy: "edit-category")]

        public IActionResult EditCategory(int? Id)
        {
            if (Id == null)
            {
                return RedirectToAction("Index", "Category");
            }
            Category? category = _unitOfWork.Category.Get((int)Id);
            if (category == null)
            {
                return RedirectToAction("Index", "Category");
            }
            EditCategoryViewModel editModel = new EditCategoryViewModel
            {
                CategoryName = category.Name,
                Image = category.ImageUrl,
                Id = category.CategoryId
            };
            return View(editModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(policy: "edit-category")]
        public IActionResult EditCategory(EditCategoryViewModel Model)
        {
            if (ModelState.IsValid)
            {
                var category = _unitOfWork.Category.Get(Model.Id);
                category.Name = Model.CategoryName;
                if (Model.NewImage != null)
                {
                    if (Model.NewImage.IsImage())
                    {
                        string newImage = FilePathGenerator.GeneraterPath(Model.NewImage, "Images/categories", Model.CategoryName, keepName: false);

                        using (FileUpload uploader = new FileUpload(_env, Model.NewImage, newImage))
                        {
                            uploader.Upload();
                            using (FileDelete fileDelete = new FileDelete(_env, category.ImageUrl))
                            {
                                fileDelete.Delete();
                            }
                        }

                        category.ImageUrl = newImage;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Only images are allowd");
                        Model.Image = category.ImageUrl;
                        return View(Model);
                    }
                }

                _unitOfWork.Complete();
                return RedirectToAction("Index", "Category");
            }
            else
            {
                return View(Model);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(policy: "delete-category")]

        public IActionResult DeleteCategory(int Id)
        {
            var category = _unitOfWork.Category.Get(Id);
            _unitOfWork.Category.Remove(category);
            _unitOfWork.Complete();
            return RedirectToAction("Index", "Category");

        }
    }
}

