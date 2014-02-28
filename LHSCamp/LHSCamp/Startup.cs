using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LHSCamp.Startup))]
namespace LHSCamp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
