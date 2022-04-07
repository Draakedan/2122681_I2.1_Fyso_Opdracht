using DomainModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FysioAppUX.Models
{
    public class DossierData
    {
        public PatientFile Dossier { get; set; }
        public bool IsPatient { get; set; }

        public DossierData(PatientFile dossier, bool isPatient)
        {
            Dossier = dossier;
            IsPatient = isPatient;
        }

    }
}
