using System;
using ShoppingCart.DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Utilities.Url;
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
            return View(product);
        }

        //TODO: For Development only
        [HttpGet]
        public IActionResult ListProducts()
        {
            var Products = _unitOfWork.Product.GetAll();
            return View(Products);
        }

    }
}