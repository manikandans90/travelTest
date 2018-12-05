using System.Linq;
using AJG.TravelApp.DataAccess.EntityFramework;
using AJG.TravelApp.DataAccess.Interfaces;

namespace AJG.TravelApp.DataAccess
{
    public class EmailDataAccess : IEmailDataAccess
    {
		public Models.Email[] GetByTripId(int tripId)
        {
            using (TravelCertEntities travelAppEntities = new TravelCertEntities())
            {
                Email[] emailEFs = travelAppEntities.Emails.Where(x => x.TripId == tripId).ToArray();
                return emailEFs.Select(x => MapToModel(new Models.Email(), x)).ToArray();
            }
        }

        public void Insert(Models.Email email)
        {
            using (TravelCertEntities travelAppEntities = new TravelCertEntities())
            {
                Email emailEf = new Email();
                travelAppEntities.Emails.Add(MapToEntity(emailEf, email));
                travelAppEntities.SaveChanges();
                email.Id = emailEf.ID;
            }
        }

        public void Update(Models.Email email)
        {
            using (TravelCertEntities travelAppEntities = new TravelCertEntities())
            {
                Email emailEf = travelAppEntities.Emails.First(x => x.ID == email.Id);
                MapToEntity(emailEf, email);
                travelAppEntities.SaveChanges();
            }
        }

        private Email MapToEntity(Email emailEf, Models.Email email)
        {
            emailEf.Body = email.Body;
            emailEf.CreatedDate = email.CreatedDate;
            emailEf.GeneratedCertificateID = email.GeneratedCertificateId;
            emailEf.ID = email.Id;
            emailEf.IsSent = email.IsSent;
            emailEf.SentDate = email.SentDate;
            emailEf.Subject = email.Subject;
            emailEf.ToEmail = email.ToEmail;
            emailEf.UserId = email.UserId;
            emailEf.TripId = email.TripId;

            return emailEf;
        }

        private Models.Email MapToModel(Models.Email email, Email emailEf)
        {
            email.Body = emailEf.Body;
            email.CreatedDate = emailEf.CreatedDate;
            email.GeneratedCertificateId = emailEf.GeneratedCertificateID;
            email.Id = emailEf.ID;
            email.IsSent = emailEf.IsSent;
            email.SentDate = emailEf.SentDate;
            email.Subject = emailEf.Subject;
            email.ToEmail = emailEf.ToEmail;
            email.UserId = emailEf.UserId;
            email.TripId = emailEf.TripId;

            return email;
        }
    }
}
