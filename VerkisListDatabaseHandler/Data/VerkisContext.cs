using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VerkisListDatabaseHandler.Models;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Reflection;

namespace VerkisListDatabaseHandler.Data
{
    class VerkisContext : DbContext
    {
        public DbSet<Diagnose> diagnoses { get; set; }
        public DbSet<Verrichting> verrichtingen { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(GetConnectionStringFromJson());
        }

        private static string GetConnectionStringFromJson()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"data/Settings.json");
            using StreamReader r = new(path);
            String json = r.ReadToEnd();
            return (string)JObject.Parse(json)["ConnectionStrings"]["DefaultConnection"];
        }
    }
}
