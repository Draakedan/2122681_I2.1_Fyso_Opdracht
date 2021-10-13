using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FysioApp.Models
{
    public class PatientFileRepository : IRepository<PatientFile>
    {
        public List<PatientFile> Items { get; init; }

        public PatientFileRepository() => Items = new();

        public void Add(PatientFile elem)
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

        public PatientFile Get(int id)
        {
            return Items[id];
        }

        public List<PatientFile> GetAll()
        {
            return Items;
        }

        public PatientFile GetItemByID(int id)
        {
            foreach (PatientFile p in Items)
                if (p.ID == id)
                    return p;
            return null;
        }
    }
}
