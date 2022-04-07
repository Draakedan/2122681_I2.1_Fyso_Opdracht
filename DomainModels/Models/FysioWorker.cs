using System;
using System.ComponentModel.DataAnnotations;

namespace DomainModels.Models
{
    public class FysioWorker
    {
        [Key]
        public int FysioWorkerID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string WorkerNumber { get; set; }
        public string BIGNumber { get; set; }
        public string StudentNumber { get; set; }
        public bool IsStudent { get; set; }
        public string AvailableDays { get; set; }
        public DateTime DayStartTime { get; set; }
        public DateTime DayEndTime { get; set; }
    }
}