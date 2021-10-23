using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DatabaseHandler.Models;
using System.Linq;
using System.Threading.Tasks;

namespace FysioAppUX.Components
{
    public class TotalPatients : ViewComponent
    {
        readonly IRepository<Patient> _repository;

        public TotalPatients(IRepository<Patient> repository)
        {
            _repository = repository;
        }

        public string Invoke()
        {
            return $"Total Patients: {_repository.GetAll().Count}";
        }
    }
}
