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
    public class AjaxController : Controller
    {
        public AjaxController(MojContext db)
        {
            this.db = db;
        }

        public MojContext db { get; }

        public IActionResult TakmicenjeStavke(int TakmicenjeId)
        {
            AjaxTakmicenjeStavkeVM model = new AjaxTakmicenjeStavkeVM
            {
                TakmicenjeId = TakmicenjeId,
                Zakljucano = db.Takmicenje.Where(x => x.Id == TakmicenjeId).SingleOrDefault().IsZakljucano,
                rows = db.TakmicenjeUcesnik
                    .Include(x => x.OdjeljenjeStavka)
                        .ThenInclude(x => x.Odjeljenje)
                    .Where(x => x.TakmicenjeId == TakmicenjeId)
                    .Select(x => new AjaxTakmicenjeStavkeVM.Row
                    {
                        Bodovi = x.BrojBodova.ToString() ?? "X",
                        BrojUDnevniku = x.OdjeljenjeStavka.BrojUDnevniku,
                        IsPristupio = x.IsPristupio,
                        StavkaId = x.Id,
                        Odjeljenje = x.OdjeljenjeStavka.Odjeljenje.Oznaka,
                    })
                    .ToList()
            };
            return PartialView(model);
        }

        public IActionResult UcesnikNijePristupio(int StavkaId)
        {
            var stavka = db.TakmicenjeUcesnik.Where(x => x.Id == StavkaId).SingleOrDefault();
            stavka.IsPristupio = false;
            db.SaveChanges();
            return Redirect("/Takmicenje/Rezultati?TakmicenjeId=" + stavka.TakmicenjeId);
        }

        public IActionResult UcesnikJePristupio(int StavkaId)
        {
            var stavka = db.TakmicenjeUcesnik.Where(x => x.Id == StavkaId).SingleOrDefault();
            stavka.IsPristupio = true;
            db.SaveChanges();
            return Redirect("/Takmicenje/Rezultati?TakmicenjeId=" + stavka.TakmicenjeId);
        }

        public IActionResult Dodaj(int TakmicenjeId)
        {
            AjaxDodajVM model = new AjaxDodajVM
            {
                TakmicenjeId = TakmicenjeId,
                Ucenici = db.OdjeljenjeStavka
                .Include(x => x.Ucenik)
                .Select(x => new SelectListItem
                {
                    Text = x.Ucenik.ImePrezime,
                    Value = x.Id.ToString()
                }).ToList(),
                StavkaId = null
            };
            return PartialView(model);
        }

        public IActionResult Uredi(int StavkaId)
        {
            var stavka = db.TakmicenjeUcesnik.Where(x => x.Id == StavkaId).SingleOrDefault();

            AjaxDodajVM model = new AjaxDodajVM
            {
                StavkaId = StavkaId,
                TakmicenjeId = stavka.TakmicenjeId,
                Ucenici = db.OdjeljenjeStavka
                .Include(x => x.Ucenik)
                .Select(x => new SelectListItem
                {
                    Text = x.Ucenik.ImePrezime,
                    Value = x.Id.ToString()
                }).ToList(),
                UcesnikId = db.TakmicenjeUcesnik.Where(x => x.Id == StavkaId).SingleOrDefault().OdjeljenjeStavkaId,
                Bodovi = stavka.BrojBodova ?? 0
            };
            return PartialView("Dodaj", model);
        }

        public IActionResult Snimi(AjaxDodajVM model)
        {
            if (model.StavkaId == null)
            {
                TakmicenjeUcesnik tu = new TakmicenjeUcesnik
                {
                    BrojBodova = model.Bodovi,
                    TakmicenjeId = model.TakmicenjeId,
                    IsPristupio = true,
                    OdjeljenjeStavkaId = model.UcesnikId,
                };
                db.TakmicenjeUcesnik.Add(tu);
            }
            else
            {
                var stavka = db.TakmicenjeUcesnik.Where(x => x.Id == model.StavkaId).SingleOrDefault();
                stavka.BrojBodova = model.Bodovi;
            }
            db.SaveChanges();

            return Redirect("/Takmicenje/Rezultati?TakmicenjeId=" + model.TakmicenjeId);
        }

        public IActionResult UpdateBodova(int StavkaId, int Bodovi)
        {
            var stavka = db.TakmicenjeUcesnik.Where(x => x.Id == StavkaId).SingleOrDefault();
            stavka.BrojBodova = Bodovi;
            db.SaveChanges();
            return Redirect("/Takmicenje/Rezultati?TakmicenjeId=" + stavka.TakmicenjeId);
        }
    }
}