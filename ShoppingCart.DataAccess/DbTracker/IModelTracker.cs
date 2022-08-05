namespace ShoppingCart.DataAccess.DbChangeTracking
{

    public interface IModelTracker<TModel>
    {
        void Deleted(TModel model);

        void Added(TModel model);

        void Modified(TModel model);

    }

}