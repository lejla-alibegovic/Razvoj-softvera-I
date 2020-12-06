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
        public string GetNajboljeg(List<TakmicenjeUcesnik>lista,int ID)
        {
            string ime = "";
            int? najbolji = 0;
            lista = lista.Where(x => x.TakmicenjeId == ID).ToList();
            foreach(var x in lista)
            {
                if (x.BrojBodova > najbolji)
                {
                    najbolji = x.BrojBodova;
                    ime = x.Odjeljenje.Skola.Naziv + " | " + x.Odjeljenje.Oznaka + " | " + x.Ucenik.ImePrezime;
                }
            }
            return ime;
        }
        public IActionResult Index(int skolaId=1, int razred=0)
        {
            List<TakmicenjeUcesnik> lista = db.TakmicenjeUcesnik.Include(x => x.Odjeljenje).Include(x => x.Takmicenje).Include(x => x.Ucenik).Include(x => x.Odjeljenje.Skola).ToList();
            IndexVM model = new IndexVM();
            model.SkolaId = skolaId;
            model.Skole = db.Skola.Select(x => new SelectListItem
            {
                Value=x.Id.ToString(),
                Text=x.Naziv
            }).ToList();
            if (razred == 0)
            {
                model.tabela = db.Takmicenje.Where(y => y.SkolaId == skolaId).Include(y => y.Predmet).Select(y => new IndexStavkeVM
                {
                    TakmicenjeId=y.Id,
                    Datum=y.Datum,
                    NajboljiUcenik=GetNajboljeg(lista,y.Id),
                    Predmet=y.Predmet.Naziv,
                    Razred=y.Predmet.Razred,
                    Skola=y.Skola.Naziv
                }).ToList();
            }
            else
            {
                model.tabela = db.Takmicenje.Where(y => y.SkolaId == skolaId && y.Predmet.Razred==razred).Include(y => y.Predmet).Select(y => new IndexStavkeVM
                {
                    TakmicenjeId = y.Id,
                    Datum = y.Datum,
                    NajboljiUcenik = GetNajboljeg(lista, y.Id),
                    Predmet = y.Predmet.Naziv,
                    Razred = y.Predmet.Razred,
                    Skola = y.Skola.Naziv
                }).ToList();
            }
            return View(model);
        }
        public IActionResult Prikaz(int skolaid, int razred)
        {
            return Redirect("/Takmicenje/Index?skolaid=" + skolaid + "&razred=" + razred);
        }
        public IActionResult Dodaj(int id)
        {
            DodajVM vm = new DodajVM();
            vm.Datum = DateTime.Now;
            vm.Predmeti = db.Predmet.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Naziv
            }).ToList();
            vm.Skole = db.Skola.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Naziv
            }).ToList();
            vm.SkolaId = id;
            return View(vm);
        }
        public IActionResult SnimiTakmicenje(int predmetid,int skolaid,string datum)
        {
            Takmicenje novo = new Takmicenje
            {
                Datum = DateTime.Parse(datum),
                PredmetId=predmetid,
                SkolaId=skolaid
            };
            db.Add(novo);
            var dodjeljenipredmet = db.DodjeljenPredmet.Include(x => x.OdjeljenjeStavka.Ucenik).Include(x => x.Predmet);
             foreach(var stavka in dodjeljenipredmet)
            {
                if ((stavka.Predmet.Id == novo.PredmetId && stavka.ZakljucnoKrajGodine == 5)
                     || db.DodjeljenPredmet.Where(y => y.Id == stavka.Id).Average(y => y.ZakljucnoKrajGodine) > 4.0)
                {
                    TakmicenjeUcesnik ucesnik = new TakmicenjeUcesnik
                    {
                        Takmicenje = novo,
                        IsPrisutan = true,
                        BrojBodova = 0,
                        OdjeljenjeId = stavka.OdjeljenjeStavka.OdjeljenjeId,
                        UcenikId = stavka.OdjeljenjeStavka.UcenikId,
                        BrojUDnevniku = stavka.OdjeljenjeStavka.BrojUDnevniku

                    };
                    db.Add(ucesnik);
                }
             }
            db.SaveChanges();
            return Redirect("/Takmicenje/Index?skolaid=" + skolaid);
        }
        public IActionResult Zakljucaj(int TakmicenjeId)
        {
            var tak = db.Takmicenje.Where(x => x.Id == TakmicenjeId).SingleOrDefault();
            tak.IsZakljucano = true;
            db.SaveChanges();
            return Redirect("Rezultati?TakmicenjeId=" + TakmicenjeId);
        }
        public IActionResult Rezultati(int TakmicenjeId)
        {
            var tak = db.Takmicenje.Include(x=>x.Predmet).Include(x=>x.Skola).Where(x => x.Id == TakmicenjeId).SingleOrDefault();
            RezultatiVM vm = new RezultatiVM
            {
                TakmicenjeId=tak.Id,
                Datum=tak.Datum.ToShortDateString(),
                Predmet=tak.Predmet.Naziv,
                Skola=tak.Skola.Naziv,
                SkolaId=tak.SkolaId,
                Zakljucano=tak.IsZakljucano
            };
            return View(vm);
        }
        public IActionResult TakmicenjeStavke(int TakmicenjeId)
        {
            TakmicenjeStavkeVM vm = new TakmicenjeStavkeVM
            {
                TakmicenjeId = TakmicenjeId,
                Zakljucano=db.Takmicenje.Where(x=>x.Id==TakmicenjeId).SingleOrDefault().IsZakljucano,
                rows=db.TakmicenjeUcesnik.Include(x=>x.Odjeljenje).Where(x=>x.TakmicenjeId==TakmicenjeId).Select(x=>new TakmicenjeStavkeVM.Row
                {
                    Odjeljenje=x.Odjeljenje.Oznaka,
                    Bodovi=x.BrojBodova.ToString()??"X",
                    IsPristupio=x.IsPrisutan,
                    StavkaId=x.Id,
                    BrojUDnevniku=x.BrojUDnevniku
                }).ToList()
            };
            return PartialView(vm);
        }
        public IActionResult UcenikJePrisutan(int StavkaId)
        {
            var stavka = db.TakmicenjeUcesnik.Where(x => x.Id == StavkaId).SingleOrDefault();
            stavka.IsPrisutan = true;
            db.SaveChanges();
            return Redirect("/Takmicenje/Rezultati?TakmicenjeId=" + stavka.TakmicenjeId);
        }
        public IActionResult UcenikJeOdsutan(int StavkaId)
        {
            var stavka = db.TakmicenjeUcesnik.Where(x => x.Id == StavkaId).SingleOrDefault();
            stavka.IsPrisutan = false;
            db.SaveChanges();
            return Redirect("/Takmicenje/Rezultati?TakmicenjeId=" + stavka.TakmicenjeId);
        }
        public IActionResult UpdateBodova(int StavkaId,int Bodovi)
        {
            var stavka = db.TakmicenjeUcesnik.Where(x => x.Id == StavkaId).SingleOrDefault();
            stavka.BrojBodova = Bodovi;
            db.SaveChanges();
            return Redirect("/Takmicenje/Rezultati?TakmicenjeId=" + stavka.TakmicenjeId);

        }
        
        
    }
}