using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class PopravniIspit
    {
        public int Id { get; set; }

        public DateTime DatumIspita { get; set; }

        [ForeignKey(nameof(PredmetId))]
        public Predmet Predmet { get; set; }
        public int PredmetId { get; set; }

        [ForeignKey(nameof(ClanKomisije1Id))]
        public Nastavnik ClanKomisije1 { get; set; }
        public int ClanKomisije1Id { get; set; }

        [ForeignKey(nameof(ClanKomisije2Id))]
        public Nastavnik ClanKomisije2 { get; set; }
        public int ClanKomisije2Id { get; set; }

        [ForeignKey(nameof(ClanKomisije3Id))]
        public Nastavnik ClanKomisije3 { get; set; }
        public int ClanKomisije3Id { get; set; }

        [ForeignKey(nameof(SkolaId))]
        public Skola Skola { get; set; }
        public int SkolaId { get; set; }

        [ForeignKey(nameof(SkolskaGodinaId))]
        public SkolskaGodina SkolskaGodina { get; set; }
        public int SkolskaGodinaId { get; set; }

        public int Razred { get; set; }
    }
}
