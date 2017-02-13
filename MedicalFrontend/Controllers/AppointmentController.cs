using MedicalFrontend.Enums;
using MedicalFrontend.Models;
using MedicalFrontend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MedicalFrontend.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private IOauthAuthenticationService _oauthService;
        public AppointmentController(IOauthAuthenticationService oauthService)
        {
            this._oauthService = oauthService;
        }
        // GET: Appointment
        public ActionResult Index()
        {
            return View();
        }
       
        public async Task<ActionResult> List()
        {
            
            ICollection<PatientViewModel> patients = await this._oauthService.GetPatientsAsync();
            if (TempData["ModelState"] != null && !ModelState.Equals(TempData["ModelState"]))
                ModelState.Merge((ModelStateDictionary) TempData["ModelState"]);
            return View(patients);
        }
        [Authorize(Roles ="Nurse")]
        public ActionResult Create()
        {
           return View();
        }

        [Authorize(Roles = "Nurse")]
        [HttpPost]
        public async Task<ActionResult> Create(PatientViewModel patient)
        {
            try {
                await this._oauthService.CreatePatientAsync(patient);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(patient);
            }
            return RedirectToAction("List");
        }

        [Authorize(Roles = "Nurse,Doctor")]
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {

           PatientViewModel patient = await _oauthService.GetPatientAsync(id);
           return View(patient);
        }

        [Authorize(Roles = "Nurse,Doctor")]
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {

            PatientViewModel patient = await _oauthService.GetPatientAsync(id);
            return View(patient);
        }

        [Authorize(Roles = "Nurse,Doctor")]
        [HttpPost]
        public async Task<ActionResult> Save(PatientViewModel patient)
        {
            try
            {
                await this._oauthService.CreatePatientAsync(patient);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View("Edit",patient);
            }
            return RedirectToAction("List");
        }

        [Authorize(Roles = "Nurse")]
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await this._oauthService.DeletePatientAsync(id);

            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                TempData["ModelState"] = ModelState;
            }
            return RedirectToAction("List");
        }

    }
}