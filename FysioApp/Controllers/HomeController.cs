using FysioApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
            return View(GeneratePatientList());
        }

        private static List<Patient> GeneratePatientList() 
        {
            List<Patient> patients = new()
            {
                new Patient("Kira", "012345", new DateTime(1998, 12, 13), DateTime.Now),
                new Patient("Frank", "344660", new DateTime(1975, 5, 20), DateTime.Now)
            };
            return patients;
        }

        public IActionResult NewPatient()
        {
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
