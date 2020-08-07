using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class TakmicenjePrikazVM
    {
        public int SkolaDomacinId { get; set; }
        public string SkolaDomacin { get; set; }
        public int Razred { get; set; }
        public List<Row> row { get; set; }
        public class Row
        {
            public int TakmicenjeId { get; set; }
            public string Predmet { get; set; }
            public int Razred { get; set; }
            public string Datum { get; set; }
            public int BrojUčesnikaKojiNisuPristupili { get; set; }
            public string NajboljiUcenik { get; set; }
            public string SkolaNajUcenika { get; set; }
            public string OdjeljenjeNajUcenika { get; set; }
        }
    }
}
