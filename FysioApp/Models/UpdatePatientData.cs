using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModels.Models;

namespace FysioAppUX.Models
{
    public class UpdatePatientData
    {
        public Patient Patient { get; set; }
        public Adress Adress { get; set; }
        public int DossierID { get; set; }

        public UpdatePatientData(Patient patient, int dossierID)
        {
            Patient = patient;
            Adress = patient.Adress;
            DossierID = dossierID;
        }
    }
}
