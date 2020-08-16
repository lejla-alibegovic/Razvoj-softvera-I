using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public IActionResult Prikaz(int NastavnikId)
        {
            PrikazVM model = new PrikazVM
            {
                NastavnikId = NastavnikId,
                Rows = db.MaturskiIspit.Where(x => x.NastavnikId == NastavnikId).Select(x => new PrikazVM.Row
                {
                    Datum = x.Datum.ToShortDateString(),
                    Škola = x.Skola.Naziv,
                    Predmet = x.Predmet.Naziv,
                    MaturskiIspitId = x.Id,


                }).ToList(),
            };
            foreach(var z in model.Rows)
            {
                var odsutni = db.MaturskiIspitStavka.Include(x => x.OdjeljenjeStavka).Include(x => x.OdjeljenjeStavka.Ucenik)
                    .Where(x => x.MaturskiIspitId == z.MaturskiIspitId && !x.IsPristupio).ToList();

                z.NisuPristupili = new List<string>();
                foreach(var t in odsutni)
                {
                    z.NisuPristupili.Add(t.OdjeljenjeStavka.Ucenik.ImePrezime);
                }
            }
            return View(model);
        }
    }
}