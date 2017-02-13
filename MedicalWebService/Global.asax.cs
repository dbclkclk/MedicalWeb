using MedicalWebService.App_Start;
using MedicalWebService.Migrations;
using MedicalWebService.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

[assembly: log4net.Config.XmlConfigurator(ConfigFile ="Web.config", Watch = true)]
namespace MedicalWebService
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
            ApplicationDbContext appContext = new ApplicationDbContext();
            appContext.Database.Initialize(true);
            log4net.Config.XmlConfigurator.Configure();
            
        }
    }
}
