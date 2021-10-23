using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace VerkisListDatabaseHandler.Models
{
    public class Verrichting
    {
        public Verrichting()
        { }
#nullable enable

        [Key]
        public int Id { get; set; }
        public string? Waarde { get; set; }
        public string? Omschrijving { get; set; }
        public string? Toelichting_verplicht { get; set; }
    }
}
