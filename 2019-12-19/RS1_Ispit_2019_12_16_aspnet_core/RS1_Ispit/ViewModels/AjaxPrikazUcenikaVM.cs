using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class AjaxPrikazUcenikaVM
    {
        public int PopravniIspitStavkaId { get; set; }
        public string Ucenik { get; set; }
        public string Odjeljenje { get; set; }
        public int BrojUDnevniku { get; set; }
        public bool IsPrisutan { get; set; }
        public float? Bodovi { get; set; }
        public bool MozePristupit { get; set; }
    }
}
