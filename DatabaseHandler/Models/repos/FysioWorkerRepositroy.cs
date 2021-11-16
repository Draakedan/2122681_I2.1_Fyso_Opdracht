using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseHandler.Models
{
    public class FysioWorkerRepositroy : IRepository<FysioWorker>
    {

        public List<FysioWorker> Items { get; init; }

        public FysioWorkerRepositroy()
        {
            Items = new();
        }

        public void Add(FysioWorker elem)
        {
            Items.Add(elem);
        }

        public bool Exists(int id)
        {
            try
            {
                return Items.Contains(Items[id]);
            }
            catch
            {
                return false;
            }
        }

        public FysioWorker Get(int id)
        {
            foreach (FysioWorker f in Items)
                if (f.FysioWorkerID == id)
                    return f;
            return null;
        }

        public List<FysioWorker> GetAll()
        {
            return Items;
        }

        public FysioWorker GetItemByID(int id)
        {
            foreach (FysioWorker f in Items)
                if (f.FysioWorkerID == id)
                    return f;
            return null;
        }
    }
}
