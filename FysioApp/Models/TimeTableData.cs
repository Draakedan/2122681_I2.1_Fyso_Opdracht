using DomainModels.Models;
using DomainServices.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FysioAppUX.Models
{
    public class TimeTableData
    {
        public int IsUpdate { get; set; }
        public int IsFromList { get; set; }
        public ActionPlan ActionPlan { get; set; }
        public int FysioId { get; set; }
        public PatientFile File { get; set; }
        public List<TherapySession> FysioSessions;
        public List<TherapySession> PatientSessions;
        public string FysioName;
        public TherapySession TherapySession { get; set; }
        public string FysioAvailability;

        public TimeTableData(int isUpdate, int isFromList, ActionPlan actionPlan, int fysioId, PatientFile file, TherapySession session, ITherapySession sessionRepo, IFysioWorker fysioRepo)
        {
            IsUpdate = isUpdate;
            IsFromList = isFromList;
            ActionPlan = actionPlan;
            FysioId = fysioId;
            File = file;
            TherapySession = session;
            FysioSessions = sessionRepo.GetTherapySessionsByFysio(fysioId);
            PatientSessions = File.Sessions.ToList();
            FysioWorker worker = fysioRepo.GetFysioWorkerByID(FysioId);
            FysioName = worker.Name;
            FysioAvailability = SetFysioAvailability(worker);
        }

        private static string SetFysioAvailability(FysioWorker fysio)
        {
            string s = "Beschikbaar iedere ";

            string[] dayNumbers = fysio.AvailableDays.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            bool first = true;
            for (int i = 0; i < dayNumbers.Length; i++)
            {
                if (i == dayNumbers.Length - 1)
                    s += " en ";
                else if (!first)
                    s += ", ";
                first = false;
                s += GetDayName(dayNumbers[i]);
            }

            s += " van " + fysio.DayStartTime.ToShortTimeString() + " tot " + fysio.DayEndTime.ToShortTimeString() + ".";

            return s;
        }

        private static string GetDayName(string number)
        {
            switch (number)
            {
                case "1":
                    return "Maandag";
                case "2":
                    return "Dinsdag";
                case "3":
                    return "Woensdag";
                case "4":
                    return "Donderdag";
                case "5":
                    return "Vrijdag";
                default:
                    return "";
            }
        }
    }
}
