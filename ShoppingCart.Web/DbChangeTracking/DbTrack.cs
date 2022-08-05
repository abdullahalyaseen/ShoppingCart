using ShoppingCart.DataAccess.DbChangeTracking;
using ShoppingCart.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
namespace ShoppingCart.Web.DbChangeTracking
{
    public class DbTrack : DbTracker
    {
        


        private IWebHostEnvironment _env;
        //Register Tracker for each model want to be tracked *Important : the name of Model Tracker must be identical with the model name in the DbContext*

        public DbTrack(IWebHostEnvironment env) : base()
        {
            _env = env;
            Image = new ImagesTracker(_env);
            Product = new ProductsTracker(_env);
            Category = new CategoriesTracker(_env);
        }
        public ImagesTracker Image;
        public ProductsTracker Product;

        public CategoriesTracker Category;



        

      
    }
}