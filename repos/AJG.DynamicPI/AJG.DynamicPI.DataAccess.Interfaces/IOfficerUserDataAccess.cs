using System;
using System.Collections.Generic;

namespace AJG.TravelApp.DataAccess.Interfaces
{
    public interface IOfficerUserDataAccess
    {
        Models.OfficerUser Create(Models.OfficerUser officerUser);

        void Update(Models.OfficerUser officerUser);

        void ToggleActivation(int officerUserId);

        Models.OfficerUser GetUserById(int officerUserId);

        Models.OfficerUser GetUserByEmail(string officerEmail);

        bool EmailExists(string email);

        int GetOrganisationOfficerUsersCount(Guid? filterByOrganisationId);

        List<Models.OfficerUser> GetFilteredPageOfficerUsers(Guid? filterByOrganisationId, int skipCount, int takeCount);

        void Delete(int officerUserId);

        int? GetUserIdByPasswordResetId(Guid passwordResetId);

        bool PasswordUsedLast5Times(int officerUserId, string passwordHash);

        void CreatePassword(int userId, string passwordHash);

        bool ChangePassword(int userId, string passwordHash);

        Models.OfficerUser ResetPassword(string officerEmail);

        void UnlockAccount(int userId);

        void IncrementFailedLoginCounter(int userId, int accessFailedMaxCount, TimeSpan lockoutPeriod);
    }
}
