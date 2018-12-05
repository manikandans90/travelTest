using System.Reflection;
using log4net;


[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace AJG.TravelApp.Web.Admin.Infrastructure
{
    public static class LoggerUtility
    {
        public static ILog Logger => LogManager.GetLogger(Assembly.GetCallingAssembly(), "TravelCert2Admin.Logger");
    }
}