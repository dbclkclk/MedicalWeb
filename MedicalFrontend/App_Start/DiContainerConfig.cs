using Autofac;
using Autofac.Integration.Mvc;
using MedicalFrontend.Services;
using MedicalFrontend.Services.Interfaces;
using Microsoft.Owin.Security;
using Owin;
using System.Web;
using System.Web.Mvc;

namespace MedicalFrontend.App_Start
{
    public class DiContainerConfig
    {

        public static void Configure(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            // Register your MVC controllers. (MvcApplication is the name of
            // the class in Global.asax.)
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            
            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication).As<IAuthenticationManager>();

            builder.RegisterType<OauthAuthenticationService>()
               .As<IOauthAuthenticationService>().UsingConstructor(typeof(IAuthenticationManager));


            // OPTIONAL: Register model binders that require DI.
            builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            builder.RegisterModelBinderProvider();

            // OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();
        }
    }
}