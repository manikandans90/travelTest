using System.Collections.Generic;

namespace AJG.TravelApp.DataAccess.Interfaces
{
    public interface ITripDataAccess
    {
        Models.Trip GetById(int tripId);

        void Insert(Models.Trip trip);

        List<Models.OrganisationTripCount> GetOrganisationsTripCount();
    }
}