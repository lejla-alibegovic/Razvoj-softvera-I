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
    public class PopravniIspitController : Controller
    {
        public PopravniIspitController(MojContext db)
        {
            this.db = db;
        }
        public MojContext db { get; }
        public IActionResult Index()
        {
            PopravniIspitIndexVM model = new PopravniIspitIndexVM
            {
                SkolskeGodine = db.SkolskaGodina
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Naziv
                    }).ToList(),
                Skole = db.Skola
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Naziv
                }).ToList()

            };
            return View(model);
        }

        public IActionResult Prikaz(PopravniIspitIndexVM model)
        {
            PopravniIspitPrikazVM vm = new PopravniIspitPrikazVM
            {
                Razred = model.Razred,
                SkolaId = model.SkolaId,
                SkolskaGodinaId = model.SkolskaGodinaId,
                Skola = db.Skola.Find(model.SkolaId).Naziv,
                SkolskaGodina = db.SkolskaGodina.Find(model.SkolskaGodinaId).Naziv
            };
            vm.Rows = db.PopravniIspit
                .Where(x => x.SkolskaGodinaId == model.SkolskaGodinaId
                && x.SkolaId == model.SkolaId
                && x.Razred == model.Razred)
                .Select(x => new PopravniIspitPrikazVM.Row
                {
                    Datum = x.DatumIspita.ToShortDateString(),
                    Predmet = x.Predmet.Naziv,
                    PopravniIspitId = x.Id,
                    BrojUcenikaNaPopravnomIspitu = db.PopravniIspitStavka.Where(y => x.Id == y.PopravniIspitId).Count(y => y.IsPristupio),
                    BrojUcenikaKojiSuPolozili = db.PopravniIspitStavka.Where(y => y.PopravniIspitId == x.Id).Count(y => y.Bodovi > 50),
                }).ToList();
            return View(vm);
        }
       
        public IActionResult Dodaj(int SkolaId, int Razred, int SkolskaGodinaId)
        {
            PopravniIspitDodajVM model = new PopravniIspitDodajVM
            {
                SkolaId = SkolaId,
                Razred = Razred,
                SkolskaGodinaId = SkolskaGodinaId,
                Skola = db.Skola.Find(SkolaId).Naziv,
                SkolskaGodina = db.SkolskaGodina.Find(SkolskaGodinaId).Naziv,
                Predmeti=db.Predmet.Select(x=>new SelectListItem
                {
                    Value=x.Id.ToString(),
                    Text=x.Naziv
                }).ToList(),
                ClanoviKomisije = db.Nastavnik.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Ime + " " + x.Prezime
                }).ToList()
            };
            return View(model);
        }
        public IActionResult Snimi(PopravniIspitDodajVM model)
        {
            PopravniIspit ispit = new PopravniIspit
            {
                ClanKomisije1Id = model.ClanKomisije1Id,
                ClanKomisije2Id = model.ClanKomisije2Id,
                ClanKomisije3Id = model.ClanKomisije3Id,
                DatumIspita = model.Datum,
                PredmetId = model.PredmetId,
                SkolskaGodinaId = model.SkolskaGodinaId,
                SkolaId = model.SkolaId,
                Razred = model.Razred
            };
            db.PopravniIspit.Add(ispit);
            db.SaveChanges();
            Random rnd = new Random();
            IEnumerable<PopravniIspitStavka> stavka = db.DodjeljenPredmet.Where(x => x.ZakljucnoKrajGodine < 2 && x.PredmetId == model.PredmetId).Select(x => new PopravniIspitStavka
            {
                OdjeljenjeStavkaId = x.OdjeljenjeStavkaId,
                PopravniIspitId = ispit.Id,
                IsPristupio = true,
                BrojUDnevniku = x.OdjeljenjeStavka.BrojUDnevniku,
                MozePristupit = true,
                Bodovi = rnd.Next(1,101)
            });
         
            foreach (var s in stavka)
            {
                if (db.DodjeljenPredmet.Where(x => x.OdjeljenjeStavkaId == s.OdjeljenjeStavkaId).Count(x => x.ZakljucnoKrajGodine < 2) >= 3)
                {
                    s.MozePristupit = false;
                    s.Bodovi = 0;
                }
                db.PopravniIspitStavka.Add(s);
            }
            db.SaveChanges();
            
  
            return RedirectToAction("Prikaz", new PopravniIspitPrikazVM { Razred = model.Razred, SkolaId = model.SkolaId, SkolskaGodinaId = model.SkolskaGodinaId });
        }
    }
}