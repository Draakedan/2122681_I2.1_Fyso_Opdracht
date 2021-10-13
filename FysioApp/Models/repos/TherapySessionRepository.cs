using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FysioApp.Models
{
    public class TherapySessionRepository : IRepository<TherapySession>
    {
        public List<TherapySession> Items { get; init; }

        public TherapySessionRepository()
        {
            Items = new();
        }

        public void Add(TherapySession elem)
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

        public TherapySession Get(int id)
        {
            return Items[id];
        }

        public List<TherapySession> GetAll()
        {
            return Items;
        }

        public TherapySession GetItemByID(int id)
        {
            foreach (TherapySession t in Items)
                if (t.Id == id)
                    return t;
            return null;
        }
    }
}
