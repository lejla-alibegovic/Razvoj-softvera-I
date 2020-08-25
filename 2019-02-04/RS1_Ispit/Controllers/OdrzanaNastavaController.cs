using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RS1_Ispit_asp.net_core.ViewModels;
using RS1_Ispit_asp.net_core.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using RS1_Ispit_asp.net_core.EntityModels;

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
            List<IndexVM> vm = db.Nastavnik.Select(x => new IndexVM
            {
                NastavnikId = x.Id,
                Nastavnik = x.Ime + " " + x.Prezime,
                BrojOdrzanihCasova = db.PredajePredmet
                .Include(y => y.Odjeljenje)
                .Include(y => y.Odjeljenje.SkolskaGodina)
                .Count(y => y.NastavnikID == x.Id && y.Odjeljenje.SkolskaGodina.Aktuelna)
            }).ToList();
            return View(vm);
        }
        public IActionResult Prikaz(int NastavnikId)
        {
            PrikazVM vm = new PrikazVM
            {
                NastavnikId = NastavnikId,
                rows = db.OdrzaniCas.
                Include(x => x.PredajePredmet)
                .Include(x => x.PredajePredmet.Odjeljenje)
                .Include(x => x.PredajePredmet.Odjeljenje.Skola)
                .Include(x => x.PredajePredmet.Odjeljenje.SkolskaGodina)
                .Include(x => x.PredajePredmet.Predmet)
                .Where(x => x.PredajePredmet.NastavnikID == NastavnikId).Select(x => new PrikazVM.Row
                {
                    Datum = x.Datum.ToShortDateString(),
                    Skola = x.PredajePredmet.Odjeljenje.Skola.Naziv,
                    SkolskaGodina = x.PredajePredmet.Odjeljenje.SkolskaGodina.Naziv,
                    Odjeljenje = x.PredajePredmet.Odjeljenje.Oznaka,
                    Predmet = x.PredajePredmet.Predmet.Naziv,
                    OdrzaniCasId = x.Id
                }).ToList()
            };
            foreach (var y in vm.rows)
            {
                var odsutni = db.OdrzaniCasDetalji.Include(z => z.Ucenik)
                    .Where(d => d.OdrzaniCasId == y.OdrzaniCasId && !d.IsPrisutan);
                y.OdsutniUcenici = new List<string>();
                foreach (var u in odsutni)
                {
                    y.OdsutniUcenici.Add(u.Ucenik.ImePrezime);
                }
            }
            return View(vm);
        }
        public IActionResult Dodaj(int NastavnikId)
        {
            var nastavnik = db.Nastavnik.Find(NastavnikId);

            DodajVM vm = new DodajVM
            {
                NastavnikId = nastavnik.Id,
                Nastavnik = nastavnik.Ime+" "+nastavnik.Prezime,
                SkolaOdjeljenjePredmet = db.PredajePredmet
                .Include(x=>x.Odjeljenje)
                .Include(x=>x.Odjeljenje.Skola)
                .Include(x=>x.Predmet)
                .Where(x=>x.NastavnikID==NastavnikId)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Odjeljenje.Skola.Naziv + " " + x.Odjeljenje.Oznaka + " " + x.Predmet.Naziv
                }).ToList()
            };
            return View(vm);
        }
        public IActionResult Obrisi(int OdrzaniCasId)
        {
            var cas = db.OdrzaniCas.Include(y => y.PredajePredmet).Where(x => x.Id == OdrzaniCasId).SingleOrDefault();
            foreach (var x in db.OdrzaniCasDetalji.Where(y => y.OdrzaniCasId == cas.Id))
            {
                db.OdrzaniCasDetalji.Remove(x);
            }
            db.OdrzaniCas.Remove(cas);
            db.SaveChanges();
            return RedirectToAction("Prikaz" + new { cas.PredajePredmet.NastavnikID});
        }

        public IActionResult Snimi(DodajVM vm)
        {
            OdrzaniCas cas = new OdrzaniCas
            {
                Datum = vm.Datum,
                PredajePredmetId = vm.PredajePredmetId,
                SadrzajCasa = vm.SadrzajCasa
            };
            db.OdrzaniCas.Add(cas);
            
            foreach(var x in db.Ucenik)
            {
                OdrzaniCasDetalji odrzaniCas = new OdrzaniCasDetalji
                {
                    OdrzaniCas = cas,
                    UcenikId = x.Id,
                    IsPrisutan = true,
                };
                db.OdrzaniCasDetalji.Add(odrzaniCas);
            }
            db.SaveChanges();
            return Redirect("Prikaz?NastavnikId="+vm.NastavnikId);

        }
  
        public IActionResult Detalji(int OdrzaniCasId)
        {
            var cas = db.OdrzaniCas.Where(x => x.Id == OdrzaniCasId).SingleOrDefault();
            DetaljiVM vm = new DetaljiVM
            {
                OdrzaniCasId = cas.Id,
                Datum = cas.Datum.ToShortDateString(),
                SkolaOdjeljenjePredmet = db.PredajePredmet.Include(x => x.Odjeljenje)
                .Include(x => x.Odjeljenje.Skola)
                .Where(x => x.Id == cas.PredajePredmetId).SingleOrDefault().Odjeljenje.Skola.Naziv + " / " +
                db.PredajePredmet.Include(x => x.Odjeljenje)
                .Where(x => x.Id == cas.PredajePredmetId).SingleOrDefault().Odjeljenje.Oznaka + " / " +
                db.PredajePredmet.Include(x => x.Predmet)
                .Where(x => x.Id == cas.PredajePredmetId).SingleOrDefault().Predmet.Naziv,
                Sadrzaj = cas.SadrzajCasa
            };
            return View(vm);
        }
       public IActionResult Stavke(int OdrzaniCasId)
        {
            var cas = db.OdrzaniCas.Where(x => x.Id == OdrzaniCasId).SingleOrDefault();
            StavkeVM vm = new StavkeVM
            {
                rows = db.OdrzaniCasDetalji
                .Where(x => x.OdrzaniCasId == OdrzaniCasId)
                .Select(x => new StavkeVM.Row
                {
                    StavkaId = x.Id,
                    Ocjena = x.Bodovi,
                    IsOpravdan = x.IsOpravdanoOdsutan ?? false,
                    IsPrisutan = x.IsPrisutan,
                    Ucenik = x.Ucenik.ImePrezime
                }).ToList()
            };
            return PartialView(vm);
        }
        public IActionResult UcenikJeOdsutan(int StavkaId)
        {
            var stavka = db.OdrzaniCasDetalji.Where(x => x.Id == StavkaId).SingleOrDefault();
            stavka.IsPrisutan = false;
            stavka.Bodovi = null;
            db.SaveChanges();
            return Redirect("Detalji?OdrzaniCasId=" + stavka.OdrzaniCasId);
        }
        public IActionResult UcenikJePrisutan(int StavkaId)
        {
            var stavka = db.OdrzaniCasDetalji.Where(x => x.Id == StavkaId).SingleOrDefault();
            stavka.IsPrisutan = true;
            db.SaveChanges();
            return Redirect("Detalji?OdrzaniCasId=" + stavka.OdrzaniCasId);
        }
        public IActionResult UrediPrisutan(int StavkaId)
        {
            var stavka = db.OdrzaniCasDetalji.Include(x => x.Ucenik).Where(x => x.Id == StavkaId).SingleOrDefault();
            UrediPrisutanVM vm = new UrediPrisutanVM
            {
                StavkaId = StavkaId,
                Ucenik = stavka.Ucenik.ImePrezime,
                Ocjena = stavka.Bodovi ?? 0
            };
            return PartialView(vm);
        }
        public IActionResult SnimiPrisutan(UrediPrisutanVM vm)
        {
            var stavka = db.OdrzaniCasDetalji.Where(x => x.Id == vm.StavkaId).SingleOrDefault();
            stavka.Bodovi = vm.Ocjena;
            db.SaveChanges();
            return Redirect("Detalji?OdrzaniCasId="+stavka.OdrzaniCasId);
        }
        public IActionResult UrediOdsutan(int StavkaId)
        {
            var stavka = db.OdrzaniCasDetalji.Include(x => x.Ucenik).Where(x => x.Id == StavkaId).SingleOrDefault();
            UrediOdsutanVM vm = new UrediOdsutanVM
            {
                StavkaId = StavkaId,
                Ucenik = stavka.Ucenik.ImePrezime,
            };
            return PartialView(vm);
        }
        public IActionResult SnimiOdsutan(UrediOdsutanVM vm)
        {
            var stavka = db.OdrzaniCasDetalji.Where(x => x.Id == vm.StavkaId).SingleOrDefault();
            stavka.Napomena = vm.Napomena;
            stavka.IsOpravdanoOdsutan = vm.Opravdano;
            db.SaveChanges();
            return Redirect("Detalji?OdrzaniCasId=" + stavka.OdrzaniCasId);
        }
        public IActionResult UpdateBodova(int Id,int Ocjena)
        {
            var stavke=db.OdrzaniCasDetalji.Include(x => x.Ucenik).Where(x => x.Id == Id).SingleOrDefault();
            stavke.Bodovi = Ocjena;
            db.SaveChanges();
            return Redirect("Detalji?OdrzaniCasId=" + stavke.OdrzaniCasId);
        }
    }
}