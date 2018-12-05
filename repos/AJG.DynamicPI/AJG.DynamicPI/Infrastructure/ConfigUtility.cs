using System;
using System.Configuration;

namespace AJG.TravelApp.Web.Admin.Infrastructure
{
    public class ConfigUtility
    {
        public static int OfficerMaxFailedLoginCount
        {
            get
            {
                int officerMaxFailedLoginCount;
                if (!int.TryParse(ConfigurationManager.AppSettings["OfficerMaxFailedLoginCount"], out officerMaxFailedLoginCount))
                {
                    officerMaxFailedLoginCount = 5;
                }

                return officerMaxFailedLoginCount;
            }
        }

        public static TimeSpan OfficerLockoutPeriod
        {
            get
            {
                int lockoutHours;
                return int.TryParse(ConfigurationManager.AppSettings["OfficerLockoutHours"], out lockoutHours) ? new TimeSpan(lockoutHours, 0, 0) : new TimeSpan(2, 0, 0);
            }
        }

        public static string ActivationEmailSubject => ConfigurationManager.AppSettings["ActivationEmailSubject"];

        public static string ActivationUrl => ConfigurationManager.AppSettings["ActivationUrl"];

        public static string ForgotPasswordEmailSubject => ConfigurationManager.AppSettings["ForgotPasswordEmailSubject"];

        public static string ForgotPasswordResetUrl => ConfigurationManager.AppSettings["ForgotPasswordResetUrl"];

        public static string ExpiredPasswordEmailSubject => ConfigurationManager.AppSettings["ExpiredPasswordEmailSubject"];

        public static string ExpiredPasswordResetUrl => ConfigurationManager.AppSettings["ExpiredPasswordResetUrl"];

        public static string OfficerActivationTemplateFileName => ConfigurationManager.AppSettings["OfficerActivationTemplateFileName"];

        public static string OfficerExpiredPasswordTemplateFileName => ConfigurationManager.AppSettings["OfficerExpiredPasswordTemplateFileName"];

        public static string OfficerForgotPasswordTemplateFileName => ConfigurationManager.AppSettings["OfficerForgotPasswordTemplateFileName"];

        public static string AjgLdapConnectionString => ConfigurationManager.AppSettings["AJGLDAPConnectionString"];

        public static string AllowedAdGroups => ConfigurationManager.AppSettings["AllowedADGroups"];

        public static string AllowedAdUsers => ConfigurationManager.AppSettings["AllowedADUsers"];

        public static string EmailFrom => ConfigurationManager.AppSettings["EmailFrom"];

        public static string TestEmailAddress => ConfigurationManager.AppSettings["TestEmailAddress"];

        public static string IsTest => ConfigurationManager.AppSettings["IsTest"];

        public static string SmtpHost => ConfigurationManager.AppSettings["SmtpHost"];

        public static string SmtpPort => ConfigurationManager.AppSettings["SmtpPort"];

        public static string SmtpUsername => ConfigurationManager.AppSettings["SmtpUsername"];

        public static string SmtpPassword => ConfigurationManager.AppSettings["SmtpPassword"];

        public static string Domain => ConfigurationManager.AppSettings["Domain"];

        public static bool GetTravelCertWebApiUrl(out Uri apiUri)
        {
            return Uri.TryCreate(ConfigurationManager.AppSettings["TravelCertWebAPI"] ?? string.Empty, UriKind.Absolute, out apiUri);
        }
    }
}