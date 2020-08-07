using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class TakmicenjeDodajVM
    {
        public int SkolaDomacinId { get; set; }
        public string SkolaDomacin { get; set; }
        public int PredmetId { get; set; }
        public List<SelectListItem> Predmeti{ get; set; }
        public int Razred { get; set; }
        [DataType(DataType.Date)]
        public DateTime Datum { get; set; }
    }
}
