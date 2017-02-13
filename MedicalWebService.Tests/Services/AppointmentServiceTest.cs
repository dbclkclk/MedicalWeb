using Microsoft.VisualStudio.TestTools.UnitTesting;
using MedicalWebService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalWebService.Repositories.Interfaces;
using Moq;
using MedicalWebService.Models;
using MedicalWebService.Enums;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using log4net;

namespace MedicalWebService.Services.Tests
{
    [TestClass()]
    public class AppointmentServiceTest
    {
        private AppointmentService _appointmentService;
        private Mock<IUnitOfWork> _mockedUnitOfWork;
        private ApplicationUser _currentUser;
        private Mock<ApplicationUserManager> _mockedUserManager;


        [TestInitialize]
        public void init()
        {
            this._currentUser = new ApplicationUser {
                Id="1234"
            };

            var userStore = new Mock<IUserStore<ApplicationUser>>();

            var mockedOptions =new Mock<IdentityFactoryOptions<ApplicationUserManager>>();

            this._mockedUserManager = new Mock<ApplicationUserManager>(userStore.Object, mockedOptions.Object);


            this._mockedUnitOfWork = new Mock<IUnitOfWork>();
            this._appointmentService = new AppointmentService(this._mockedUnitOfWork.Object, this._mockedUserManager.Object);
        }

        [TestMethod()]
        public async Task deleteTestThatWhenItFindsRecordItDeletes()
        {
            this._mockedUnitOfWork.Setup(a => a.PatientRepository.GetByID(It.IsAny<object>())).Returns(new Patient
            {
                completed = false,
                Id = 1
            });

            this._mockedUserManager.Setup(a => a.GetRolesAsync(It.IsAny<string>())).Returns(Task.FromResult((IList<string>) new List<string> {
                RoleTypes.NURSE
            }));

            this._mockedUnitOfWork.Setup(a => a.PatientRepository.Delete(It.IsAny<object>()));
            this._mockedUnitOfWork.Setup(a => a.Save());

            await this._appointmentService.DeleteAsync(1, this._currentUser);
            
            this._mockedUnitOfWork.Verify(a => a.PatientRepository.GetByID(It.IsAny<object>()), Times.Once());
            this._mockedUnitOfWork.Verify(a => a.PatientRepository.Delete(It.IsAny<object>()), Times.Once());
            this._mockedUnitOfWork.Verify(a => a.Save(), Times.Once());
            this._mockedUserManager.Verify(a => a.GetRolesAsync(It.IsAny<string>()), Times.Once()) ;
        }

