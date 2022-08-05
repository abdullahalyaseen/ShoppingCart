namespace ShoppingCart.DataAccess.DbChangeTracking
{
    public interface IDbTracker
    {
        void SavingChanges(object source, EventArgs args);
        void Fire(string modelName, Object Entity, string state);
        
    }
}