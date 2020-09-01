using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class IndexVM
    {
        public int SkolaId { get; set; }
        public List<SelectListItem> Skole { get; set; }
        public List<Row> rows { get; set; }
        public class Row
        {
            public int TakmicenjeId { get; set; }
           
            public string Skola { get; set; }
            public string Predmet { get; set; }
            public int Razred { get; set; }
            public string Datum { get; set; }
            public string NajboljiUcesnik { get; set; }
            public string SkolaNajboljeg { get; set; }
            public string OdjeljenjeNajboljeg { get; set; }
        }
    }
}
