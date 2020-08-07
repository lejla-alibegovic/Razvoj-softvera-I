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
        public TakmicenjeController(MojContext db)
        {
            this.db = db;
        }
        public MojContext db { get; }
        public IActionResult Index()
        {
            TakmicenjeIndexVM model = new TakmicenjeIndexVM
            {
                Skole = db.Skola
                .Select(x => new SelectListItem
                {
                    Text = x.Naziv,
                    Value = x.Id.ToString()
                }).ToList()
            };
            return View(model);
        }
        public IActionResult Prikaz(TakmicenjeIndexVM model)
        {
            var takmicenja = db.Takmicenje
                .Include(x => x.Predmet)
                .Where(x => x.Razred == model.Razred && x.SkolaDomacinId == model.SKolaDomacinId);

            TakmicenjePrikazVM prikaz = new TakmicenjePrikazVM
            {
                SkolaDomacinId = model.SKolaDomacinId,
                SkolaDomacin = db.Skola.Where(x => x.Id == model.SKolaDomacinId).SingleOrDefault().Naziv,
                Razred = model.Razred,
                row = takmicenja.Select(x => new TakmicenjePrikazVM.Row
                {
                    Datum = x.Datum.ToShortDateString(),
                    Razred = x.Razred,
                    Predmet = x.Predmet.Naziv,
                    TakmicenjeId = x.Id,
                    BrojUčesnikaKojiNisuPristupili = db.TakmicenjeUcesnik.Where(y => y.TakmicenjeId == x.Id).Count(y => y.IsPristupio == false),
                    NajboljiUcenik = db.TakmicenjeUcesnik.Where(y => y.TakmicenjeId == x.Id).OrderByDescending(y => y.BrojBodova)
                      .Select(y => y.OdjeljenjeStavka.Ucenik).FirstOrDefault().ImePrezime,
                }).ToList()
            };
            foreach (var x in prikaz.row)
            {
                var naj = db.OdjeljenjeStavka
                .Include(z => z.Odjeljenje)
                .Include(z => z.Ucenik)
                .Include(z => z.Odjeljenje.Skola)
                .Where(a => a.Ucenik.ImePrezime == x.NajboljiUcenik).FirstOrDefault();
                if (x.NajboljiUcenik != null)
                {
                    x.SkolaNajUcenika = naj.Odjeljenje.Skola.Naziv;
                    x.OdjeljenjeNajUcenika = naj.Odjeljenje.Oznaka;
                }
            }
            return View(prikaz);
        }
    
        public IActionResult Dodaj(int skoladomacinId)
        {
            TakmicenjeDodajVM model = new TakmicenjeDodajVM
            {
                Predmeti = db.Predmet
                .GroupBy(x => x.Naziv)
                .Select(x => x.First())
                .Select(x => new SelectListItem
                {
                    Text = x.Naziv,
                    Value = x.Id.ToString()
                }).ToList(),
                SkolaDomacin = db.Skola.Where(x => x.Id == skoladomacinId).SingleOrDefault().Naziv,
                SkolaDomacinId = db.Skola.Where(x => x.Id == skoladomacinId).SingleOrDefault().Id
            };
            return View(model);
        }

        public IActionResult Snimi (TakmicenjeDodajVM model)
        {
            Takmicenje takmicenje = new Takmicenje
            {
                Datum = model.Datum,
                PredmetId = model.PredmetId,
                Razred = model.Razred,
                SkolaDomacinId = model.SkolaDomacinId
            };
            db.Takmicenje.Add(takmicenje);

            var dodjeljenPredmet = db.DodjeljenPredmet
                .Include(x => x.OdjeljenjeStavka)
                    .ThenInclude(x => x.Ucenik)
                .Include(x => x.Predmet);

            foreach(var stavka in dodjeljenPredmet)
            {
                if((stavka.Predmet.Id==takmicenje.PredmetId && stavka.ZakljucnoKrajGodine==5)
                    || db.DodjeljenPredmet.Where(y=>y.Id==stavka.Id).Average(y=>y.ZakljucnoKrajGodine)>4.0)
                {
                    TakmicenjeUcesnik ucesnik = new TakmicenjeUcesnik
                    {
                        Takmicenje = takmicenje,
                        IsPristupio = false,
                        BrojBodova = 0,
                        OdjeljenjeStavkaId = stavka.OdjeljenjeStavkaId
                    };
                    db.TakmicenjeUcesnik.Add(ucesnik);
                }
            }
            db.SaveChanges();
            return Redirect("Index");
        }
        public IActionResult Rezultati(int TakmicenjeId)
        {
            var takmicenje = db.Takmicenje.Include(x => x.SkolaDomacin)
                .Include(x => x.Predmet).Where(x => x.Id == TakmicenjeId).SingleOrDefault();
            TakmicenjeRezultatiVM model = new TakmicenjeRezultatiVM
            {
                TakmicenjeId = takmicenje.Id,
                Datum = takmicenje.Datum.ToShortDateString(),
                Predmet = takmicenje.Predmet.Naziv,
                Razred = takmicenje.Razred,
                SkolaDomacin = takmicenje.SkolaDomacin.Naziv,
                SkolaDomacinId = takmicenje.SkolaDomacinId,
                Zakljucano = takmicenje.IsZakljucano
            };
            return View(model);
        }

        public IActionResult Zakljucaj(int TakmicenjeId)
        {
            var takmicenje = db.Takmicenje.Where(x => x.Id == TakmicenjeId).SingleOrDefault();
            takmicenje.IsZakljucano = true;
            db.SaveChanges();
            return Redirect("Rezultati?TakmicenjeId="+TakmicenjeId);
        }
    
    }
}