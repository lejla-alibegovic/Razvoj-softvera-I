using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class TakmicenjeStavkeVM
    {
        public int TakmicenjeId { get; set; }
        public bool Zakljucano { get; set; }
        public List<Row>rows{ get; set; }
        public class Row
        {
            public int StavkaId { get; set; }
            public int BrojUDnevniku { get; set; }
            public bool IsPristupio { get; set; }
            public string Bodovi { get; set; }
            public string Odjeljenje { get; set; }
        }

    }
}
