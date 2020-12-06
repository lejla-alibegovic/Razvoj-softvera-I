using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class TakmicenjeUcesnik
    {
        public int Id { get; set; }
        [ForeignKey(nameof(TakmicenjeId))]
        public Takmicenje Takmicenje { get; set; }
        public int TakmicenjeId { get; set; }
        [ForeignKey(nameof(OdjeljenjeId))]
        public Odjeljenje Odjeljenje { get; set; }
        public int OdjeljenjeId { get; set; }
        [ForeignKey(nameof(UcenikId))]
        public Ucenik Ucenik { get; set; }
        public int UcenikId { get; set; }
        public int? BrojBodova { get; set; }
        public bool IsPrisutan { get; set; }
        public int BrojUDnevniku { get; set; }
    }
}
