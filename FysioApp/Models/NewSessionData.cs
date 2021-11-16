using DatabaseHandler.Models;
using FysioAppUX.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FysioAppUX.Models
{
    public class NewSessionData
    {
        public int DossierID { get; set; }
        public List<Behandeling> Behandelings { get; set; }
        public IEnumerable<SelectListItem> BehandelingStrings { get; set; }
        public IEnumerable<SelectListItem> FysioStrings { get; set; }
        public IEnumerable<SelectListItem> PatientStrings { get; set; }
        public TherapySession Session { get; set; }
        public FysioWorker Worker{ get; set; }
        public int TimeSesssion { get; set; }
        public int PatientID { get; set; }
        public int IsFromList { get; set; }

        public NewSessionData(DataReciever reciever, int dossierID)
        {
            DossierID = dossierID;
            Behandelings = new();
            Session = new();
            SetFysioStrings(reciever);
            TimeSesssion = reciever.GetOnePatientFile(dossierID).actionPlan.TimePerSession;
        }

        public NewSessionData(DataReciever reciever, FysioWorker worker)
        {
            Worker = worker;
            Behandelings = new();
            Session = new();
            SetPatientStrings(reciever);
        }

        public NewSessionData(DataReciever reciever, int dossierID, TherapySession session)
        {
            DossierID = dossierID;
            Behandelings = new();
            Session = session;
            SetFysioStrings(reciever);
            TimeSesssion = reciever.GetOnePatientFile(dossierID).actionPlan.TimePerSession;
            SetSelectedFysio();
        }

        private void SetSelectedFysio()
        {
            if (Session.SessionDoneByID != 0)
            {
                foreach (var item in FysioStrings)
                {
                    if (item.Value == Session.SessionDoneByID.ToString())
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }
        }

        private List<Patient> GetPatients(DataReciever reciever)
        {
            List<Patient> patients = new();
            foreach (PatientFile p in reciever.GetAllPatientFiles())
            {
                if (p.mainTherapist.FysioWorkerID == Worker.FysioWorkerID)
                    patients.Add(p.patient);
                else if (Worker.IsStudent)
                {
                    if (!p.isStudent)
                        patients.Add(p.patient);
                }
            }
            return patients;
        }

        private void SetPatientStrings(DataReciever reciever)
        {
            List<SelectListItem> sList = new();
            SelectListItem sli = new();
            foreach (Patient p in GetPatients(reciever))
            {
                sli = new(
                    $"{p.Name}",
                    p.PatientID.ToString()
                    );
                sList.Add(sli);
            }
            PatientStrings = sList;
        }

        private List<FysioWorker> GetFysioWorkers(DataReciever reciever)
        {
            List<FysioWorker> workers = new();
            PatientFile patientFile = reciever.GetOnePatientFile(DossierID);
            workers.Add(patientFile.mainTherapist);
            if (!patientFile.mainTherapist.IsStudent)
            {
                foreach (FysioWorker worker in reciever.GetAllFysioWorkers())
                    if (worker.IsStudent)
                        workers.Add(worker);
            }
            return workers;
        }

        private void SetFysioStrings(DataReciever reciever)
        {
            List<SelectListItem> sList = new();
            SelectListItem sli = new();
            foreach (FysioWorker w in GetFysioWorkers(reciever))
            {
                sli = new(
                    $"{w.FysioWorkerID} - {w.Name}",
                    w.FysioWorkerID.ToString()
                    );
                sList.Add(sli);
            }
            FysioStrings = sList;
        }

        public async Task FillBehandelings()
        {
            Behandelings = await APIReader.ProcessAllBehandelingen();
            createBehandelingList();
        }

        public void SetSelectedBehandeling()
        {
            if (Session.Type != null && Session.Type != "")
                foreach (var item in BehandelingStrings)
                    if (item.Value == Session.Type)
                    {
                        item.Selected = true;
                        break;
                    }
        }

        private void createBehandelingList()
        {
            List<SelectListItem> dList = new();
            SelectListItem sli;
            foreach (Behandeling b in Behandelings)
            {
                sli = new(
                    $"{b.Waarde} - {b.Omschrijving}, Toelichting verplicht: {b.Toelichting_verplicht}",
                    b.Waarde
                    );
                dList.Add(sli);
            }
            BehandelingStrings = dList;
        }


    }
}
