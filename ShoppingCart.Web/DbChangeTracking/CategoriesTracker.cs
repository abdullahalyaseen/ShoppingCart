using ShoppingCart.DataAccess.DbChangeTracking;
using ShoppingCart.Models;
using ShoppingCart.Utilities.files;
namespace ShoppingCart.Web.DbChangeTracking
{
    public class CategoriesTracker : IModelTracker<Category>
    {
        private IWebHostEnvironment _env;

        public CategoriesTracker(IWebHostEnvironment env)
        {
            _env = env;
        }

        public void Added(Category model)
        {
        }

        public void Deleted(Category model)
        {
            string imageUrl = model.ImageUrl;
            using(FileDelete filedelete = new FileDelete(_env,imageUrl)){
                filedelete.Delete();
            }
        }

        public void Modified(Category model)
        {
        }
        public void Unchanged(Category model)
        {
        }
    }
}