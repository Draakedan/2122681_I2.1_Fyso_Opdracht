using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DatabaseHandler.Models
{
    public class TherapySession
    {
#nullable enable

        [Key]
        public int TherapySessionId { get; set; }

        public int Type { get; set; }

        public string? Description { get; set; }

        public string? Location { get; set; }

        public string? Specials { get; set; }

        public int SessionDoneByID { get; set; }

        public FysioWorker SesionDoneBy { get; set; }

        public DateTime SessionTime { get; set; }
    }
}
