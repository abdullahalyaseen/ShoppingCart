using System.ComponentModel.DataAnnotations;
namespace ShoppingCart.Utilities.Validators{
    public class LessThanAttribute : ValidationAttribute{
        public LessThanAttribute(string greater) : base(){
            this.Greater = greater;
        }
        public string Greater {get; set;}


        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

           var num = validationContext.ObjectInstance.GetType().GetProperty(Greater).GetValue(validationContext.ObjectInstance);
            if(Convert.ToDouble(value) < Convert.ToDouble(num)){
                return ValidationResult.Success;
            }else{
                var less = validationContext.DisplayName;
                
                ErrorMessage = "The " + less +" field" +" must be less than the " + Greater +" field" ;
                return new ValidationResult(ErrorMessage);
            }
        }
    }
}