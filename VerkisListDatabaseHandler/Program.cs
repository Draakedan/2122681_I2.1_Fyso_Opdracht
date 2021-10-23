using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using VerkisListDatabaseHandler.Data;
using VerkisListDatabaseHandler.Models;

namespace VerkisListDatabaseHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            VerkisContext context = new();
            foreach (Diagnose d in context.diagnoses)
            {
                //Console.WriteLine($"{d.Code}\t{d.lichaamslocalisatie}\t{d.pathologie}");
            }

            foreach (Verrichting v in context.verrichtingen)
            {
                Console.WriteLine($"{v.Id}\t{v.Waarde}\t{v.Omschrijving}\t{v.Toelichting_verplicht}");
            }
            //Console.WriteLine("");
            //Console.WriteLine(GetJson("VektisLijstVerrichtingen.csv"));

        }

    }
}
