using System.Collections.Generic;
using DomainModels.Models;
using DomainServices.Repos;
using DatabaseHandler.Data;

namespace DatabaseHandler.Models
{
    public class AdressRepository : IAdress
    {
        private readonly FysioDataContext Context;

        public AdressRepository(FysioDataContext context) => Context = context;

        public void AddAdress(Adress adress)
        {
            Context.Adresses.Add(adress);
            Context.SaveChanges();
        }

        public bool AdressExists(int id) => GetAdressByID(id) != null;

        public List<Adress> GetAllAdresses()
        {
            List<Adress> adressList = new();
            foreach (Adress adress in Context.Adresses)
                adressList.Add(adress);
            return adressList;
        }

        public Adress GetAdressByID(int id)
        {
            foreach (Adress a in Context.Adresses)
                if (a.AdressID == id)
                    return a;
            return null;
        }

        public void UpdateAdress(Adress adress)
        {
            Context.Adresses.Update(adress);
            Context.SaveChanges();
        }
    }
}