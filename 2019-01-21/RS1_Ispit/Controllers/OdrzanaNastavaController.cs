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
            MaturskiIspit ispit = db.MaturskiIspit.Find(model.MaturskiIspitId);
            ispit.Napomena = model.Napomena;
            db.SaveChanges();
            return Redirect("/OdrzanaNastava/Uredi/" + model.MaturskiIspitId);  
        }
        public IActionResult AjaxPrikazUcenika(int ID)
        {
            IEnumerable<AjaxPrikazVM> vm = db.MaturskiIspitStavka.Where(x => x.MaturskiIspitId == ID).Select(x => new AjaxPrikazVM
            {
                IsPristupio = x.IsPristupio,
                Prosjek = x.Prosjek,
                BrojBodova = x.BrojBodova,
                Ucenik = x.OdjeljenjeStavka.Ucenik.ImePrezime,
                MaturskiIspitStavkaId = x.Id
            });
            return PartialView(vm);
        }
        public IActionResult UcenikJeOdsutan(int MaturskiIspitStavkaId)
        {
            MaturskiIspitStavka stavka = db.MaturskiIspitStavka.Where(x => x.Id == MaturskiIspitStavkaId).SingleOrDefault();
            stavka.IsPristupio = false;
            db.SaveChanges();
            return Redirect("/OdrzanaNastava/AjaxPrikazUcenika" + stavka.Id);
        }
        public IActionResult UcenikJePrisutan(int MaturskiIspitStavkaId)
        {
            MaturskiIspitStavka stavka = db.MaturskiIspitStavka.Where(x => x.Id == MaturskiIspitStavkaId).SingleOrDefault();
            stavka.IsPristupio = true;
            db.SaveChanges();
            return Redirect("/OdrzanaNastava/AjaxPrikazUcenika" + stavka.Id);
        }
        public IActionResult AjaxUredi(int id)
        {
            AjaxUrediVM vm = db.MaturskiIspitStavka.Where(x => x.Id == id).Select(x => new AjaxUrediVM
            {
                BrojBodova=x.BrojBodova,
                Ucenik=x.OdjeljenjeStavka.Ucenik.ImePrezime,
                MaturskiIspitStavkaId=x.Id
            }).FirstOrDefault();
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
        public IActionResult UpdateBodova(int MaturskiIspitStavkaId,int Bodovi)
        {
            var stavka = db.MaturskiIspitStavka.Where(x => x.Id == MaturskiIspitStavkaId).SingleOrDefault();
            stavka.BrojBodova = Bodovi;
            db.SaveChanges();
            return Redirect("/OdrzanaNastava/AjaxPrikazUcenika/" + stavka.MaturskiIspitId);
        }
    }
}