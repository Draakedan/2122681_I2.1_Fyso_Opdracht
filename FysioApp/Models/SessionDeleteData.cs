using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseHandler.Models;

namespace FysioAppUX.Models
{
    public class SessionDeleteData
    {
        public TherapySession Session { get; set; }
        public int DossierID { get; set; }

        public SessionDeleteData()
        { 
        }
    }
}
