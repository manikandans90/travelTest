using System.Collections.Generic;
using System.Linq;
using AJG.TravelApp.DataAccess.EntityFramework;
using AJG.TravelApp.DataAccess.Interfaces;

namespace AJG.TravelApp.DataAccess
{
    public class OrganisationDataAccess : IOrganisationDataAccess
    {
		public List<Organisation> GetAllOrganisations()
        {
            using (TravelCertEntities travelAppEntities = new TravelCertEntities())
            {
                return travelAppEntities.Organisations.ToList();
            }
        }
    }
}
