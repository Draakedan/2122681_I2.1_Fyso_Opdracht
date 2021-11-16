using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FysioAppUX.Models
{
    public class Diagnose
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("code")]
        public int? Code { get; set; }

        [JsonPropertyName("lichaamslocalisatie")]
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? lichaamslocalisatie { get; set; }

        [JsonPropertyName("pathologie")]
        public string? Pathologie { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Code)}: {Code}, {nameof(lichaamslocalisatie)}: {lichaamslocalisatie}, {nameof(Pathologie)}: {Pathologie}";
        }
    }
}
