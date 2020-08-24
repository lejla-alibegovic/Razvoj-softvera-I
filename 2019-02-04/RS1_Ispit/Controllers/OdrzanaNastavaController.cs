using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RS1_Ispit_asp.net_core.ViewModels;
using RS1_Ispit_asp.net_core.EF;
using Microsoft.EntityFrameworkCore;

namespace RS1_Ispit_asp.net_core.Controllers
{
    public class OdrzanaNastavaController : Controller
    {
        public OdrzanaNastavaController (MojContext db)
        {
            this.db = db;
        }
        public readonly MojContext db;
        public IActionResult Index()
        {
            List<IndexVM> vm = db.Nastavnik.Select(x => new IndexVM
            {
                NastavnikId=x.Id,
                Nastavnik=x.Ime+" "+x.Prezime,
                BrojOdrzanihCasova=db.OdrzaniCas
                .Include(y=>y.PredajePredmet)
                .Include(y=>y.PredajePredmet.Odjeljenje)
                .Include(y=>y.PredajePredmet.Odjeljenje.SkolskaGodina)
                .Count(y=>y.PredajePredmet.NastavnikID==x.Id && y.PredajePredmet.Odjeljenje.SkolskaGodina.Aktuelna)
            }).ToList();
            return View(vm);
        }
    }
}