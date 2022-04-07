using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModels.Models;

namespace FysioAppUX.Models
{
    public class SessionListDatea
    {
        public List<SessionItemData> Sessions { get; set; }
        public int DossierID { get; set; }
        public bool IsFromSession { get; set; }

        public SessionListDatea()
        {
            Sessions = new();
        }

        public void SortSessions()
        {
            List<SessionItemData> sortedSessions = Sessions.OrderBy(o => o.StartTime).ToList();
            Sessions = sortedSessions;
        }

        public void RemovePastSessions()
        {
            List<SessionItemData> currentSessions = new();
            foreach (SessionItemData ts in Sessions)
                if (ts.StartTime >= DateTime.Today)
                    currentSessions.Add(ts);
            Sessions = currentSessions;
        }
    }

}
