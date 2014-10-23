using DAL;
using MVCMedicoSoft.Areas.Agenda.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCMedicoSoft.Models
{
    public static class MySession
    {
        public static string Login
        {
            get
            {
                try
                {
                    return HttpContext.Current.Session["Login"].ToString();
                }
                catch 
                {
                    return null;
                }
            }
            set { HttpContext.Current.Session["Login"] = value; }
        }

        public static Utilisateur User
        {
            get { return (Utilisateur)HttpContext.Current.Session["User"]; }
            set { HttpContext.Current.Session["User"] = value; }
      
        }

        public static bool isMedecin
        {
            get { return (bool)HttpContext.Current.Session["isMedecin"]; }
            set { HttpContext.Current.Session["isMedecin"] = value; }
        }

        public static BoiteMedecinEtPersonne LesPatientsEtLesMedecins
        {
            get { return (BoiteMedecinEtPersonne)HttpContext.Current.Session["lstPatMed"]; }
            set { HttpContext.Current.Session["lstPatMed"] = value; }
        }

    }
}