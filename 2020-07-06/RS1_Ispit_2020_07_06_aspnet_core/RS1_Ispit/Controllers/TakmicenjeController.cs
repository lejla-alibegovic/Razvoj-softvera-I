using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.EntityModels;
using RS1_Ispit_asp.net_core.ViewModels;

namespace RS1_Ispit_asp.net_core.Controllers
{
    public class TakmicenjeController : Controller
    {
        public TakmicenjeController (MojContext db)
        {
            this.db = db;
        }
        public readonly MojContext db;

   
        public IActionResult Index(int SkolaId)
        {
            IndexVM novi = new IndexVM();
            novi.SkolaId = SkolaId;
            int broj = SkolaId;
            novi.Skole = db.Skola.Select(w => new SelectListItem
            {
                Value = w.Id.ToString(),
                Text = w.Naziv
            }).ToList();

            if (broj == 0)
            {
                novi.rows = db.Takmicenje.Include(x => x.Predmet).Select(x => new IndexVM.Row
                {
                    Skola = x.Skola.Naziv,
                    Datum = x.Datum.ToShortDateString(),
                    Razred = x.Predmet.Razred,
                    Predmet = x.Predmet.Naziv,
                    TakmicenjeId = x.Id,
                    NajboljiUcesnik = db.TakmicenjeUcesnik.Where(y => y.TakmicenjeId == x.Id).OrderByDescending(y => y.BrojBodova).Select(y => y.OdjeljenjeStavka.Ucenik).SingleOrDefault().ImePrezime
                }).ToList();
            } 

            else
            {
                novi.rows = db.Takmicenje.Where(a => a.SkolaId == SkolaId).Include(x => x.Predmet).Select(x => new IndexVM.Row
                {

                    Skola = x.Skola.Naziv,
                    Datum = x.Datum.ToShortDateString(),
                    Razred = x.Predmet.Razred,
                    Predmet = x.Predmet.Naziv,
                    TakmicenjeId = x.Id,
                    NajboljiUcesnik = db.TakmicenjeUcesnik.Where(y => y.TakmicenjeId == x.Id).OrderByDescending(y => y.BrojBodova).Select(y => y.OdjeljenjeStavka.Ucenik).SingleOrDefault().ImePrezime
                }).ToList();
            }
            foreach (var x in novi.rows)
            {
                var najbolji = db.OdjeljenjeStavka
                    .Include(z => z.Odjeljenje)
                    .Include(z => z.Ucenik)
                    .Include(z => z.Odjeljenje.Skola)
                    .Where(a => a.Ucenik.ImePrezime == x.NajboljiUcesnik).FirstOrDefault();
                if (x.NajboljiUcesnik != null)
                {
                    x.OdjeljenjeNajboljeg = najbolji.Odjeljenje.Oznaka;
                    x.SkolaNajboljeg = najbolji.Odjeljenje.Skola.Naziv;
                }
            }
            return View(novi);
        }
    }
}