using System.Web.Mvc;

namespace MVCMedicoSoft.Areas.saveMe
{
    public class saveMeAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "saveMe";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "saveMe_default",
                "saveMe/{controller}/{action}/{id}",
                new { controller="Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}