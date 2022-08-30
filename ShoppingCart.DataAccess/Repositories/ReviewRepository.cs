using Microsoft.EntityFrameworkCore;
using ShoppingCart.DataAccess.Interfaces;
using ShoppingCart.Models;

namespace ShoppingCart.DataAccess.Repositories
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        
        private ShoppingCartContext context
        {
            get { return Context as ShoppingCartContext; }
        }

        public ReviewRepository(ShoppingCartContext context) : base(context)
        {
        }
    }
}