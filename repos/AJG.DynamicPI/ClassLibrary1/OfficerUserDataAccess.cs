using System;
using System.Collections.Generic;
using System.Linq;
using AJG.TravelApp.DataAccess.EntityFramework;
using AJG.TravelApp.DataAccess.Interfaces;
using AJG.TravelApp.Models;

namespace AJG.TravelApp.DataAccess
{
    public class OfficerUserDataAccess : IOfficerUserDataAccess
    {
        public void ToggleActivation(int officerUserId)
        {
            using (TravelCertEntities travelAppEntities = new TravelCertEntities())
            {
                EntityFramework.OfficerUser officerUser = travelAppEntities.OfficerUsers.First(u => u.OfficerUserId == officerUserId);
                officerUser.IsActive = !officerUser.IsActive;
                travelAppEntities.SaveChanges();
            }
        }

        public Models.OfficerUser Create(Models.OfficerUser officerUser)
        {
            using (TravelCertEntities travelAppEntities = new TravelCertEntities())
            {
                EntityFramework.OfficerUser entityUser = new EntityFramework.OfficerUser();
                ModelToEntity(entityUser, officerUser);
                ResetPassword(entityUser, travelAppEntities);
                entityUser.CreatedOn = DateTime.UtcNow;
                
                travelAppEntities.OfficerUsers.Add(entityUser);
                travelAppEntities.SaveChanges();
                return EntityToModel(entityUser);
            }
        }

        public void Update(Models.OfficerUser officerUser)
        {
            using (TravelCertEntities travelAppEntities = new TravelCertEntities())
            {
                EntityFramework.OfficerUser entityUser = travelAppEntities.OfficerUsers.First(u => u.OfficerUserId == officerUser.Id);
                // prevent Email from updating
                officerUser.Email = entityUser.Email;
                ModelToEntity(entityUser, officerUser);

                // remove links to previous
                IEnumerable<OrganisationOfficer> orgsToRemove =
                    entityUser.OrganisationOfficers.Where(
                        eu => !officerUser.Organisations.Select(o => o.Id).Contains(eu.SitecoreOrganisationId));
                travelAppEntities.OrganisationOfficers.RemoveRange(orgsToRemove);

                travelAppEntities.SaveChanges();
            }
        }

        public int GetOrganisationOfficerUsersCount(Guid? filterByOrganisationId)
        {
            using (TravelCertEntities travelAppEntities = new TravelCertEntities())
            {
                return travelAppEntities.OfficerUsers
                    .Count(o => filterByOrganisationId == null
                                || filterByOrganisationId == Guid.Empty
                                || o.OrganisationOfficers.Any(oo => oo.SitecoreOrganisationId == filterByOrganisationId));
            }
        }

        public List<Models.OfficerUser> GetFilteredPageOfficerUsers(Guid? filterByOrganisationId, int skipCount, int takeCount)
        {
            using (TravelCertEntities travelAppEntities = new TravelCertEntities())
            {
                IQueryable<EntityFramework.OfficerUser> filteredOfficersOfficerUsers = travelAppEntities.OfficerUsers
                .Where(o => filterByOrganisationId == null
                    || filterByOrganisationId == Guid.Empty
                    || o.OrganisationOfficers.Any(oo => oo.SitecoreOrganisationId == filterByOrganisationId))
                .OrderBy(o => o.Email);

                return filteredOfficersOfficerUsers
                    .Skip(skipCount)
                    .Take(takeCount)
                    .ToList()
                    .Select(EntityToModel).ToList();
            }
        }

        public Models.OfficerUser GetUserById(int officerUserId)
        {
            using (TravelCertEntities travelAppEntities = new TravelCertEntities())
            {
                EntityFramework.OfficerUser entityUser = travelAppEntities.OfficerUsers.First(u => u.OfficerUserId == officerUserId);
                return EntityToModel(entityUser);
            }
        }

