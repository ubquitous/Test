using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrossDoubleN.Models;
using CrossDoubleN.ViewModels;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json;

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
            int numb = ab.Count() / 5;
            if (ab.Count() % 5 > 0)
            {
                numb++;
            }
            ViewBag.num=numb;
            ViewBag.now = 1;
            List<cross> perem = new List<cross>();
            
            for(int i=0; i<ab.Count();i++)
            {
                perem.Add(new cross() { name = ab[i].name, id=ab[i].id });
                if (i == 4)
                {
                    break;
                }
            }
            return View(perem);
        }

        [HttpPost]
        public ActionResult CrWord(int num)
        {
            CrossContext db = new CrossContext();
            db.cross.Load();
            db.znach.Load();

            List<Crossword> ab = db.cross.ToList();
            int numb = ab.Count() / 5;
            if (ab.Count() % 5 > 0)
            {
                numb++;
            }
            ViewBag.num = numb;
            ViewBag.now = num;
            List<cross> perem = new List<cross>();
            return View(perem);
        }

        [HttpGet]
        public ActionResult Crosswordes(string id)
        {
            CrossContext db = new CrossContext();
            db.cross.Load();
            db.znach.Load();
                int di = Convert.ToInt32(id);
                Crossword cro=new Crossword();
                foreach (Crossword a in db.cross.Local)
                {
                    if (a.id == di)
                    {
                        cro = a;
                    }
                }
                
                InfoCr perem = new InfoCr();

                cro.sort();
                List<string> aber = new List<string>();
                foreach (znach a in cro.IdZnach)
                {
                    aber.Add(a.row);
                }
                perem.preob(aber);
                byte[,] str = new byte[perem.inf.GetLength(0), perem.inf.GetLength(1)];
                for (int i = 0; i < perem.inf.GetLength(0); i++)
                {
                    for (int j = 0; j < perem.inf.GetLength(1); j++)
                    {
                        if (perem.inf[i, j])
                        {
                            str[i, j] = 1;
                        }
                        else
                        {
                            str[i, j] = 0;
                        }
                    }
                }
                string res = JsonConvert.SerializeObject(perem.inf);
                string res2 = JsonConvert.SerializeObject(str);
            
                ViewBag.res = res;
                ViewBag.resu = res2;
                return PartialView(perem);
        }

        public ActionResult Regular()
        {
            List<string> str = StaticFunc.GetText();
            StaticFunc.AS();
            ViewBag.Str = str;
            return View();
        }

        public ActionResult AngSha()
        {
            List<string> str = StaticFunc.AS();
            ViewBag.Str = str;
            return View();
        }
    }
}