using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModels.Models;
using DomainServices.Repos;

namespace InfrastructureAPIHandler.Data.ResponseTypes
{
    public class ResponseDiagnoseCollectionType
    {
        public List<Diagnose> Diagnoses { get; set; }
    }
}
