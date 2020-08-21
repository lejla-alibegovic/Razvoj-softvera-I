using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.ViewModels;
namespace RS1_Ispit_asp.net_core.Controllers
{
    public class IspitController : Controller
    {
        public IspitController(MojContext db)
        {
            this.db = db;
        }
        public readonly MojContext db;
        public IActionResult Index()
        {
            IEnumerable<IndexVM> vm = db.Predmet.Select(x => new IndexVM
            {
                NazivPredmeta = x.Naziv,
                rows = db.Angazovan.Where(y => y.PredmetId == x.Id).Select(y => new IndexVM.Row
                {

                    AkademskaGodina = y.AkademskaGodina.Opis,
                    AngazovanId = y.Id,
                    NastavnikImePrezime = y.Nastavnik.Ime + " " + y.Nastavnik.Prezime,
                    BrojOdrzanihCasova = db.OdrzaniCas.Count(z => z.AngazovaniId == y.Id),
                    BrojStudenataNaPredmetu = db.SlusaPredmet.Count(z => z.AngazovanId == y.Id),
                })
            });
            return View("Index",vm);
        }
        public IActionResult Prikaz(int Id)
        {
            PrikazVM vm = db.Angazovan.Where(x => x.Id == Id).Select(x => new PrikazVM
            {
                AkademskaGodian = x.AkademskaGodina.Opis,
                Predmet = x.Predmet.Naziv,
                NastavnikImePrezime = x.Nastavnik.Ime + " " + x.Nastavnik.Prezime,
                AngazovanId = x.Id,
                rows = db.Ispit.Where(y => y.AngazovanId == x.Id).Select(y => new PrikazVM.Row
                {
                    BrojPrijavljenih = db.IspitStavka.Count(z => z.IspitId == y.Id && z.IsPrijavio),
                    BrojStutenataNisuPolozili = db.IspitStavka.Count(z => z.IspitId == y.Id && !z.IsPolozio),
                    IsEvidentirano = y.IsZakljucano,
                    Datum = y.Datum.ToShortDateString(),
                    IspitId = y.Id
                })
            }).FirstOrDefault();
            return View(vm);
        }
    }
}