using System.Collections.Generic;
using DomainModels.Models;

namespace DomainServices.Repos
{
    public interface IPatientFile
    {
        void AddPatientFile(PatientFile patientFile);
        bool PatientFileExists(int id);
        List<PatientFile> GetAllPatientFiles();
        PatientFile GetPatientFileByID(int id);
        void UpdatePatientFile(PatientFile file);
        List<PatientFile> GetPatientFileByFysio(int fysioID);
        void AddComment(Comment comment, int id);
        PatientFile GetPatientFileByPatient(int id);
        PatientFile GetPatientFileBySession(int id);
        void RemoveSessionFromPatientFile(TherapySession session, bool isInLoop);
        void AddSession(TherapySession session, int id);
        void RemoveAllFiredPatientFiles();
    }
}