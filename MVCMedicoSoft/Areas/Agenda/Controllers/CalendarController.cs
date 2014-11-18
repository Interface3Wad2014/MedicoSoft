using DAL;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using MedicoSoft.Calendar;
using MVCMedicoSoft.Areas.Agenda.Models;
using MVCMedicoSoft.Infrastructure.helper;
using MVCMedicoSoft.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MVCMedicoSoft.Areas.Agenda.Controllers
{
    public class CalendarController : Controller
    {
        public ActionResult Index()
        {
            List<MedicoEvent> e = new List<MedicoEvent>();
            GoogleV3 myGoogle = new GoogleV3(@"c:\client_secrets.json");
            if (myGoogle.ConnectToGoogleAgenda())
            {
                //Ajout d'un event
                myGoogle.AddEventtoGoogleCalendar("Ajout from Medicosoft", "Medicosft Event", DateTime.Now, DateTime.Now.AddDays(1));
                 e = myGoogle.getAllEvent(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(10));

               // return Json(e, JsonRequestBehavior.AllowGet);
            }
            //return Json("Error", JsonRequestBehavior.AllowGet);
                return View("Calendrier",e);
            
        }

        //
        // GET: /Agenda/Calendar/
        [HttpPost]
        public ActionResult AddRdv(string selPers, long? selMed, string txtHeureRdv, string txtDateRdv, string descr, string duree)
        {
            DateTime dt = DateTime.Parse(txtDateRdv);
            string[] tab = txtHeureRdv.Split(':');
            if(tab.Count()>0)
            {
              dt=  dt.AddHours(double.Parse(tab[0]));
              dt=  dt.AddMinutes(double.Parse(tab[1]));
            }
            DateTime dtfin = new DateTime();
            //Calcul heure de fin
            switch (duree)
            {
                case "15min": dtfin = dt.AddMinutes(15); break;
                case "30min": dtfin = dt.AddMinutes(30); break;
                default: dtfin = dt.AddHours(1);
                    break;
            }


            RendezVous rdv = new RendezVous() 
            { 
                Medecin = Medecin.getInfo(selMed.ToString()), 
                Secretaire = Secretaire.getInfoFromUser(MySession.User.IdUtilisateur), 
                DebutRdv = dt,
                Patient= Personne.getInfo(selPers),
                Description=descr,
                FinRdv = dtfin
            };
            RdvLocal rdvloc = new RdvLocal(rdv);
            return View(rdvloc);
        }

        [HttpPost]
        public ActionResult getAllEvents(DateTime start, DateTime end)
        {
             GoogleV3 myGoogle = new GoogleV3(@"c:\client_secrets.json");
             if (myGoogle.ConnectToGoogleAgenda())
             {
                 //Ajout d'un event
                 myGoogle.AddEventtoGoogleCalendar("Ajout from Medicosoft", "Medicosft Event", DateTime.Now, DateTime.Now.AddDays(1));
                 List<MedicoEvent> e = myGoogle.getAllEvent(start, end);

                 return Json(e, JsonRequestBehavior.AllowGet);
             }
             return Json("Error", JsonRequestBehavior.AllowGet);
            
        }
    }
}