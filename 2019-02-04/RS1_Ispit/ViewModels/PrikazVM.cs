using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class PrikazVM
    {
        public int NastavnikId { get; set; }
        public List<Row> rows { get; set; }
        public class Row
        {
            public string Datum { get; set; }
            public string Skola { get; set; }
            public string SkolskaGodina { get; set; }
            public string Odjeljenje { get; set; }
            public string Predmet { get; set; }
            public List<string> OdsutniUcenici { get; set; }
            public int OdrzaniCasId { get; set; }
        }
    }
}
