using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModels.Models;

namespace FysioAppUX.Models
{
    public class SessionItemData
    {
        public TherapySession Session { get; set; }
        public bool IsFromDossier { get; set; }
        public int DossierID { get; set; }
        public DateTime StartTime { get; set; }
        public bool IsFromSessionList { get; set; }
        public bool IsPatient { get; set; }
        public bool IsFromHome { get; set; }

        public SessionItemData(bool isFromList, TherapySession session, bool isFromDossier, bool isPatient, bool isFromHome)
        {
            IsPatient = isPatient;
            IsFromSessionList = isFromList;
            Session = session;
            StartTime = session.SessionStartTime;
            IsFromDossier = isFromDossier;
            IsFromHome = isFromHome;
        }

        public SessionItemData(bool isFromList, TherapySession session, int dossierID, bool isPatient, bool isFromHome)
        {
            IsPatient = isPatient;
            IsFromSessionList = isFromList;
            Session = session;
            StartTime = session.SessionStartTime;
            DossierID = dossierID;
            IsFromDossier = false;
            IsFromHome = isFromHome;
        }
    }
}
