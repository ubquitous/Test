using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrossDoubleN.Models;
using CrossDoubleN.ViewModels;
using System.Data.Entity;
using System.Threading.Tasks;

namespace CrossDoubleN.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string str;
            try
            {
                str = User.Identity.GetUserRole();
            }
            catch
            {
                str = "";
            }
            ViewData["str"] = str;
            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult About()
        {
            return View();
        }

        [HttpPost]
        public ActionResult About(cr pstData)
        {
            CrossContext db = new CrossContext();
            db.cross.Load();
            db.znach.Load();
            Crossword cross;
            List<Crossword> ber = db.cross.ToList();
            int y=1;
            string str = "";
            for (int i = 0; i < ber.Count(); i++)
            {
                if (ber[i].name == pstData.name+str)
                {
                    str = "(" + y + ")";
                    y++;
                    i = -1;
                }
            }
            pstData.name += str;
            Crossword cros = new Crossword() { name=pstData.name, IdZnach=new List<znach>() };
            int k=0;
            foreach (string a in pstData.zn)
            {
                cros.IdZnach.Add(new znach() { row = a, number = k, IdCross = cros });
                k++;
            }
            db.cross.Add(cros);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public  ActionResult CrWord()
        {
            CrossContext db = new CrossContext();
            db.cross.Load();
            db.znach.Load();
            List<Crossword> ab = db.cross.ToList();
            List<cr> perem = new List<cr>();
            foreach (Crossword a in ab)
            {
                perem.Add(new cr() { name = a.name, col = a.IdZnach[0].row.Length, row = a.IdZnach.Count });
                int num=perem.Count()-1;
                perem[num].zn = new List<string>();
                foreach (znach b in a.IdZnach)
                {
                    perem[num].zn.Add(b.row);
                }
            }
            ViewBag.Aber = new List<string> { "sd", "sd" };
            return View(perem);
        }

        [HttpPost]
        public async Task<ActionResult> CrWord(string gar)
        {
            CrossContext db = new CrossContext();
            db.cross.Load();
            db.znach.Load();
            Crossword cross=await db.cross.Include(u => u.name).FirstOrDefaultAsync(u => u.name == gar);
            List<cr> perem = new List<cr>();
            
            
            cross.sort();
            List<string> aber = new List<string>();
            foreach(znach a in cross.IdZnach)
            {
                aber.Add(a.row);
            }
            ViewBag.Aber = aber;
            return View(perem);
        }
    }
}