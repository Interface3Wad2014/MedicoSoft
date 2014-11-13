using DAL;
using MVCMedicoSoft.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCMedicoSoft.Areas.Agenda.Views.Home
{
    public class CalendarController : Controller
    {
        //
        // GET: /Agenda/Calendar/
        public ActionResult AddRdv(string selPers, long? selMed, string txtHeureRdv, string txtDateRdv)
        {
            DateTime dt = DateTime.Parse(txtDateRdv);
            RendezVous rdv = new RendezVous() { Medecin = Medecin.getInfo(selMed.ToString()), DebutRdv = dt };
            return View(rdv);
        }
	}
}