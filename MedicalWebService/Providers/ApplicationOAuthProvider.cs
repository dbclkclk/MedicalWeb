using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using MedicalWebService.Models;
using MedicalWebService.Repositories.Interfaces;

namespace MedicalWebService.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {

        private IUnitOfWork _unitOfWork;
        private ApplicationUserManager _userManager;

        public ApplicationOAuthProvider(IUnitOfWork unitOfWork, ApplicationUserManager userManager)
        {
            this._unitOfWork = unitOfWork;
            this._userManager = userManager;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        { 

            ApplicationUser user = await _userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(_userManager,
               OAuthDefaults.AuthenticationType);
            ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(_userManager,
                CookieAuthenticationDefaults.AuthenticationType);

            IList<string> roles = await _userManager.GetRolesAsync(user.Id);

            AuthenticationProperties properties = CreateProperties(user.UserName, roles.ElementAt(0).ToString());
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.

            string clientId = "";
            string clientSecret = "";
            context.TryGetBasicCredentials(out clientId, out clientSecret);
            Client client = this._unitOfWork.ClientRepository.Get(filter: a=>a.ClientName == clientId).FirstOrDefault();

            if (client != null && client.ClientSecret==clientSecret)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName, string role)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName },
                { "role",role}
            };
            return new AuthenticationProperties(data);
        }
    }
}