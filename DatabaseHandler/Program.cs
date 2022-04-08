using System;
using DatabaseHandler.Data;
using Microsoft.EntityFrameworkCore;

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

            FysioDataContext context = new();
            new Seed(context);
        }
    }
}