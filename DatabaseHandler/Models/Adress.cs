using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DatabaseHandler.Models
{
    public class Adress
    {
        [Key]
        public int AdressID { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }

        public override string ToString()
        {
            return $"{Country}-{City}-{PostalCode}-{Street}-{HouseNumber}";
        }
    }
}
