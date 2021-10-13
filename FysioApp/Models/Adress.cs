using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FysioApp.Models
{
    public class Adress
    {
        public int AdressID { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }

        public override string ToString()
        {
            return $"AdressID: {AdressID}, Country: {Country}, City: {City}, PostalCode: {PostalCode}, Street: {Street}, HouseNumber: {HouseNumber}";
        }
    }
}
