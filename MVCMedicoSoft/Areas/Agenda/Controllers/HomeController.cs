using DAL;
using MVCMedicoSoft.Areas.Agenda.Models;
using MVCMedicoSoft.Models;
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
            if(MySession.User == null)
            {
              return  RedirectToAction("Forms", new { controller = "Login", area = "" });
            }
            else
            {
                if(MySession.User.getRole()== DAL.TypeOfUser.Secretaire)
                {
                    MySession.LesPatientsEtLesMedecins = new Models.BoiteMedecinEtPersonne();

                    MySession.LesPatientsEtLesMedecins.LstPers = Personne.getInfos();
                    return View(MySession.LesPatientsEtLesMedecins);
                }
                else
                {
                 return   RedirectToAction("Forms", new { controller = "Login", area = "" });
                }
            }
            
        }
	
        [HttpPost]
        public ActionResult Index(string txtSearchName, DateTime dtNaiss)
        {
            //lp[i].Nom == txtSearchName???
            List<Personne> listeFiltre = new List<Personne>();
            foreach (var item in MySession.LesPatientsEtLesMedecins.LstPers)
            {
                if (item.Nom.ToUpper().Contains(txtSearchName.ToUpper())
                    ||
                    item.DateNaissance == dtNaiss                    
                    )
                    listeFiltre.Add(item);
            }

            BoiteMedecinEtPersonne newBoite = new BoiteMedecinEtPersonne();
            newBoite.LstPers = listeFiltre;
            newBoite.LstMed = MySession.LesPatientsEtLesMedecins.LstMed;

            return View(newBoite);
        }
    
    }
}