using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class DodajVM
    {
        public int NastavnikId { get; set; }
        public string Nastavnik { get; set; }
        [DataType(DataType.Date)]
        public DateTime Datum { get; set; }
        public string SadrzajCasa { get; set; }
        public int PredajePredmetId { get; set; }
        public List<SelectListItem> SkolaOdjeljenjePredmet { get; set; }
    }
}
