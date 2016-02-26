using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LHSCampaign.Startup))]
namespace LHSCampaign
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
