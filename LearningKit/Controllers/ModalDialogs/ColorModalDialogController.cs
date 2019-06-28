using System.Collections.Generic;
using System.Web.Mvc;

using LearningKit.Models.ModalDialogs;

namespace LearningKit.Controllers.ModalDialogs
{
    public class ColorModalDialogController : Controller
    {
        public ActionResult Index()
        {
            var model = new ColorModalDialogViewModel
            {
                Colors = new List<string>() { "red", "blue", "white", "green", "black", "gray", "yellow" }
            };

            return View("ModalDialogs/ColorModalDialog/_ColorModalDialog", model);
        }
    }
}