        public Models.OfficerUser GetUserByEmail(string officerEmail)
        {
            using (TravelCertEntities travelAppEntities = new TravelCertEntities())
            {
                EntityFramework.OfficerUser entityUser = travelAppEntities.OfficerUsers.FirstOrDefault(u => u.Email == officerEmail);
                return EntityToModel(entityUser);
            }
        }

        public bool EmailExists(string email)
        {
            using (TravelCertEntities travelAppEntities = new TravelCertEntities())
            {
                return travelAppEntities.OfficerUsers.Any(u => u.Email == email);
            }
        }

        public int? GetUserIdByPasswordResetId(Guid passwordResetId)
        {
            using (TravelCertEntities travelAppEntities = new TravelCertEntities())
            {
                return
                    travelAppEntities.OfficerUsers.FirstOrDefault(
                        u => u.IsActive && u.OfficerUserPasswordResets.Any(pr => pr.PasswordResetKey == passwordResetId))
                        ?.OfficerUserId;
            }
        }

        public bool PasswordUsedLast5Times(int officerUserId, string passwordHash)
        {
            using (TravelCertEntities travelAppEntities = new TravelCertEntities())
            {
                return travelAppEntities.OfficerUsers.Any(u => u.OfficerUserId == officerUserId &&
                                                               u.OfficerUserOldPasswords.OrderByDescending(
                                                                   op => op.OfficerUserOldPasswordId)
                                                                   .Select(p => p.PasswordHash)
                                                                   .Take(5)
                                                                   .Contains(passwordHash));
            }
        }

        public void CreatePassword(int userId, string passwordHash)
        {
            using (TravelCertEntities travelAppEntities = new TravelCertEntities())
            {
                EntityFramework.OfficerUser officerUser = travelAppEntities.OfficerUsers.First(u => u.OfficerUserId == userId);
                UpdatePassword(officerUser, passwordHash, travelAppEntities);
            }
        }

        public bool ChangePassword(int userId, string passwordHash)
        {
            using (TravelCertEntities travelAppEntities = new TravelCertEntities())
            {
                EntityFramework.OfficerUser officerUser = travelAppEntities.OfficerUsers.FirstOrDefault(u => u.OfficerUserId == userId);

                if (officerUser != null &&
                    !officerUser.OfficerUserOldPasswords
                        .OrderByDescending(op => op.OfficerUserOldPasswordId)
                        .Take(5)
                        .Select(op => op.PasswordHash)
                        .Contains(passwordHash))
                {
                    UpdatePassword(officerUser, passwordHash, travelAppEntities);
                    return true;
                }

                return false;
            }
        }

        public Models.OfficerUser ResetPassword(string officerEmail)
        {
            using (TravelCertEntities travelAppEntities = new TravelCertEntities())
            {
                EntityFramework.OfficerUser officerUser = travelAppEntities.OfficerUsers.FirstOrDefault(u => u.Email == officerEmail && u.IsActive);
                if (officerUser == null)
                {
                    return null;
                }

                ResetPassword(officerUser, travelAppEntities);
                travelAppEntities.SaveChanges();
                return EntityToModel(officerUser);
            }
        }

        public void UnlockAccount(int userId)
        {
            using (TravelCertEntities travelAppEntities = new TravelCertEntities())
            {
                EntityFramework.OfficerUser officerUser = travelAppEntities.OfficerUsers.First(u => u.OfficerUserId == userId);
                officerUser.AccessFailedCount = 0;
                officerUser.LockoutEndDateUtc = null;
                travelAppEntities.SaveChanges();
            }
        }

        public void IncrementFailedLoginCounter(int userId, int accessFailedMaxCount, TimeSpan lockoutPeriod)
        {
            using (TravelCertEntities travelAppEntities = new TravelCertEntities())
            {
                EntityFramework.OfficerUser officerUser = travelAppEntities.OfficerUsers.First(u => u.OfficerUserId == userId);
                officerUser.AccessFailedCount++;
                if (officerUser.AccessFailedCount >= accessFailedMaxCount)
                {
                    officerUser.LockoutEndDateUtc = DateTime.UtcNow.Add(lockoutPeriod);
                }

                travelAppEntities.SaveChanges();
            }
        }

