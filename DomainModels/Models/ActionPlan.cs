using System.ComponentModel.DataAnnotations;

namespace DomainModels.Models
{
    public class ActionPlan
    {
        [Key]
        public int ActionID { get; set; }
        public int SessionsPerWeek { get; set; }
        public int TimePerSession { get; set; }
    }
}