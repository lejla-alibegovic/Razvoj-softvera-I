using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class AjaxDodajVM
    {
        public int TakmicenjeId { get; set; }
        public int UcesnikId { get; set; }
        public List<SelectListItem> Ucenici { get; set; }
        public int Bodovi { get; set; }
        public int? StavkaId { get; set; }
    }
}