        public void Delete(int officerUserId)
        {
            using (TravelCertEntities travelAppEntities = new TravelCertEntities())
            {
                EntityFramework.OfficerUser officerUser = travelAppEntities.OfficerUsers.First(u => u.OfficerUserId == officerUserId);
                travelAppEntities.OfficerUsers.Remove(officerUser);
                travelAppEntities.SaveChanges();
            }
        }

        public static Models.OfficerUser EntityToModel(EntityFramework.OfficerUser entityUser)
        {
            if (entityUser == null)
            {
                return null;
            }

            Models.OfficerUser modelUser = new Models.OfficerUser
            {
                Id = entityUser.OfficerUserId,
                UserName = entityUser.UserName,
                Email = entityUser.Email,
                PasswordHash = entityUser.PasswordHash,
                PasswordCreatedOn = entityUser.PasswordCreatedOn,
                LockoutEndDateUtc = entityUser.LockoutEndDateUtc,
                AccessFailedCount = entityUser.AccessFailedCount,
                UserCreatedOn = entityUser.CreatedOn,
                IsActive = entityUser.IsActive,
                OldPasswords = entityUser.OfficerUserOldPasswords.Select(p => new OldPassword
                {
                    OldPasswordId = p.OfficerUserOldPasswordId,
                    PasswordCreatedOn = p.PasswordCreatedOn,
                    PasswordHash = p.PasswordHash
                }).ToList(),
                PasswordResetLinks = entityUser.OfficerUserPasswordResets.Select(r => new PasswordResetLink
                {
                    OfficerUserPasswordResetId = r.OfficerUserPasswordResetId,
                    PasswordResetKey = r.PasswordResetKey
                }).ToList(),
                Organisations = entityUser.OrganisationOfficers.Select(o => new OfficerUserOrganisation
                {
                    Id = o.SitecoreOrganisationId,
                    Name = o.SitecoreOrganisationName
                }).ToList()
            };

            return modelUser;
        }

        private static void ModelToEntity(EntityFramework.OfficerUser entityUser, Models.OfficerUser modelUser)
        {
            //entityUser.OfficerUserId = modelUser.Id;
            entityUser.UserName = modelUser.UserName;
            entityUser.Email = modelUser.Email;
            entityUser.PasswordHash = string.IsNullOrEmpty(modelUser.PasswordHash) ? entityUser.PasswordHash : modelUser.PasswordHash;
            entityUser.IsActive = modelUser.IsActive;

            // Update organisation references
            foreach (OfficerUserOrganisation organisation in modelUser.Organisations.Where(oid => oid != null))
            {
                entityUser.OrganisationOfficers.Add(new OrganisationOfficer
                {
                    OfficerUserId = entityUser.OfficerUserId,
                    SitecoreOrganisationId = organisation.Id,
                    SitecoreOrganisationName = organisation.Name
                });
            }
        }

        private void ResetPassword(EntityFramework.OfficerUser user, TravelCertEntities travelAppEntities)
        {
            travelAppEntities.OfficerUserPasswordResets.RemoveRange(user.OfficerUserPasswordResets);
            user.OfficerUserPasswordResets.Add(new OfficerUserPasswordReset
            {
                PasswordResetKey = Guid.NewGuid()
            });
        }

        private void UpdatePassword(EntityFramework.OfficerUser officerUser, string passwordHash, TravelCertEntities travelAppEntities)
        {
            officerUser.PasswordHash = passwordHash;
            officerUser.PasswordCreatedOn = DateTime.UtcNow;
            officerUser.LockoutEndDateUtc = null;
            officerUser.OfficerUserOldPasswords.Add(new OfficerUserOldPassword
            {
                PasswordCreatedOn = officerUser.PasswordCreatedOn.Value,
                PasswordHash = officerUser.PasswordHash
            });

            travelAppEntities.OfficerUserPasswordResets.RemoveRange(officerUser.OfficerUserPasswordResets);
            travelAppEntities.SaveChanges();
        }
    }
}
