using System;
using DatabaseHandler.Data;

namespace DatabaseHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }
            Seed seed = new();
            DataRepository reop = new();
            Console.WriteLine(reop.GetPatientFiles());
        }
    }
}