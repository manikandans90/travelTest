using AJG.TravelApp.DataAccess.EntityFramework;

namespace AJG.TravelApp.DataAccess
{
    public class BaseDataAccess : Interfaces.Database.IBaseDataAccess
    {
        private TravelCertEntities context;
        
        public object ObjectContex => context ?? (context = new TravelCertEntities());
    }
}
