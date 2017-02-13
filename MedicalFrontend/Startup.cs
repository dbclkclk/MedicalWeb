using MedicalFrontend.App_Start;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MedicalFrontend.Startup))]
namespace MedicalFrontend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            DiContainerConfig.Configure(app);
            ConfigureAuth(app);
        }
    }
}
