using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.EntityModels
{
    public class OdrzaniCasDetalji
    {
        public int Id { get; set; }
        [ForeignKey(nameof(OdrzaniCasId))]
        public OdrzaniCas OdrzaniCas { get; set; }
        public int OdrzaniCasId { get; set; }
        public bool IsPrisutan { get; set; }
        public bool? IsOpravdanoOdsutan { get; set; }
        public int? Bodovi { get; set; }
        [ForeignKey(nameof(UcenikId))]
        public Ucenik Ucenik { get; set; }
        public int UcenikId { get; set; }
        public string Napomena { get; set; }
    }
}
