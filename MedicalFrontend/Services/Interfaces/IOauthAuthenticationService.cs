using MedicalFrontend.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MedicalFrontend.Services.Interfaces
{
    public interface IOauthAuthenticationService
    {
        Task<SignInStatus> LoginAsync(string username, string password);

        Task<List<PatientViewModel>> GetPatientsAsync();

        void Logout(string username, string password);

        Task CreatePatientAsync(PatientViewModel patient);

        Task<PatientViewModel> GetPatientAsync(int Id);

        Task DeletePatientAsync(int Id);
    }
}
