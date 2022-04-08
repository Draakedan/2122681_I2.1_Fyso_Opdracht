using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using DomainModels.Models;

namespace DatabaseHandler.Data
{
    class Seed
    {
        private readonly FysioDataContext Context;
        private Adress Adress;
        private Adress Adress2;

        public Seed(FysioDataContext context)
        {
            Context = context;
            SeedAddresses();
            SeedFysioWorkers();
            SeedPatients();
        }

        public void SeedAddresses()
        {
            Adress = new()
            {
                Country = "Nederland",
                City = "Breda",
                PostalCode = "4818 AJ",
                Street = "Lovensdijkstraat",
                HouseNumber = "61"
            };
            Context.Adresses.Add(Adress);

            Adress2 = new()
            {
                Country = "Nederland",
                City = "Breda",
                PostalCode = "4818 CR",
                Street = "Hogeschoollaan",
                HouseNumber = "1"
            };
            Context.Adresses.Add(Adress2);

            Context.SaveChanges();
        }

        public void SeedFysioWorkers()
        {
            FysioWorker worker = new()
            {
                Name = "Rudolf de Rooie",
                Email = "rnederooie@avans.nl",
                PhoneNumber = "+2990112358",
                WorkerNumber = "012343",
                BIGNumber = "0012345678",
                IsStudent = false
            };
            Context.FysioWorkers.Add(worker);

            FysioWorker student = new()
            {
                Name = "Pino Smidt",
                Email = "pinsmidt@avans.nl",
                PhoneNumber = "+310118999",
                StudentNumber = "881999119",
                IsStudent = true

            };
            Context.FysioWorkers.Add(student);

            Context.SaveChanges();
        }

        public void SeedPatients()
        {
            Patient patient = new()
            {
                EnsuranceCompany = "Insured",
                Email = "Ahmekie@avans.nl",
                PhoneNumber = "+310606060606",
                StudentNumber = "2122693",
                Name = "Ahfieds Ekielhe",
                AdressID = Adress.AdressID,
                Adress = Adress,
                PatientNumber = "000555333111",
                IsMale = true,
                Age = 19,
                Birthdate = DateTime.Now.AddYears(-19).AddDays(-5).AddMonths(-3),
                ImageUrl = "https://i.imgur.com/EHW5HhX.png",
                IsStudent = true
            };
            Context.Patients.Add(patient);

            Patient patient2 = new()
            {
                EnsuranceCompany = "Interpolis",
                Email = "Rugdeklac@avans.nl",
                PhoneNumber = "+315454767658",
                WorkerNumber = "4654676487845",
                Adress = Adress2,
                AdressID = Adress2.AdressID,
                Name = "Renie de Klachel",
                PatientNumber = "3214334567",
                ImageUrl = "https://i.imgur.com/EHW5HhX.png",
                IsMale = true,
                Age = 45,
                Birthdate = DateTime.Now.AddYears(-45).AddDays(-17).AddMonths(-6),
                IsStudent = false
            };
            Context.Patients.Add(patient2);

            Context.SaveChanges();
        }
    }
}
