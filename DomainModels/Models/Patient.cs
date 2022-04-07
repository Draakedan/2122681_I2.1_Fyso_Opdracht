using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DomainModels.Models
{
    public class Patient
    {
        [Key]
        public int PatientID { get; set; }
        public string EnsuranceCompany { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int AdressID { get; set; }
        public Adress Adress { get; set; }
        public string StudentNumber { get; set; }
        public string WorkerNumber { get; set; }
        public string Name { get; set; }
        public string PatientNumber { get; set; }
        public bool IsMale { get; set; }
        public int Age { get; set; }
        public bool IsStudent { get; set; }
        public int Index { get; set; }
        public DateTime Birthdate { get; set; }
        public string ImageUrl { get; set; }
    }
}