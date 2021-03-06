﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using MedicalWebService.Providers;
using MedicalWebService.Models;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using MedicalWebService.Repositories;

namespace MedicalWebService
{
    public partial class Startup
    {

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseAutofacMvc().UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseAutofacMvc().UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

           
            //  app.UseOAuthAuthorizationServer();
            app.UseAutofacMvc().UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }
    }
}
