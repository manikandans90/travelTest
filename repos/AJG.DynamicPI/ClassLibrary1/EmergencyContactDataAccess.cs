using AJG.TravelApp.DataAccess.EntityFramework;
using AJG.TravelApp.DataAccess.Interfaces;

namespace AJG.TravelApp.DataAccess
{
    public class EmergencyContactDataAccess : IEmergencyContactDataAccess
    {
		public void Insert(Models.EmergencyContact emergencyContact)
        {
            using (TravelCertEntities travelAppEntities = new TravelCertEntities())
            {
                EmergencyContact emergencyContactEf = new EmergencyContact();
                travelAppEntities.EmergencyContacts.Add(MapToEntity(emergencyContactEf, emergencyContact));
                travelAppEntities.SaveChanges();
                emergencyContact.EmergencyContactId = emergencyContactEf.EmergencyContactId;
            }
        }

        private EmergencyContact MapToEntity(EmergencyContact emergencyContactEf, Models.EmergencyContact emergencyContact)
        {
            emergencyContactEf.EmergencyContactId = emergencyContact.EmergencyContactId;
            emergencyContactEf.UserId = emergencyContact.UserId;
            emergencyContactEf.Name = emergencyContact.Name;
            emergencyContactEf.Email = emergencyContact.Email;
            emergencyContactEf.Phone = emergencyContact.Phone;
            emergencyContactEf.Relationship = emergencyContact.Relationship;

            return emergencyContactEf;
        }
    }
}
