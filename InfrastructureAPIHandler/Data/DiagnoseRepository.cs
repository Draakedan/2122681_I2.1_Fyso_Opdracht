using DomainModels.Models;
using DomainServices.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureAPIHandler.Data
{
    public class DiagnoseRepository : IDiagnose
    {

        private readonly OwnerConsumer _consumer;

        public DiagnoseRepository(OwnerConsumer consumer)
        {
            _consumer = consumer;
        }

        public List<Diagnose> Diagnoses { get; init; }

        public bool DiagnoseExists(int id)
        {
            return _consumer.GetOneDiagnose(id) != null;
        }

        public List<Diagnose> GetAllDiagnoses()
        {
            return _consumer.GetAllDiagnoses().Result;
        }

        public Diagnose GetDiagnoseByID(int id)
        {
            return _consumer.GetOneDiagnose(id).Result;
        }
    }
}