using System;
using System.ComponentModel.DataAnnotations;
namespace ShoppingCart.Web.Areas.Management.ViewModels.Tags
{
    public class AddTagViewModel
    {
        [Display(Name ="Tag Name")]
        [MaxLength(10)]
        public string Name{get; set;}
    }
}