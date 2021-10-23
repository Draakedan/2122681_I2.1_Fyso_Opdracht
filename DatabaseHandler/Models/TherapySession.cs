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
        public int Id { get; set; }

        public int Type { get; set; }

        public string Description { get; set; }

        public bool IsPractiseRoom { get; set; }

        public string? Specials { get; set; }

        public int SessionDoneByID { get; set; }

        public FysioWorker SesionDoneBy { get; set; }

        public DateTime SessionStartTime { get; set; }
        public DateTime SessionEndTime { get; set; }

        public override string ToString()
        {
            return $"ID: {Id}, Type: {Type}, Description: {Description}, Location: {IsPractiseRoom}, Specials: {Specials ?? "none"}, Session Done By: {SesionDoneBy}, SessionTime: {SessionStartTime}";
        }
    }
}
