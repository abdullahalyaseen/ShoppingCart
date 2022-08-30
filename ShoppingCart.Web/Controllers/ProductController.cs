using System;
using System.Diagnostics;
using ShoppingCart.DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Utilities.Url;
using ShoppingCart.Web.ViewModels.Products;
using ShoppingCart.Web.Services.ProductServices;
using ShoppingCart.Models;

namespace ShoppingCart.Web.Controllers
{
    public class ProductController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{id}")]
        public IActionResult Product(string id)
        {
            int productId = ProductUrl.GetProductId(id);
            var product = _unitOfWork.Product.GetWith(p => p.ProductId == productId, "Images,Tags");
            if (product != null)
            {
                var reviews = _unitOfWork.Review.Find(r => r.ProductId == product.ProductId, "ApplicationUser");
                List<string> gallery = new List<string>();
                gallery.Add(product.MainImage);
                foreach (var image in product.Images)
                {
                    gallery.Add(image.Url);
                }

                ProductViewModel Model = new ProductViewModel();
                Model.Id = product.ProductId;
                Model.Title = product.Title;
                Model.Price = product.Price / 100.0;
                Model.SalesPrice = product.SalesPrice / 100.0;
                Model.Quantity = product.Quantity;
                Model.Sku = product.Sku;
                Model.Description = product.Description.Split(Environment.NewLine).ToList();
                Model.ShortDescription = product.ShortDescription;
                Model.Tags = product.Tags;
                Model.Gallery = gallery;
                Model.Rating = ProductServices.CalculateRating(reviews);
                Model.NumberOfReview = reviews.Any() ? reviews.Count() : 0;
                Model.Reviews = reviews;
                return View(Model);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("/search")]
        public IActionResult Search(string q,int category, double min, double max,string rate,string sortBy)
        {
            if (q != null)
            {
                var rateList =rate != null ? rate.Split('-').ToList() : null;
                
                var result = _unitOfWork.Product.Search(q,categoryid: category,lowerPrice:(int)min*100,upperPrice:(int)max*100,rating:rateList,orderby:sortBy);
                return View(result);
            }

            return RedirectToAction("Index", "Home");
        }
        //TODO: For Development only
        [HttpGet]
        public IActionResult ListProducts()
        {
            var Products = _unitOfWork.Product.Find(includeProperties:"Reviews");
            return View(Products);
        }
    }
}