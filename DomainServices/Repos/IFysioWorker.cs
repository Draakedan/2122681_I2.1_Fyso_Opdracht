using System.Collections.Generic;
using System.Linq;
using DomainModels.Models;

namespace DomainServices.Repos
{
    public interface IFysioWorker
    {
        void AddFysioWorker(FysioWorker fysioWorker);
        bool FysioWorkerExists(int id);
        List<FysioWorker> GetAllFysioWorkers();
        FysioWorker GetFysioWorkerByID(int id);
        FysioWorker GetFysioWorkerByEmail(string email);
        void UpdateFysioWorker(FysioWorker worker);
    }
}