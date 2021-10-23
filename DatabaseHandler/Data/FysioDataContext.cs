using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DatabaseHandler.Models;
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                optionsBuilder.UseSqlServer(GetConnectionStringFromJson());
            }
            catch { 
            }
        }

        private static string GetConnectionStringFromJson()
        {
            try
            {
                string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"data/Settings.json");
                using StreamReader r = new(path);
                String json = r.ReadToEnd();
                string res = (string)JObject.Parse(json)["ConnectionStrings"]["DefaultConnection"];
                return res;
            }
            catch {
                return null;
            }
        }
    }
}
