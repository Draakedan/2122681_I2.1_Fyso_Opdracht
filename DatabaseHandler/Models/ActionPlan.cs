using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DatabaseHandler.Models
{
    public class ActionPlan
    {
        [Key]
        public int ActionID { get; set; }

        public int SessionsPerWeek { get; set; }

        public int TimePerSession { get; set; }
    }
}
