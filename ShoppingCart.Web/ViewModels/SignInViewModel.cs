using System;
using System.ComponentModel.DataAnnotations;
namespace ShoppingCart.Web.ViewModels
{
    public class SignInViewModel
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Email Address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email Address format: name@domain.com")]
        [MaxLength(60, ErrorMessage = "Maximum Email Address length is: 60 character")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter Password")]
        [StringLength(60, MinimumLength = 8, ErrorMessage = "Password length between 8 and 60 character")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string? ReturnUrl { get; set; }


    }
}

