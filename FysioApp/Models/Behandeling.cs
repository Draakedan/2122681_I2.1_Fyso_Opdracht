using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FysioAppUX.Models
{
    public class Behandeling
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("waarde")]
        public string Waarde { get; set; }

        [JsonPropertyName("omschrijving")]
        public string Omschrijving { get; set; }

        [JsonPropertyName("toelichting_verplicht")]
        public string Toelichting_verplicht { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Waarde)}: {Waarde}, {nameof(Omschrijving)}: {Omschrijving}, {nameof(Toelichting_verplicht)}, {Toelichting_verplicht}";
        }
    }
}
