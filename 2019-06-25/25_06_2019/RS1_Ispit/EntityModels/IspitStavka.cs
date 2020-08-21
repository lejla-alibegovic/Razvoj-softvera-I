using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class IspitStavka
    {
        public int Id { get; set; }
        [ForeignKey(nameof(IspitId))]
        public Ispit Ispit { get; set; }
        public int IspitId { get; set; }
        [ForeignKey(nameof(SlusaPredmetId))]
        public SlusaPredmet SlusaPredmet { get; set; }
        public int SlusaPredmetId { get; set; }
        public bool IsPrijavio { get; set; }
        public bool IsPolozio { get; set; }
        public bool IsPristupio { get; set; }
        public int Ocjena { get; set; }
    }
}
