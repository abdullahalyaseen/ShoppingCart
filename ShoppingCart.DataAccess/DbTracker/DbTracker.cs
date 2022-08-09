namespace ShoppingCart.DataAccess.DbChangeTracking
{

        abstract public class DbTracker : IDbTracker
    {
        //Register Tracker for each model want to be tracked *Important : the name of Model Tracker must be identical with the model name in the DbContext*


        public void SavingChanges(object source, EventArgs args)
        {
            //cast source object to the project dbcontext
            ShoppingCartContext context = (ShoppingCartContext)source;
            //Taking the entries that will be change
            var changes = context.ChangeTracker.Entries();

            foreach (var change in changes)
            {
                string ModelName = change.Metadata.Name.Split('.').Last();
                Fire(ModelName, change.Entity, change.State.ToString());
            }

        }

        public void Fire(string modelName, Object Entity, string state)
        {

            object[] parms = new object[1];
            parms[0] = Entity;
            Type thisclass = this.GetType();

            var member = thisclass.GetField(modelName);

            if (member != null)
            {
                object instance = member.GetValue(this);
                if (instance != null)
                {
                    instance.GetType().GetMethod(state).Invoke(instance, parms);
                }
            }
        }
    }
    }
