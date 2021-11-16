using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DatabaseHandler.Models
{
    public class TherapySession
    {

        [Key]
        public int Id { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }

        public bool IsPractiseRoom { get; set; }

#nullable enable
        public string? Specials { get; set; }

        public int SessionDoneByID { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public FysioWorker SesionDoneBy { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public DateTime SessionStartTime { get; set; }
        public DateTime SessionEndTime { get; set; }
        public DateTime CreationDate { get; set; }

        public string GetIDString()
        {
            return $"{Type}-{Description}-{IsPractiseRoom}-{SessionDoneByID}-{SessionStartTime}-{SessionEndTime}";
        }

        public override string ToString()
        {
            return $"ID: {Id}, Type: {Type}, Description: {Description}, Location: {IsPractiseRoom}, Specials: {Specials ?? "none"}, Session Done By: {SesionDoneBy}, SessionTime: {SessionStartTime}";
        }
    }
}
