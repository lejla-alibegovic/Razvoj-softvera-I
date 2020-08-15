using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class PopravniIspitPrikazVM
    {
        public int SkolaId { get; set; }
        public int SkolskaGodinaId { get; set; }
        public int Razred { get; set; }
        public string Skola { get; set; }
        public string SkolskaGodina { get; set; }
        public List<Row> Rows { get; set; }
        public class Row
        {
            public int PopravniIspitId { get; set; }
            public string Datum { get; set; }
            public int PredmetId { get; set; }
            public string Predmet { get; set; }
            public int BrojUcenikaNaPopravnomIspitu { get; set; }
            public int BrojUcenikaKojiSuPolozili { get; set; }

        }
    }
}
