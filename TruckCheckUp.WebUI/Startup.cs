using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TruckCheckUp.WebUI.Startup))]
namespace TruckCheckUp.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
