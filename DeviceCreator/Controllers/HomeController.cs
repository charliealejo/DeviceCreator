using System;
using System.Web.Mvc;

namespace DeviceCreator.Controllers
{
    /// <summary>
    /// Home controller.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Gets the index page (the default).
        /// </summary>
        /// <returns>The index page.</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Get the test page (accesible from /Home/Test).
        /// </summary>
        /// <returns>The test page.</returns>
        public ActionResult Test()
        {
            var mvcName = typeof(Controller).Assembly.GetName();
            var isMono = Type.GetType("Mono.Runtime") != null;

            ViewData["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
            ViewData["Runtime"] = isMono ? "Mono" : ".NET";

            return View();
        }
    }
}
