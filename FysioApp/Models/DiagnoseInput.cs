using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FysioAppUX.Models
{
    public class DiagnoseInput
    {
        public int Id { get; set; }

        public int Code { get; set; }

        public string Lichaamslocatie { get; set; }

        public string Pathologie { get; set; }
    }
}
