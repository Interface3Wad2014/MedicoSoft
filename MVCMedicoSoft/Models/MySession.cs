using DAL;
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
           get{ return HttpContext.Current.Session["Login"].ToString();}
            set { HttpContext.Current.Session["Login"] = value; }
        }

        public static Utilisateur User
        {
            get { return (Utilisateur)HttpContext.Current.Session["User"]; }
            set { HttpContext.Current.Session["User"] = value; }
      
        }
    }
}