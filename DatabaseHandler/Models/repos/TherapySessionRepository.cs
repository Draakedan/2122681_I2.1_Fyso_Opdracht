﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseHandler.Models
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
            foreach (TherapySession s in Items)
                if (s.Id == id)
                    return s;
            return null;
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
