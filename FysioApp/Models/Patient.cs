using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FysioApp.Models
{
    public class Patient
    {
        public Patient()
        { }
        public Patient(string Name, string ID, DateTime Birthdate, DateTime RegisterDate)
        {
            this.Name = Name;
            this.ID = ID;
            this.Birthdate = Birthdate;
            this.RegisterDate = RegisterDate;
        }

        public string Name { get; set; }
        public string ID { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime RegisterDate { get; set; }

        public string ToString()
        {
            return $"{Name}, {ID}, {Birthdate.ToString()}, {RegisterDate.ToString()}";
        }
    }

}
