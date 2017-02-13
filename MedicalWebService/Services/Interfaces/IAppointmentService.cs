using Autofac.Extras.DynamicProxy2;
using MedicalWebService.Interceptors;
using MedicalWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalWebService.Services.Interfaces
{
    [Intercept(typeof(LoggerInterceptor))]
    public interface IAppointmentService : IDisposable
    {
        Task SaveAsync(Patient patient, ApplicationUser user);

        Task DeleteAsync(int patientId, ApplicationUser user);

        Task<List<Patient>> getPatientsAsync();

        Task<Patient> getPatientAsync(int Id);

       // Task UpdateAsync(Patient patient);
    }
}
