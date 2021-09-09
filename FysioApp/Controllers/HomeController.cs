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
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (Repository.GetSize() == 0)
            {
                GeneratePatientList();
                return View(Repository.Patients);
            }
            foreach (Patient p in Repository.Patients)
            {
                Console.WriteLine($"{p}, {p.ToString()}");
            }
            Console.WriteLine($"{Repository.GetSize()} \n");
            return View(Repository.Patients);
        }

        private static void GeneratePatientList() 
        {
            Repository.AddPatient(new("Kira", "012345", new DateTime(1998, 12, 13), DateTime.Now));
            Repository.AddPatient(new("Frank", "344660", new DateTime(1975, 5, 20), DateTime.Now));
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
            if (ModelState.GetValidationState(nameof(patient.Birthdate)) == ModelValidationState.Valid && patient.Birthdate >= DateTime.Now)
                ModelState.AddModelError(nameof(patient.Birthdate), "Datum kan niet later dan vandaag");
            if (ModelState.GetValidationState(nameof(patient.RegisterDate)) == ModelValidationState.Valid && patient.RegisterDate >= DateTime.Now)
                ModelState.AddModelError(nameof(patient.RegisterDate), "Datum kan niet later dan vandaag");

            if (ModelState.IsValid)
            {
                Repository.AddPatient(patient);
                //Console.WriteLine(patient.ToString());
                return View("Index", Repository.Patients);
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
