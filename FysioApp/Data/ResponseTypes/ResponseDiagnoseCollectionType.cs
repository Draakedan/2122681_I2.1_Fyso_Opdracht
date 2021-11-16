using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FysioAppUX.Models;

namespace FysioAppUX.Data.ResponseTypes
{
    public class ResponseDiagnoseCollectionType
    {
        public List<Diagnose> diagnoses { get; set; }
        public List<Diagnose> some { get; set; }
    }
}
