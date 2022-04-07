using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DatabaseHandler.Models;
using System.Linq;
using System.Threading.Tasks;
using DomainServices.Repos;
using FysioAppUX.Data;
using DomainModels.Models;

namespace FysioAppUX.Components
{
    public class TotalPatients : ViewComponent
    {
        private readonly ITherapySession _therapySession;
        private readonly IFysioWorker _fysioWorker;
        private readonly List<TherapySession> Sessions;
        private readonly FysioIdentityDBContext _context;

        public TotalPatients(IFysioWorker worker, ITherapySession session, FysioIdentityDBContext context)
        {
            _therapySession = session;
            _fysioWorker = worker;
            _context = context;
            Sessions = new();
        }

        public string Invoke()
        {
            FysioWorker fysioUser = null;
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.Name;
                string email = _context.GetUserEmail(userId);
                if (User.IsInRole("PhysicalTherapist") || User.IsInRole("Intern"))
                {
                    fysioUser = _fysioWorker.GetFysioWorkerByEmail(email);
                    foreach (TherapySession session in _therapySession.GetTherapySessionsByFysio(fysioUser.FysioWorkerID))
                    {
                        if (session.SessionStartTime.Date == DateTime.Now.Date)
                            Sessions.Add(session);
                    }
                }
            }
            if (fysioUser == null)
                return "";
            return $"Aantal sessies vandaag: {Sessions.Count}";
        }
    }
}
