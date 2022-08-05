using System.ComponentModel.DataAnnotations;
using ShoppingCart.DataAccess.Interfaces;
namespace ShoppingCart.Utilities.Validators
{
    public class UniqueCustomerEmailAttribute : ValidationAttribute
    {

        public UniqueCustomerEmailAttribute() : base("Email already taken")
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var _unitOfWork = (IUnitOfWork)validationContext.GetService(typeof(IUnitOfWork));
            if (value != null)
            {
                var result = _unitOfWork.ApplicationUser.Find(b => b.Email == value.ToString());
                if (result.Count() == 0)
                {
                    return ValidationResult.Success;
                }


            }
            return new ValidationResult(ErrorMessage);
        }
    }
}