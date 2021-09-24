using FysioApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FysioApp.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly IRepository _repository;

        public HomeController(/*ILogger<HomeController> logger,*/ IRepository repository)
        {
            //_logger = logger;
            _repository = repository;
        }

        [Route("Home/Index")]
        [Route("Home/Patients")]
        [Route("/")]
        [Route("Patients")]
        public IActionResult Index()
        {
            if (_repository.GetAll().Count == 0)
            {
                GeneratePatientList();
                return View(_repository.GetAll());
            }
            return View(_repository.GetAll());
        }

        [Route("Home/Patients/{id:int}")]
        public IActionResult GetPatientDetails(int id)
        {
            if (_repository.Exists(id))
                return View("PatientDetails", _repository.Get(id));
            else return View("NotFound");
        }

        private void GeneratePatientList() 
        {
            _repository.Add(new("Kira", "012345", new DateTime(1999, 12, 17), DateTime.Now, DateTime.Now.AddDays(20)));
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
            if (ModelState.GetValidationState(nameof(patient.ID)) == ModelValidationState.Valid && patient.ID == null)
                ModelState.AddModelError(nameof(patient.ID), "ID mag niet leeg zijn!");
            if (ModelState.GetValidationState(nameof(patient.Birthdate)) == ModelValidationState.Valid && patient.Birthdate > DateTime.Now)
                ModelState.AddModelError(nameof(patient.Birthdate), "Datum kan niet later dan vandaag");
            if (ModelState.GetValidationState(nameof(patient.RegisterDate)) == ModelValidationState.Valid && patient.RegisterDate > DateTime.Now)
                ModelState.AddModelError(nameof(patient.RegisterDate), "Datum kan niet later dan vandaag");
            if (ModelState.GetValidationState(nameof(patient.FireDate)) == ModelValidationState.Valid && patient.FireDate <= patient.RegisterDate)
                ModelState.AddModelError(nameof(patient.FireDate), "Datum ontslag kan niet voorafgaand aan de registratie datum gaan");

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
