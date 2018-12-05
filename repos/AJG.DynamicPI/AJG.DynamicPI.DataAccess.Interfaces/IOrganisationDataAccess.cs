using System.Collections.Generic;

namespace AJG.TravelApp.DataAccess.Interfaces
{
    public interface IOrganisationDataAccess
    {
        List<EntityFramework.Organisation> GetAllOrganisations();
    }
}
