using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedicoSoft.Calendar
{
    public class GoogleV3
    {
        string jsonSecret;
        CalendarService service;
        List<CalendarListEntry> listCalendar;
        List<string> scopes = new List<string>();

        public GoogleV3(string pathToJsonSecret)
        {
            this.jsonSecret = pathToJsonSecret;
        }

        public bool ConnectToGoogleAgenda()
        {
            try
            {
                scopes.Add(CalendarService.Scope.Calendar);

                UserCredential credential;
                using (FileStream stream = new FileStream(jsonSecret, FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                            GoogleClientSecrets.Load(stream).Secrets, scopes, "user",
                            CancellationToken.None,
                            new FileDataStore("MvcMedicoSoft.Areas.Agenda")).Result;
                }
                var initializer = new BaseClientService.Initializer();
                initializer.HttpClientInitializer = credential;
                initializer.ApplicationName = "MedicoSoftAgenda";
                service = new CalendarService(initializer);
                listCalendar = service.CalendarList.List().Execute().Items.ToList();
                if (listCalendar.Count > 0) return true;
                else return false;
            }
            catch (Exception)
            {

                return false;
            }
        }
    
        public bool AddEventtoGoogleCalendar(string description, string subject, DateTime Begin, DateTime End)
        {
            Event e = new Event();
            e.Summary = subject;
            e.Description = description;
            EventDateTime edt = new EventDateTime();
            edt.DateTime = DateTime.Parse(Begin.ToString("yyyy-MM-ddTHH:mm:ss.ffK"));
            EventDateTime edtf = new EventDateTime();
            edtf.DateTime = DateTime.Parse(End.ToString("yyyy-MM-ddTHH:mm:ss.ffK"));
            e.Start = edt;
            e.End = edtf;
            EventsResource evr = new EventsResource(service);

            try
            {
                Event ereinsterted = service.Events.Insert(e, listCalendar[0].Id).Execute();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
  
        public List<MedicoEvent> getAllEvent(DateTime from, DateTime To)
        {
            List<MedicoEvent> lretour = new List<MedicoEvent>();
            var MyEvents = service.Events.List(listCalendar[0].Id).Execute();
            foreach (Event item in MyEvents.Items)
	        {
                lretour.Add(new MedicoEvent() { title = item.Description, start = item.Start.DateTime, end = item.End.DateTime });
	        }
            lretour = lretour.Where(r => r.start >= from && r.end <= To).ToList();
            return lretour;
        }
    }
}
