using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace VerkisListDatabaseHandler.Models
{
    public class Diagnose
    {
        public Diagnose()
        { }

#nullable enable
        [Key]
        public int Id { get; set; }
        public int? Code { get; set; }
        public string? lichaamslocalisatie { get; set; }
        public string? pathologie { get; set; }
        
    }
}
