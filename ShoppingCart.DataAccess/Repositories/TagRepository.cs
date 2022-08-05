using ShoppingCart.DataAccess.Interfaces;
using ShoppingCart.Models;
namespace ShoppingCart.DataAccess.Repositories
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {

        
        private ShoppingCartContext context{
            get{
                return Context as ShoppingCartContext;
            }
        }


        public TagRepository(ShoppingCartContext context) : base(context){

        }
        
    }
}