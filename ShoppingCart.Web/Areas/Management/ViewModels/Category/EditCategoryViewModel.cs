using System;
using ShoppingCart.DataAccess.Repositories;
using ShoppingCart.Models;
using System.ComponentModel.DataAnnotations;
namespace ShoppingCart.Web.Areas.Management.ViewModels.Categorys
{
    public class EditCategoryViewModel
    {
        public int Id { get; set; }

        [Display(Name ="Category Name")]
        public string CategoryName { get; set; }

        [Display(Name = "Category Image")]
        public string? Image { get; set; }
        [Display(Name ="New Image")]
        public IFormFile? NewImage {get; set;}

    }

}