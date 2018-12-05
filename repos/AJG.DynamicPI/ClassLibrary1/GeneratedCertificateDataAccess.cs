using System.Linq;
using AJG.TravelApp.DataAccess.EntityFramework;
using AJG.TravelApp.DataAccess.Interfaces;

namespace AJG.TravelApp.DataAccess
{
    public class GeneratedCertificateDataAccess : IGeneratedCertificateDataAccess
    {
		public Models.GeneratedCertificate GetById(int id)
        {
            using (TravelCertEntities travelAppEntities = new TravelCertEntities())
            {
                GeneratedCertificate generatedCertificate = travelAppEntities.GeneratedCertificates.FirstOrDefault(x => x.ID == id);
                return MapToModel(new Models.GeneratedCertificate(), generatedCertificate);
            }
        }

        public void Insert(Models.GeneratedCertificate generatedCertificate)
        {
            using (TravelCertEntities travelAppEntities = new TravelCertEntities())
            {
                GeneratedCertificate generatedCertificateEf = new GeneratedCertificate();
                travelAppEntities.GeneratedCertificates.Add(MapToEntity(generatedCertificateEf, generatedCertificate));
                travelAppEntities.SaveChanges();
                generatedCertificate.ID = generatedCertificateEf.ID;
            }
        }
        
        private GeneratedCertificate MapToEntity(GeneratedCertificate generatedCertificateEf, Models.GeneratedCertificate generatedCertificate)
        {
            generatedCertificateEf.ID = generatedCertificate.ID;
            generatedCertificateEf.Certificate = generatedCertificate.Certificate;
            generatedCertificateEf.CreatedDate = generatedCertificate.CreatedDate;
            generatedCertificateEf.TripId = generatedCertificate.TripId;
            generatedCertificateEf.UserId = generatedCertificate.UserId;

            return generatedCertificateEf;
        }

        private Models.GeneratedCertificate MapToModel(Models.GeneratedCertificate generatedCertificate, GeneratedCertificate generatedCertificateEf)
        {
            generatedCertificate.ID = generatedCertificateEf.ID;
            generatedCertificate.Certificate = generatedCertificateEf.Certificate;
            generatedCertificate.CreatedDate = generatedCertificateEf.CreatedDate;
            generatedCertificate.TripId = generatedCertificateEf.TripId;
            generatedCertificate.UserId = generatedCertificateEf.UserId;

            return generatedCertificate;
        }
    }
}
