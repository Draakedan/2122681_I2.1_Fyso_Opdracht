using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseHandler.Models
{
    public class AdressRepository : IRepository<Adress>
    {
        public List<Adress> Items { get; init; }

        public AdressRepository()
        {
            Items = new();
        }

        public void Add(Adress elem)
        {
            Items.Add(elem);
        }

        public bool Exists(int id)
        {
            try
            {
                return Items.Contains(Items[id]);
            }
            catch {
                return false;
            }
        }

        public Adress Get(int id)
        {
            foreach (Adress a in Items)
                if (a.AdressID == id)
                    return a;
            return null;
        }

        public List<Adress> GetAll()
        {
            return Items;
        }

        public Adress GetItemByID(int id)
        {
            foreach (Adress a in Items)
                if (a.AdressID == id)
                    return a;
            return null;
        }
    }
}
