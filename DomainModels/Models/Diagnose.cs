using System;

namespace DomainModels.Models
{
    public class Diagnose
    {
        public int Id { get; set; }
        public int? Code { get; set; }
        public string Lichaamslocalisatie { get; set; }
        public string Pathologie { get; set; }
    }
}