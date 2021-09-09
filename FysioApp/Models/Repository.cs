using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FysioApp.Models
{
    public class Repository
    {
        private static List<Patient> patients = new();

        public static IEnumerable<Patient> Patients => patients;

        public static void AddPatient(Patient patient) 
        {
            patients.Add(patient);
            for (int i = 0; i < patients.Count; i++)
            {
                //Console.WriteLine(patients[i]);
                //Console.WriteLine(patients[i].ToString());
            }
        }

        public static int GetSize()
        {
            return patients.Count;
        }

    }
}
