using System;
using System.ComponentModel.DataAnnotations;
namespace ShoppingCart.Web.Areas.Management.ViewModels.Categorys
{
    public class AddCategoryViewModel
    {
        [Required]
        [Display(Name ="Category Name")]
        public string CategoryName { get; set; }
        
        public IFormFile Image { get; set; }
    }
}

