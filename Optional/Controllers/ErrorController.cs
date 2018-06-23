using System.Web.Mvc;

namespace Optional.Controllers
{
    /// <summary>
    /// Custom errors view
    /// </summary>
    public class ErrorController : Controller
    {
        /// <summary>
        /// View for code 404.
        /// </summary>
        public ViewResult NotFound()
        {
            Response.StatusCode = 404;
            ViewBag.Message = "404 Not Found";
            ViewBag.Title = "404";
            return View("Error");
        }
        /// <summary>
        /// View for code 401
        /// </summary>
        public ViewResult Unauthorized()
        {
            Response.StatusCode = 401;
            ViewBag.Message = "401 Unautorized";
            ViewBag.Title = "401";
            return View("Error");
        }
    }
}