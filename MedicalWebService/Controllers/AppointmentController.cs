using MedicalWebService.ExceptionHandlers;
using MedicalWebService.Models;
using MedicalWebService.Services.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace MedicalWebService.Controllers
{

    [RoutePrefix("api/Appointment")]
    [AppointmentExceptionHandler]
    public class AppointmentController : ApiController
    {
        private IAppointmentService _appointmentService { set; get; }
        private ApplicationUserManager _userManager { get; set; }

        public AppointmentController(IAppointmentService service, ApplicationUserManager userManager) : base()
        {
            this._appointmentService = service;
            this._userManager = userManager;
        }
        [Authorize(Roles = "Nurse,Doctor")]
        public async Task Post(Patient patient)
        {
           ApplicationUser user = await this._userManager.FindByIdAsync(HttpContext.Current.User.Identity.GetUserId());
           await this._appointmentService.SaveAsync(patient, user);
            Ok();
        }
        [Authorize(Roles = "Nurse,Doctor")]
        public async Task Put(Patient patient)
        {
            ApplicationUser user = await this._userManager.FindByIdAsync(HttpContext.Current.User.Identity.GetUserId());
            await this._appointmentService.SaveAsync(patient, user);
            Ok();
        }


        [Authorize(Roles = "Nurse")]
        public async Task Delete(int Id)
        {
            ApplicationUser user = await this._userManager.FindByIdAsync(HttpContext.Current.User.Identity.GetUserId());
            await _appointmentService.DeleteAsync(Id, user);
            Ok();
        }
        [Authorize(Roles = "Nurse,Doctor")]
        [HttpGet]
        public async Task<PatientViewModel> Get(int Id)
        {
            Patient patient = await this._appointmentService.getPatientAsync(Id);

            PatientViewModel result = new PatientViewModel
            {
                Id = patient.Id,
                Appointment = patient.Appointment,
                completed = patient.completed,
                DoctorComment = patient.DoctorComment,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Gender = patient.Gender,
                RowVersion = patient.RowVersion
            };
            return result;
        }
        [Authorize(Roles = "Nurse,Doctor")]
        [HttpGet]
        public async Task<List<PatientViewModel>> Get()
        {
            List<Patient> patients = await this._appointmentService.getPatientsAsync();
            List<PatientViewModel> viewModels = new List<PatientViewModel>();
            foreach (var patient in patients)
            {
                viewModels.Add(
                    new PatientViewModel
                    {
                        Id = patient.Id,
                        Appointment = patient.Appointment,
                        completed = patient.completed,
                        DoctorComment = patient.DoctorComment,
                        FirstName = patient.FirstName,
                        LastName = patient.LastName,
                        Gender = patient.Gender,
                        RowVersion = patient.RowVersion
                    }
                    );
            }
            return viewModels;
        }
    }
}
