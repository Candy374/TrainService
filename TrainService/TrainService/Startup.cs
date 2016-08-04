using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TrainService.Startup))]
namespace TrainService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
