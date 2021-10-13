using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseHandler.Models;
using Newtonsoft.Json.Linq;

namespace DatabaseHandler.Data
{
    public class DataRepository
    {
        public FysioDataContext context = new();

        public JArray GetActionPlans()
        {
             return JArray.FromObject(context.ActionPlans.ToArray());
        }

        public JArray GetAdresses()
        {
            return JArray.FromObject(context.Adresses.ToArray());
        }

        public JArray GetComments()
        {
            return JArray.FromObject(context.Comments.ToArray());
        }

        public JArray GetFysioWorkers()
        {
            return JArray.FromObject(context.FysioWorkers.ToArray());
        }

        public JArray GetPatients()
        {
            return JArray.FromObject(context.Patients.ToArray());
        }

        public JArray GetPatientFiles()
        {
            return JArray.FromObject(context.PatientFiles.ToArray());
        }

        public JArray GetTherapySessions()
        {
            return JArray.FromObject(context.TherapySessions.ToArray());
        }
    }
}
