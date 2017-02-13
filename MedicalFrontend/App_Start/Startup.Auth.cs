using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using MedicalFrontend.Models;
using Microsoft.Owin.Security.OAuth;
using MedicalFrontend.Providers;
using Microsoft.Owin.Security;

namespace MedicalFrontend
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.UseAutofacMvc().CreatePerOwinContext(ApplicationDbContext.Create);
            app.UseAutofacMvc().CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.UseAutofacMvc().CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            app.UseAutofacMvc().SetDefaultSignInAsAuthenticationType("External");
            
        

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
              app.UseAutofacMvc().UseCookieAuthentication(new CookieAuthenticationOptions
              {
                  AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                  LoginPath = new PathString("/Account/Login"),
                  Provider = new CookieAuthenticationProvider
                  {

                      // Enables the application to validate the security stamp when the user logs in.
                      // This is a security feature which is used when you change a password or add an external login to your account.  
                      OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                          validateInterval: TimeSpan.FromMinutes(30),
                          regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                  }
              }); 
           
            


          //  app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
           //app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

        
          
              
            // Uncomment the following lines to enable logging in with third party login providers
           // app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
             //   clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

        /*    app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "blah blah blah",
                ClientSecret = "blah blah blah"
            });
            */
        }
    }
}