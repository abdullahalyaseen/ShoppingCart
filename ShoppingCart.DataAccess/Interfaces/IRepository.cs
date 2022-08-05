using System;
using System.Linq.Expressions;
namespace ShoppingCart.DataAccess.Interfaces
{
    public interface IRepository<TModel> where TModel : class
    {
        TModel Get(int id);

        TModel GetWith(Expression<Func<TModel,bool>> filter,string? includeProperties = null);
        IEnumerable<TModel> GetAll();
        IEnumerable<TModel> Find(Expression<Func<TModel, bool>>? filter = null, string? includeProperties = null, Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null);

        void Add(TModel model);
        void AddRange(IEnumerable<TModel> models);

        void Remove(TModel model);
        void RemoveRange(IEnumerable<TModel> models);
    }
}