using DatabaseHandler.Models;
using DomainModels.Models;
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
using DomainServices.Repos;

namespace FysioAppUX.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        private readonly IActionPlan _actionPlan;
        private readonly IAdress _adress;
        private readonly IComment _comment;
        private readonly IPatient _patient;
        private readonly IPatientFile _patientFile;
        private readonly IFysioWorker _fysioWorker;
        private readonly ITherapySession _therapySession;
        private readonly ITreatment _treatment;
        private readonly IDiagnose _diagnose;
        private readonly FysioIdentityDBContext _context;

        public HomeController(IActionPlan actionPlan, IAdress adress, IComment comment, IPatient patient, IPatientFile patientFile, IFysioWorker fysioWorker, IDiagnose diagnose, ITherapySession therapySession, ITreatment treatment, FysioIdentityDBContext context)
        {
            _actionPlan = actionPlan;
            _comment = comment;
            _patient = patient;
            _patientFile = patientFile;
            _fysioWorker = fysioWorker;
            _adress = adress;
            _therapySession = therapySession;
            _treatment = treatment;
            _diagnose = diagnose;
            _context = context;
        }

        [Route("Home/Index")]
        [Route("/")]
        public IActionResult Index()
        {
            _patientFile.RemoveAllFiredPatientFiles();
            _therapySession.RemovePastSessions();
            if (User.Identity.Name != null)
            {
                if (User.IsInRole("Patient"))
                    return RedirectToAction("PatientHome");
                return RedirectToAction("FysioHome");
            }
            return View();
        }

        [Authorize(Roles = "PhysicalTherapist, Intern")]
        [Route("Home/FysioTimes/{id:int}")]
        public IActionResult FysioTimes(int id)
        {
            return View("UpdateFysioTimes", _fysioWorker.GetFysioWorkerByID(id));
        }

        [Authorize(Roles = "PhysicalTherapist, Intern")]
        [Route("Home/UpdateFysioTime")]
        [HttpPost]
        public IActionResult UpdateFysioTimes(IFormCollection form)
        {
            FysioWorker worker = _fysioWorker.GetFysioWorkerByID(int.Parse(form["FysioWorkerID"]));

            if (form["AvailableDays"] == string.Empty)
            {
                ModelState.AddModelError("AvailableDays", "Er moet minimaal 1 beschikbare dag zijn");
            }
            else
            {
                string days = form["AvailableDays"];
                days = days.ToUpper();
                string[] dayStrings = days.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                days = "";
                foreach (string day in dayStrings)
                {
                    switch (day)
                    {
                        case "MAANDAG":
                            days += "1 ";
                            break;
                        case "DINSDAG":
                            days += "2 ";
                            break;
                        case "WOENSDAG":
                            days += "3 ";
                            break;
                        case "DONDERDAG":
                            days += "4 ";
                            break;
                        case "VRIJDAG":
                            days += "5 ";
                            break;
                        default:
                            break;
                    }
                }
                if (days.Equals(""))
                    ModelState.AddModelError("AvailableDays", "geen geldige dagen ingevoerd!");
                else
                    worker.AvailableDays = days;
            }

            worker.DayStartTime = DateTime.Parse(form["DayStartTime"]);

            DateTime end = DateTime.Parse(form["DayEndTime"]);
            if (end < worker.DayStartTime)
                ModelState.AddModelError("DayEndTime", "Eind tijd mag niet eerder zijn dan begin tijd");
            else
                worker.DayEndTime = end;

            if (ModelState.IsValid)
            {
                _fysioWorker.UpdateFysioWorker(worker);
                return RedirectToAction("FysioHome");
            }
            return View(worker);
        }

        [Authorize(Roles = "Patient")]
        [Route("Home/PatientHome")]
        public IActionResult PatientHome()
        {
            var userId = User.Identity.Name;
            string email = _context.GetUserEmail(userId);
            Patient pat = _patient.GetPatientByEmail(email);
            if (pat == null)
                return RedirectToAction("NoDossierHome");
            PatientFile p = _patientFile.GetPatientFileByPatient(pat.PatientID);
            if (p == null)
                return RedirectToAction("NoDossierHome");

            List<Comment> comments = new();

            foreach (Comment c in p.Comments)
            {
                if (c.VisibleToPatient)
                {
                    c.CommentMadeBy = _fysioWorker.GetFysioWorkerByID(c.CommenterID);
                    comments.Add(c);
                }
            }

            p.Comments = comments;

            foreach (TherapySession s in p.Sessions)
            {
                s.SesionDoneBy = _fysioWorker.GetFysioWorkerByID(s.SessionDoneByID);
            }

            return View("DossierDetails", new DossierData(p, true));
        }

        [Authorize(Roles = "Patient")]
        [Route("Home/NoDossierHome")]
        public IActionResult NoDossierHome()
        {
            return View();
        }

        [Route("Home/UpdateAdress/{id:int}")]
        [HttpGet]
        public IActionResult UpdateAdress(int id)
        {
            Patient p = _patientFile.GetPatientFileByID(id).Patient;
            UpdateAdressData data = new(p.Adress, p.PatientID);
            return View(data);
        }

        [HttpPost]
        public IActionResult UpdateAdress(IFormCollection form)
        {
            int patientID = int.Parse(form["PatientID"]);
            Adress adress = _patient.GetPatientByID(patientID).Adress;

            if (form["Adress.Country"] == string.Empty)
                ModelState.AddModelError("Adress.Country", "Land mag niet leeg zijn!");
            else
                adress.Country = form["Adress.Country"];

            if (form["Adress.City"] == string.Empty)
                ModelState.AddModelError("Adress.City", "Stad mag niet leeg zijn!");
            else
                adress.City = form["Adress.City"];

            if (form["Adress.PostalCode"] == string.Empty)
                ModelState.AddModelError("Adress.PostalCode", "Postcode mag niet leeg zijn!");
            else
                adress.PostalCode = form["Adress.PostalCode"];

            if (form["Adress.Street"] == string.Empty)
                ModelState.AddModelError("Adress.Street", "Straat naam mag niet leeg zijn!");
            else
                adress.Street = form["Adress.Street"];

            if (form["Adress.HouseNumber"] == string.Empty)
                ModelState.AddModelError("Adress.HouseNumber", "Huis nummer mag niet leeg zijn!");
            else
                adress.HouseNumber = form["Adress.HouseNumber"];

            if (ModelState.IsValid)
            {
                _adress.UpdateAdress(adress);
                return RedirectToAction("PatientHome");
            }
            UpdateAdressData data = new(adress, patientID);
            return View(data);
        }

        [Authorize(Roles = "Patient")]
        [Route("Home/DeletePatientSession/{id:int}")]
        [HttpGet]
        public IActionResult DeletePatientSession(int id)
        {
            TherapySession session = _therapySession.GetTherapySessionByID(id);
            return View("DeletePatientSession", session);
        }

        [Authorize]
        [Route("Home/CancelDeleteDossierListSession/{id:int}")]
        [HttpGet]
        public IActionResult CancelDeleteDossierListSession(int id)
        {
            int FileId = _patientFile.GetPatientFileBySession(id).ID;
            return RedirectToAction("SessionsFromPatientFile", "Home", new { id = FileId });
        }

        [Route("Home/CancelDeleteSession/{id:int}")]
        public IActionResult CancelDeleteSession(int id)
        {
            int fileId = _patientFile.GetPatientFileBySession(id).ID;
            return RedirectToAction("GetDossierDetails", "Home", new { id = fileId });
        }

        [Authorize(Roles = "Patient")]
        [Route("Home/ComfirmDeletePatientSession/{id:int}")]
        public IActionResult ComfirmDeletePatientSession(int id)
        {
            TherapySession session = _therapySession.GetTherapySessionByID(id);
            if (DateTime.Now.Subtract(session.SessionStartTime).TotalHours < 24)
            {
                _patientFile.RemoveSessionFromPatientFile(session, false);
                _therapySession.RemoveTherapySession(session);

                return RedirectToAction("PatientHome");
            }
            return View("Index");
        }


        [Authorize(Roles = "PhysicalTherapist, Intern")]
        public IActionResult Patients()
        {
            return View(_patient.GetAllPatients());
        }

        [Authorize(Roles = "PhysicalTherapist, Intern")]
        [Route("Home/UpdatePatient/{id:int}")]
        [HttpGet]
        public IActionResult UpdatePatient(int id)
        {
            PatientFile patientFile = _patientFile.GetPatientFileByID(id);
            UpdatePatientData data = new(patientFile.Patient, id);
            return View(data);
        }

        [Authorize(Roles = "PhysicalTherapist, Intern")]
        [HttpPost]
        public IActionResult UpdatePatient(IFormCollection form)
        {
            int dossierID = int.Parse(form["DossierID"]);
            Patient p = _patientFile.GetPatientFileByID(dossierID).Patient;
            Adress a = p.Adress;

            if (form["Patient.EnsuranceCompany"] == string.Empty)
                ModelState.AddModelError("Patient.EnsuranceCompany", "Verzekering mag niet leeg zijn!");
            else
                p.EnsuranceCompany = form["Patient.EnsuranceCompany"];

            if (form["Patient.ImageUrl"] == string.Empty)
                p.ImageUrl = "https://i.imgur.com/EHW5HhX.png";
            else
                p.ImageUrl = form["Patient.ImageUrl"];

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
                _patient.UpdatePatient(p);
                return RedirectToAction("GetDossierDetails", "Home", new { id = dossierID });
            }
            return View(new UpdatePatientData(p, dossierID));
        }

        [Authorize(Roles = "PhysicalTherapist, Intern")]
        [HttpPost]
        public IActionResult UpdateDossier(IFormCollection form)
        {
            PatientFile file = _patientFile.GetPatientFileByID(int.Parse(form["File.ID"]));
            ActionPlan plan = file.ActionPlan;

            if (form["File.IdmainTherapist"] == string.Empty)
                ModelState.AddModelError("File.IdmainTherapist", "Geen hoofdbehandelaar geselecteerd!");
            else
            {
                int mainID = int.Parse(form["File.IdmainTherapist"]);
                file.IdmainTherapist = mainID;
                FysioWorker main = _fysioWorker.GetFysioWorkerByID(mainID);
                if (main != null)
                    file.MainTherapist = main;
                else
                    ModelState.AddModelError("File.IdmainTherapist", "Geselecteerde behandelaar bestaat niet!");
            }

            var timelist = form["file.fireDate"].ToString().Split("-");
            if (timelist[0] != "")
            {
                DateTime time2 = new(int.Parse(timelist[0]), int.Parse(timelist[1]), int.Parse(timelist[2]));
                if (time2 < file.RegisterDate)
                    ModelState.AddModelError("file.fireDate", "Datum van ontslag mag niet voorafgaand aan datum van registratie zijn!");
                file.FireDate = time2;
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
                file.ActionPlan = plan;
                _actionPlan.UpdateActionPlan(plan);
                _patientFile.UpdatePatientFile(file);

                if (User.IsInRole("Patient"))
                    return RedirectToAction("PatientHome", "Home");
                return RedirectToAction("GetDossierDetails", "Home", new { id = file.ID });
            }
            file.ActionPlan = plan;
            UpdateDossierData data = new(file, _fysioWorker);
            return View(data);
        }

        [Authorize(Roles = "PhysicalTherapist, Intern")]
        [Route("Home/UpdateDossier/{id:int}")]
        [HttpGet]
        public IActionResult UpdateDossier(int id)
        {
            PatientFile file = _patientFile.GetPatientFileByID(id);
            UpdateDossierData data = new(file, _fysioWorker);
            return View(data);
        }

        [Authorize(Roles = "PhysicalTherapist, Intern")]
        public IActionResult Dossiers()
        {
            var userId = User.Identity.Name;
            string email = _context.GetUserEmail(userId);
            int id = _fysioWorker.GetFysioWorkerByEmail(email).FysioWorkerID;
            return View(_patientFile.GetPatientFileByFysio(id));
        }

        [Authorize]
        [Route("Home/DeleteSession/{id:int}")]
        public IActionResult DeleteSession(int id)
        {
            TherapySession session = _therapySession.GetTherapySessionByID(id);
            return View("DeleteSession", session);
        }

        [Authorize]
        [Route("Home/DeleteSessionDossierSessionList/{id:int}")]
        public IActionResult DeleteSessionDossierSessionList(int id)
        {
            TherapySession session = _therapySession.GetTherapySessionByID(id);
            return View("DeleteSessionDossierSessionList", session);
        }

        [Authorize]
        [Route("Home/HomeDeleteSession/{id:int}")]
        public IActionResult HomeDeleteSession(int id)
        {
            TherapySession session = _therapySession.GetTherapySessionByID(id);
            return View("HomeDeleteSession", session);
        }

        [Authorize]
        [Route("Home/ListDeleteSession/{id:int}")]
        public IActionResult ListDeleteSession(int id)
        {
            TherapySession session = _therapySession.GetTherapySessionByID(id);
            return View("ListDeleteSession", session);
        }

        [Authorize]
        [Route("Home/ComfirmDeleteSessionDossierSessionList/{id:int}")]
        public IActionResult ComfirmDeleteSessionDossierSessionList(int id)
        {
            TherapySession session = _therapySession.GetTherapySessionByID(id);
            int fileId = _patientFile.GetPatientFileBySession(id).ID;
            if (CanDelete(session))
            {
                _patientFile.RemoveSessionFromPatientFile(session, false);
                _therapySession.RemoveTherapySession(session);
            }
            return RedirectToAction("SessionsFromPatientFile", "Home", new { id = fileId });
        }

        [Authorize]
        [Route("Home/ListComfirmDeleteSession/{id:int}")]
        public IActionResult ListComfirmDeleteSession(int id)
        {
            TherapySession session = _therapySession.GetTherapySessionByID(id);
            if (CanDelete(session))
            {
                _patientFile.RemoveSessionFromPatientFile(session, false);
                _therapySession.RemoveTherapySession(session);
            }

            return RedirectToAction("SessionsFromTherapist");
        }


        [Authorize]
        [Route("Home/HomeComfirmDeleteSession/{id:int}")]
        public IActionResult HomeComfirmDeleteSession(int id)
        {
            TherapySession session = _therapySession.GetTherapySessionByID(id);
            if (CanDelete(session))
            {
                _patientFile.RemoveSessionFromPatientFile(session, false);
                _therapySession.RemoveTherapySession(session);
            }

            return RedirectToAction("FysioHome");
        }

        private bool CanDelete(TherapySession session)
        {
            if (User.IsInRole("Patient"))
                return session.SessionStartTime.Subtract(DateTime.Now).TotalHours > 24;
            else
                return DateTime.Now.Subtract(session.CreationDate).TotalHours < 24;
        }

        [Authorize]
        [Route("Home/ComfirmDeleteSession/{id:int}")]
        public IActionResult ComfirmDeleteSession(int id)
        {
            TherapySession session = _therapySession.GetTherapySessionByID(id);
            if (CanDelete(session))
            {
                _patientFile.RemoveSessionFromPatientFile(session, false);
                _therapySession.RemoveTherapySession(session);
            }
            return RedirectToAction("GetDossierDetails", "Home", new { id = _patientFile.GetPatientFileBySession(id).ID });
        }

        [Authorize]
        [HttpPost]
        public IActionResult UpdateSession(IFormCollection form)
        {
            int sessionID = int.Parse(form["Session.Id"]);
            TherapySession session = _therapySession.GetTherapySessionByID(sessionID);
            int FileId = int.Parse(form["DossierID"]);
            PatientFile file = _patientFile.GetPatientFileByID(FileId);

            if (form["Session.Type"] == string.Empty)
                ModelState.AddModelError("Session.Type", "Type behandeling mag niet leeg zijn!");
            else
            {
                session.Type = form["Session.Type"];
                Treatment b = _treatment.GetTreatmentByID(form["Session.Type"]);

                if (b.toelichting_verplicht.ToLower() == "ja" && form["Session.Specials"] == string.Empty)
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
                session.SesionDoneBy = _fysioWorker.GetFysioWorkerByID(id);
            }

            if (!_therapySession.CanFitSessionInPlan(file, session.SessionStartTime, session.Id))
                ModelState.AddModelError("Session.SessionStartTime", "Het maximale aantal sessies voor deze week is bereikt, er kunnen deze week geen sessies meer toegevoed worden!");

            if (ModelState.IsValid)
            {
                _therapySession.UpdateSession(session);

                TimeTableData timeData = new(1, int.Parse(form["IsFromList"]), file.ActionPlan, session.SessionDoneByID, file, session, _therapySession, _fysioWorker);
                return View("SessionTimeSelector", timeData);

            }
            NewSessionData data = new(_treatment, _fysioWorker, _patientFile, FileId, session);
            data.FillBehandelings();
            data.SetSelectedBehandeling();
            return View(data);
        }

        [Authorize]
        [Route("Home/UpdateSession/{id:int}")]
        [HttpGet]
        public IActionResult UpdateSession(int id)
        {
            TherapySession session = _therapySession.GetTherapySessionByID(id);
            int dossierId = _patientFile.GetPatientFileBySession(id).ID;
            NewSessionData data = new(_treatment, _fysioWorker, _patientFile, dossierId, session);
            data.FillBehandelings();
            data.SetSelectedBehandeling();
            data.IsFromList = 0;
            return View("UpdateSession", data);
        }

        [Authorize]
        [Route("Home/UpdateSessionFromList/{id:int}")]
        [HttpGet]
        public IActionResult UpdateSessionFromList(int id)
        {
            TherapySession session = _therapySession.GetTherapySessionByID(id);
            int dossierId = _patientFile.GetPatientFileBySession(id).ID;
            NewSessionData data = new(_treatment, _fysioWorker, _patientFile, dossierId, session);
            data.FillBehandelings();
            data.SetSelectedBehandeling();
            data.IsFromList = 1;
            return View("UpdateSession", data);
        }

        [Authorize]
        [Route("Home/UpdateSessionHome/{id:int}")]
        [HttpGet]
        public IActionResult UpdateSessionHome(int id)
        {
            TherapySession session = _therapySession.GetTherapySessionByID(id);
            int dossierId = _patientFile.GetPatientFileBySession(id).ID;
            NewSessionData data = new(_treatment, _fysioWorker, _patientFile, dossierId, session);
            data.FillBehandelings();
            data.SetSelectedBehandeling();
            data.IsFromList = 2;
            return View("UpdateSession", data);
        }

        [Authorize]
        [Route("Home/UpdateSessionDossierSessionList/{id:int}")]
        [HttpGet]
        public IActionResult UpdateSessionDossierSessionList(int id)
        {
            TherapySession session = _therapySession.GetTherapySessionByID(id);
            int dossierId = _patientFile.GetPatientFileBySession(id).ID;
            NewSessionData data = new(_treatment, _fysioWorker, _patientFile, dossierId, session);
            data.FillBehandelings();
            data.SetSelectedBehandeling();
            data.IsFromList = 3;
            return View("UpdateSession", data);
        }

        [Authorize]
        [Route("Home/Sessions/{id:int}")]
        [Route("Home/SessionsFromPatientFile/{id:int}")]
        public IActionResult SessionsFromPatientFile(int id)
        {
            if (User.IsInRole("Patient"))
                return RedirectToAction("SessionsForPatient");

            SessionListDatea s = new();
            foreach (TherapySession ts in _patientFile.GetPatientFileByID(id).Sessions)
            {
                ts.SesionDoneBy = _fysioWorker.GetFysioWorkerByID(ts.SessionDoneByID);
                s.Sessions.Add(new(true, ts, true, User.IsInRole("Patient"), false));
            }
            s.DossierID = id;
            s.IsFromSession = true;

            return View("Sessions", s);
        }

        [Authorize(Roles = "Patient")]
        public IActionResult SessionsForPatient()
        {
            var userId = User.Identity.Name;
            string email = _context.GetUserEmail(userId);

            PatientFile p = _patientFile.GetPatientFileByPatient(_patient.GetPatientByEmail(email).PatientID);
            if (p == null)
                return RedirectToAction("Index");

            SessionListDatea data = new();
            List<SessionItemData> sessions = new();
            foreach (TherapySession s in p.Sessions)
            {
                s.SesionDoneBy = (_fysioWorker.GetFysioWorkerByID(s.SessionDoneByID));
                sessions.Add(new(true, s, true, true, false));
            }
            data.Sessions = sessions;
            data.DossierID = p.ID;
            data.IsFromSession = true;
            return View("Sessions", data);
        }

        [Authorize(Roles = "PhysicalTherapist, Intern")]
        [Route("Home/Sessions")]
        public IActionResult SessionsFromTherapist()
        {
            FysioWorker w;
            SessionListDatea s = new();
            var userId = User.Identity.Name;
            string email = _context.GetUserEmail(userId);

            w = _fysioWorker.GetFysioWorkerByEmail(email);
            ;
            if (w != null)
            {
                foreach (TherapySession ts in _therapySession.GetTherapySessionsByFysio(w.FysioWorkerID))
                {
                    int dossierID = _patientFile.GetPatientFileBySession(ts.Id).ID;
                    s.Sessions.Add(new(true, ts, dossierID, User.IsInRole("Patient"), true));
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
        public IActionResult NewSession(int dossier)
        {
            NewSessionData SessionData;
            if (dossier != 0)
            {
                SessionData = new(_treatment, _fysioWorker, _patientFile, dossier, null);
                SessionData.FillBehandelings();
                return View(SessionData);
            }
            FysioWorker w;
            var userId = User.Identity.Name;
            string email = _context.GetUserEmail(userId);

            w = _fysioWorker.GetFysioWorkerByEmail(email);
            SessionData = new(_treatment, _patientFile, w);
            SessionData.FillBehandelings();
            return View("NewSessionForFysio", SessionData);
        }

        [Authorize]
        [Route("Home/NewSessionFromList/{dossier:int}")]
        [HttpGet]
        public IActionResult NewSessionFromList(int dossier)
        {
            NewSessionData SessionData;
            if (dossier != 0)
            {
                SessionData = new(_treatment, _fysioWorker, _patientFile, dossier, null);
                SessionData.IsFromList = 1;
                SessionData.FillBehandelings();
                return View("NewSession", SessionData);
            }
            FysioWorker w;
            var userId = User.Identity.Name;
            string email = _context.GetUserEmail(userId);
            w = _fysioWorker.GetFysioWorkerByEmail(email);
            SessionData = new(_treatment, _patientFile, w);
            SessionData.FillBehandelings();
            SessionData.IsFromList = 1;
            return View("NewSessionForFysio", SessionData);
        }

        [Authorize]
        [Route("Home/NewSessionFromHome")]
        [HttpGet]
        public IActionResult NewSessionFromHome()
        {
            NewSessionData SessionData;

            FysioWorker w;
            var userId = User.Identity.Name;
            string email = _context.GetUserEmail(userId);
            w = _fysioWorker.GetFysioWorkerByEmail(email);
            SessionData = new(_treatment, _patientFile, w);
            SessionData.FillBehandelings();
            SessionData.IsFromList = 2;
            return View("NewSessionForFysio", SessionData);
        }


        [Authorize]
        [HttpPost]
        public IActionResult NewSessionForFysio(IFormCollection form)
        {
            int FileId = 0;
            Patient p;
            TherapySession session = new();
            session.CreationDate = DateTime.Now;
            int FysioID = int.Parse(form["Worker.FysioWorkerID"]);
            session.SessionDoneByID = FysioID;
            session.SesionDoneBy = _fysioWorker.GetFysioWorkerByID(FysioID);

            if (form["PatientID"] == string.Empty)
                ModelState.AddModelError("PatientID", "Patient mag niet leeg zijn!");
            else
            {
                p = _patient.GetPatientByID(int.Parse(form["PatientID"]));
                if (p == null)
                    ModelState.AddModelError("PatientID", "Patient bestaat niet!");
                else
                    FileId = _patientFile.GetPatientFileByPatient(p.PatientID).ID;
            }

            PatientFile file = _patientFile.GetPatientFileByID(FileId);

            if (form["Session.Type"] == string.Empty)
                ModelState.AddModelError("Session.Type", "Type behandeling mag niet leeg zijn!");
            else
            {
                session.Type = form["Session.Type"];
                Treatment b = _treatment.GetTreatmentByID(form["Session.Type"]);

                if (b.toelichting_verplicht.ToLower() == "ja" && form["Session.Specials"] == string.Empty)
                    ModelState.AddModelError("Session.Specials", "Toelichting mag niet leeg zijn als deze verplicht is!");
                else
                    session.Specials = form["Session.Specials"];
            }

            if (form["Session.Description"] == string.Empty)
                ModelState.AddModelError("Session.Description", "Beschrijving mag niet leeg zijn!");
            else
                session.Description = form["Session.Description"];

            session.IsPractiseRoom = bool.Parse(form["Session.IsPractiseRoom"]);

            if (ModelState.IsValid)
            {
                _therapySession.AddTherapySession(session);
                TimeTableData timeData = new(0, int.Parse(form["IsFromList"]), file.ActionPlan, session.SessionDoneByID, file, session, _therapySession, _fysioWorker);
                return View("SessionTimeSelector", timeData);
            }
            NewSessionData data = new(_treatment, _patientFile, session.SesionDoneBy);
            data.FillBehandelings();
            data.Session = session;
            return View(data);
        }

        public IActionResult SessionTimeSelector(IFormCollection form)
        {
            int FileId = int.Parse(form["File.ID"]);
            PatientFile file = _patientFile.GetPatientFileByID(FileId);
            int FysioID = int.Parse(form["FysioId"]);
            TherapySession session = _therapySession.GetTherapySessionByID(int.Parse(form["TherapySession.Id"]));

            if (form["TherapySession.SessionStartTime"] == string.Empty)
                ModelState.AddModelError("TherapySession.SessionStartTime", "Start tijd mag niet leeg zijn!");
            else
            {
                DateTime startTime = DateTime.Parse(form["TherapySession.SessionStartTime"]);
                DateTime endTime = startTime.AddHours(file.ActionPlan.TimePerSession);


                string registerError = IsWithinRegisterTime(startTime, endTime, file);
                string timeError = CheckTime(FysioID, startTime, endTime, session.Id);
                if (timeError != null)
                    ModelState.AddModelError("TherapySession.SessionStartTime", timeError);
                else if (registerError != null)
                    ModelState.AddModelError("TherapySession.SessionStartTime", registerError);
                else if (User.IsInRole("Patient") && startTime < DateTime.Today.AddDays(1))
                    ModelState.AddModelError("Session.SessionStartTime", "Sessie mag niet vandaag in worden geplaned");
                else
                {
                    session.SessionStartTime = startTime;
                    session.SessionEndTime = endTime;
                }
            }


            if (!_therapySession.CanFitSessionInPlan(file, session.SessionStartTime, session.Id))
                ModelState.AddModelError("TherapySession.SessionStartTime", "Het maximale aantal sessies voor deze week is bereikt, er kunnen deze week geen sessies meer toegevoed worden!");

            if (ModelState.IsValid)
            {
                _therapySession.UpdateSession(session);
                _patientFile.AddSession(session, FileId);

                if (int.Parse(form["IsUpdate"]) == 1)
                {
                    if (int.Parse(form["IsFromList"]) == 1)
                    {
                        return RedirectToAction("SessionsFromTherapist", "Home");
                    }
                    else if (int.Parse(form["IsFromList"]) == 2)
                    {
                        if (User.IsInRole("Patient"))
                            return RedirectToAction("PatientHome", "Home");
                        return RedirectToAction("FysioHome", "Home");
                    }
                    else if (int.Parse(form["IsFromList"]) == 3)
                    {
                        return RedirectToAction("SessionsFromPatientFile", "Home", new { id = FileId });
                    }
                    return RedirectToAction("GetDossierDetails", "Home", new { id = FileId });
                }
                else
                {

                    if (int.Parse(form["IsFromList"]) == 1)
                    {
                        List<SessionItemData> sessions = new();
                        foreach (TherapySession s in file.Sessions)
                        {
                            s.SesionDoneBy = (_fysioWorker.GetFysioWorkerByID(s.SessionDoneByID));
                            sessions.Add(new(true, s, true, User.IsInRole("Patient"), false));
                        }
                        return RedirectToAction("SessionsFromPatientFile", "Home", new { id = FileId });
                    }
                    else if (int.Parse(form["IsFromList"]) == 2)
                    {
                        if (User.IsInRole("Patient"))
                            return RedirectToAction("PatientHome", "Home");
                        return RedirectToAction("FysioHome", "Home");
                    }
                    return RedirectToAction("GetDossierDetails", "Home", new { id = FileId });
                }
            }

            TimeTableData data = new(int.Parse(form["IsUpdate"]), int.Parse(form["IsFromList"]), file.ActionPlan, session.SessionDoneByID, file, session, _therapySession, _fysioWorker);
            return View(data);
        }

        public string IsWithinRegisterTime(DateTime startTime, DateTime endTime, PatientFile file)
        {
            if (startTime < file.RegisterDate)
                return "Sessie mag niet voor registratie plaatsvinden";
            else if (startTime > file.FireDate && file.FireDate != new DateTime())
                return "Sessie mag niet na ontslag plaatsvinden!";
            else if (endTime > file.FireDate && file.FireDate != new DateTime())
                return "Sessie mag niet na ontslag eindigen!";
            else if (startTime < DateTime.Today)
                return "Sessie mag niet in het verleden plaatsvinden";
            return null;
        }

        [Authorize]
        [HttpPost]
        public IActionResult NewSession(IFormCollection form)
        {
            TherapySession session = new();
            session.CreationDate = DateTime.Now;
            int FileId = int.Parse(form["DossierID"]);
            PatientFile file = _patientFile.GetPatientFileByID(FileId);

            if (form["Session.Type"] == string.Empty)
                ModelState.AddModelError("Session.Type", "Type behandeling mag niet leeg zijn!");
            else
            {
                session.Type = form["Session.Type"];
                Treatment b = _treatment.GetTreatmentByID(form["Session.Type"]);

                if (b.toelichting_verplicht.ToLower() == "ja" && form["Session.Specials"] == string.Empty)
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
                session.SesionDoneBy = _fysioWorker.GetFysioWorkerByID(id);
            }

            if (!_therapySession.CanFitSessionInPlan(file, session.SessionStartTime, -1))
                ModelState.AddModelError("Session.SessionStartTime", "Het maximale aantal sessies voor deze week is bereikt, er kunnen deze week geen sessies meer toegevoed worden!");

            if (ModelState.IsValid)
            {
                _therapySession.AddTherapySession(session);
                TimeTableData timeData = new(0, int.Parse(form["IsFromList"]), file.ActionPlan, session.SessionDoneByID, file, session, _therapySession, _fysioWorker);
                return View("SessionTimeSelector", timeData);

            }
            NewSessionData data = new(_treatment, _fysioWorker, _patientFile, FileId, null);
            data.FillBehandelings();
            data.Session = session;
            return View(data);
        }

        public string CheckTime(int fysioID, DateTime startTime, DateTime endTime, int SessionID)
        {
            FysioWorker fysio = _fysioWorker.GetFysioWorkerByID(fysioID);
            List<int> availableDays = GetAvailableDayNumbers(fysio.AvailableDays);
            if (!availableDays.Contains((int)startTime.DayOfWeek))
                return "Therapist is niet beschikbaar op de gekozen dag";
            if (startTime.TimeOfDay < fysio.DayStartTime.TimeOfDay)
                return "Afspraak valt (gedeeltelijk) buiten de werktijd van de therapist";
            if (endTime.TimeOfDay > fysio.DayEndTime.TimeOfDay)
                return "Afspraak valt (gedeeltelijk) buiten de werktijd van de therapist";


            List<TherapySession> sessions = _therapySession.GetTherapySessionsByFysio(fysioID);
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

        private static List<int> GetAvailableDayNumbers(string numbers)
        {
            List<int> dayNumbers = new();
            string[] numberStrings = numbers.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in numberStrings)
                dayNumbers.Add(int.Parse(s));
            return dayNumbers;
        }

        [Authorize]
        [Route("Home/Comments/{id:int}")]
        public IActionResult CommentsFromPatentFile(int id)
        {
            CommentListData data = new(_patientFile.GetPatientFileByID(id).Comments, id, User.IsInRole("Patient"));
            return View("Comments", data);
        }

        [Authorize(Roles = "PhysicalTherapist, Intern")]
        [Route("Home/NewComment/{id:int}")]
        [HttpGet]
        public IActionResult NewComment(int id)
        {
            NewCommentData data = new(_patientFile, _fysioWorker, id, null);
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
            FysioWorker w = _fysioWorker.GetFysioWorkerByID(c.CommenterID);
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
                _comment.AddComment(c, dossierID);
                return RedirectToAction("GetDossierDetails", new { id = dossierID });
            }
            NewCommentData data = new(_patientFile, _fysioWorker, dossierID, c);
            return View(data);
        }

        [Authorize]
        [Route("Home/DossierDetails/{id:int}")]
        [Route("Home/PatientDossier")]
        public IActionResult GetDossierDetails(int id)
        {
            PatientFile p = _patientFile.GetPatientFileByID(id);
            if (p != null)
            {
                foreach (TherapySession ts in p.Sessions)
                {
                    ts.SesionDoneBy = _fysioWorker.GetFysioWorkerByID(ts.SessionDoneByID);
                }
                return View("DossierDetails", new DossierData(p, false));
            }
            else return View("NotFound");
        }

        [Authorize(Roles = "PhysicalTherapist")]
        [HttpGet]
        public IActionResult NewDossier()
        {
            NewDossierData data = new(_actionPlan, _patient, _fysioWorker, _patientFile);
            data.FillDiagnoses(_diagnose);
            return View(data);
        }

        [Authorize(Roles = "PhysicalTherapist")]
        [HttpGet]
        public IActionResult FysioHome()
        {
            var userId = User.Identity.Name;
            string email = _context.GetUserEmail(userId);

            FysioWorker fysio = _fysioWorker.GetFysioWorkerByEmail(email);
            if (fysio == null)
            {
                return RedirectToAction("Index");
            }
            FysioData data = new(_therapySession, _patientFile, fysio);
            return View(data);
        }

        [Authorize(Roles = "PhysicalTherapist")]
        [HttpPost]
        public IActionResult NewDossier(IFormCollection form)
        {
            PatientFile f = new();
            if (form["selectedPatient"] == string.Empty)
                ModelState.AddModelError("selectedPatient", "Patient mag niet leeg zijn!");
            else if (int.Parse(form["selectedPatient"]) < 0)
                ModelState.AddModelError("selectedPatient", "PatientID mag niet negatief zijn!");
            else
                f.PatientID = int.Parse(form["selectedPatient"]);

            if (_patient.GetPatientByID(f.PatientID) == null)
                ModelState.AddModelError("selectedPatient", "Patent bestaat niet!");
            else
                f.Patient = _patient.GetPatientByID(f.PatientID);
            if (f.Patient != null)
            {
                f.Age = f.Patient.Age;
                f.IsStudent = f.Patient.IsStudent;
            }

            if (form["file.issueDescription"] == string.Empty)
                ModelState.AddModelError("file.issueDescription", "Beschrijving mag niet leeg zijn!");
            else if (form["file.issueDescription"] == "")
                ModelState.AddModelError("file.issueDescription", "Beschrijving mag niet leeg zijn!");
            else
                f.IssueDescription = form["file.issueDescription"];

            Diagnose d = null;
            if (form["selectedDiagnose"] == string.Empty)
                ModelState.AddModelError("selectedDiagnose", "Geen diagnose geselecteerd!");
            else
            {
                f.DiagnoseCode = form["selectedDiagnose"];
                d = _diagnose.GetDiagnoseByID(int.Parse(f.DiagnoseCode));
            }
            if (d == null)
                ModelState.AddModelError("selectedDiagnose", "Geen diagnose gevonden!");
            else
                f.DiagnoseCodeComment = $"{d.Lichaamslocalisatie}: {d.Pathologie}";

            if (form["SelectedIntaker"] == string.Empty)
                ModelState.AddModelError("SelectedIntaker", "'Intake gedaan door:' mag niet leeg zijn!");
            else
                f.IntakeDoneByID = int.Parse(form["SelectedIntaker"]);
            if (_fysioWorker.GetFysioWorkerByID(f.IntakeDoneByID) == null)
                ModelState.AddModelError("SelectedIntaker", "Fysiotherapeut of student bestaat niet!");
            f.IntakeDoneBy = _fysioWorker.GetFysioWorkerByID(f.IntakeDoneByID);

            if (f.IntakeDoneBy != null)
            {
                if (f.IntakeDoneBy.IsStudent && form["selectedSupperviser"] == string.Empty)
                    ModelState.AddModelError("selectedSupperviser", "Intake onder supervisie van mag niet leeg zijn als de intake door een student gedaan is!");
                else if (!f.IntakeDoneBy.IsStudent && form["selectedSupperviser"] != string.Empty)
                    ModelState.AddModelError("selectedSupperviser", "Intake onder supervisie van moet leeg zijn als de intake door een fysiotherapeut is gedaan!");
                else
                {
                    if (form["selectedSupperviser"] != string.Empty)
                        f.IdintakeSuppervisedBy = int.Parse(form["selectedSupperviser"]);
                }

            }
            int id = f.IdintakeSuppervisedBy;
            if (id != 0)
            {
                if (_fysioWorker.GetFysioWorkerByID(id) == null)
                    ModelState.AddModelError("selectedSupperviser", "Fysiotherapeut bestaat niet!");
                else
                    f.IntakeSuppervisedBy = _fysioWorker.GetFysioWorkerByID(id);
            }

            if (form["selectedMain"] == string.Empty)
                ModelState.AddModelError("selectedMain", "Geen hoofdbehandelaar geselecteerd!");
            else
                f.IdmainTherapist = int.Parse(form["selectedMain"]);
            if (_fysioWorker.GetFysioWorkerByID(f.IdmainTherapist) == null)
                ModelState.AddModelError("selectedMain", "Fysiotherapeut of student bestaat niet!");
            else
                f.MainTherapist = _fysioWorker.GetFysioWorkerByID(f.IdmainTherapist);

            string[] timelist = form["file.registerDate"].ToString().Split("-");
            if (timelist[0] == "")
                ModelState.AddModelError("file.registerDate", "Datum van registratie mag niet leeg zijn!");
            else
            {
                DateTime time = new(int.Parse(timelist[0]), int.Parse(timelist[1]), int.Parse(timelist[2]));
                if (time > DateTime.Now)
                    ModelState.AddModelError("file.registerDate", "Datum van registratie mag niet in de toekomst zijn!");
                else if (f.Patient != null)
                    if (time < f.Patient.Birthdate)
                        ModelState.AddModelError("file.registerDate", "Datum van registratie mag niet voorafgaand aan geboorte zijn!");
                    else
                        f.RegisterDate = time;
            }

            timelist = form["file.fireDate"].ToString().Split("-");
            if (timelist[0] != "")
            {
                DateTime time2 = new(int.Parse(timelist[0]), int.Parse(timelist[1]), int.Parse(timelist[2]));
                if (time2 < f.RegisterDate)
                    ModelState.AddModelError("file.fireDate", "Datum van ontslag mag niet voorafgaand aan datum van registratie zijn!");
                f.FireDate = time2;
            }

            if (form["selectedPlan"] == string.Empty)
                ModelState.AddModelError("selectedPlan", "Behandel plan mag niet leeg zijn!");
            else
                f.IdactionPlan = int.Parse(form["selectedPlan"]);
            if (_actionPlan.GetActionPlanByID(f.IdactionPlan) == null)
                ModelState.AddModelError("selectedPlan", "Behandelplan bestaat niet!");
            f.ActionPlan = _actionPlan.GetActionPlanByID(f.IdactionPlan);

            if (ModelState.IsValid)
            {
                _patientFile.AddPatientFile(f);
                if (User.IsInRole("Patient"))
                    return RedirectToAction("PatientHome", "Home");
                return RedirectToAction("FysioHome", "Home");
            }
            NewDossierData data = new(_actionPlan, _patient, _fysioWorker, _patientFile);
            data.FillDiagnoses(_diagnose);
            data.File = f;
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
                _actionPlan.AddActionPlan(plan);
                return View("Index");
            }
            return View(plan);
        }

        [Authorize]
        [Authorize(Roles = "PhysicalTherapist, Intern")]
        [Route("Home/Patients/{id:int}")]
        public IActionResult GetPatientDetails(int id)
        {
            if (_patient.GetPatientByID(id) != null)
                return View("PatientDetails", _patient.GetPatientByID(id));
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
                patient.Adress = _adress.GetAdressByID(patient.AdressID);
            if (patient.ImageUrl == null)
                patient.ImageUrl = "https://i.imgur.com/EHW5HhX.png";
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
            patient.Age = GetAge(patient.Birthdate);
            if (patient.Age < 16)
                ModelState.AddModelError(nameof(patient.Birthdate), "Patient mag niet jonger dan 16 zijn!");
            if (patient.Age > 100)
                ModelState.AddModelError(nameof(patient.Birthdate), "Patient kan niet ouder zijn dan 100 jaar!");

            if (patient.Email == null)
                ModelState.AddModelError(nameof(patient.Email), "Email mag niet Leeg zijn!");

            if (ModelState.IsValid)
            {
                patient.Age = GetAge(patient.Birthdate);
                _patient.AddPatient(patient);
                return View("Index");
            }
            return View();
        }

        private static int GetAge(DateTime birthdate) => new DateTime(DateTime.Now.Subtract(birthdate).Ticks).Year;

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
                _adress.AddAdress(adress);
                return RedirectToAction("NewPatient", adress);
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
