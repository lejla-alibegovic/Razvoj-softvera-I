using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class PrikazVM
    {
        public string Predmet { get; set; }
        public string NastavnikImePrezime { get; set; }
        public string AkademskaGodian { get; set; }
        public int AngazovanId { get; set; }
        public IEnumerable<Row> rows { get; set; }
        public class Row
        {
            public string Datum { get; set; }
            public int BrojStutenataNisuPolozili { get; set; }
            public int BrojPrijavljenih { get; set; }
            public bool IsEvidentirano { get; set; }
            public int IspitId { get; set; }
        }
    }
}
