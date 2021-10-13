using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FysioApp.Models
{
    public interface IRepository<T>
    {
        List<T> Items { get; init; }

        void Add(T elem);

        T Get(int id);

        bool Exists(int id);

        List<T> GetAll();

        T GetItemByID(int id);
    }
}
