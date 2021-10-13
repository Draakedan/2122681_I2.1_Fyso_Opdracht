using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DatabaseHandler.Models
{
    public class FysioWorker
    {
        [Key]
        public int FysioWorkerID { get; set; }
#nullable enable
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? WorkerNumber { get; set; }

        public string? BIGNumber { get; set; }

        public string? StudentNumber { get; set; }

        [Required]
        public bool IsStudent { get; set; }
    }
}
