using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class IndexVM
    {
       public string NazivPredmeta { get; set; }
       public IEnumerable<Row> rows { get; set; }
        public class Row
        {
            public int AngazovanId { get; set; }
            public string AkademskaGodina { get; set; }
            public string NastavnikImePrezime { get; set; }
            public int BrojOdrzanihCasova { get; set; }
            public int BrojStudenataNaPredmetu { get; set; }
            public int Brojac { get; set; }
        }

    }
}
