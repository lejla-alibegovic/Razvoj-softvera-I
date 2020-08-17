using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class AjaxPrikazVM
    {
        public int MaturskiIspitStavkaId { get; set; }
        public string Ucenik { get; set; }
        public bool IsPristupio { get; set; }
        public double Prosjek { get; set; }
        public int BrojBodova { get; set; }

    }
}
