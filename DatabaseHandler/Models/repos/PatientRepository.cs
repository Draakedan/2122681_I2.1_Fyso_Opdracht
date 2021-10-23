using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseHandler.Models
{
    public class PatientRepository : IRepository<Patient>
    {
        public List<Patient> Items { get; init; }

        public PatientRepository() => Items = new List<Patient>();

        public void Add(Patient patient) 
        {
            patient.Index = GetSize();
            Items.Add(patient);
        }

        public Patient Get(int id)
        {
            return Items[id];
        }

        public int GetSize()
        {
            return Items.Count;
        }

        public bool Exists(int id)
        {
            try
            {
                return Items.Contains(Items[id]);
            }
            catch 
            {
                return false;
            }
        }

        public List<Patient> GetAll() 
        {
            return Items;
        }

        public List<Patient> GetAll(bool isTest, Patient patient)
        {
            if (isTest)
                Items.Add(patient);
            return GetAll();
        }

        public Patient GetItemByID(int id)
        {
            foreach (Patient p in Items)
                if (p.PatientID == id)
                    return p;
            return null;
        }
    }
}
