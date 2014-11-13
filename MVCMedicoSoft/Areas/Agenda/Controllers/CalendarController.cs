using DAL;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
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
                        GoogleClientSecrets.Load(stream).Secrets, scopes, "user", CancellationToken.None,
                        new FileDataStore("Calendar.VB.Sample")).Result;
            }
            var initializer =  new BaseClientService.Initializer();
            initializer.HttpClientInitializer = credential;
            initializer.ApplicationName = "MedicoSoftAgenda";
            service = new CalendarService(initializer);
            List<CalendarListEntry> list = service.CalendarList.List().Execute().Items.ToList();



            return View(list);
        }

        //
        // GET: /Agenda/Calendar/
        public ActionResult AddRdv(string selPers, long? selMed, string txtHeureRdv, string txtDateRdv)
        {
            DateTime dt = DateTime.Parse(txtDateRdv);
            string[] tab = txtHeureRdv.Split(':');
            if(tab.Count()>0)
            {
              dt=  dt.AddHours(double.Parse(tab[0]));
              dt=  dt.AddMinutes(double.Parse(tab[1]));
            }
            RendezVous rdv = new RendezVous() { Medecin = Medecin.getInfo(selMed.ToString()), Secretaire = Secretaire.getInfoFromUser(MySession.User.IdUtilisateur), DebutRdv = dt };
            return View(rdv);
        }
	}
}