using MedicalFrontend.Models;
using MedicalFrontend.Services.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Http;
using DotNetOpenAuth.OAuth2;
using System.IO;
using MedicalFrontend.Extensions;
using System.Web.Mvc;

namespace MedicalFrontend.Services
{
    public class OauthAuthenticationService : IOauthAuthenticationService
    {
        private readonly string _clientId = ConfigurationManager.AppSettings["clientId"];
        private readonly string _clientSecret = ConfigurationManager.AppSettings["clientSecret"];
        public WebServerClient _webServerClient { get; set; }
        public IAuthenticationManager _signInManager { get; set; }

        public OauthAuthenticationService()
        {
        }
        public OauthAuthenticationService(IAuthenticationManager signInManager)
        {
            this._signInManager = signInManager;
            var authorizationServer = new AuthorizationServerDescription
            {
                TokenEndpoint = new Uri(AddressPath.ServerToken)
            };
            _webServerClient = new WebServerClient(authorizationServer, this._clientId,this._clientSecret);

        }
        public async Task<SignInStatus> LoginAsync(string username, string password)
        {
            SignInStatus result = SignInStatus.Success;
            IAuthorizationState state=null;
            try
            {
                state = await Task.FromResult(_webServerClient.ExchangeUserCredentialForToken(username, password, null));
                UserInfoViewModel user = await this.GetUser(state.AccessToken);
                await this.LoginUser(user);
                HttpContext.Current.Session["accesstoken"] = state.AccessToken;
            }
            catch (Exception e)
            {
                result = SignInStatus.Failure;
            }
            return result;
        }
        private async Task<UserInfoViewModel> GetUser(string accessToken)
        {
            UserInfoViewModel user = null;
            String resourceServerUri = new Uri(AddressPath.UserInfo).ToString();
            var client = new HttpClient(_webServerClient.CreateAuthorizingHandler(accessToken));

            HttpResponseMessage response = await client.GetAsync(resourceServerUri);
            if (response.IsSuccessStatusCode)
            {
                user = await response.Content.ReadAsAsync<UserInfoViewModel>();
            }
            return user;
        }
        private async Task LoginUser(UserInfoViewModel user)
        {

            var claims = new List<Claim>();

            // create required claims
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.Role, user.Role));

            // custom – my serialized AppUserState object
            claims.Add(new Claim("userState", user.ToString()));

            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

             await  Task.Run(()=>this._signInManager.SignIn(new AuthenticationProperties()
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddDays(7)
            }, identity));
        }

        public async Task<List<PatientViewModel>> GetPatientsAsync()
        {
            String resourceServerUri = new Uri(AddressPath.Patients).ToString();
            List<PatientViewModel> models = null;
            string accessToken = HttpContext.Current.Session["accesstoken"].ToString();
            var client = new HttpClient(_webServerClient.CreateAuthorizingHandler(accessToken));

            HttpResponseMessage response = await client.GetAsync(resourceServerUri);
            if (response.IsSuccessStatusCode)
            {
                models = await response.Content.ReadAsAsync<List<PatientViewModel>>();
            }
            return models;
        }

        public async Task CreatePatientAsync(PatientViewModel patient)
        {
            String resourceServerUri = new Uri(AddressPath.Patients).ToString();
            string accessToken = HttpContext.Current.Session["accesstoken"].ToString();
            var client = new HttpClient(_webServerClient.CreateAuthorizingHandler(accessToken));
          
            HttpResponseMessage response = await client.PostAsJsonAsync(resourceServerUri,patient);
            if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                string message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }
        }


        public async Task<PatientViewModel> GetPatientAsync(int Id)
        {
            String resourceServerUri = new Uri(AddressPath.Patients).ToString();
            string accessToken = HttpContext.Current.Session["accesstoken"].ToString();
            PatientViewModel result=null;
            var client = new HttpClient(_webServerClient.CreateAuthorizingHandler(accessToken));

            HttpResponseMessage response = await client.GetAsync(resourceServerUri+"/"+Id);
            result = await response.Content.ReadAsAsync<PatientViewModel>();
            return result;
        }

        public async Task DeletePatientAsync(int Id)
        {
            String resourceServerUri = new Uri(AddressPath.Patients).ToString();
            string accessToken = HttpContext.Current.Session["accesstoken"].ToString();
            var client = new HttpClient(_webServerClient.CreateAuthorizingHandler(accessToken));

            HttpResponseMessage response =  await client.DeleteAsync(resourceServerUri + "/" + Id);
            if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                string message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }
        }

        public void Logout(string username, string password)
        {
            this._signInManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}