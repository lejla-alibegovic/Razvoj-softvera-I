using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class MaturskiIspitStavka
    {
        public int Id { get; set; }
        [ForeignKey(nameof(MaturskiIspitId))]
        public MaturskiIspit MaturskiIspit { get; set; }
        public int MaturskiIspitId { get; set; }
        public bool IsPristupio { get; set; }
        public float Prosjek { get; set; }
        public int BrojBodova { get; set; }
        [ForeignKey(nameof(OdjeljenjeStavkaId))]
        public OdjeljenjeStavka OdjeljenjeStavka { get; set; }
        public int OdjeljenjeStavkaId { get; set; }

    }
}
