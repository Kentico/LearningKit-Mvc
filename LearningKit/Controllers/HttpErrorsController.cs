using System.Web.Mvc;

namespace LearningKit.Controllers
{
    public class HttpErrorsController : Controller
    {
        //DocSection:NotFound
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;

            return View();
        }
        //EndDocSection:NotFound
    }
}