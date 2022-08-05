using ShoppingCart.DataAccess.Interfaces;
using ShoppingCart.Models;
namespace ShoppingCart.DataAccess.Repositories
{
    public class ImageRepository : Repository<Image>, IImageRepository
    {

        public ImageRepository(ShoppingCartContext context) : base(context){
            
        }
        private ShoppingCartContext context{
            get{
                return Context as ShoppingCartContext;
            }
        }
        
    }
}