using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FysioApp.Models
{
    public interface IRepository
    {
        List<Patient> Patients { init; }
        void Add(Patient patient);
        Patient Get(int id);
        bool Exists(int id);

        List<Patient> GetAll();
    }
}
