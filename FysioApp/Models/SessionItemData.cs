using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseHandler.Models;

namespace FysioAppUX.Models
{
    public class SessionItemData
    {
        public TherapySession Session { get; set; }
        public bool IsFromDossier { get; set; }
        public int DossierID { get; set; }
        public DateTime startTime { get; set; }
        public bool IsFromSessionList { get; set; }
        public bool IsPatient { get; set; }

        public SessionItemData(bool isFromList, TherapySession session, bool isFromDossier, bool isPatient)
        {
            IsPatient = isPatient;
            IsFromSessionList = isFromList;
            Session = session;
            startTime = session.SessionStartTime;
            IsFromDossier = isFromDossier;
        }

        public SessionItemData(bool isFromList, TherapySession session, int dossierID, bool isPatient)
        {
            IsPatient = isPatient;
            IsFromSessionList = isFromList;
            Session = session;
            startTime = session.SessionStartTime;
            DossierID = dossierID;
            IsFromDossier = false;
        }
    }
}
