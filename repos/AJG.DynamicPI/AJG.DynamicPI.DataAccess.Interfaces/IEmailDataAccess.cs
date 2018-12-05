namespace AJG.TravelApp.DataAccess.Interfaces
{
    public interface IEmailDataAccess
    {
		Models.Email[] GetByTripId(int tripId);

        void Insert(Models.Email email);

        void Update(Models.Email email);
    }
}