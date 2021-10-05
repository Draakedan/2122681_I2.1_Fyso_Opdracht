using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FysioApp.Models
{
    public class ActionPlan
    {
        public int ActionID { get; set; }

        public int SessionsPerWeek { get; set; }

        public int TimePerSession { get; set; }

        public override string ToString()
        {
            return $"ID: {ActionID}, Sessions per week: {SessionsPerWeek}, Time per session{TimePerSession}";
        }
    }
}
