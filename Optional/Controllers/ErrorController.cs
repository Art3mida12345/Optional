using System.Web.Mvc;

namespace Optional.Controllers
{
    public class ErrorController : Controller
    {
        public ViewResult NotFound()
        {
            Response.StatusCode = 404;
            ViewBag.Message = "404 Not Found";
            ViewBag.Title = "404";
            return View("Error");
        }

        public ViewResult Unauthorized()
        {
            Response.StatusCode = 401;
            ViewBag.Message = "401 Unautorized";
            ViewBag.Title = "401";
            return View("Error");
        }
    }
}