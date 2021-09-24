using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FysioApp.Models
{
    public class Repository : IRepository
    {
        public List<Patient> Patients { get; init; }

        public Repository() => Patients = new List<Patient>();

        public void Add(Patient patient) 
        {
            patient.Index = GetSize();
            Patients.Add(patient);
        }

        public Patient Get(int id)
        {
            return Patients[id];
        }

        public int GetSize()
        {
            return Patients.Count;
        }

        public bool Exists(int id)
        {
            try
            {
                return Patients.Contains(Patients[id]);
            }
            catch 
            {
                return false;
            }
        }

        public List<Patient> GetAll() 
        {
            return Patients;
        }

    }
}
