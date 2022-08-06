using System;
using ShoppingCart.DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Utilities.Url;
using ShoppingCart.Web.ViewModels.Product;
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

        [HttpGet]
        public IActionResult Product(string Id)
        {
            int productId = ProductUrl.GetProductId(Id);
            var product = _unitOfWork.Product.GetWith(p=>p.ProductId == productId,"Images,Tags,Reviews");
            List<string> gallery = new List<string>();
            gallery.Add(product.MainImage);
            foreach(var image in product.Images){
                gallery.Add(image.Url);
            }
            ProductViewModel Model = new ProductViewModel();
            Model.Id = product.ProductId;
            Model.Title = product.Title;
            Model.Price = product.Price / 100.0;
            Model.SalesPrice = product.SalesPrice / 100.0;
            Model.Sku = product.Sku;
            Model.Description = product.Description;
            Model.ShortDescription = product.ShortDescription;
            Model.Tags = product.Tags;
            Model.Gallery = gallery;
            Model.Rating = CalculateRating(product.Reviews);
            Model.NumberOfReview = product.Reviews == null ? 0 : product.Reviews.Count();






            return View(Model);
        }

        //TODO: For Development only
        [HttpGet]
        public IActionResult ListProducts()
        {
            var Products = _unitOfWork.Product.GetAll();
            return View(Products);
        }


        private double CalculateRating(IEnumerable<Review> Reviews){
            double TotalRate = 0;
            if(Reviews.Count() != 0){
                foreach(var review in Reviews){
                TotalRate += review.Rate;
            }
            return TotalRate / Reviews.Count();
            }else{
                return 0;
            }
        }

    }
}