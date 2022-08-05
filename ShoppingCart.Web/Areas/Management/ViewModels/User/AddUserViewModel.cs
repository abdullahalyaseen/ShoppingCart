using System;
using System.ComponentModel.DataAnnotations;
using ShoppingCart.Utilities.Validators;
namespace ShoppingCart.Web.Areas.Management.ViewModels.User
{
    public class AddUserViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter First Name")]
        [DataType(DataType.Text, ErrorMessage = "Please enter alphabets only")]
        [MaxLength(30, ErrorMessage = "Maximum length is 30 letter")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Last Name")]
        [DataType(DataType.Text, ErrorMessage = "Please enter alphabets only")]
        [MaxLength(30, ErrorMessage = "Maximum length is 30 letter")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Email Address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email Address format: name@domain.com")]
        [MaxLength(60, ErrorMessage = "Maximum Email Address length is: 60 character")]
        [Display(Name = "Email Address")]
        [UniqueCustomerEmail(ErrorMessage = "Sorry this Email is used")]
        public string Email { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Country Code")]
        [RegularExpression(pattern: "^[0-9]{0,4}$", ErrorMessage = "Country Code maximum allowed digits is 4")]
        [Display(Name = "Code")]
        public string CountryCode { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Plase enter Mobile Number")]
        [RegularExpression(pattern: "^[0-9]{0,15}$", ErrorMessage = "Mobile Number maximum allowed digits is 15")]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Please enter Password")]
        [StringLength(60, MinimumLength = 8, ErrorMessage = "Password length between 8 and 60 character")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter Password")]
        [StringLength(60, MinimumLength = 8, ErrorMessage = "Password length between 8 and 60 character")]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password does not match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }


    }
}