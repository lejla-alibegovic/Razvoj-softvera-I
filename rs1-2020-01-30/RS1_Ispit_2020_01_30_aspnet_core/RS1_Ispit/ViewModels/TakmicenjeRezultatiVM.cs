using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class TakmicenjeRezultatiVM
    {
        public int TakmicenjeId { get; set; }
        public int SkolaDomacinId { get; set; }
        public string SkolaDomacin { get; set; }
        public string Predmet { get; set; }
        public int Razred { get; set; }
        public string Datum { get; set; }
        public bool Zakljucano { get; set; }

    }
}
