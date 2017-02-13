using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MedicalWebService.Controllers;
using MedicalWebService.Services.Interfaces;
using Moq;
using System.Threading.Tasks;
using MedicalWebService.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.AspNet.Identity;
using MedicalWebService.Enums;
using Microsoft.AspNet.Identity.Owin;

namespace MedicalWebService.Tests.Controllers
{
    [TestClass]
    public class AppointmentControllerTest
    {
        private AppointmentController controller;
        private Mock<IAppointmentService> mockService;
        private ApplicationUser _currentUser;

        private Mock<ApplicationUserManager> _mockedUserManager;

        [TestInitialize]
        public void init()
        {
            this._currentUser = new ApplicationUser
            {
                Id = "1234"
            };

            var userStore = new Mock<IUserStore<ApplicationUser>>();

            var options = new Mock<IdentityFactoryOptions<ApplicationUserManager>>();

            this._mockedUserManager = new Mock<ApplicationUserManager>(userStore.Object, options.Object);

            this.mockService = new Mock<IAppointmentService>();
            this.controller = new AppointmentController(this.mockService.Object, _mockedUserManager.Object);

        }
        [TestMethod]
        public void TestGetThatReturnsResult()
        {
            this.mockService.Setup(a => a.getPatientsAsync()).Returns(Task.FromResult(new List<Patient> {
                new Patient {
                    Id =12,
                    completed =false,
                    Appointment =DateTime.Now,
                    Gender =Enums.GenderTypes.MALE,
                    DoctorComment = "",
                    FirstName = "",
                    LastName = "",
                    RowVersion =  new byte[] { }
                },
                new Patient {
                    Id =11,
                    completed =false,
                    Appointment =DateTime.Now,
                    Gender =Enums.GenderTypes.FEMALE,
                    DoctorComment = "",
                    FirstName = "",
                    LastName = "",
                    RowVersion =  new byte[] { }
                }
            }));

            List<PatientViewModel> results = this.controller.Get().Result;

            this.mockService.Verify(a => a.getPatientsAsync(), Times.Once());
            Assert.AreEqual(results.Count, 2);

        }
        [TestMethod]
        public void TestGetThatReturnsNoResult()
        {
            this.mockService.Setup(a => a.getPatientsAsync()).Returns(Task.FromResult(new List<Patient>()));

            List<PatientViewModel> results = this.controller.Get().Result;

            this.mockService.Verify(a => a.getPatientsAsync(), Times.Once());
            Assert.AreEqual(results.Count, 0);

        }
    }
}
