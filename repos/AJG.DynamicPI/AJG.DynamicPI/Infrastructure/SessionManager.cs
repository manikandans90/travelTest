using System.Globalization;
namespace AJG.TravelApp.Web.Admin.Infrastructure
{
    public static class SessionManager
    {
        public static CultureInfo CurrentCultureInfo
        {
            get
            {
                object currentCultureInfo = System.Web.HttpContext.Current.Session != null &&
                                            System.Web.HttpContext.Current.Session[Constants.Session.CurrentCultureInfoKey] != null
                    ? System.Web.HttpContext.Current.Session[Constants.Session.CurrentCultureInfoKey] : "";


                CultureInfo cultureInfo = TravelApp.Infrastructure.Utils.ConverterHelper.GetCultureInfo(currentCultureInfo.ToString());
                return cultureInfo;
            }
            set
            {
                System.Web.HttpContext.Current.Session[Constants.Session.CurrentCultureInfoKey] = value.Name;
            }
        }
    }
}