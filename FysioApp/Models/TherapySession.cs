using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FysioApp.Models
{
    public class TherapySession
    {

        public int Id { get; set; }

        public int Type { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public string Specials { get; set; }

        public FysioWorker SesionDoneBy { get; set; }

        public DateTime SessionTime { get; set; }

        public override string ToString()
        {
            return $"ID: {Id}, Type: {Type}, Description: {Description}, Location: {Location}, Specials: {Specials ?? "none"}, Session Done By: {SesionDoneBy}, SessionTime: {SessionTime}";
        }
    }
}
