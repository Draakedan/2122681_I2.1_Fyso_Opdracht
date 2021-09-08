using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FysioApp.Models
{
    public class Patient
    {

        public Patient(string Name, string ID, DateTime Birthdate, DateTime RegisterDate)
        {
            this.Name = Name;
            this.ID = ID;
            this.Birthdate = Birthdate;
            this.RegisterDate = RegisterDate;
        }

        public string Name { get; }
        public string ID { get; }
        public DateTime Birthdate { get; }
        public DateTime RegisterDate { get; }

    }
}
