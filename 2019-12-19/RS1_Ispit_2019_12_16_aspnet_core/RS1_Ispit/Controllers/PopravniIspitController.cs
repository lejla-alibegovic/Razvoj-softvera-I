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
                    BrojUcenikaKojiSuPolozili = db.PopravniIspitStavka.Where(y => x.Id == y.Id).Count(y => y.IsPristupio == true),
                    BrojUcenikaNaPopravnomIspitu = db.PopravniIspitStavka.Where(y => x.Id == y.Id).Count(y => y.Bodovi > 50),
                }).ToList();
            return View(vm);
        }
    }
}