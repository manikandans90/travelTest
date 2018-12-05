using System.Collections.Generic;
using System.Linq;
using AJG.TravelApp.DataAccess.EntityFramework;
using AJG.TravelApp.DataAccess.Interfaces;
using AJG.TravelApp.Models;
using Trip = AJG.TravelApp.DataAccess.EntityFramework.Trip;

namespace AJG.TravelApp.DataAccess
{
    public class TripDataAccess : ITripDataAccess
    {
		public Models.Trip GetById(int tripId)
        {
            Models.Trip modelTrip = new Models.Trip();
            using (TravelCertEntities travelAppEntities = new TravelCertEntities())
            {
                Trip trip = travelAppEntities.Trips.FirstOrDefault(p => p.TripId == tripId);
                if (trip != null)
                {
                    return MapToModel(modelTrip, trip);
                }
            }

            return modelTrip;
        }
        
        public void Insert(Models.Trip trip)
        {
            using (TravelCertEntities travelAppEntities = new TravelCertEntities())
            {
                Trip tripEf = new Trip();
                travelAppEntities.Trips.Add(MapToEntity(tripEf, trip));
                travelAppEntities.SaveChanges();
                trip.TripId = tripEf.TripId;
            }
        }

        public List<OrganisationTripCount> GetOrganisationsTripCount()
        {
            using (TravelCertEntities travelAppEntities = new TravelCertEntities())
            {
                return travelAppEntities.TripViews
                    .Where(t => t.SitecoreOrganisationId.HasValue)
                    .GroupBy(t => t.SitecoreOrganisationId)
                    .Select(t => new OrganisationTripCount
                    {
                        SitecoreOrganisationId = t.Key.Value,
                        TripCount = t.Count()
                    }).ToList();
            }
        }

        private Trip MapToEntity(Trip tripEf, Models.Trip trip)
        {
            tripEf.TripId = trip.TripId;
            tripEf.UserId = trip.UserId;
			tripEf.tNeedsReferral = trip.NeedsReferral;
			tripEf.DateTimeCreated = trip.DateTimeCreated;
			tripEf.C_DateCreated = trip.DateCreated;

			tripEf.tMinStartDate = trip.MinStartDate;
			tripEf.tMaxEndDate= trip.MaxEndDate;
			tripEf.tFirstCountryId = trip.FirstCountryId;
            tripEf.tFirstSitecoreCountryName = trip.FirstSitecoreCountryName;
			
            tripEf.DynamicQuestions = trip.DynamicQuestions;

            return tripEf;
        }

        private Models.Trip MapToModel(Models.Trip trip, Trip tripEf)
        {
            trip.TripId = tripEf.TripId;
            trip.UserId = tripEf.UserId;
			trip.NeedsReferral = tripEf.tNeedsReferral;
			trip.DateTimeCreated = tripEf.DateTimeCreated;
			trip.DateCreated = tripEf.C_DateCreated;

			trip.MinStartDate = tripEf.tMinStartDate;
			trip.MaxEndDate = tripEf.tMaxEndDate;
			trip.FirstCountryId = tripEf.tFirstCountryId;
			
            trip.FirstSitecoreCountryName = tripEf.tFirstSitecoreCountryName;
            trip.DynamicQuestions = tripEf.DynamicQuestions;

            return trip;
        }
    }
}
