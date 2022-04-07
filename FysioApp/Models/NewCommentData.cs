using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModels.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using FysioAppUX.Data;
using DomainServices.Repos;

namespace FysioAppUX.Models
{
    public class NewCommentData
    {
        public int DossierID { get; set; }
        public Comment Comment { get; set; }
        public IEnumerable<SelectListItem> CommenterStrings { get; set; }

        public NewCommentData(IPatientFile patientFile, IFysioWorker fysioWorker, int dossierId, Comment comment)
        {
            DossierID = dossierId;
            Comment = comment ?? new();
            FillCommenterString(GetFysioWorkers(patientFile, fysioWorker));
            SetSelectedWorker(patientFile);
        }

        private List<FysioWorker> GetFysioWorkers(IPatientFile patientFile, IFysioWorker fysioWorker)
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

        private void FillCommenterString(List<FysioWorker> workers)
        {
            List<SelectListItem> items = new();
            SelectListItem sli;
            foreach (FysioWorker w in workers)
            {
                sli = new(
                    $"{w.FysioWorkerID} - {w.Name}",
                    w.FysioWorkerID.ToString()
                    );
                items.Add(sli);
            }
           CommenterStrings = items;
        }

        private void SetSelectedWorker(IPatientFile patientFile)
        {
            int id = patientFile.GetPatientFileByID(DossierID).IdmainTherapist;
            if (Comment != null)
                id = Comment.CommenterID;
            foreach (SelectListItem sli in CommenterStrings)
                if (sli.Value == id.ToString())
                {
                    sli.Selected = true;
                    break;
                }

        }
    }
}
