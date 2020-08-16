using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class PrikazVM
    {
       public int NastavnikId { get; set; }
       public IEnumerable<Row> Rows { get; set; }
       public class Row
        {
            public string Datum { get; set; }
            public string Predmet { get; set; }
            public string Škola { get; set; }
            public List<string> NisuPristupili { get; set; }
            public int MaturskiIspitId { get; set; }
        }
        
    }
}
