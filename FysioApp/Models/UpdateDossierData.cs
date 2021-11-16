using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseHandler.Models;
using FysioAppUX.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FysioAppUX.Models
{
    public class UpdateDossierData
    {
        public PatientFile File { get; set; }
        public ActionPlan Plan { get; set; }
        public IEnumerable<SelectListItem> Workers { get; set; }

        public UpdateDossierData(PatientFile file, DataReciever reciever)
        {
            File = file;
            Plan = file.actionPlan;
            FillWorkers(reciever.GetAllFysioWorkers());
        }

        private void FillWorkers(ICollection<FysioWorker> workers)
        {
            List<SelectListItem> sList = new();
            SelectListItem sli = new();
            foreach (FysioWorker w in workers)
            {
                sli = new(
                    $"{w.FysioWorkerID} - {w.Name}",
                    w.FysioWorkerID.ToString()
                    );
                sList.Add(sli);
            }
            Workers = sList;
            SetWorker();
        }

        private void SetWorker()
        {
            foreach (SelectListItem sli in Workers)
                if (sli.Value == File.IdmainTherapist.ToString())
                {
                    sli.Selected = true;
                    break;
                }    
        }
    }
}
