using FysioApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FysioApp.Data;

namespace FysioApp.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly IRepository<Patient> _repository;
        private readonly DataReviever _reciever;

        public HomeController(/*ILogger<HomeController> logger,*/ IRepository<Patient> repository, DataReviever dataReviever)
        {
            //_logger = logger;
            _repository = repository;
            _reciever = dataReviever;
        }

        [Route("Home/Index")]
        [Route("Home/Patients")]
        [Route("/")]
        [Route("Patients")]
        public IActionResult Index()
        {
            return View(_repository.GetAll());
        }

        [Route("Home/Patients/{id:int}")]
        public IActionResult GetPatientDetails(int id)
        {
            if (_repository.Exists(id))
                return View("PatientDetails", _repository.Get(id));
            else return View("NotFound");
        }

        [HttpGet]
        public IActionResult NewPatient()
        {
            return View();
        }

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
                return View("Index", _repository.GetAll());
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
