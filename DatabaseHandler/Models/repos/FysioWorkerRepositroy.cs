using System.Collections.Generic;
using System.Linq;
using DatabaseHandler.Data;
using DomainModels.Models;
using DomainServices.Repos;

namespace DatabaseHandler.Models
{
    public class FysioWorkerRepositroy : IFysioWorker
    {
        private readonly FysioDataContext Context;

        public FysioWorkerRepositroy(FysioDataContext context) => Context = context;

        public void AddFysioWorker(FysioWorker fysioWorker)
        {
            Context.FysioWorkers.Add(fysioWorker);
            Context.SaveChanges();
        }

        public bool FysioWorkerExists(int id) => GetFysioWorkerByID(id) != null;

        public List<FysioWorker> GetAllFysioWorkers() 
        {
            List<FysioWorker> fysioWorkerList = new();
            foreach (FysioWorker worker in Context.FysioWorkers)
            {
                fysioWorkerList.Add(worker);
            }
            return fysioWorkerList;
        }

        public FysioWorker GetFysioWorkerByID(int id)
        {
            foreach (FysioWorker f in Context.FysioWorkers)
                if (f.FysioWorkerID == id)
                    return f;
            return null;
        }

        public void UpdateFysioWorker(FysioWorker worker)
        {
            Context.FysioWorkers.Update(worker);
            Context.SaveChanges();
        }

        public FysioWorker GetFysioWorkerByEmail(string email)
        {
            if (email.Equals("default@therapist.com"))
            {
                foreach (FysioWorker fw in Context.FysioWorkers)
                    if (!fw.IsStudent)
                        return fw;
                return null;
            }
            else if (email.Equals("default@student.com"))
            {
                foreach (FysioWorker fw in Context.FysioWorkers)
                {
                    if (fw.IsStudent)
                        return fw;
                }
                return null;
            }
            else
                foreach (FysioWorker fw in Context.FysioWorkers)
                    if (fw.Email == email)
                        return fw;
            return null;
        }
    }
}