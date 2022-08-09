using ShoppingCart.DataAccess.DbChangeTracking;
using ShoppingCart.Models;
using ShoppingCart.Utilities.files;
namespace ShoppingCart.Web.DbChangeTracking
{
    public class ImagesTracker : IModelTracker<Image>
    {
        private IWebHostEnvironment _env;
        public ImagesTracker(IWebHostEnvironment env)
        {
            _env = env;
        }
        public void Deleted(Image image)
        {
            string url = image.Url;
            using (FileDelete filedelete = new FileDelete(_env, url))
            {
                filedelete.Delete();
            }
        }

        public void Added(Image model)
        {

        }


        public void Modified(Image model){

        }
        public void Unchanged(Image model)
        {
        }

    }
}