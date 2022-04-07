using System;
using Microsoft.EntityFrameworkCore;
using DomainModels.Models;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Reflection;

namespace DatabaseHandler.Data
{
    public class FysioDataContext : DbContext
    {
        public DbSet<ActionPlan> ActionPlans { get; set; }
        public DbSet<Adress> Adresses { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<FysioWorker> FysioWorkers { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientFile> PatientFiles { get; set; }
        public DbSet<TherapySession> TherapySessions { get; set; }

        public FysioDataContext(DbContextOptions<FysioDataContext> contextOptions) : base(contextOptions)
        { }
    }
}