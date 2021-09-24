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
        public Patient(string Name, string ID, DateTime Birthdate, DateTime RegisterDate, DateTime FireDate)
        {
            this.Name = Name;
            this.ID = ID;
            this.birthdate = Birthdate;
            this.registerDate = RegisterDate;
            this.fireDate = FireDate;
            SetAge();
        }

        public string Name { get; set; }
        public string ID { get; set; }
        public int Age { get; set; }

        private DateTime birthdate;
        public DateTime Birthdate
        {
            get { return this.birthdate; }
            set
            {
                SetBirthDate(value);
            }
        }
        private DateTime registerDate;
        public DateTime RegisterDate
        {
            get { return registerDate; }
            set { SetRegisterDate(value); }
        }

        private DateTime fireDate;
        public DateTime FireDate
        {
            get { return fireDate; }
            set { SetFireDate(value); }
        }
        public int Index { get; set; }

        public override string ToString()
        {
            return $"{Name}, {ID}, {birthdate}, {registerDate}, {fireDate}";
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
                throw new InvalidOperationException("Date can't be in the fututre");
        }

        private void SetRegisterDate(DateTime value)
        {
            if (DateTime.Now.CompareTo(value) > 0)
            {
                this.registerDate = value;
            }
            else
                throw new InvalidOperationException("Date can't be in the fututre");
        }

        private void SetFireDate(DateTime value)
        {
            var date = registerDate;
            if (DateTime.Now.CompareTo(value) > 0)
            {
                if (registerDate.CompareTo(value) < 0)
                    this.fireDate = value;
                else
                    throw new InvalidOperationException("The Fire Date can't be before the Register date");
            }
            else
                throw new InvalidOperationException("Date can't be in the fututre");
        }
    }

}
