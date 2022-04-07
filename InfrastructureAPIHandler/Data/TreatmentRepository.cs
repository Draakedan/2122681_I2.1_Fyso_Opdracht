using DomainModels.Models;
using DomainServices.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureAPIHandler.Data
{
    public class TreatmentRepository : ITreatment
    {
        public List<Treatment> GetAllTreatments()
        {
            return APIReader.ProcessAllBehandelingen().Result;
        }

        public Treatment GetTreatmentByID(string id)
        {
            return APIReader.ProcessOneBehandeling(id).Result;
        }

        public bool TreatmentExists(string id)
        {
            return APIReader.ProcessOneBehandeling(id).Result != null;
        }
    }
}
