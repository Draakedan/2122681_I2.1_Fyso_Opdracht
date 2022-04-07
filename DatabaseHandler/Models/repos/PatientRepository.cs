using System;
using System.Collections.Generic;
using DomainModels.Models;
using DomainServices.Repos;
using DatabaseHandler.Data;

namespace DatabaseHandler.Models
{
    public class PatientRepository : IPatient
    {
        private readonly FysioDataContext Context;
        public PatientRepository(FysioDataContext context) => Context = context;

        public void AddPatient(Patient patient)
        {
            Context.Patients.Add(patient);
            Context.SaveChanges();
        }

        public bool PatientExists(int id) => GetPatientByID(id) != null;

        public List<Patient> GetAllPatients()
        {
            List<Patient> patientList = new();
            foreach (Patient p in Context.Patients)
            {
                p.Adress = new AdressRepository(Context).GetAdressByID(p.AdressID);
                patientList.Add(p);
            }
            return patientList;
        }

        public Patient GetPatientByID(int id)
        {
            foreach (Patient p in Context.Patients)
                if (p.PatientID == id)
                {
                    p.Adress = new AdressRepository(Context).GetAdressByID(p.AdressID);
                    return p;
                }
            return null;
        }

        public Patient GetPatientByEmail(string email)
        {
            if (email == "default@patient.com")
                foreach (Patient p in Context.Patients)
                    return p;
            foreach (Patient p in Context.Patients)
                if (p.Email == email)
                {
                    p.Adress = new AdressRepository(Context).GetAdressByID(p.AdressID);
                    return p;
                }
            return null;
        }

        public void UpdatePatient(Patient patient)
        {
            Context.Patients.Update(patient);
            Context.SaveChanges();
        }
    }
}