using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace DeanAndSons.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        // GET: CMSIndex
        [Authorize(Roles = "Admin, Staff")]
        public ActionResult IndexCMS()
        {
            return View();
        }

        /// <summary>
        /// Inserts partial view for another image upload
        /// </summary>
        /// <param name="obj">File field Collection</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Staff")]
        public ActionResult MoreImages(ICollection<HttpPostedFileBase> obj)
        {
            return PartialView("_ImageUpload", obj);
        }
    }
}