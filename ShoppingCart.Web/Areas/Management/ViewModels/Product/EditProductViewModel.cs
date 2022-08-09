using System;
using System.ComponentModel.DataAnnotations;
using ShoppingCart.Utilities.Validators;
using ShoppingCart.Models;

namespace ShoppingCart.Web.Areas.Management.ViewModels.Products
{
    public class EditProductViewModel
    {
        // Data From Product
        public int Id { get; set; }
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Price is Required")]
        [Display(Name = "Price")]
        public double Price { get; set; }

        [Display(Name = "Sales Price")]
        [LessThan("Price")]
        public double SalesPrice { get; set; } = 0;
        [Required(ErrorMessage = "Category is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Category is required")]
        [Display(Name = "Catagory")]
        public int CategoryId { get; set; }

        public string? MainImage { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "New Image")]
        public IFormFile? NewImage { get; set; }
        [Required]
        [Display(Name = "Short Description")]
        [DataType(DataType.MultilineText)]
        public string ShortDescription { get; set; }

        [Required]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "SKU")]
        public string Sku { get; set; }
        // [Range(1, int.MaxValue, ErrorMessage = "Quantity is Required")]
        [Required]
        [Display(Name = "Inventory Quantity")]
        public int Quantity { get; set; }

        public bool Featured { get; set; }

        public bool Archived { get; set; }

        public DateTime? AddedAt { get; set; }

        // Data From Category

        public IEnumerable<Category>? Categories { get; set; }

        // Data From Tags
        [Display(Name = "Tags")]
        public List<int>? SelfTags { get; set; }
        public IEnumerable<Tag>? AllTags { get; set; }

        // Data From Images
        [Display(Name = "Gallery Images")]
        [DataType(DataType.Upload)]
        public IEnumerable<IFormFile>? NewGallery { get; set; }
        public IEnumerable<Image>? Images { get; set; }
    }
}