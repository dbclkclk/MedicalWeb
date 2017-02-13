using MedicalWebService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MedicalWebService.Models;
using MedicalWebService.Repositories.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using MedicalWebService.Enums;
using log4net;
using Autofac.Extras.DynamicProxy2;
using MedicalWebService.Interceptors;

namespace MedicalWebService.Services
{
    public class AppointmentService : IAppointmentService
    {
        private IUnitOfWork _unitOfWork;
        private ApplicationUserManager _userManager;

       public AppointmentService(IUnitOfWork unitOfWork,  ApplicationUserManager userManager)
       {
            this._unitOfWork = unitOfWork;
            this._userManager = userManager;
       }

        public async Task DeleteAsync(int patientId, ApplicationUser user)
        {
            IList<string> roles = await this._userManager.GetRolesAsync(user.Id);
            string role = roles.FirstOrDefault();
            if (role == RoleTypes.DOCTOR)
                throw new UnauthorizedAccessException("A doctor isn't allowed to delete an appointment");

            Patient patient = await Task.FromResult(this._unitOfWork.PatientRepository.GetByID(patientId));
            if(patient == null)
                throw new KeyNotFoundException("Can't delete an completed appointment");
            if (patient.completed)
                throw new InvalidOperationException("Can't delete an completed appointment");
            await Task.Run(() => this._unitOfWork.PatientRepository.Delete(patient.Id));
            await Task.Run(()=>this._unitOfWork.Save());
        }

        public async Task<Patient> getPatientAsync (int Id)
        {
            return await Task.FromResult(this._unitOfWork.PatientRepository.Get(filter: a => a.Id == Id).FirstOrDefault());
        }

        public async Task<List<Patient>> getPatientsAsync()
        {
            return  await Task.FromResult(this._unitOfWork.PatientRepository.Get().ToList());
        }

        public async Task SaveAsync(Patient patient, ApplicationUser user)
        {

            if (patient.Id !=0)
               await this.Update(patient, user);
            else
            {
                await this.Save(patient, user);
            }
            await Task.Run(() => this._unitOfWork.Save());
        }
        private async Task Update(Patient patient, ApplicationUser user)
        {
            Patient result = await Task.FromResult(this._unitOfWork.PatientRepository.GetByID(patient.Id));
            if (result == null)
                throw new InvalidOperationException("The patient record no longer exist");
            if (result.completed)
                throw new InvalidOperationException("Can't update a completed appointment");

            IList<string> roles = await this._userManager.GetRolesAsync(user.Id);
            string role = roles.FirstOrDefault();
            if (RoleTypes.DOCTOR == role)
            {
                result = this.DoctorUpdate(result,patient, user);
            }
            else if (RoleTypes.NURSE == role)
            {
                result = this.NurseUpdate(result, patient, user);
            }
            else
            {
                throw new UnauthorizedAccessException("You're not allowed to update this model");
            }
            await Task.Run(() => this._unitOfWork.PatientRepository.Update(result));
        }
        private async Task Save(Patient patient, ApplicationUser user)
        {
            IList<string> roles = await this._userManager.GetRolesAsync(user.Id);
            string role = roles.FirstOrDefault();
            if (role == RoleTypes.DOCTOR)
                throw new UnauthorizedAccessException("A doctor isn't allowed to create an appointment");
            //Assign current nurse 
            patient = this.NurseSave(patient, user);
            await Task.Run(() => this._unitOfWork.PatientRepository.Insert(patient));
            
        }
        private Patient NurseSave(Patient patient, ApplicationUser user)
        {
            return new Patient
            {
                Gender = patient.Gender,
                Appointment = patient.Appointment,
                Nurse = user,
                LastName = patient.LastName,
                FirstName = patient.FirstName
            };
        }
        private Patient NurseUpdate(Patient current, Patient modified, ApplicationUser user)
        {
            current.Gender = modified.Gender;
            current.Appointment = modified.Appointment;
            current.Nurse = user;
            current.LastName = modified.LastName;
            current.RowVersion = modified.RowVersion;
            current.FirstName = modified.FirstName;
            return current;

        }
        private Patient DoctorUpdate(Patient current, Patient modified, ApplicationUser user)
        {
            if (modified.completed == true && modified.DoctorComment == null)
            {
                throw new InvalidOperationException("Doctor's comment needed when case is marked complete");
            }
            current.completed = modified.completed;
            current.DoctorComment = modified.DoctorComment;
            current.RowVersion = modified.RowVersion;
            current.Doctor = user;
            return current;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this._unitOfWork.Dispose();
                    this._userManager.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}