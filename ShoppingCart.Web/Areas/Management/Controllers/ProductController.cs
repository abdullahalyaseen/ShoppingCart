using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShoppingCart.Models;
using ShoppingCart.DataAccess.Interfaces;
using ShoppingCart.Web.Areas.Management.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore;
using ShoppingCart.Utilities.files;
using ShoppingCart.Utilities.Url;
using ShoppingCart.Web.Areas.Management.ViewModels.Products;
namespace ShoppingCart.Web.Areas.Management.Controllers
{

    public class ProductController : ManagementController
    {

        private IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _env = env;

        }

        [HttpGet]
        [Authorize(Policy = "view-products")]
        public IActionResult Index()
        {

            ProductIndexViewModel model = new ProductIndexViewModel();

            model.Products = _unitOfWork.Product.Find(includeProperties: "Category");

            return View(model);
        }


        [HttpGet]
        [Authorize(Policy = "add-product")]
        public IActionResult AddProduct()
        {
            var Categories = CategoriesList();
            var tags = _unitOfWork.Tag.GetAll();
            AddProductViewModel model = new AddProductViewModel();
            model.Categories = Categories;
            model.AllTags = tags;
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "add-product")]
        [ValidateAntiForgeryToken]
        public IActionResult SaveProduct(AddProductViewModel Model)
        {

            if (ModelState.IsValid)
            {
                if (Model.MainImage.IsImage())
                {
                    string mainImageurl = PrepairImageUrl(Model.MainImage);
                    ICollection<Image> Gallery;
                    Dictionary<IFormFile, string> GalleryForUpload = PrepairGallery(Model.Images, out Gallery);
                    Product product = new Product();
                    product.MainImage = mainImageurl;
                    product.Images = Gallery;
                    product.Title = Model.Title;
                    product.Price = ((int)(Model.Price * 100));
                    product.SalesPrice = ((int)(Model.SalePrice * 100));
                    product.CategoryId = Model.CategoryId;
                    product.ShortDescription = Model.ShortDescription;
                    product.Description = Model.Description;
                    product.Sku = Model.Sku;
                    product.Quantity = Model.Quantity;
                    product.Featured = Model.Featured;
                    product.Archived = Model.Archived;
                    product.AddAt = DateTime.Now;


                    if (Model.SelfTags != null)
                    {
                        product.Tags = LinkProductToTags(Model.SelfTags);
                    }

                    _unitOfWork.Product.Add(product);

                    int status = _unitOfWork.Complete();

                    if (status != 0)
                    {
                        UploadProductImage(Model.MainImage, mainImageurl);
                        UploadGallery(GalleryForUpload);
                    }

                    return Json(new { redirectToUrl = Url.Action("Index", "Product", new { area = "Management" }) });
                }
                else
                {
                    ModelState.AddModelError("MainImage", "Only Images are allowd");
                }

            }
            var err = ModelState.ToDictionary(k=>k.Key,v=>v.Value.Errors.Select(e=>e.ErrorMessage).ToArray());
            Response.StatusCode = 422;
            Response.WriteAsJsonAsync(err);
            return new EmptyResult();
        }


        [HttpGet]
        [Authorize(Policy = "view-products")]
        public IActionResult EditProduct(int? Id)
        {
            if (Id != null)
            {
                var product = _unitOfWork.Product.GetForEdit((int)Id);
                if (product != null)
                {
                    var tags = _unitOfWork.Tag.GetAll();
                    var Categories = _unitOfWork.Category.GetAll();

                    EditProductViewModel Model = new EditProductViewModel
                    {
                        Id = product.ProductId,
                        Title = product.Title,
                        Price = product.Price / 100.0,
                        SalesPrice = product.SalesPrice / 100.0,
                        CategoryId = product.CategoryId,
                        MainImage = product.MainImage,
                        ShortDescription = product.ShortDescription,
                        Description = product.Description,
                        Sku = product.Sku,
                        Quantity = product.Quantity,
                        Featured = product.Featured,
                        Archived = product.Archived,
                        AddedAt = product.AddAt,
                        Images = product.Images,
                        Categories = Categories,
                        AllTags = tags,
                        SelfTags = product.Tags.Select(t => t.TagId).ToList(),

                    };


                    return View(Model);
                }
            }

            return RedirectToAction("Index", "Product");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveEditProduct(EditProductViewModel Model)
        {
            if (ModelState.IsValid)
            {
                Product product = _unitOfWork.Product.GetWith(p => p.ProductId == Model.Id, "Tags");

                var AllTags = _unitOfWork.Tag.GetAll();


                string mainImageUrl = null;
                if (Model.NewImage != null && Model.NewImage.IsImage())
                {
                    mainImageUrl = PrepairImageUrl(Model.NewImage);
                }
                ICollection<Image> Gallery = null;
                Dictionary<IFormFile, string> GalleryForUpload = null;
                if (Model.NewGallery != null)
                {
                    GalleryForUpload = PrepairGallery(Model.NewGallery, out Gallery);
                }

                //Delete All Tags

                foreach (var tag in product.Tags)
                {
                    product.Tags.Remove(tag);
                }

                if (Model.SelfTags != null)
                {
                    product.Tags = LinkProductToTags(Model.SelfTags);
                }


                string OldImage = product.MainImage;
                product.Title = Model.Title;
                product.Price = ((int)(Model.Price * 100));
                product.SalesPrice = ((int)(Model.SalesPrice * 100));
                product.CategoryId = Model.CategoryId;
                product.ShortDescription = Model.ShortDescription;
                product.Description = Model.Description;
                product.Sku = Model.Sku;
                product.Quantity = Model.Quantity;
                product.Featured = Model.Featured;
                product.Archived = Model.Archived;
                if (mainImageUrl != null)
                {
                    product.MainImage = mainImageUrl;
                }
                if (Gallery != null)
                {
                    product.Images = Gallery;
                }
                product.ModifiedAt = DateTime.Now;

                int changes = _unitOfWork.Complete();
                if (changes != 0)
                {
                    if (mainImageUrl != null)
                    {
                        DeleteMainImage(OldImage);
                        UploadProductImage(Model.NewImage, mainImageUrl);
                    }
                    if (GalleryForUpload != null)
                    {
                        UploadGallery(GalleryForUpload);
                    }


                }
                return Json(new { redirectToUrl = Url.Action("Index", "Product", new { area = "Management" }) });



            }
            else
            {

            var err = ModelState.ToDictionary(k=>k.Key,v=>v.Value.Errors.Select(e=>e.ErrorMessage).ToArray());
            Response.StatusCode = 422;
            Response.WriteAsJsonAsync(err);
            return new EmptyResult();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteProduct(int id){
            var product = _unitOfWork.Product.GetWith(p=>p.ProductId == id,"Images,Tags");
            if(product != null){
                //TODO: Add logic to check if there is any orders related to this product
                _unitOfWork.Product.Remove(product);
                int ss = _unitOfWork.Complete();
                Response.StatusCode = 200;
                Response.WriteAsJsonAsync( new {redirectToUrl = Url.ActionLink("Index","Product",new {area = "Management"})});
                return new EmptyResult();
            }else{
                Response.StatusCode = 422;
                return new EmptyResult();
            }
        }







        private IEnumerable<Category> CategoriesList()
        {
            return _unitOfWork.Category.GetAll();
        }


        private Dictionary<IFormFile, string> PrepairGallery(IEnumerable<IFormFile> InputImages, out ICollection<Image> ImageModels)
        {
            ImageModels = new List<Image>();
            Dictionary<IFormFile, string> ReadyForUpload = new Dictionary<IFormFile, string>();

            if (InputImages != null)
            {
                foreach (var image in InputImages)
                {
                    if (image.IsImage())
                    {
                        string ImageUrl = FilePathGenerator.GeneraterPath(image, inRootPath: "Images/Products", keepName: false);
                        Image imageModel = new Image
                        {
                            Url = ImageUrl
                        };
                        ImageModels.Add(imageModel);
                        ReadyForUpload.Add(image, ImageUrl);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return ReadyForUpload;
        }


        private void UploadGallery(Dictionary<IFormFile, string> Files)
        {
            if (Files != null)
            {
                foreach (KeyValuePair<IFormFile, string> image in Files)
                {
                    using (FileUpload uploader = new FileUpload(_env, image.Key, image.Value))
                    {
                        uploader.Upload();
                    }
                }
            }
        }


        private string PrepairImageUrl(IFormFile image)
        {
            return FilePathGenerator.GeneraterPath(image, "Images/Products", keepName: false);
        }


        private void UploadProductImage(IFormFile File, string uploadPath)
        {
            if (File != null)
            {
                using (FileUpload uploader = new FileUpload(_env, File, uploadPath))
                {
                    uploader.Upload();
                }
            }

        }
        private void DeleteMainImage(string Url)
        {
            using (FileDelete delete = new FileDelete(_env, Url))
            {
                delete.Delete();
            }
        }

        private ICollection<Tag> LinkProductToTags(List<int> SelfTags)
        {
            var AllTags = _unitOfWork.Tag.GetAll();
            ICollection<Tag> TagsList = new List<Tag>();
            foreach (var tagid in SelfTags)
            {
                var tag = AllTags.FirstOrDefault(t => t.TagId == tagid);
                if (tag != null)
                {
                    TagsList.Add(tag);
                }
            }
            return TagsList;
        }
    }
}