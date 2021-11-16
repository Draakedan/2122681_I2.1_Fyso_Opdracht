using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseHandler.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using FysioAppUX.Data;

namespace FysioAppUX.Models
{
    public class NewCommentData
    {
        public int DossierID { get; set; }
        public Comment Comment { get; set; }
        public IEnumerable<SelectListItem> CommenterStrings { get; set; }


        public NewCommentData(DataReciever reciever, int dossierId)
        {
            DossierID = dossierId;
            Comment = new();
            FillCommenterString(GetFysioWorkers(reciever));
            SetSelectedWorker(reciever);
        }

        public NewCommentData(DataReciever reciever, int dossierId, Comment comment)
        {
            DossierID = dossierId;
            Comment = comment;
            FillCommenterString(GetFysioWorkers(reciever));
            SetSelectedWorker(reciever);
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

        private void FillCommenterString(List<FysioWorker> workers)
        {
            List<SelectListItem> items = new();
            SelectListItem sli = new();
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

        private void SetSelectedWorker(DataReciever reciever)
        {
            int id = reciever.GetOnePatientFile(DossierID).IdmainTherapist;
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
