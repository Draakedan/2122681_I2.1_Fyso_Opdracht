using System.Collections.Generic;
using DomainModels.Models;

namespace DomainServices.Repos
{
    public interface IDiagnose
    {
        List<Diagnose> Diagnoses { get; init; }
        bool DiagnoseExists(int id);
        List<Diagnose> GetAllDiagnoses();
        Diagnose GetDiagnoseByID(int id);
    }
}