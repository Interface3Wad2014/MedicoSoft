using DAL;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
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

            List<string> scopes = new List<string>();
            CalendarService service;
            scopes.Add(CalendarService.Scope.Calendar);

            UserCredential credential ;
            using  (FileStream stream = new FileStream(@"c:\Users\Mike\Documents\Visual Studio 2013\Projects\MVCMedicoSoft\MVCMedicoSoft\Content\client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets, scopes, "user",
                        CancellationToken.None,
                        new FileDataStore("MvcMedicoSoft.Areas.Agenda")).Result;
            }
            var initializer =  new BaseClientService.Initializer();
            initializer.HttpClientInitializer = credential;
            initializer.ApplicationName = "MedicoSoftAgenda";
            service = new CalendarService(initializer);
            List<CalendarListEntry> list = service.CalendarList.List().Execute().Items.ToList();

        
            //Ajout d'un événement
            Event e = new Event();
            e.Description = "Ajout à partir de Medicosoft";
            EventDateTime edt = new EventDateTime();
            edt.DateTime =DateTime.Parse(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.ffK"));
            EventDateTime edtf = new EventDateTime();
            edtf.DateTime = DateTime.Parse(edt.DateTime.Value.AddHours(4).ToString("yyyy-MM-ddTHH:mm:ss.ffK"));
            e.Start = edt;
            e.End = edtf;
            EventsResource evr = new EventsResource(service);

            try
            {
                Event ereinsterted = service.Events.Insert(e, list[0].Id).Execute();
               //List<Event> le =  service.Events.List(list[0].Id);
            }
            catch (Exception)
            {
                
                throw;
            }



            return View(list);
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
	}
}