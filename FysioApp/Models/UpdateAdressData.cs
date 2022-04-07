using DomainModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FysioAppUX.Models
{
    public class UpdateAdressData
    {
        public Adress Adress { get; set; }
        public int PatientID { get; set; }

        public UpdateAdressData(Adress adress, int patientId)
        {
            Adress = adress;
            PatientID = patientId;
        }
    }
}
