using DomainModels.Models;
using FysioAppUX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainServices.Repos;

namespace FysioAppUX.Models
{
    public class FysioData
    {
        public ICollection<PatientFile> Dossiers { get; set; }
        public ICollection<TherapySession> Sessions { get; set; }
        public FysioWorker Fysio { get; set; }
        public int Id { get; set; }
        public IPatientFile PatientFile;

        public FysioData(ITherapySession therapySession, IPatientFile patientFile, FysioWorker fysio) {
            Fysio = fysio;
            Id = Fysio.FysioWorkerID;
            PatientFile = patientFile;
            Dossiers = PatientFile.GetPatientFileByFysio(Id);
            Sessions = therapySession.GetTherapySessionsByFysio(Id);
            Sessions = therapySession.SortSessions(Sessions.ToList());
        }
    }
}
