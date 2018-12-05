using System.DirectoryServices;
using System.Linq;
using System.Runtime.InteropServices;

namespace AJG.TravelApp.Web.Admin.Infrastructure
{
    public static class ActiveDirectoryUtility
	{
        public class UserInfo
		{
			public string Name { get; set; }

			public string Email { get; set; }
		}
        
		public static UserInfo GetAccount(string domainAndUsername, string password, bool getFullInfo)
		{
            string ldapConnectionString = ConfigUtility.AjgLdapConnectionString ?? string.Empty;
            using (DirectoryEntry directoryEntry = new DirectoryEntry(ldapConnectionString, domainAndUsername, password))
			{
				using (DirectorySearcher directorySearcher = new DirectorySearcher(directoryEntry))
				{
					string username;
					{
						int k = domainAndUsername.LastIndexOf('\\');
						if(k < 0)
						{
							username = domainAndUsername;
						}
						else
						{
							username = domainAndUsername.Substring(k + 1);
						}
					}

					string filterByGroup = string.Empty;
					string filterByUser = string.Empty;

					{
						string allowedGroupsS = ConfigUtility.AllowedAdGroups ?? string.Empty;
						string[] allowedGroups = allowedGroupsS.Split('|').Select(g => g.Trim()).Where(g => g.Length > 0).ToArray();

						if(allowedGroups.Any())
						{
							foreach(string g in allowedGroups)
							{
								filterByGroup += "(memberOf=" + g + ")";
							}
						}
					}

					{
						string allowedUsersS = ConfigUtility.AllowedAdUsers ?? string.Empty;
						string[] allowedUsers = allowedUsersS.Split('|').Select(u => u.Trim()).Where(u => u.Length > 0).ToArray();

						if (allowedUsers.Any())
						{
							foreach(string u in allowedUsers)
							{
								filterByUser += "(sAMAccountName=" + u + ")";
							}
						}
					}

					string filter = $"(&(objectClass=user)(sAMAccountName={username})"; // (No closing ')' yet).

					if (filterByGroup.Length > 0 || filterByUser.Length > 0)
					{
						filter += "(|" + filterByGroup + filterByUser + ")";
					}

					filter += ")";
					directorySearcher.Filter = filter;


					if (getFullInfo)
					{
						directorySearcher.PropertiesToLoad.Add("cn");
						directorySearcher.PropertiesToLoad.Add("mail");
					}

					try
					{
						SearchResult result = directorySearcher.FindOne();

						if (result == null)
						{
                            LoggerUtility.Logger.ErrorFormat("[TravelCert] User not found (getFullInfo={0}): {1}", getFullInfo, domainAndUsername);

                            return null;
						}

						UserInfo userInfo = new UserInfo();

						if (getFullInfo)
						{
							userInfo.Name = GetProp(result, "cn");
							userInfo.Email = GetProp(result, "mail");
						}

						return userInfo;
					}
					catch (DirectoryServicesCOMException exc)
					{
                        LoggerUtility.Logger.ErrorFormat("[TravelCert] GetAccount: {0}", exc);

						return null;
					}
					catch (COMException exc)
					{
                        // (This could happen when domain and username are good, but password is bad)

                        LoggerUtility.Logger.ErrorFormat("[TravelCert] GetAccount: {0}", exc);

					    if ((uint) exc.HResult == 0x8007052Eu)
					    {
					        return null;
					    }

						throw;
					}
				}
			}
		}
        
		private static string GetProp(SearchResult result, string name)
		{
			ResultPropertyValueCollection props = result.Properties[name];
		    if (props == null )
		    {
		        return null;
		    }

			return props[0].ToString();
		}
	}
}
