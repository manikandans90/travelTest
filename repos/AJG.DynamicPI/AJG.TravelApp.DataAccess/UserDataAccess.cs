using System;
using System.Collections.Generic;
using System.Linq;
using AJG.TravelApp.DataAccess.EntityFramework;
using AJG.TravelApp.DataAccess.Interfaces;

namespace AJG.TravelApp.DataAccess
{
    public class UserDataAccess : IUserDataAccess
    {
        public void Insert(Models.User user)
        {
            using (TravelCertEntities travelAppEntities = new TravelCertEntities())
            {
                User userEf = new User();
                travelAppEntities.Users.Add(MapToEntity(userEf, user));
                travelAppEntities.SaveChanges();
                user.UserId = userEf.UserId;
            }
        }

        public void Update(Models.User user)
        {
            using (TravelCertEntities travelAppEntities = new TravelCertEntities())
            {
                User userEf = travelAppEntities.Users.First(x => x.UserId == user.UserId);
                MapToEntity(userEf, user);
                travelAppEntities.SaveChanges();
            }
        }

        public Models.User GetUserById(int userId)
        {
            using (TravelCertEntities travelAppEntities = new TravelCertEntities())
            {
                User userEf = travelAppEntities.Users.FirstOrDefault(p => p.UserId == userId);
                if (userEf == null)
                {
                    return null;
                }

                Models.User user = MapToModel(userEf);

                return user; 
            }
        }

        public Models.User[] GetUsersByOrganisationdId(int organisationId)
        {
            using (TravelCertEntities travelAppEntities = new TravelCertEntities())
            {
                List<User> users = travelAppEntities.Users.Where(p => p.OrganisationId == organisationId).ToList();
                List<Models.User> modelUser = new List<Models.User>();

                foreach (User userEf in users)
                {
                    Models.User user = MapToModel(userEf);

                    modelUser.Add(user);
                }

                return modelUser.ToArray(); 
            }
        }

        public bool CheckIfDublicateTrip(Models.User user)
        {
            using (TravelCertEntities travelAppEntities = new TravelCertEntities())
            {
                bool result = false;
                IQueryable<User> dublicateList = travelAppEntities.Users.Where(
                        dbUser => dbUser.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase) &&
                             dbUser.Firstname.Equals(user.Firstname, StringComparison.OrdinalIgnoreCase) &&
                             dbUser.Surname.Equals(user.Surname, StringComparison.OrdinalIgnoreCase));

                foreach (User item in dublicateList)
                {
                    result = result || item.Trips.Any(dbTrip => dbTrip.Destinations.Any(dbDestination =>
                                         user.Trips.Any(
                                             userTrip => userTrip.Destinations.Any(userTripDestination =>
                                                userTripDestination.StartDate == dbDestination.StartDate &&
                                                userTripDestination.EndDate == dbDestination.EndDate))
                                 )
                             );
                }

                return result; 
            }
        }

        private static Models.User MapToModel(User userEf)
        {
            List<Models.Trip> trips = new List<Models.Trip>();

            foreach (Trip userEfTrip in userEf.Trips)
            {
                trips.Add(MapToModel(userEfTrip));
            }

            Models.User user = new Models.User
            {
                Email = userEf.Email,
                Firstname = userEf.Firstname,
                Surname = userEf.Surname,
                OrganisationId = userEf.OrganisationId ?? -1,
                Phone = userEf.Phone,
                UserId = userEf.UserId,
                SitecoreOrganisationId = userEf.SitecoreOrganisationId ?? Guid.Empty,
                SitecoreOrganisationName = userEf.SitecoreOrganisationName,
                DynamicQuestions = userEf.DynamicQuestions,
                Trips = trips.ToArray(),
                OccupationId = userEf.OccupationId ?? -1
            };

            return user;
        }

        private static Models.Trip MapToModel(Trip tripEf)
        {
            List<Models.Destination> destinations = new List<Models.Destination>();

            foreach (Destination tripEfDestination in tripEf.Destinations)
            {
                destinations.Add(MapToModel(tripEfDestination));
            }

            Models.Trip trip = new Models.Trip
            {
                DateCreated = tripEf.C_DateCreated,
                DateTimeCreated = tripEf.DateTimeCreated,
                DynamicQuestions = tripEf.DynamicQuestions,
                NeedsReferral = tripEf.tNeedsReferral,
                TripId = tripEf.TripId,
                UserId = tripEf.UserId,
                FirstCountryId = tripEf.tFirstCountryId,
                FirstSitecoreCountryName = tripEf.tFirstSitecoreCountryName,
                MaxEndDate = tripEf.tMaxEndDate,
                MinStartDate = tripEf.tMinStartDate,
                Destinations = destinations.ToArray()
            };

            return trip;
        }

        private static Models.Destination MapToModel(Destination destinationEf)
        {
            Models.Destination destination = new Models.Destination
            {
                CountryId = destinationEf.CountryId,
                DestinationId = destinationEf.DestinationId,
                DynamicQuestions = destinationEf.DynamicQuestions,
                EndDate = destinationEf.EndDate,
                IsRestrictedZone = destinationEf.IsRestrictedZone,
                StartDate = destinationEf.StartDate,
                TripId = destinationEf.TripId
            };

            return destination;
        }
        
        private static User MapToEntity(User userEf, Models.User user)
        {
            userEf.UserId = user.UserId;
            userEf.OrganisationId = user.OrganisationId == 0 ? -1 : user.OrganisationId;
			userEf.Firstname = user.Firstname;
			userEf.Surname = user.Surname;
			userEf.OccupationId = user.OccupationId <= 0 ? -1 : user.OccupationId;
            userEf.Email = user.Email;
            userEf.Phone = user.Phone;
            userEf.SitecoreOrganisationId = user.SitecoreOrganisationId;
            userEf.SitecoreOrganisationName = user.SitecoreOrganisationName;
            userEf.DynamicQuestions = user.DynamicQuestions;

            return userEf;
        }
    }
}