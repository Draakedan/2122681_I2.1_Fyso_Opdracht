using System.ComponentModel.DataAnnotations;

namespace DomainModels.Models
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
    }
}