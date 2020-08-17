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

        public IActionResult Dodaj(int NastavnikId)
        {
            DodajVM model = new DodajVM
            {
                NastavnikId = NastavnikId,
                SkolskaGodina = db.SkolskaGodina.Where(x => x.Aktuelna).FirstOrDefault().Naziv,
                Skole = db.Skola.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Naziv
                }).ToList(),
                Predmeti = db.Predmet.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Naziv
                }).ToList(),
                Nastavnik = db.Nastavnik.Where(x => x.Id == NastavnikId).Select(x => x.Ime + "  " + x.Prezime).FirstOrDefault()

            };
            return View(model);
        }
        public bool Provjera(int id)
        {
            foreach(var x in db.DodjeljenPredmet
                .Include(c=>c.OdjeljenjeStavka).ThenInclude(c=>c.Odjeljenje)
                .Where(c=>c.OdjeljenjeStavkaId==id && c.OdjeljenjeStavka.Odjeljenje.Razred == 4).ToList())
            {
                if (x.ZakljucnoKrajGodine == 1)
                    return false;
            }
            MaturskiIspitStavka zadnjiPokusaj = db.MaturskiIspitStavka.Where(c => c.OdjeljenjeStavkaId == id).LastOrDefault();
            if (zadnjiPokusaj != null && zadnjiPokusaj.BrojBodova < 55)
                return true;
            return false;
        }
        public IActionResult Snimi(DodajVM model)
        {
            PredajePredmet p = db.PredajePredmet.Find(model.NastavnikId);
            if (p != null)
            {
                MaturskiIspit ispit = new MaturskiIspit
                {
                    Datum = model.Datum,
                    PredmetId = model.PredmetId,
                    SkolaId = model.SkolaId,
                    NastavnikId = model.NastavnikId
                    
                };
                db.Add(ispit);
                db.SaveChanges();

                List<OdjeljenjeStavka> ucenici = db.OdjeljenjeStavka
                    .Include(x => x.Odjeljenje)
                    .Where(x => x.Odjeljenje.Razred == 4 && x.Odjeljenje.SkolaID == model.SkolaId).ToList();
                foreach (var i in ucenici)
                {
                    if (Provjera(i.Id))
                    {
                        MaturskiIspitStavka stavka = new MaturskiIspitStavka
                        {
                            MaturskiIspitId = ispit.Id,
                            OdjeljenjeStavkaId = i.Id,
                            IsPristupio = false,
                            Prosjek = db.DodjeljenPredmet.Where(y => y.OdjeljenjeStavkaId == i.Odjeljenje.Id).Average(y => y.ZakljucnoKrajGodine),
                            BrojBodova = 0
                        };
                        db.Add(stavka);
                        db.SaveChanges();
                    }
                }
                return Redirect("Prikaz?NastavnikId=" + model.NastavnikId);
            }
            return Redirect("Index");
        }
    }
}