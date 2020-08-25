using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class StavkeVM
    {
        public List<Row> rows { get; set; }
        public class Row
        {
            public int StavkaId { get; set; }
            public string Ucenik { get; set; }
            public bool IsPrisutan { get; set; }
            public bool IsOpravdan { get; set; }
            public int? Ocjena { get; set; }
        }
    }
}
