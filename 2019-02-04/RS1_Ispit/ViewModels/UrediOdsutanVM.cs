using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class UrediOdsutanVM
    {
        public int StavkaId { get; set; }
        public string Ucenik { get; set; }
        public string Napomena { get; set; }
        public bool Opravdano { get; set; }
    }
}
