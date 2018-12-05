using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AJG.DynamicPI.Startup))]
namespace AJG.DynamicPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
