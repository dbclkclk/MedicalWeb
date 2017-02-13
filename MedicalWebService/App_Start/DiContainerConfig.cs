using Autofac;
using Autofac.Extras.DynamicProxy2;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Castle.Windsor;
using MedicalWebService.Interceptors;
using MedicalWebService.Models;
using MedicalWebService.Providers;
using MedicalWebService.Repositories;
using MedicalWebService.Repositories.Interfaces;
using MedicalWebService.Services;
using MedicalWebService.Services.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataHandler.Serializer;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace MedicalWebService.App_Start
{
    public class DiContainerConfig
    {

        public static void Configure(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            
            var config = GlobalConfiguration.Configuration;

            // Register your MVC controllers. (MvcApplication is the name of
            // the class in Global.asax.)
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(config);

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<AppointmentService>().As<IAppointmentService>()
              .EnableInterfaceInterceptors().InstancePerLifetimeScope();

            builder.RegisterModule(new LoggingModule());
            builder.Register(c => new LoggerInterceptor()).PropertiesAutowired();

            // OPTIONAL: Register model binders that require DI.
            builder.RegisterModelBinders(typeof(WebApiApplication).Assembly);
            builder.RegisterModelBinderProvider();

            builder.RegisterType<ApplicationOAuthProvider>().As<IOAuthAuthorizationServerProvider>()
              .PropertiesAutowired();

           
            builder.RegisterType<ApplicationDbContext>().As<DbContext>().InstancePerLifetimeScope();
            builder.RegisterType<UserStore<ApplicationUser>>().As<IUserStore<ApplicationUser>>().InstancePerLifetimeScope();



            var b = new DpapiDataProtectionProvider("ApplicationName");
         
            builder.Register<IdentityFactoryOptions<ApplicationUserManager>>(e => new IdentityFactoryOptions<ApplicationUserManager>()
            {
                DataProtectionProvider = b
            });
            builder.RegisterType<ApplicationUserManager>().InstancePerLifetimeScope();
            // OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();
            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
        
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            app.UseAutofacMiddleware(container);
            OAuthAuthorizationServerOptions OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = container.Resolve<IOAuthAuthorizationServerProvider>(),
                //  AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                //ApplicationCanDisplayErrors = true,
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(60),
                // In production mode set AllowInsecureHttp = false
                AllowInsecureHttp = true,
                // AuthenticationMode = AuthenticationMode.Active
            };

            app.UseAutofacMvc().UseOAuthAuthorizationServer(OAuthOptions);
             
         

        }
    }
}