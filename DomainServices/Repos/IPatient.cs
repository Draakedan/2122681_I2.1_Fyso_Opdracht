using System.Collections.Generic;
using DomainModels.Models;

namespace DomainServices.Repos
{
    public interface IPatient
    {
        void AddPatient(Patient patient);
        bool PatientExists(int id);
        List<Patient> GetAllPatients();
        Patient GetPatientByID(int id);
        Patient GetPatientByEmail(string email);
        void UpdatePatient(Patient patient);
    }
}