using System;
using System.Net;
using System.Net.Mail;

namespace AJG.TravelApp.Web.Admin.Infrastructure
{
    public class MailManager
    {
        private readonly string fromEmailAddress;
        private SmtpClient smtpClient;
        private readonly string testEmailAddress;
        private readonly bool isTest;

        public MailManager()
        {
            SetupSmtpClient();
            fromEmailAddress = ConfigUtility.EmailFrom;
            testEmailAddress = ConfigUtility.TestEmailAddress;
            isTest = ConfigUtility.IsTest == "1";
        }

        public bool SendEmail(string destinationEmail, string subject, string messageText)
        {
            MailMessage message = new MailMessage();

            if (isTest && !(destinationEmail.EndsWith("@endava.com", StringComparison.OrdinalIgnoreCase) || destinationEmail.EndsWith("@ajg.com", StringComparison.OrdinalIgnoreCase)))
            {
                destinationEmail = testEmailAddress;
            }

            message.To.Add(destinationEmail);
            message.Subject = subject;
            message.From = new MailAddress(fromEmailAddress);
            message.IsBodyHtml = true;
            message.Body = messageText;
            return SendMessage(message);
        }

        private bool SendMessage(MailMessage message)
        {
            try
            {
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                LoggerUtility.Logger.ErrorFormat("[TravelCert] Send email failed. Error: {0}", ex.Message);
                return false;
            }

            return true;
        }

        private void SetupSmtpClient()
        {
            string host = ConfigUtility.SmtpHost;
            string port = ConfigUtility.SmtpPort;
            string username = ConfigUtility.SmtpUsername;
            string password = ConfigUtility.SmtpPassword;
            string domain = ConfigUtility.Domain;

            smtpClient = new SmtpClient(host, int.Parse(port))
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(username, password, domain)
            };
        }
    }
}