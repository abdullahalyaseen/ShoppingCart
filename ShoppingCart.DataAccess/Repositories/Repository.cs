using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.DataAccess.Interfaces;
namespace ShoppingCart.DataAccess.Repositories
{
    public class Repository<TModel> : IRepository<TModel> where TModel : class
    {

        protected DbContext Context;

        public Repository(DbContext context)
        {
            Context = context;
        }

        public void Add(TModel model)
        {
            Context.Set<TModel>().Add(model);
        }

        public void AddRange(IEnumerable<TModel> models)
        {
            Context.Set<TModel>().AddRange(models);
        }

        public IEnumerable<TModel> Find(Expression<Func<TModel, bool>>? filter = null,string? includeProperties = null, Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null)
        {
            IQueryable<TModel> query = Context.Set<TModel>();

            if(filter != null)
            {
                query = query.Where(filter);
            }

            if(includeProperties != null)
            {
                var properties = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var includeProperty in properties)
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }


        }

        public TModel Get(int id)
        {
            return Context.Set<TModel>().Find(id);
        }

        public TModel GetWith(Expression<Func<TModel,bool>> filter,string? includeProperties = null){
            IQueryable<TModel> query = Context.Set<TModel>();

            if(includeProperties != null){
                var properties = includeProperties.Split(',',StringSplitOptions.RemoveEmptyEntries);

                foreach(var property in properties){
                    query = query.Include(property);
                }
            }

            return query.FirstOrDefault(filter);

        }

        public IEnumerable<TModel> GetAll()
        {
            return Context.Set<TModel>().ToList();
        }

        public void Remove(TModel model)
        {
            Context.Set<TModel>().Remove(model);
        }

        public void RemoveRange(IEnumerable<TModel> models)
        {
            Context.Set<TModel>().RemoveRange(models);
        }
    }
}

