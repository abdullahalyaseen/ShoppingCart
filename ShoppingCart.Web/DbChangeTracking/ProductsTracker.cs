using ShoppingCart.DataAccess.DbChangeTracking;
using ShoppingCart.Models;
using ShoppingCart.Utilities.files;

namespace ShoppingCart.Web.DbChangeTracking
{
    public class ProductsTracker : IModelTracker<Product>
    {
        private IWebHostEnvironment _env;

        public ProductsTracker(IWebHostEnvironment env)
        {
            _env = env;
        }

        public void Deleted(Product model)
        {
            string url = model.MainImage;
            using (FileDelete filedelete = new FileDelete(_env, url))
            {
                filedelete.Delete();
            }

            foreach (var image in model.Images)
            {
                string imageurl = image.Url;
                using (FileDelete filedelete = new FileDelete(_env, imageurl))
                {
                    filedelete.Delete();
                }
            }
        }

        public void Added(Product model)
        {
        }

        public void Modified(Product model)
        {
        }

        public void Unchanged(Product model)
        {
        }
    }
}