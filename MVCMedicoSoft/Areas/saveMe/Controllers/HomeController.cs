using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCMedicoSoft.Areas.saveMe.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /saveMe/Home/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string txtregnat)
        {
            ViewBag.step = 1;
            Personne p = Personne.getInfo(txtregnat);
            string NomMedecin = "";
            if(p !=null)
            { 
                if (p.getReferent(out NomMedecin))
                {
                    return View("rescue", p);
                }
                else
                {
                    return View("Death", p);
                }
            }
            else
            {
                return View("Death", p);
            }
        }
	}
}