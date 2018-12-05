using AJG.TravelApp.DataAccess.EntityFramework;
using AJG.TravelApp.DataAccess.Interfaces;

namespace AJG.TravelApp.DataAccess
{
    public class DestinationDataAccess : IDestinationDataAccess
    {
		public void Insert(Models.Destination destination)
        {
            Destination destinationEf = new Destination
            {
                DestinationId = destination.DestinationId,
                TripId = destination.TripId,
                CountryId = destination.CountryId,
                StartDate = destination.StartDate.Date,
                EndDate = destination.EndDate.Date,
                IsRestrictedZone = destination.IsRestrictedZone,
                DynamicQuestions = destination.DynamicQuestions
            };

            using (TravelCertEntities travelAppEntities = new TravelCertEntities())
            {
                travelAppEntities.Destinations.Add(destinationEf);
                travelAppEntities.SaveChanges();
                destination.DestinationId = destinationEf.DestinationId;
            }
        }
    }
}
