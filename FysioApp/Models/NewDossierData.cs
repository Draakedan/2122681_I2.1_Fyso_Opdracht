using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseHandler.Models;
using FysioAppUX.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FysioAppUX.Models
{
    public class NewDossierData
    {
        public List<Patient> patients { get; set; }
        public IEnumerable<SelectListItem> patientStrings { get; set; }
        public List<FysioWorker> workers { get; set; }
        public IEnumerable<SelectListItem> AllWorkers { get; set; }
        public List<FysioWorker> students { get; set; }
        public List<FysioWorker> fysios { get; set; }
        public IEnumerable<SelectListItem> FysioStrings { get; set; }
        public List<ActionPlan> plans { get; set; }
        public IEnumerable<SelectListItem> PlanStrings { get; set; }
        public PatientFile file { get; set; }
        public IEnumerable<Diagnose> diagnoses { get; set; }
        public IEnumerable<SelectListItem> diagnoseList { get; set; }

        public int selectedPatient { get; set; } = 0;
        public string SelectedIntaker { get; set; }
        public string selectedSupperviser { get; set; }
        public string selectedMain { get; set; }
        public string selectedPlan { get; set; }
        public string selectedDiagnose { get; set; }

        public NewDossierData()
        { }

        public NewDossierData(DataReciever reciever)
        {
            patients = reciever.GetAllPatients().ToList();
            workers = reciever.GetAllFysioWorkers().ToList();
            students = new();
            fysios = new();
            FillStudentsAndFysios();
            plans = reciever.GetAllActionPlans().ToList();
            file = new();
            CreateStrings();

        }

        private void CreateStrings()
        {
            List<SelectListItem> sList = new();
            SelectListItem sli;
            foreach (Patient p in patients)
            {
                sli = new(
                    $"{p.Name}",
                    p.PatientID.ToString()
                    );
                sList.Add(sli);
            }
            patientStrings = sList;

            sList = new();
            foreach (FysioWorker w in workers)
            {
                sli = new(
                    $"{w.FysioWorkerID} - {w.Name}",
                    w.FysioWorkerID.ToString()
                    );
                sList.Add(sli);
            }
            AllWorkers = sList;

            sList = new();
            foreach (FysioWorker w in fysios)
            {
                sli = new(
                    $"{w.FysioWorkerID} - {w.Name}, {w.WorkerNumber}, {w.BIGNumber}",
                    w.FysioWorkerID.ToString()
                    );
                sList.Add(sli);
            }
            FysioStrings = sList;

            sList = new();
            foreach (ActionPlan a in plans)
            {
                sli = new(
                    $"{a.ActionID} - {a.TimePerSession} uur, {a.SessionsPerWeek}x per week",
                    a.ActionID.ToString()
                    );
                sList.Add(sli);
            }
            PlanStrings = sList;
        }


        private void FillStudentsAndFysios()
        {
            foreach (FysioWorker w in workers)
            {
                if (w.IsStudent)
                    students.Add(w);
                else
                    fysios.Add(w);
            }
        }

        public async Task FillDiagnoses(OwnerConsumer consumer)
        {
            diagnoses = await consumer.GetAllDiagnoses();
            CreateDiagnoseList();
        }

        public void CreateDiagnoseList()
        {
            List<SelectListItem> dList = new();
            SelectListItem sli;
            foreach (Diagnose d in diagnoses)
                {
                if (d.Pathologie != null && d.Code != null && d.lichaamslocalisatie != null)
                {
                    sli = new(
                    $"{d.Code}: {d.Pathologie} in {d.lichaamslocalisatie}",
                    d.Code.ToString()
                    );
                    dList.Add(sli);
                }
            }
            diagnoseList = dList;
        }
    }
}
