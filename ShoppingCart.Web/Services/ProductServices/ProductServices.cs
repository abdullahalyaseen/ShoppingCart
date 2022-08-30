using ShoppingCart.Models;
namespace ShoppingCart.Web.Services.ProductServices
{

    public class ProductServices
    {
        public static double CalculateRating(IEnumerable<Review> Reviews)
        {
            double TotalRate = 0;
            if (Reviews.Any())
            {
                foreach (var review in Reviews)
                {
                    TotalRate += review.Rate;
                }

                return TotalRate / Reviews.Count();
            }
            else
            {
                return 0;
            }
        }

    }

}