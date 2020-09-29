using System.Web.Mvc;

namespace PracticalAssignment.Controllers
{
    public class ErrorController : Controller
    {
        /// <summary>
        /// Generates view when page is not found
        /// </summary>
        /// <returns></returns>
        // GET: Error
        public ActionResult PageNotFound()
        {
            return View();
        }
    }
}