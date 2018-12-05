namespace AJG.TravelApp.DataAccess.Interfaces
{
    public interface IUserDataAccess
    {
        void Insert(Models.User user);

        void Update(Models.User user);

        Models.User GetUserById(int userId);

        Models.User[] GetUsersByOrganisationdId(int organisationId);

        bool CheckIfDublicateTrip(Models.User user);
    }
}
