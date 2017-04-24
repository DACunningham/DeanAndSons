using DeanAndSons.Models;
using DeanAndSons.Models.Global.ViewModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeanAndSons.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var model = db.Propertys.Include(p => p.Images)
                .Where(p => p.Deleted != true)
                .OrderByDescending(p => p.Created)
                .Take(3);

            HomeIndexViewModel vm = new HomeIndexViewModel(model);

            return View(vm);
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