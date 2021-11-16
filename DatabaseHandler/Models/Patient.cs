using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DatabaseHandler.Models
{
    public class Patient
    {
#nullable enable
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Patient()
        { }
        public Patient(string Name, string PatientNumber, DateTime Birthdate)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            this.PatientNumber = PatientNumber;
            this.Name = Name;
            this.birthdate = Birthdate;
            SetAge();
        }

        [Key]
        public int PatientID { get; set; }
        public string EnsuranceCompany { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int AdressID { get; set; }
        public Adress Adress { get; set; }
        public string? StudentNumber { get; set; }
        public string? WorkerNumber { get; set; }
        public string Name { get; set; }
        public string PatientNumber { get; set; }
        public bool IsMale { get; set; }
        public int Age { get; set; }
        public bool IsStudent { get; set; }

        public int Index { get; set; }

        private DateTime birthdate;
        public DateTime Birthdate
        {
            get { return this.birthdate; }
            set
            {
                SetBirthDate(value);
            }
        }

        public string SetGenderString()
        {
            if (IsMale)
                return "Man";
            else
                return "Vrouw";
        }

        public override string ToString()
        {
            return $"name: {Name}, PatientNumber: {PatientNumber}, Birthdate: {birthdate}, Email: {Email}, PhoneNumber: {PhoneNumber}";
        }

        public void SetAge()
        {
            Age = DateTime.Now.Year - birthdate.Year;
            if (DateTime.Now.Month == birthdate.Month && DateTime.Now.Day <= birthdate.Day)
                Age--;
            else if (DateTime.Now.Month < birthdate.Month)
                Age--;
        }

        private void SetBirthDate(DateTime value)
        {
            if (DateTime.Now.CompareTo(value) > 0)
            {
                this.birthdate = value;
                SetAge();
            }
            else
                Age = 0;
        }
    }

}
