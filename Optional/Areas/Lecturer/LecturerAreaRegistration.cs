using System.Web.Mvc;

namespace Optional.Areas.Lecturer
{
    public class LecturerAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Lecturer";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Lecturer_default",
                "Lecturer/{controller}/{action}/{id}",
                new { controller="Lecturer",action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}