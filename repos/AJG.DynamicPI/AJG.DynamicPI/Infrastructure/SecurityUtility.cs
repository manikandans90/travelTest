using System.Security.Cryptography;
using System.Text;

namespace AJG.TravelApp.Web.Admin.Infrastructure
{
    public class SecurityUtility
    {
        public static string HashPassword(string password)
        {
            byte[] data = Encoding.ASCII.GetBytes(password);
            SHA256CryptoServiceProvider sha1 = new SHA256CryptoServiceProvider();
            byte[] hashStream = sha1.ComputeHash(data);
            return Encoding.ASCII.GetString(hashStream);
        }
    }
}