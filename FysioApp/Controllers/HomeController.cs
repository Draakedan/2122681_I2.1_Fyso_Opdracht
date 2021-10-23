using DatabaseHandler.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FysioAppUX.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace FysioAppUX.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly IRepository<Patient> _repository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly DataReviever _dataReviever;

        public HomeController(/*ILogger<HomeController> logger,*/ IRepository<Patient> repository, DataReviever reviever, UserManager<IdentityUser> userManager)
        {
            //_logger = logger;
            _repository = repository;
            _userManager = userManager;
            _dataReviever = reviever;
        }

        [Route("Home/Index")]
       
        [Route("/")]
        
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [Authorize(Roles = "PhysicalTherapist, Intern")]
        public IActionResult Patients()
        {
            return View(_repository.GetAll()/*.ToList()*/);
        }

        [Authorize]
        [Authorize(Roles = "PhysicalTherapist, Intern")]
        [Route("Home/Patients/{id:int}")]
        public IActionResult GetPatientDetails(int id)
        {
            if (_repository.Exists(id))
                return View("PatientDetails", _repository.Get(id));
            else return View("NotFound");
        }

        [Authorize(Roles = "PhysicalTherapist")]
        [HttpGet]
        public IActionResult NewPatient()
        {
            return View();
        }

        [Authorize(Roles = "PhysicalTherapist")]
        [HttpPost]
        public IActionResult NewPatient(Patient patient)
        {
            if (ModelState.GetValidationState(nameof(patient.Name)) == ModelValidationState.Valid && patient.Name == null)
                ModelState.AddModelError(nameof(patient.Name), "Naam mag niet leeg zijn!");
            if (ModelState.GetValidationState(nameof(patient.PatientNumber)) == ModelValidationState.Valid && patient.PatientNumber == null)
                ModelState.AddModelError(nameof(patient.PatientNumber), "ID mag niet leeg zijn!");
            if (ModelState.GetValidationState(nameof(patient.Birthdate)) == ModelValidationState.Valid && patient.Birthdate > DateTime.Now)
                ModelState.AddModelError(nameof(patient.Birthdate), "Datum kan niet later dan vandaag");

            if (ModelState.IsValid)
            {
                patient.SetAge();
                _repository.Add(patient);
                return View("Patients", _repository.GetAll());
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
