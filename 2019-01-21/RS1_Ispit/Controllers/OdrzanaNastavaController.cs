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
        private bool Provjera(int id)
        {
            foreach (var u in db.DodjeljenPredmet
                        .Include(d => d.OdjeljenjeStavka).ThenInclude(d => d.Odjeljenje)
                        .Where(d => d.OdjeljenjeStavkaId == id && d.OdjeljenjeStavka.Odjeljenje.Razred == 4).ToList())
            {
                if (u.ZakljucnoKrajGodine == 1)
                    return false;

            }
            MaturskiIspitStavka zadnji = db.MaturskiIspitStavka.Where(d => d.OdjeljenjeStavkaId == id).LastOrDefault();
            if (zadnji != null && zadnji.BrojBodova < 55)
                return true;
            return false;
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
     
        public IActionResult Snimi(DodajVM model)
        {
            MaturskiIspit maturskiIspit = new MaturskiIspit
            {
                Datum = model.Datum,
                NastavnikId = model.NastavnikId,
                PredmetId = model.PredmetId,
                SkolaId = model.SkolaId,
                
            };
            db.Add(maturskiIspit);
            db.SaveChanges();
            List<OdjeljenjeStavka> ucenici = db.OdjeljenjeStavka.Include(x => x.Odjeljenje).
                Where(x => x.Odjeljenje.Razred == 4 && x.Odjeljenje.SkolaID == model.SkolaId).ToList();

            foreach(var i in ucenici)
            {
                if (Provjera(i.Id))
                {
                    MaturskiIspitStavka m = new MaturskiIspitStavka
                    {
                        MaturskiIspitId = maturskiIspit.Id,
                        OdjeljenjeStavkaId = i.Id
                    };
                    db.Add(m);
                    db.SaveChanges();
                }
            }
            return Redirect("Prikaz?NastavnikId=" + model.NastavnikId);
            
        }
        public IActionResult Uredi(int MaturskiIspitId)
        {

            UrediVM viewModel = db.MaturskiIspit.Where(x => x.Id == MaturskiIspitId).Select(x => new UrediVM
            {
                Datum = x.Datum.ToShortDateString(),
                Predmet = x.Predmet.Naziv,
                Napomena = x.Napomena,
                MaturskiIspitId = x.Id
            }).FirstOrDefault();

            return View(viewModel);
            
        }
        public IActionResult AjaxSnimi(UrediVM model)
        {
            var ispit = db.MaturskiIspit.Where(x => x.Id == model.MaturskiIspitId).SingleOrDefault();
            ispit.Napomena = model.Napomena;
            db.SaveChanges();
            return Redirect("Uredi?MaturskiIspitId=" + model.MaturskiIspitId);  
        }
        public IActionResult AjaxPrikazUcenika(int ID)
        {
            IEnumerable<AjaxPrikazVM> vm = db.MaturskiIspitStavka.Where(x => x.MaturskiIspitId == ID).Include(x=>x.OdjeljenjeStavka).ThenInclude(x=>x.Ucenik).Select(x => new AjaxPrikazVM
            {
                IsPristupio = x.IsPristupio,
                Prosjek = db.DodjeljenPredmet.Where(y=>y.OdjeljenjeStavkaId==x.OdjeljenjeStavka.Id).Average(y=>y.ZakljucnoKrajGodine),
                BrojBodova = x.BrojBodova,
                Ucenik = x.OdjeljenjeStavka.Ucenik.ImePrezime,
                MaturskiIspitStavkaId = x.Id
            });
            return PartialView(vm);
        }
        public IActionResult UcenikJeOdsutan(int MaturskiIspitStavkaId)
        {
            var stavka = db.MaturskiIspitStavka.Where(x => x.Id == MaturskiIspitStavkaId).SingleOrDefault();
            var ispit = db.MaturskiIspit.Where(x => x.Id == stavka.MaturskiIspitId).SingleOrDefault();
            stavka.IsPristupio = false;
            db.SaveChanges();
            return Redirect("/OdrzanaNastava/Uredi?MaturskiIspitId=" + ispit.Id);
        }
        public IActionResult UcenikJePrisutan(int MaturskiIspitStavkaId)
        {
            var stavka = db.MaturskiIspitStavka.Where(x => x.Id == MaturskiIspitStavkaId).SingleOrDefault();
            var ispit = db.MaturskiIspit.Where(x => x.Id == stavka.MaturskiIspitId).SingleOrDefault();

            stavka.IsPristupio = true;
            db.SaveChanges();
            return Redirect("/OdrzanaNastava/Uredi?MaturskiIspitId=" + ispit.Id);
        }
        public IActionResult AjaxUredi(int MaturskiIspitStavkaid)
        {
            AjaxUrediVM vm = db.MaturskiIspitStavka.Where(x => x.Id == MaturskiIspitStavkaid).Select(x => new AjaxUrediVM
            {
                BrojBodova=x.BrojBodova,
                Ucenik=x.OdjeljenjeStavka.Ucenik.ImePrezime,
                MaturskiIspitStavkaId=x.Id,
            }).SingleOrDefault();
            return PartialView(vm);
        }
        public IActionResult AjaxUrediSnimi(AjaxUrediVM model)
        {
            MaturskiIspitStavka stavka = db.MaturskiIspitStavka.Find(model.MaturskiIspitStavkaId);
            stavka.BrojBodova = model.BrojBodova;
            stavka.IsPristupio = true;
            db.SaveChanges();
            return Redirect("/OdrzanaNastava/AjaxPrikazUcenika/" + stavka.MaturskiIspitId);
        }
        public IActionResult UpdateBodova(int BrojBodova, int MaturskiIspitStavkaId)
        {
            MaturskiIspitStavka stavka = db.MaturskiIspitStavka.Find(MaturskiIspitStavkaId);
            stavka.BrojBodova = BrojBodova;
            db.SaveChanges();
            return Redirect("/OdrzanaNastava/Uredi?MaturskiIspitId=" + stavka.MaturskiIspitId);
        }
    }
}