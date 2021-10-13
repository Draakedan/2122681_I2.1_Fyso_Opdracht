using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FysioApp.Models
{
    public class FysioWorker
    {
        public int FysioWorkerID { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string WorkerNumber { get; set; }

        public string BIGNumber { get; set; }

        public string StudentNumber { get; set; }

        public bool IsStudent { get; set; }

        public override string ToString()
        {
            if (IsStudent)
                return $"ID: {FysioWorkerID}, Name: {Name}, Email: {Email}, PhoneNumber: {PhoneNumber}, StudentNumber: {StudentNumber}";
            return $"ID: {FysioWorkerID}, Name: {Name}, Email: {Email}, PhoneNumber: {PhoneNumber}, WorkerNumber: {WorkerNumber}, BIGNumber: {BIGNumber}";
        }
    }
}
