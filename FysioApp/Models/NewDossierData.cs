using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModels.Models;
using DomainServices.Repos;
using FysioAppUX.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FysioAppUX.Models
{
    public class NewDossierData
    {
        public List<Patient> Patients { get; set; }
        public IEnumerable<SelectListItem> PatientStrings { get; set; }
        public List<FysioWorker> Workers { get; set; }
        public IEnumerable<SelectListItem> AllWorkers { get; set; }
        public List<FysioWorker> Students { get; set; }
        public List<FysioWorker> Fysios { get; set; }
        public IEnumerable<SelectListItem> FysioStrings { get; set; }
        public List<ActionPlan> Plans { get; set; }
        public IEnumerable<SelectListItem> PlanStrings { get; set; }
        public PatientFile File { get; set; }
        public IEnumerable<Diagnose> Diagnoses { get; set; }
        public IEnumerable<SelectListItem> DiagnoseList { get; set; }

        public int SelectedPatient { get; set; } = 0;
        public string SelectedIntaker { get; set; }
        public string SelectedSupperviser { get; set; }
        public string SelectedMain { get; set; }
        public string SelectedPlan { get; set; }
        public string SelectedDiagnose { get; set; }

        public NewDossierData()
        { }

        public NewDossierData(IActionPlan actionPlan, IPatient patient, IFysioWorker fysioWorker, IPatientFile file)
        {
            Patients = GetPatientsWithNoDossier(patient.GetAllPatients().ToList(), file);
            Workers = fysioWorker.GetAllFysioWorkers().ToList();
            Students = new();
            Fysios = new();
            FillStudentsAndFysios();
            Plans = actionPlan.GetAllActionPlans().ToList();
            File = new();
            CreateStrings();

        }

        private static List<Patient> GetPatientsWithNoDossier(List<Patient> allPatients, IPatientFile file)
        {
            List<Patient> patientList = new();
            foreach (Patient p in allPatients)
            {
                if (file.GetPatientFileByPatient(p.PatientID) == null)
                    patientList.Add(p);
            }
            return patientList;
        }

        private void CreateStrings()
        {
            List<SelectListItem> sList = new();
            SelectListItem sli;
            foreach (Patient p in Patients)
            {
                sli = new(
                    $"{p.Name}",
                    p.PatientID.ToString()
                    );
                sList.Add(sli);
            }
            PatientStrings = sList;

            sList = new();
            foreach (FysioWorker w in Workers)
            {
                sli = new(
                    $"{w.FysioWorkerID} - {w.Name} {GetFysioStudentString(w)}",
                    w.FysioWorkerID.ToString()
                    );
                sList.Add(sli);
            }
            AllWorkers = sList;

            sList = new();
            foreach (FysioWorker w in Fysios)
            {
                sli = new(
                    $"{w.FysioWorkerID} - {w.Name}, {w.WorkerNumber}, {w.BIGNumber}",
                    w.FysioWorkerID.ToString()
                    );
                sList.Add(sli);
            }
            FysioStrings = sList;

            sList = new();
            foreach (ActionPlan a in Plans)
            {
                sli = new(
                    $"{a.ActionID} - {a.TimePerSession} uur, {a.SessionsPerWeek}x per week",
                    a.ActionID.ToString()
                    );
                sList.Add(sli);
            }
            PlanStrings = sList;
        }

        private static string GetFysioStudentString(FysioWorker w)
        {
            if (w.IsStudent)
                return "(student)";
            else return "(Fysio)";
        }

        private void FillStudentsAndFysios()
        {
            foreach (FysioWorker w in Workers)
            {
                if (w.IsStudent)
                    Students.Add(w);
                else
                    Fysios.Add(w);
            }
        }

        public void FillDiagnoses(IDiagnose diagnose)
        {
            Diagnoses = diagnose.GetAllDiagnoses();
            CreateDiagnoseList();
        }

        public void CreateDiagnoseList()
        {
            List<SelectListItem> dList = new();
            SelectListItem sli;
            foreach (Diagnose d in Diagnoses)
            {
                if (d.Pathologie != null && d.Code != 0 && d.Lichaamslocalisatie != null)
                {
                    sli = new(
                    $"{d.Code}: {d.Pathologie} in {d.Lichaamslocalisatie}",
                    d.Code.ToString()
                    );
                    dList.Add(sli);
                }
            }
            DiagnoseList = dList;
        }
    }
}
