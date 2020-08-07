using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class Takmicenje
    {
        public int Id { get; set; }
        [ForeignKey(nameof(PredmetId))]
        public Predmet Predmet { get; set; }
        public int PredmetId { get; set; }
        public int Razred { get; set; }
        public DateTime Datum { get; set; }
        [ForeignKey(nameof(SkolaDomacinId))]
        public Skola SkolaDomacin { get; set; }
        public int SkolaDomacinId { get; set; }
        public bool IsZakljucano { get; set; } = false;

    }
}
