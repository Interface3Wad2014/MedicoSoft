using DAL;
using MVCMedicoSoft.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCMedicoSoft.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        [HttpGet]
        public ActionResult Forms()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Forms(string txtLogin, string txtPassword)
        {
            Utilisateur U = Utilisateur.AuthentifieMoi(txtLogin, txtPassword);
            if (U == null)
            {
                ViewBag.Error = "Try Again!!!";
                return View();
            }
            else
            {
                //1 - stocker en session le login de l'utilisateur
                MySession.Login = U.Login;
                MySession.User = U;
                //2 - rediriger vers Home/Index
                //return View();
                return RedirectToRoute(
                    new { controller = "Home", action = "Index" });
            }
        }
	
        [HttpGet]
        public RedirectToRouteResult LogOut()

        {
            MySession.Login = null;
            Session.Abandon();

            return RedirectToRoute
                (
                    new { 
                        controller = "Home", 
                        action = "Index" 
                        }
                );
 
        }
    }
}