        [TestMethod()]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task DeleteAsyncThatWhenItDoesNotFindsRecordItThrowsNotFoundException()
        {
            this._mockedUnitOfWork.Setup(a => a.PatientRepository.GetByID(It.IsAny<object>())).Returns((Patient)null);
            this._mockedUnitOfWork.Setup(a => a.PatientRepository.Delete(It.IsAny<object>()));
            this._mockedUnitOfWork.Setup(a => a.Save());
            this._mockedUserManager.Setup(a => a.GetRolesAsync(It.IsAny<string>())).Returns(Task.FromResult((IList<string>)new List<string> {
                RoleTypes.NURSE
            }));
            await this._appointmentService.DeleteAsync(1,this._currentUser);

            this._mockedUnitOfWork.Verify(a => a.PatientRepository.GetByID(It.IsAny<object>()), Times.Once());
            this._mockedUnitOfWork.Verify(a => a.PatientRepository.Delete(It.IsAny<object>()), Times.Never());
            this._mockedUnitOfWork.Verify(a => a.Save(), Times.Never());
            this._mockedUserManager.Verify(a => a.GetRolesAsync(It.IsAny<string>()), Times.Once());
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task deleteTestThatWhenRecordMarkedCompletedThrowsException()
        {
            this._mockedUnitOfWork.Setup(a => a.PatientRepository.GetByID(It.IsAny<object>())).Returns(new Patient
            {
                completed = true,
                Id = 1
            });
            this._mockedUserManager.Setup(a => a.GetRolesAsync(It.IsAny<string>())).Returns(Task.FromResult((IList<string>)new List<string> {
                RoleTypes.NURSE
            }));
            this._mockedUnitOfWork.Setup(a => a.PatientRepository.Delete(It.IsAny<object>()));
            this._mockedUnitOfWork.Setup(a => a.Save());

            await this._appointmentService.DeleteAsync(1, this._currentUser);

            this._mockedUnitOfWork.Verify(a => a.PatientRepository.GetByID(It.IsAny<object>()), Times.Once());
            this._mockedUnitOfWork.Verify(a => a.PatientRepository.Delete(It.IsAny<object>()), Times.Never());
            this._mockedUnitOfWork.Verify(a => a.Save(), Times.Never());
            this._mockedUserManager.Verify(a => a.GetRolesAsync(It.IsAny<string>()), Times.Once());
        }


        [TestMethod()]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public async Task deleteTestDoctorShouldNotBeAllowedtoDelete()
        {
            this._mockedUnitOfWork.Setup(a => a.PatientRepository.GetByID(It.IsAny<object>())).Returns(new Patient
            {
                completed = true,
                Id = 1
            });
            this._mockedUserManager.Setup(a => a.GetRolesAsync(It.IsAny<string>())).Returns(Task.FromResult((IList<string>)new List<string> {
                RoleTypes.DOCTOR
            }));
            this._mockedUnitOfWork.Setup(a => a.PatientRepository.Delete(It.IsAny<object>()));
            this._mockedUnitOfWork.Setup(a => a.Save());

            await this._appointmentService.DeleteAsync(1, this._currentUser);

            this._mockedUnitOfWork.Verify(a => a.PatientRepository.GetByID(It.IsAny<object>()), Times.Never());
            this._mockedUnitOfWork.Verify(a => a.PatientRepository.Delete(It.IsAny<object>()), Times.Never());
            this._mockedUnitOfWork.Verify(a => a.Save(), Times.Never());
            this._mockedUserManager.Verify(a => a.GetRolesAsync(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public async Task SaveAsyncTestThatPerformsInsertWhenNewRecord()
        {

            Patient patient = new Patient
            {
                FirstName="blah",
                LastName="blah",
                Gender = GenderTypes.MALE,
                completed=false,
                Appointment = DateTime.Now
            };

            this._mockedUserManager.Setup(a => a.GetRolesAsync(It.IsAny<string>())).Returns(Task.FromResult((IList<string>)new List<string> {
                RoleTypes.NURSE
            }));

            this._mockedUnitOfWork.Setup(a => a.PatientRepository.Insert(It.IsAny<Patient>()));
            this._mockedUnitOfWork.Setup(a => a.Save());

            await this._appointmentService.SaveAsync(patient, this._currentUser);

            this._mockedUserManager.Verify(a => a.GetRolesAsync(It.IsAny<string>()), Times.Once());
            this._mockedUnitOfWork.Verify(a => a.Save(), Times.Once());
            this._mockedUnitOfWork.Verify(a => a.PatientRepository.Insert(It.IsAny<Patient>()), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public async Task SaveAsyncTestThatDoctorCanNotPerformInsert()
        {

            Patient patient = new Patient
            {
                FirstName = "blah",
                LastName = "blah",
                Gender = GenderTypes.MALE,
                completed = false,
                Appointment = DateTime.Now
            };

            this._mockedUserManager.Setup(a => a.GetRolesAsync(It.IsAny<string>())).Returns(Task.FromResult((IList<string>)new List<string> {
                RoleTypes.DOCTOR
            }));

            this._mockedUnitOfWork.Setup(a => a.PatientRepository.Insert(It.IsAny<Patient>()));
            this._mockedUnitOfWork.Setup(a => a.Save());

            await this._appointmentService.SaveAsync(patient, this._currentUser);

            this._mockedUserManager.Verify(a => a.GetRolesAsync(It.IsAny<string>()), Times.Once());

            this._mockedUnitOfWork.Verify(a => a.PatientRepository.Insert(It.IsAny<Patient>()), Times.Never());
            this._mockedUnitOfWork.Verify(a => a.Save(), Times.Never);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task SaveAsyncTestUpdateDoctorCanNotSetCompleteWithoutComment()
        {

            Patient patient = new Patient
            {
                FirstName = "blah",
                LastName = "blah",
                Gender = GenderTypes.MALE,
                completed = true,
                Appointment = DateTime.Now,
                Id=1
            };

            this._mockedUserManager.Setup(a => a.GetRolesAsync(It.IsAny<string>())).Returns(Task.FromResult((IList<string>)new List<string> {
                RoleTypes.DOCTOR
            }));


            this._mockedUnitOfWork.Setup(a => a.PatientRepository.GetByID(It.IsAny<object>())).Returns(new Patient {
                Id=1,
                completed=false
            });

           // this._unitOfWork.PatientRepository.GetByID(patient.Id)

            this._mockedUnitOfWork.Setup(a => a.PatientRepository.Update(It.IsAny<Patient>()));


            await this._appointmentService.SaveAsync(patient, this._currentUser);

            this._mockedUserManager.Verify(a => a.GetRolesAsync(It.IsAny<string>()), Times.Once());
            this._mockedUnitOfWork.Verify(a => a.PatientRepository.GetByID(It.IsAny<object>()), Times.Once());
            this._mockedUnitOfWork.Verify(a => a.PatientRepository.Update(It.IsAny<Patient>()), Times.Never());

        }


        [TestMethod]
        public async Task SaveAsyncTestUpdateDoctorCanOnlyUpdateRespectiveObjectProperties()
        {

            Patient patient = new Patient
            {
                FirstName = "UNK",
                LastName = "UNK",
                Gender = GenderTypes.FEMALE,
                completed = true,
                Appointment = DateTime.Now,
                Id = 1,
                DoctorComment="bah blah blah"
            };

            this._mockedUserManager.Setup(a => a.GetRolesAsync(It.IsAny<string>())).Returns(Task.FromResult((IList<string>)new List<string> {
                RoleTypes.DOCTOR
            }));

            Patient result = new Patient
            {
                FirstName = "blah",
                LastName = "blah",
                Gender = GenderTypes.MALE,
                completed = false,
                Appointment = DateTime.Now,
                Id = 1

            };
            this._mockedUnitOfWork.Setup(a => a.PatientRepository.GetByID(It.IsAny<object>())).Returns(result);

            // this._unitOfWork.PatientRepository.GetByID(patient.Id)

            this._mockedUnitOfWork.Setup(a => a.PatientRepository.Update(It.Is<Patient>(c=>c.DoctorComment==patient.DoctorComment 
            && c.completed==patient.completed && c.FirstName==result.FirstName & c.LastName==result.LastName && c.Gender==result.Gender && c.Appointment == result.Appointment)));


            await this._appointmentService.SaveAsync(patient, this._currentUser);

            this._mockedUserManager.Verify(a => a.GetRolesAsync(It.IsAny<string>()), Times.Once());
            this._mockedUnitOfWork.Verify(a => a.PatientRepository.GetByID(It.IsAny<object>()), Times.Once());
            this._mockedUnitOfWork.Verify(a => a.PatientRepository.Update(It.Is<Patient>(c => c.DoctorComment == patient.DoctorComment
           && c.completed == patient.completed && c.FirstName == result.FirstName & c.LastName == result.LastName && c.Gender == result.Gender && c.Appointment == result.Appointment)), Times.Once());


        }
        [TestMethod]
        public async Task SaveAsyncTestThatNurseCanPerformInsertOnRespectiveFields()
        {

            Patient patient = new Patient
            {
                FirstName = "blah",
                LastName = "blah",
                Gender = GenderTypes.MALE,
                completed = true,
                Appointment = DateTime.Now,
                DoctorComment = "blahdfdda",

            };

            this._mockedUserManager.Setup(a => a.GetRolesAsync(It.IsAny<string>())).Returns(Task.FromResult((IList<string>)new List<string> {
                RoleTypes.NURSE
            }));

            this._mockedUnitOfWork.Setup(a => a.PatientRepository.Insert(It.Is<Patient>(c => c.DoctorComment == null
           && c.completed == false && c.FirstName == patient.FirstName && c.LastName == patient.LastName && c.Gender==patient.Gender && c.Appointment == patient.Appointment)));


            await this._appointmentService.SaveAsync(patient, this._currentUser);

            this._mockedUserManager.Verify(a => a.GetRolesAsync(It.IsAny<string>()), Times.Once());
            

            this._mockedUnitOfWork.Verify(a => a.PatientRepository.Insert(It.Is<Patient>(c => c.DoctorComment == null
         && c.completed == false && c.FirstName == patient.FirstName && c.LastName == patient.LastName && c.Gender == patient.Gender && c.Appointment == patient.Appointment)), Times.Once());

        }

    }
}