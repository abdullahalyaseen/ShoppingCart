using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Linq.Expressions;
using ShoppingCart.Models;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.DataAccess.Interfaces;
using ShoppingCart.DataAccess.DTOs;

namespace ShoppingCart.DataAccess.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public dynamic Search(string q, int categoryId = 0, int lowerPrice = 0, int upperPrice = 0,
            List<string> rating = null, string orderby = "orderumber")
        {
            string qLower = q.ToLower();

            IQueryable<Product> RawQuery = context.Set<Product>();

            RawQuery = RawQuery.Where(p =>
                p.Title.ToLower().Contains(qLower) ||
                p.Description.ToLower().Contains(qLower) ||
                p.ShortDescription.ToLower().Contains(qLower));

            if (categoryId > 0)
            {
                RawQuery = RawQuery.Where(p => p.CategoryId == categoryId);
            }

            if (lowerPrice > 0)
            {
                RawQuery = RawQuery.Where(p => p.Price >= lowerPrice);
            }

            if (upperPrice > 0)
            {
                RawQuery = RawQuery.Where(p => p.Price <= upperPrice);
            }

            if (rating != null)
            {
                RawQuery = RawQuery.Where(p =>
                    (rating.Contains("1") ? (p.Reviews.Average(r => r.Rate) <= 1 || !p.Reviews.Any()) : false) ||
                    (rating.Contains("2")
                        ? (p.Reviews.Average(r => r.Rate) >= 1 && p.Reviews.Average(r => r.Rate) <= 2)
                        : false) || (rating.Contains("3")
                        ? (p.Reviews.Average(r => r.Rate) >= 2 && p.Reviews.Average(r => r.Rate) <= 3)
                        : false) || (rating.Contains("4")
                        ? (p.Reviews.Average(r => r.Rate) >= 3 && p.Reviews.Average(r => r.Rate) <= 4)
                        : false) || (rating.Contains("5")
                        ? (p.Reviews.Average(r => r.Rate) >= 4)
                        : false));
            }


            switch (orderby)
            {
                case "price-asc":
                    RawQuery = RawQuery.OrderBy(p => p.Price);
                    break;
                case "price-desc":
                    RawQuery = RawQuery.OrderByDescending(p => p.Price);
                    break;
                case "review-rank":
                    RawQuery = RawQuery.OrderByDescending(p => p.Reviews.Count);
                    break;
                case "date-desc":
                    RawQuery = RawQuery.OrderByDescending(p => p.AddAt);
                    break;
                default:
                    RawQuery = RawQuery.OrderByDescending(p => p.OrderItems.Count);
                    break;
            }


            var Query = RawQuery.Include(p => p.Reviews).Include(p => p.Category).ToList();


            IEnumerable<ProductSearchCategories> categories = Query.GroupBy(c => c.Category).Select(p =>
                    new ProductSearchCategories
                        { CategoryName = p.Key.Name, CategoryId = p.Key.CategoryId, ProductsCount = p.Count() })
                .OrderByDescending(c => c.ProductsCount);
            return new ProductSearchResult { Products = Query, Categories = categories };
        }

        public Product GetForEdit(int id)
        {
            var product = context.Set<Product>()
                .Include(p => p.Images)
                .Include(p => p.Tags)
                .FirstOrDefault(p => p.ProductId == id);
            return product;
        }

        public KeyValuePair<bool, int> CheckProductAvailability(int productId, int quantity)
        {
            var product = context.Products.Find(productId);
            if (product != null)
            {
                if (product.Quantity >= quantity)
                {
                    return new KeyValuePair<bool, int>(key: true, product.Quantity);
                }

                if (product.Quantity == 0)
                {
                    return new KeyValuePair<bool, int>(false, 0);
                }

                if (product.Quantity < quantity)
                {
                    return new KeyValuePair<bool, int>(false, product.Quantity);
                }
            }

            return new KeyValuePair<bool, int>(false, -1);
        }

        public ShoppingCartContext context
        {
            get { return Context as ShoppingCartContext; }
        }

        public ProductRepository(ShoppingCartContext context) : base(context)
        {
        }
    }
}