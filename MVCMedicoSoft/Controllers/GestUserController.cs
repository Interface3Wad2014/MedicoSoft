using DAL;
using MVCMedicoSoft.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCMedicoSoft.Controllers
{
    public class GestUserController : Controller
    {
        //
        // GET: /GestUser/
        public ActionResult Index()
        {
            //Si on n'est pas loggué, on redirige vers la home
            if (MySession.Login == null) return RedirectToRoute(
                    new { controller = "Home", action = "Index" });

            List<Utilisateur> lu = Utilisateur.getInfos();

            //if (MySession.User.getRole() == TypeOfUser.Medecin)
            //    MySession.isMedecin = true;
            //else
            //{
            //    MySession.isMedecin = false;
            //}
            // OU

            MySession.isMedecin = MySession.User.getRole() == TypeOfUser.Medecin;

            return View(lu);
        }

        public ActionResult ListPatient()
        {
            //Si on n'est pas loggué, on redirige vers la home
            if (MySession.Login == null) return RedirectToRoute(
                    new { controller = "Home", action = "Index" });
            Medecin me = MySession.User.ConvertMedecin();

            return View(me.MesPatients());
        }
    }
}