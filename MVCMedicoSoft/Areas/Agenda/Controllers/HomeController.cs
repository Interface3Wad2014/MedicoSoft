using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCMedicoSoft.Areas.Agenda.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Agenda/Home/
        public ActionResult Index()
        {
            return View();
        }
	}
}