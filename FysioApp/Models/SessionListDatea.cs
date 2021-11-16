using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseHandler.Models;

namespace FysioAppUX.Models
{
    public class SessionListDatea
    {
        public List<SessionItemData> sessions { get; set; }
        public int DossierID { get; set; }
        public bool IsFromSession { get; set; }

        public SessionListDatea()
        {
            sessions = new();
        }

        public void SortSessions()
        {
            List<SessionItemData> sortedSessions = sessions.OrderBy(o => o.startTime).ToList();
            sessions = sortedSessions;
        }

        public void RemovePastSessions()
        {
            List<SessionItemData> currentSessions = new();
            foreach (SessionItemData ts in sessions)
                if (ts.startTime >= DateTime.Today)
                    currentSessions.Add(ts);
            sessions = currentSessions;
        }
    }

}
