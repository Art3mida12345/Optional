using System.Web.Mvc;

namespace Optional.Areas.Student
{
    public class StudentAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Student";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Student_default",
                "Student/{controller}/{action}/{id}",
                new { controller="Student",action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}