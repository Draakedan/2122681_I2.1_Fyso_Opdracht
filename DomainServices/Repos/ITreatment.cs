using System.Collections.Generic;
using DomainModels.Models;

namespace DomainServices.Repos
{
    public interface ITreatment
    {
        bool TreatmentExists(string id);
        List<Treatment> GetAllTreatments();
        Treatment GetTreatmentByID(string id);
    }
}