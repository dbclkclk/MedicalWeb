using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using MedicalWebService.App_Start;

[assembly: OwinStartup(typeof(MedicalWebService.Startup))]

namespace MedicalWebService
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
