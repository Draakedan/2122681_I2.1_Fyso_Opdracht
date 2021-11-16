using DatabaseHandler.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FysioAppUX.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using FysioAppUX.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;

namespace FysioAppUX.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        private readonly UserManager<IdentityUser> _userManager;
        private readonly DataReciever _dataReciever;
        private readonly OwnerConsumer _consumer;
        private bool IsPatient = false;

        public HomeController(DataReciever reciever, UserManager<IdentityUser> userManager, OwnerConsumer consumer)
        {
            //_logger = logger;
            _userManager = userManager;
            _dataReciever = reciever;
            _consumer = consumer;
        }

        [Route("Home/Index")]
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Patient")]
        [Route("Home/PatientHome")]
        public IActionResult PatientHome()
        {
            IsPatient = true;
            var userId = User.Identity.Name;
            string email = "";
            using (var context = new FysioIdentityDBContext())
            {
                email = context.GetUserEmail(userId);
            }
            PatientFile p = _dataReciever.GetDossierByPatient(_dataReciever.GetPatientByEmail(email).PatientID);
            if (p == null)
                return RedirectToAction("Index");

            SessionListDatea data = new();
            List<SessionItemData> sessions = new();
            foreach (TherapySession s in p.sessions)
            {
                s.SesionDoneBy = (_dataReciever.GetOneFysioWorker(s.SessionDoneByID));
                sessions.Add(new(true, s, true, true));
            }
            data.sessions = sessions;
            data.DossierID = p.ID;
            data.IsFromSession = true;
            return View("Sessions", data);
        }

        [Authorize(Roles = "Patient")]
        [Route("Home/DeletePatientSession/{id:int}")]
        [HttpGet]
        public IActionResult DeletePatientSession(int id)
        {
            TherapySession session = _dataReciever.GetOneTherapySession(id);
            return View("DeletePatientSession", session);
        }

        [Authorize(Roles = "Patient")]
        [Route("Home/ComfirmDeletePatientSession/{id:int}")]
        public IActionResult ComfirmDeletePatientSession(int id)
        {
            TherapySession session = _dataReciever.GetOneTherapySession(id);
            if (DateTime.Now.Subtract(session.SessionStartTime).TotalHours < 24)
            {
                _dataReciever.DeleteTherapySession(session);
                return RedirectToAction("PatientHome");
            }
            return View("Index");
        }


        [Authorize(Roles = "PhysicalTherapist, Intern")]
        public IActionResult Patients()
        {
            return View(_dataReciever.GetAllPatients()/*.ToList()*/);
        }

        [Authorize(Roles = "PhysicalTherapist, Intern")]
        [Route("Home/UpdatePatient/{id:int}")]
        [HttpGet]
        public IActionResult UpdatePatient(int id)
        {
            PatientFile patientFile = _dataReciever.GetOnePatientFile(id);
            UpdatePatientData data = new(patientFile.patient, id);
            return View(data);
        }

        [Authorize(Roles = "PhysicalTherapist, Intern")]
        [HttpPost]
        public IActionResult UpdatePatient(IFormCollection form)
        {
            int dossierID = int.Parse(form["DossierID"]);
            Patient p = _dataReciever.GetOnePatientFile(dossierID).patient;
            Adress a = p.Adress;

            if (form["Patient.EnsuranceCompany"] == string.Empty)
                ModelState.AddModelError("Patient.EnsuranceCompany", "Verzekering mag niet leeg zijn!");
            else
                p.EnsuranceCompany = form["Patient.EnsuranceCompany"];

            if (form["Patient.PhoneNumber"] == string.Empty)
                p.PhoneNumber = "";
            else
                p.PhoneNumber = form["Patient.PhoneNumber"];

            if (form["Adress.Country"] == string.Empty)
                ModelState.AddModelError("Adress.Country", "Land mag niet leeg zijn!");
            else
                a.Country = form["Adress.Country"];

            if (form["Adress.City"] == string.Empty)
                ModelState.AddModelError("Adress.City", "Stad mag niet leeg zijn!");
            else
                a.City = form["Adress.City"];

            if (form["Adress.PostalCode"] == string.Empty)
                ModelState.AddModelError("Adress.PostalCode", "Postcode mag niet leeg zijn!");
            else
                a.PostalCode = form["Adress.PostalCode"];

            if (form["Adress.Street"] == string.Empty)
                ModelState.AddModelError("Adress.Street", "Straat naam mag niet leeg zijn!");
            else
                a.Street = form["Adress.Street"];

            if (form["Adress.HouseNumber"] == string.Empty)
                ModelState.AddModelError("Adress.HouseNumber", "Huisnummer mag niet leeg zijn!");
            else
                a.HouseNumber = form["Adress.HouseNumber"];

            if (ModelState.IsValid)
            {
                _dataReciever.UpdatePatient(p, dossierID);
                return RedirectToAction("Dossiers", "Home", dossierID);
            }
            return View(new UpdatePatientData(p, dossierID));
        }

        [Authorize(Roles = "PhysicalTherapist, Intern")]
        [HttpPost]
        public IActionResult UpdateDossier(IFormCollection form)
        {
            PatientFile file = _dataReciever.GetOnePatientFile(int.Parse(form["File.ID"]));
            ActionPlan plan = file.actionPlan;

            if (form["File.IdmainTherapist"] == string.Empty)
                ModelState.AddModelError("File.IdmainTherapist", "Geen hoofdbehandelaar geselecteerd!");
            else
            {
                int mainID = int.Parse(form["File.IdmainTherapist"]);
                file.IdmainTherapist = mainID;
                FysioWorker main = _dataReciever.GetOneFysioWorker(mainID);
                if (main != null)
                    file.mainTherapist = main;
                else
                    ModelState.AddModelError("File.IdmainTherapist", "Geselecteerde behandelaar bestaat niet!");
            }

            if (form["Plan.SessionsPerWeek"] == string.Empty)
                ModelState.AddModelError("Plan.SessionsPerWeek", "Sessies per week mag niet leeg zijn!");
            else
            {
                int sessionsPerWeek = int.Parse(form["Plan.SessionsPerWeek"]);
                if (sessionsPerWeek <= 0)
                    ModelState.AddModelError("Plan.SessionsPerWeek", "Sessies per week moet een positieve waarde hebben!");
                else
                    plan.SessionsPerWeek = sessionsPerWeek;
            }

            if (form["Plan.TimePerSession"] == string.Empty)
                ModelState.AddModelError("Plan.TimePerSession", "Tijd per sessie mag niet leeg zijn!");
            else
            {
                int timePerSession = int.Parse(form["Plan.TimePerSession"]);
                if (timePerSession <= 0)
                    ModelState.AddModelError("Plan.TimePerSession", "Tijd per sessie moet een positieve waarde hebben!");
                else
                    plan.TimePerSession = timePerSession;
            }

            if (ModelState.IsValid)
            {
                file.actionPlan = plan;
                _dataReciever.UpdateActionPlan(plan, file);
                return RedirectToAction("Dossiers", "Home", file.ID);
            }
            file.actionPlan = plan;
            UpdateDossierData data = new(file, _dataReciever);
            return View(data);
        }

        [Authorize(Roles = "PhysicalTherapist, Intern")]
        [Route("Home/UpdateDossier/{id:int}")]
        [HttpGet]
        public IActionResult UpdateDossier(int id)
        {
            PatientFile file = _dataReciever.GetOnePatientFile(id);
            UpdateDossierData data = new(file, _dataReciever);
            return View(data);
        }

        [Authorize(Roles = "PhysicalTherapist, Intern")]
        public IActionResult Dossiers()
        {
            return View(_dataReciever.GetAllPatientFiles());
        }

        [Authorize]
        [Route("Home/DeleteSession/{id:int}")]
        public IActionResult DeleteSession(int id)
        {
            TherapySession session = _dataReciever.GetOneTherapySession(id);
            return View("DeleteSession", session);
        }

        [Authorize]
        [Route("Home/ComfirmDeleteSession/{id:int}")]
        public IActionResult ComfirmDeleteSession(int id)
        {
            TherapySession session = _dataReciever.GetOneTherapySession(id);
            if (DateTime.Now.Subtract(session.CreationDate).TotalHours < 24)
            {
                _dataReciever.DeleteTherapySession(session);
                return RedirectToAction("Dossiers");
            }
            return View("Index");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateSession(IFormCollection form)
        {
            int sessionID = int.Parse(form["Session.Id"]);
            TherapySession session = _dataReciever.GetOneTherapySession(sessionID);
            int FileId = int.Parse(form["DossierID"]);
            PatientFile file = _dataReciever.GetOnePatientFile(FileId);

            if (form["Session.Type"] == string.Empty)
                ModelState.AddModelError("Session.Type", "Type behandeling mag niet leeg zijn!");
            else
            {
                session.Type = form["Session.Type"];
                Behandeling b = await APIReader.ProcessOneBehandeling(form["Session.Type"]);

                if (b.Toelichting_verplicht.ToLower() == "ja" && form["Session.Specials"] == string.Empty)
                    ModelState.AddModelError("Session.Specials", "Toelichting mag niet leeg zijn als deze verplicht is!");
                else
                    session.Specials = form["Session.Specials"];
            }

            if (form["Session.Description"] == string.Empty)
                ModelState.AddModelError("Session.Description", "Beschrijving mag niet leeg zijn!");
            else
                session.Description = form["Session.Description"];

            session.IsPractiseRoom = bool.Parse(form["Session.IsPractiseRoom"]);

            if (form["Session.SessionDoneByID"] == string.Empty)
                ModelState.AddModelError("Session.SessionDoneByID", "Geen uitvoerder geselecteed!");
            else
            {
                int id = int.Parse(form["Session.SessionDoneByID"]);
                session.SessionDoneByID = id;
                session.SesionDoneBy = _dataReciever.GetOneFysioWorker(id);

                if (form["Session.SessionStartTime"] == string.Empty)
                    ModelState.AddModelError("Session.SessionStartTime", "Start tijd mag niet leeg zijn!");
                else
                {
                    DateTime startTime = DateTime.Parse(form["Session.SessionStartTime"]);
                    DateTime endTime = startTime.AddHours(file.actionPlan.TimePerSession);
                    if (startTime < file.registerDate)
                        ModelState.AddModelError("Session.SessionStartTime", "Sessie mag niet voor registratie plaatsvinden");
                    else if (startTime > file.fireDate)
                        ModelState.AddModelError("Session.SessionStartTime", "Sessie mag niet na ontslag plaatsvinden!");
                    else if (endTime > file.fireDate)
                        ModelState.AddModelError("Session.SessionStartTime", "Sessie mag niet na ontslag eindigen!");
                    else
                    {
                        string error = CheckTime(id, startTime, endTime, sessionID);
                        if (error != null)
                            ModelState.AddModelError("Session.SessionStartTime", error);
                        else
                        {
                            session.SessionStartTime = startTime;
                            session.SessionEndTime = endTime;
                        }
                    }

                }
            }

            if (!_dataReciever.CanFitSessionInPlan(file, session.SessionStartTime, session.Id))
                ModelState.AddModelError("Session.SessionStartTime", "Het maximale aantal sessies voor deze week is bereikt, er kunnen deze week geen sessies meer toegevoed worden!");

            if (ModelState.IsValid)
            {
                _dataReciever.UpdateSession(session);
                if (int.Parse(form["IsFromList"]) == 1)
                {

                    SessionListDatea info = new();
                    List<SessionItemData> sessions = new();
                    foreach (TherapySession s in file.sessions)
                    {
                        s.SesionDoneBy = (_dataReciever.GetOneFysioWorker(s.SessionDoneByID));
                        sessions.Add(new(true ,s, true, IsPatient));
                    }
                    info.sessions = sessions;
                    info.DossierID = file.ID;
                    info.IsFromSession = true;
                    return View("Sessions", info);
                }
                return RedirectToAction("Dossiers", "Home", FileId);
            }
            NewSessionData data = new(_dataReciever, FileId, session);
            await data.FillBehandelings();
            data.SetSelectedBehandeling();
            return View(data);
        }

        [Authorize]
        [Route("Home/UpdateSession/{id:int}")]
        [HttpGet]
        public async Task<IActionResult> UpdateSession(int id)
        {
            TherapySession session = _dataReciever.GetOneTherapySession(id);
            PatientFile file = _dataReciever.GetDossierBySession(id);
            int dossierId = _dataReciever.GetDossierBySession(id).ID;
            NewSessionData data = new(_dataReciever, dossierId, session);
            await data.FillBehandelings();
            data.SetSelectedBehandeling();
            data.IsFromList = 0;
            return View("UpdateSession", data);
        }

        [Authorize]
        [Route("Home/UpdateSessionFromList/{id:int}")]
        [HttpGet]
        public async Task<IActionResult> UpdateSessionFromList(int id)
        {
            TherapySession session = _dataReciever.GetOneTherapySession(id);
            PatientFile file = _dataReciever.GetDossierBySession(id);
            int dossierId = _dataReciever.GetDossierBySession(id).ID;
            NewSessionData data = new(_dataReciever, dossierId, session);
            await data.FillBehandelings();
            data.SetSelectedBehandeling();
            data.IsFromList = 1;
            return View("UpdateSession", data);
        }

        [Authorize]
        [Route("Home/Sessions/{id:int}")]
        public IActionResult SessionsFromPatientFile(int id)
        {
            SessionListDatea s = new();
            foreach (TherapySession ts in _dataReciever.GetOnePatientFile(id).sessions)
            {
                ts.SesionDoneBy = _dataReciever.GetOneFysioWorker(ts.SessionDoneByID);
                s.sessions.Add(new(true, ts, true, IsPatient));
            }
            s.DossierID = id;
            s.IsFromSession = true;

            return View("Sessions", s);
        }

        [Authorize(Roles = "PhysicalTherapist, Intern")]
        [Route("Home/Sessions")]
        public IActionResult SessionsFromTherapist()
        {
            FysioWorker w = null;
            SessionListDatea s = new();
            var userId = User.Identity.Name;
            string email = "";
            using (var context = new FysioIdentityDBContext())
            {
                email = context.GetUserEmail(userId);
            }
            w = _dataReciever.GetFysioWorkerByEmail(email);
            ;
            if (w != null)
            {
                foreach (TherapySession ts in _dataReciever.GetAllTheraphySessionsByFysio(w.FysioWorkerID))
                {
                    int dossierID = _dataReciever.GetDossierBySession(ts.Id).ID;
                    s.sessions.Add(new(true, ts, dossierID, IsPatient));
                }
                s.RemovePastSessions();
                s.SortSessions();
                s.IsFromSession = true;
            }

            return View("Sessions", s);

        }

        [Authorize]
        [Route("Home/NewSession/{dossier:int}")]
        [HttpGet]
        public async Task<IActionResult> NewSession(int dossier)
        {
            NewSessionData sessionData = null;
            if (dossier != 0)
            {
                sessionData = new(_dataReciever, dossier);
                await sessionData.FillBehandelings();
                return View(sessionData);
            }
            FysioWorker w = null;
            var userId = User.Identity.Name;
            string email = "";
            using (var context = new FysioIdentityDBContext())
            {
                email = context.GetUserEmail(userId);
            }
            w = _dataReciever.GetFysioWorkerByEmail(email);
            sessionData = new(_dataReciever, w);
            await sessionData.FillBehandelings();
            return View("NewSessionForFysio", sessionData);
        }

        [Authorize]
        [Route("Home/NewSessionFromList/{dossier:int}")]
        [HttpGet]
        public async Task<IActionResult> NewSessionFromList(int dossier)
        {
            NewSessionData sessionData = null;
            if (dossier != 0)
            {
                sessionData = new(_dataReciever, dossier);
                sessionData.IsFromList = 1;
                await sessionData.FillBehandelings();
                return View("NewSession", sessionData);
            }
            FysioWorker w = null;
            var userId = User.Identity.Name;
            string email = "";
            using (var context = new FysioIdentityDBContext())
            {
                email = context.GetUserEmail(userId);
            }
            w = _dataReciever.GetFysioWorkerByEmail(email);
            sessionData = new(_dataReciever, w);
            await sessionData.FillBehandelings();
            sessionData.IsFromList = 1;
            return View("NewSessionForFysio", sessionData);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> NewSessionForFysio(IFormCollection form)
        {
            int FileId = 0;
            Patient p = null;
            TherapySession session = new();
            session.CreationDate = DateTime.Now;
            int FysioID = int.Parse(form["Worker.FysioWorkerID"]);
            session.SessionDoneByID = FysioID;
            session.SesionDoneBy = _dataReciever.GetOneFysioWorker(FysioID);

            if (form["PatientID"] == string.Empty)
                ModelState.AddModelError("PatientID", "Patient mag niet leeg zijn!");
            else
            {
                p = _dataReciever.GetOnePatient(int.Parse(form["PatientID"]));
                if (p == null)
                    ModelState.AddModelError("PatientID", "Patient bestaat niet!");
                else
                    FileId = _dataReciever.GetDossierByPatient(p.PatientID).ID;
            }

            PatientFile file = _dataReciever.GetOnePatientFile(FileId);

            if (form["Session.Type"] == string.Empty)
                ModelState.AddModelError("Session.Type", "Type behandeling mag niet leeg zijn!");
            else
            {
                session.Type = form["Session.Type"];
                Behandeling b = await APIReader.ProcessOneBehandeling(form["Session.Type"]);

                if (b.Toelichting_verplicht.ToLower() == "ja" && form["Session.Specials"] == string.Empty)
                    ModelState.AddModelError("Session.Specials", "Toelichting mag niet leeg zijn als deze verplicht is!");
                else
                    session.Specials = form["Session.Specials"];
            }

            if (form["Session.Description"] == string.Empty)
                ModelState.AddModelError("Session.Description", "Beschrijving mag niet leeg zijn!");
            else
                session.Description = form["Session.Description"];

            session.IsPractiseRoom = bool.Parse(form["Session.IsPractiseRoom"]);

            if (file != null)
            {
                if (form["Session.SessionStartTime"] == string.Empty)
                    ModelState.AddModelError("Session.SessionStartTime", "Start tijd mag niet leeg zijn!");
                else
                {
                    DateTime startTime = DateTime.Parse(form["Session.SessionStartTime"]);
                    DateTime endTime = startTime.AddHours(file.actionPlan.TimePerSession);
                    if (startTime < file.registerDate)
                        ModelState.AddModelError("Session.SessionStartTime", "Sessie mag niet voor registratie plaatsvinden");
                    else if (startTime > file.fireDate)
                        ModelState.AddModelError("Session.SessionStartTime", "Sessie mag niet na ontslag plaatsvinden!");
                    else if (endTime > file.fireDate)
                        ModelState.AddModelError("Session.SessionStartTime", "Sessie mag niet na ontslag eindigen!");
                    else if (startTime < DateTime.Today)
                        ModelState.AddModelError("Session.SessionStartTime", "Sessie mag niet in het verleden plaatsvinden");
                    else
                    {
                        string error = CheckTime(FysioID, startTime, endTime, -1);
                        if (error != null)
                            ModelState.AddModelError("Session.SessionStartTime", error);
                        else
                        {
                            session.SessionStartTime = startTime;
                            session.SessionEndTime = endTime;
                        }
                    }
                }
            }

            if (!_dataReciever.CanFitSessionInPlan(file, session.SessionStartTime, -1))
                ModelState.AddModelError("Session.SessionStartTime", "Het maximale aantal sessies voor deze week is bereikt, er kunnen deze week geen sessies meer toegevoed worden!");

            if (ModelState.IsValid)
            {
                _dataReciever.AddTherapySession(session, FileId);
                if (int.Parse(form["IsFromList"]) == 1)
                {

                    SessionListDatea info = new();
                    List<SessionItemData> sessions = new();
                    foreach (TherapySession s in file.sessions)
                    {
                        s.SesionDoneBy = (_dataReciever.GetOneFysioWorker(s.SessionDoneByID));
                        sessions.Add(new(true, s, true, IsPatient));
                    }
                    info.sessions = sessions;
                    info.DossierID = file.ID;
                    info.IsFromSession = true;
                    return View("Sessions", info);
                }
                return RedirectToAction("SessionsFromTherapist", "Home");
            }
            NewSessionData data = new(_dataReciever, session.SesionDoneBy);
            await data.FillBehandelings();
            data.Session = session;
            return View(data);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> NewSession(IFormCollection form)
        {
            TherapySession session = new();
            session.CreationDate = DateTime.Now;
            int FileId = int.Parse(form["DossierID"]);
            PatientFile file = _dataReciever.GetOnePatientFile(FileId);

            if (form["Session.Type"] == string.Empty)
                ModelState.AddModelError("Session.Type", "Type behandeling mag niet leeg zijn!");
            else
            {
                session.Type = form["Session.Type"];
                Behandeling b = await APIReader.ProcessOneBehandeling(form["Session.Type"]);

                if (b.Toelichting_verplicht.ToLower() == "ja" && form["Session.Specials"] == string.Empty)
                    ModelState.AddModelError("Session.Specials", "Toelichting mag niet leeg zijn als deze verplicht is!");
                else
                    session.Specials = form["Session.Specials"];
            }

            if (form["Session.Description"] == string.Empty)
                ModelState.AddModelError("Session.Description", "Beschrijving mag niet leeg zijn!");
            else
                session.Description = form["Session.Description"];

            session.IsPractiseRoom = bool.Parse(form["Session.IsPractiseRoom"]);

            if (form["Session.SessionDoneByID"] == string.Empty)
                ModelState.AddModelError("Session.SessionDoneByID", "Geen uitvoerder geselecteed!");
            else
            {
                int id = int.Parse(form["Session.SessionDoneByID"]);
                session.SessionDoneByID = id;
                session.SesionDoneBy = _dataReciever.GetOneFysioWorker(id);

                if (form["Session.SessionStartTime"] == string.Empty)
                    ModelState.AddModelError("Session.SessionStartTime", "Start tijd mag niet leeg zijn!");
                else
                {
                    DateTime startTime = DateTime.Parse(form["Session.SessionStartTime"]);
                    DateTime endTime = startTime.AddHours(file.actionPlan.TimePerSession);
                    if (startTime < file.registerDate)
                        ModelState.AddModelError("Session.SessionStartTime", "Sessie mag niet voor registratie plaatsvinden");
                    else if (startTime > file.fireDate)
                        ModelState.AddModelError("Session.SessionStartTime", "Sessie mag niet na ontslag plaatsvinden!");
                    else if (endTime > file.fireDate)
                        ModelState.AddModelError("Session.SessionStartTime", "Sessie mag niet na ontslag eindigen!");
                    else if (startTime < DateTime.Today)
                        ModelState.AddModelError("Session.SessionStartTime", "Sessie mag niet in het verleden plaatsvinden");
                    else
                    {
                        string error = CheckTime(id, startTime, endTime, -1);
                        if (error != null)
                            ModelState.AddModelError("Session.SessionStartTime", error);
                        else
                        {
                            session.SessionStartTime = startTime;
                            session.SessionEndTime = endTime;
                        }
                    }

                }
            }

            if (!_dataReciever.CanFitSessionInPlan(file, session.SessionStartTime, -1))
                ModelState.AddModelError("Session.SessionStartTime", "Het maximale aantal sessies voor deze week is bereikt, er kunnen deze week geen sessies meer toegevoed worden!");

            if (ModelState.IsValid)
            {
                _dataReciever.AddTherapySession(session, FileId);
                if (int.Parse(form["IsFromList"]) == 1)
                {

                    SessionListDatea info = new();
                    List<SessionItemData> sessions = new();
                    foreach (TherapySession s in file.sessions)
                    {
                        s.SesionDoneBy = (_dataReciever.GetOneFysioWorker(s.SessionDoneByID));
                        sessions.Add(new(true, s, true, IsPatient));
                    }
                    info.sessions = sessions;
                    info.DossierID = file.ID;
                    info.IsFromSession = true;
                    return View("Sessions", info);
                }
                return RedirectToAction("GetDossierDetails", new { id = FileId });
            }
            NewSessionData data = new(_dataReciever, FileId);
            await data.FillBehandelings();
            data.Session = session;
            return View(data);
        }

        private string CheckTime(int fysioID, DateTime startTime, DateTime endTime, int SessionID)
        {
            List<TherapySession> sessions = _dataReciever.GetAllTheraphySessionsByFysio(fysioID);
            foreach (TherapySession ts in sessions)
            {
                if (ts.Id == SessionID)
                    continue;
                if (ts.SessionStartTime <= startTime && ts.SessionEndTime >= startTime)
                    return "Start tijd zit binnen een andere afspraak";
                if (ts.SessionStartTime <= endTime && ts.SessionEndTime >= endTime)
                    return "Een andere sessie staat gepland voor dat deze afgerond kan worden!";
                if (startTime <= ts.SessionStartTime && endTime >= ts.SessionStartTime)
                    return "Een andere sessie staat midden in deze sessie gepland!";
                if (startTime <= ts.SessionEndTime && endTime >= ts.SessionEndTime)
                    return "Een andere sessie staat midden in deze sessie gepland!";

            }
            return null;
        }

        [Authorize]
        [Route("Home/Comments/{id:int}")]
        public IActionResult CommentsFromPatentFile(int id)
        {
            CommentListData data = new();
            data.dossierID = id;
            data.comments = _dataReciever.GetOnePatientFile(id).comments;
            return View("Comments", data);
        }

        [Authorize(Roles = "PhysicalTherapist, Intern")]
        [Route("Home/NewComment/{id:int}")]
        [HttpGet]
        public IActionResult NewComment(int id)
        {
            NewCommentData data = new(_dataReciever, id);
            return View("NewComment", data);
        }

        [Authorize(Roles = "PhysicalTherapist, Intern")]
        [HttpPost]
        public IActionResult NewComment(IFormCollection form)
        {
            Comment c = new();
            c.DateMade = DateTime.Now;
            int dossierID = int.Parse(form["DossierID"]);
            c.VisibleToPatient = bool.Parse(form["Comment.VisibleToPatient"]);
            c.CommenterID = int.Parse(form["Comment.CommenterID"]);
            FysioWorker w = _dataReciever.GetOneFysioWorker(c.CommenterID);
            if (w == null)
                ModelState.AddModelError("Comment.CommenterID", "Fysio bestaat niet!");
            else
                c.CommentMadeBy = w;

            if (form["Comment.CommentText"] == string.Empty)
                ModelState.AddModelError("Comment.CommentText", "Commentaar moet text bevatten!");
            else
                c.CommentText = form["Comment.CommentText"];

            if (ModelState.IsValid)
            {
                _dataReciever.addComment(c, dossierID);
                return RedirectToAction("GetDossierDetails", new { id = dossierID });
            }
            NewCommentData data = new(_dataReciever, dossierID, c);
            return View(data);
        }

        [Authorize]
        [Route("Home/DossierDetails/{id:int}")]
        [Route("Home/PatientDossier")]
        public IActionResult GetDossierDetails(int id)
        {
            PatientFile p = _dataReciever.GetOnePatientFile(id);
            if (p != null)
            {
                foreach (TherapySession ts in p.sessions)
                {
                    ts.SesionDoneBy = _dataReciever.GetOneFysioWorker(ts.SessionDoneByID);
                }
                return View("DossierDetails", p);
            }
            else return View("NotFound");
        }

        [Authorize(Roles = "PhysicalTherapist")]
        [HttpGet]
        public async Task<IActionResult> NewDossier()
        {
            NewDossierData data = new(_dataReciever);
            await data.FillDiagnoses(_consumer);
            return View(data);
        }


        [Authorize(Roles = "PhysicalTherapist")]
        [HttpPost]
        public async Task<IActionResult> NewDossier(IFormCollection form)
        {
            PatientFile f = new();
            if (form["selectedPatient"] == string.Empty)
                ModelState.AddModelError("selectedPatient", "Patient mag niet leeg zijn!");
            else if (int.Parse(form["selectedPatient"]) < 0)
                ModelState.AddModelError("selectedPatient", "PatientID mag niet negatief zijn!");
            else
                f.patientID = int.Parse(form["selectedPatient"]);

            if (_dataReciever.GetOnePatient(f.patientID) == null)
                ModelState.AddModelError("selectedPatient", "Patent bestaat niet!");
            else
                f.patient = _dataReciever.GetOnePatient(f.patientID);
            if (f.patient != null)
            {
                f.age = f.patient.Age;
                f.isStudent = f.patient.IsStudent;
            }

            if (form["file.issueDescription"] == string.Empty)
                ModelState.AddModelError("file.issueDescription", "Beschrijving mag niet leeg zijn!");
            else if (form["file.issueDescription"] == "")
                ModelState.AddModelError("file.issueDescription", "Beschrijving mag niet leeg zijn!");
            else
                f.issueDescription = form["file.issueDescription"];

            Diagnose d = null;
            if (form["selectedDiagnose"] == string.Empty)
                ModelState.AddModelError("selectedDiagnose", "Geen diagnose geselecteerd!");
            else
            {
                f.diagnoseCode = form["selectedDiagnose"];
                d = await _consumer.GetOneDiagnose(int.Parse(f.diagnoseCode));
            }
            if (d == null)
                ModelState.AddModelError("selectedDiagnose", "Geen diagnose gevonden!");
            else
                f.diagnoseCodeComment = $"{d.lichaamslocalisatie}: {d.Pathologie}";

            if (form["SelectedIntaker"] == string.Empty)
                ModelState.AddModelError("SelectedIntaker", "'Intake gedaan door:' mag niet leeg zijn!");
            else
                f.intakeDoneByID = int.Parse(form["SelectedIntaker"]);
            if (_dataReciever.GetOneFysioWorker(f.intakeDoneByID) == null)
                ModelState.AddModelError("SelectedIntaker", "Fysiotherapeut of student bestaat niet!");
            f.intakeDoneBy = _dataReciever.GetOneFysioWorker(f.intakeDoneByID);

            if (f.intakeDoneBy != null)
            {
                if (f.intakeDoneBy.IsStudent && form["selectedSupperviser"] == string.Empty)
                    ModelState.AddModelError("selectedSupperviser", "Intake onder supervisie van mag niet leeg zijn als de intake door een student gedaan is!");
                else if (!f.intakeDoneBy.IsStudent && form["selectedSupperviser"] != string.Empty)
                    ModelState.AddModelError("selectedSupperviser", "Intake onder supervisie van moet leeg zijn als de intake door een fysiotherapeut is gedaan!");
                else
                    f.IdintakeSuppervisedBy = int.Parse(form["selectedSupperviser"]);
            }
            int id = f.IdintakeSuppervisedBy ?? default(int);
            if (id != 0)
            {
                if (_dataReciever.GetOneFysioWorker(id) == null)
                    ModelState.AddModelError("selectedSupperviser", "Fysiotherapeut bestaat niet!");
                else
                    f.intakeSuppervisedBy = _dataReciever.GetOneFysioWorker(id);
            }

            if (form["selectedMain"] == string.Empty)
                ModelState.AddModelError("selectedMain", "Geen hoofdbehandelaar geselecteerd!");
            else
                f.IdmainTherapist = int.Parse(form["selectedMain"]);
            if (_dataReciever.GetOneFysioWorker(f.IdmainTherapist) == null)
                ModelState.AddModelError("selectedMain", "Fysiotherapeut of student bestaat niet!");
            else
                f.mainTherapist = _dataReciever.GetOneFysioWorker(f.IdmainTherapist);

            string[] timelist = form["file.registerDate"].ToString().Split("-");
            if (timelist[0] == "")
                ModelState.AddModelError("file.registerDate", "Datum van registratie mag niet leeg zijn!");
            else
            {
                DateTime time = new DateTime(int.Parse(timelist[0]), int.Parse(timelist[1]), int.Parse(timelist[2]));
                if (time > DateTime.Now)
                    ModelState.AddModelError("file.registerDate", "Datum van registratie mag niet in de toekomst zijn!");
                else if (f.patient != null)
                    if (time < f.patient.Birthdate)
                        ModelState.AddModelError("file.registerDate", "Datum van registratie mag niet voorafgaand aan geboorte zijn!");
                    else
                        f.registerDate = time;
            }

            timelist = form["file.fireDate"].ToString().Split("-");
            if (timelist[0] != "")
            {
                DateTime time2 = new DateTime(int.Parse(timelist[0]), int.Parse(timelist[1]), int.Parse(timelist[2]));
                if (time2 < f.registerDate)
                    ModelState.AddModelError("file.fireDate", "Datum van ontslag mag niet voorafgaand aan datum van registratie zijn!");
                f.fireDate = time2;
            }

            if (form["selectedPlan"] == string.Empty)
                ModelState.AddModelError("selectedPlan", "Behandel plan mag niet leeg zijn!");
            else
                f.IdactionPlan = int.Parse(form["selectedPlan"]);
            if (_dataReciever.GetOneActionPlan(f.IdactionPlan) == null)
                ModelState.AddModelError("selectedPlan", "Behandelplan bestaat niet!");
            f.actionPlan = _dataReciever.GetOneActionPlan(f.IdactionPlan);

            if (ModelState.IsValid)
            {
                _dataReciever.AddPatientFile(f);
                return View("Index");
            }
            NewDossierData data = new(_dataReciever);
            await data.FillDiagnoses(_consumer);
            data.file = f;
            return View(data);
        }

        [Authorize(Roles = "PhysicalTherapist, Intern")]
        [HttpGet]
        public IActionResult NewActionPlan()
        {
            return View();
        }

        [Authorize(Roles = "PhysicalTherapist, Intern")]
        [HttpPost]
        public IActionResult NewActionPlan(IFormCollection formColl)
        {
            ActionPlan plan = new();
            if (formColl[nameof(plan.SessionsPerWeek)].ToString() == string.Empty)
                ModelState.AddModelError(nameof(plan.SessionsPerWeek), "Aantal sessies mag niet leeg zijn!");
            else
                plan.SessionsPerWeek = int.Parse(formColl["SessionsPerWeek"].ToString());

            if (formColl[nameof(plan.TimePerSession)].ToString() == string.Empty)
                ModelState.AddModelError(nameof(plan.TimePerSession), "Tijd per sessie mag niet leeg zijn!");
            else
                plan.TimePerSession = int.Parse(formColl["TimePerSession"].ToString());

            if (plan.SessionsPerWeek < 0)
                ModelState.AddModelError(nameof(plan.SessionsPerWeek), "Aantal sesises moet een positief getal zijn!");
            if (plan.SessionsPerWeek == 0)
                ModelState.AddModelError(nameof(plan.SessionsPerWeek), "Aantal sesises mag niet 0 zijn!");
            if (plan.TimePerSession < 0)
                ModelState.AddModelError(nameof(plan.TimePerSession), "Tijd per sessie moet een positief getal zijn!");
            if (plan.TimePerSession == 0)
                ModelState.AddModelError(nameof(plan.TimePerSession), "Tijd per sessie mag niet 0 zijn!");

            if (ModelState.IsValid)
            {
                _dataReciever.AddActionPlan(plan);
                return View("Index");
            }
            return View(plan);
        }

        [Authorize]
        [Authorize(Roles = "PhysicalTherapist, Intern")]
        [Route("Home/Patients/{id:int}")]
        public IActionResult GetPatientDetails(int id)
        {
            if (_dataReciever.GetOnePatient(id) != null)
                return View("PatientDetails", _dataReciever.GetOnePatient(id));
            else return View("NotFound");
        }

        [Authorize(Roles = "PhysicalTherapist")]
        [HttpGet]
        public IActionResult NewPatient(Adress adress)
        {
            Patient p = new();
            p.Adress = adress;
            p.AdressID = adress.AdressID;
            return View(p);
        }

        [Authorize(Roles = "PhysicalTherapist")]
        [HttpPost]
        public IActionResult NewPatient(Patient patient)
        {
            if (patient.AdressID != 0)
                patient.Adress = _dataReciever.GetOneAdress(patient.AdressID);
            ModelState[nameof(patient.Adress)].ValidationState = ModelValidationState.Valid;
            if (patient.Name == null)
                ModelState.AddModelError(nameof(patient.Name), "Naam mag niet leeg zijn!");
            if (patient.PatientNumber == null)
                ModelState.AddModelError(nameof(patient.PatientNumber), "ID mag niet leeg zijn!");
            if (patient.IsStudent)
                patient.StudentNumber = patient.PatientNumber;
            else
                patient.WorkerNumber = patient.PatientNumber;
            if (patient.Birthdate > DateTime.Now)
                ModelState.AddModelError(nameof(patient.Birthdate), "Datum kan niet later dan vandaag!");
            patient.SetAge();
            if (patient.Age < 16)
                ModelState.AddModelError(nameof(patient.Birthdate), "Patient mag niet jonger dan 16 zijn!");
            if (patient.Age > 100)
                ModelState.AddModelError(nameof(patient.Birthdate), "Patient kan niet ouder zijn dan 100 jaar!");

            if (patient.Email == null)
                ModelState.AddModelError(nameof(patient.Email), "Email mag niet Leeg zijn!");

            if (ModelState.IsValid)
            {
                patient.SetAge();
                _dataReciever.AddPatient(patient);
                return View("Index");
            }
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult NewAdress()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult NewAdress(Adress adress)
        {
            if (adress.Country == null)
                ModelState.AddModelError(nameof(adress.Country), "Land mag niet leeg zijn!");

            if (adress.City == null)
                ModelState.AddModelError(nameof(adress.City), "Stad mag niet leeg zijn!");
            if (adress.PostalCode == null)
                ModelState.AddModelError(nameof(adress.PostalCode), "Postcode mag niet leeg zijn!");
            if (adress.Street == null)
                ModelState.AddModelError(nameof(adress.Street), "Straat naam mag niet leeg zijn!");
            if (adress.HouseNumber == null)
                ModelState.AddModelError(nameof(adress.HouseNumber), "Huis nummer mag niet leeg zijn!");

            if (ModelState.IsValid)
            {
                _dataReciever.AddAdress(adress);
                Adress a = _dataReciever.getLastAddedAdress(adress);
                return RedirectToAction("NewPatient", a);
            }
            return View(adress);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
