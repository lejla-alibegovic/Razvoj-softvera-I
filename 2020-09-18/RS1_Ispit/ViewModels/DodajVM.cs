﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class DodajVM
    {
        public List<SelectListItem> Predmeti { get; set; }
        public List<SelectListItem> Skole { get; set; }
        public DateTime Datum { get; set; }
        public int SkolaId { get; set; }
    }
}
