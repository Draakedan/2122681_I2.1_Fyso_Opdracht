using DomainModels.Models;
using DomainServices.Repos;
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
        public List<Treatment> Behandelings { get; set; }
        public IEnumerable<SelectListItem> BehandelingStrings { get; set; }
        public IEnumerable<SelectListItem> FysioStrings { get; set; }
        public IEnumerable<SelectListItem> PatientStrings { get; set; }
        public TherapySession Session { get; set; }
        public FysioWorker Worker { get; set; }
        public int TimeSesssion { get; set; }
        public int PatientID { get; set; }
        public int IsFromList { get; set; }
        private readonly ITreatment Treatment;

        public NewSessionData(ITreatment treatment, IPatientFile patientFile, FysioWorker worker)
        {
            Treatment = treatment;
            Worker = worker;
            Behandelings = new();
            Session = new();
            SetPatientStrings(patientFile);
        }

        public NewSessionData(ITreatment treatment, IFysioWorker fysioWorker, IPatientFile patientFile, int dossierID, TherapySession session)
        {
            Treatment = treatment;
            DossierID = dossierID;
            Behandelings = new();
            Session = session ?? new();
            SetFysioStrings(fysioWorker, patientFile);
            TimeSesssion = patientFile.GetPatientFileByID(dossierID).ActionPlan.TimePerSession;
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

        private List<Patient> GetPatients(IPatientFile patientFile)
        {
            List<Patient> patients = new();
            foreach (PatientFile p in patientFile.GetAllPatientFiles())
            {
                if (p.MainTherapist.FysioWorkerID == Worker.FysioWorkerID)
                    patients.Add(p.Patient);
                else if (Worker.IsStudent)
                {
                    if (!p.IsStudent)
                        patients.Add(p.Patient);
                }
            }
            return patients;
        }

        private void SetPatientStrings(IPatientFile patientFile)
        {
            List<SelectListItem> sList = new();
            SelectListItem sli;
            foreach (Patient p in GetPatients(patientFile))
            {
                sli = new(
                    $"{p.Name}",
                    p.PatientID.ToString()
                    );
                sList.Add(sli);
            }
            PatientStrings = sList;
        }

        private List<FysioWorker> GetFysioWorkers(IFysioWorker fysioWorker, IPatientFile patientFile)
        {
            List<FysioWorker> workers = new();
            PatientFile file = patientFile.GetPatientFileByID(DossierID);
            workers.Add(file.MainTherapist);
            if (!file.MainTherapist.IsStudent)
            {
                foreach (FysioWorker worker in fysioWorker.GetAllFysioWorkers())
                    if (worker.IsStudent)
                        workers.Add(worker);
            }
            return workers;
        }

        private void SetFysioStrings(IFysioWorker fysioWorker, IPatientFile patientFile)
        {
            List<SelectListItem> sList = new();
            SelectListItem sli;
            foreach (FysioWorker w in GetFysioWorkers(fysioWorker, patientFile))
            {
                sli = new(
                    $"{w.FysioWorkerID} - {w.Name} {GetFysioStudentString(w)}",
                    w.FysioWorkerID.ToString()
                    );
                sList.Add(sli);
            }
            FysioStrings = sList;
        }

        private static string GetFysioStudentString(FysioWorker w)
        {
            if (w.IsStudent)
                return "(student)";
            else return "(Fysio)";
        }

        public void FillBehandelings()
        {
            Behandelings = Treatment.GetAllTreatments();
            CreateBehandelingList();
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

        private void CreateBehandelingList()
        {
            List<SelectListItem> dList = new();
            SelectListItem sli;
            foreach (Treatment b in Behandelings)
            {
                sli = new(
                    $"{b.waarde} - {b.omschrijving}, Toelichting verplicht: {b.toelichting_verplicht}",
                    b.waarde
                    );
                dList.Add(sli);
            }
            BehandelingStrings = dList;
        }


    }
}
