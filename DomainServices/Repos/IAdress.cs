using System.Collections.Generic;
using DomainModels.Models;

namespace DomainServices.Repos
{
    public interface IAdress
    {
        void AddAdress(Adress comment);
        bool AdressExists(int id);
        List<Adress> GetAllAdresses();
        Adress GetAdressByID(int id);
        void UpdateAdress(Adress adress);
    }
}