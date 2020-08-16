using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.EntityModels;
using RS1_Ispit_asp.net_core.ViewModels;

namespace RS1_Ispit_asp.net_core.Controllers
{
    public class OdrzanaNastavaController : Controller
    {
        public OdrzanaNastavaController(MojContext db)
        {
            this.db = db;
        }
        public readonly MojContext db;
        public IActionResult Index()
        {
            IEnumerable<OdrzanaNastavaIndexVM> model = db.Nastavnik.Select(x => new OdrzanaNastavaIndexVM
            {
                NastavnikId=x.Id,
                Skola=db.PredajePredmet.Where(y=>y.NastavnikID==x.Id).Select(y=>y.Odjeljenje.Skola.Naziv).FirstOrDefault(),
                Nastavnik=x.Ime+" "+x.Prezime
            }).ToList();
            return View(model);
        }
    }
}