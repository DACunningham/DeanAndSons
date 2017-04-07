using DeanAndSons.Models;
using DeanAndSons.Models.IMS.ViewModels;
using Microsoft.AspNet.Identity;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace DeanAndSons.Controllers
{
    public class IMSController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: IMS
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TeamViewer()
        {
            //Get logged in user ID then use this ID to get ApplicationUser instance
            var usrID = User.Identity.GetUserId();
            var appUser = db.Users.OfType<Staff>()
                .Include(s => s.Subordinates)
                .Include(su => su.Superior)
                .Where(usr => usr.Id == usrID)
                .Single();

            var vm = new HierarchyIndexViewModel(appUser);

            return View(vm);
        }

        public ActionResult StaffManagerIndex()
        {
            var Directors = db.Users.OfType<Staff>().Where(s => s.Rank == Rank.Director).ToList();
            var Managers = db.Users.OfType<Staff>().Where(s => s.Rank == Rank.Manager).ToList();
            var Agents = db.Users.OfType<Staff>().Where(s => s.Rank == Rank.Agent).ToList();

            var vm = new StaffManagerIndexIMSViewModel(Directors, Managers, Agents);

            return View("StaffManagerindex", vm);
        }

        public ActionResult StaffManagerEdit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var staff = db.Users.OfType<Staff>().Include(a => a.Superior).Include(b => b.Subordinates)
                .Where(u => u.Id == id).Single();

            var vm = new StaffManagerEditViewModel(staff);
            var subordinateList = new MultiSelectList(db.Users.OfType<Staff>(), "Id", "Forename", vm.SubordinateIds);
            vm.Subordinates = subordinateList;
            ViewBag.SuperiorID = new SelectList(db.Users.OfType<Staff>(), "Id", "Forename");

            if (staff == null)
            {
                return HttpNotFound();
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StaffManagerEdit([Bind(Exclude = "Subordinates")] StaffManagerEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                // Add all users seleced in listBox to newly created Staff member's subordinate list
                // This changes the subordinate's superior ID via EF framework magic
                var _subordinates = new Collection<Staff>();
                foreach (var item in vm.SubordinateIds)
                {
                    var _tmpUsr = db.Users.OfType<Staff>().Where(u => u.Id == item).Single();
                    _subordinates.Add(_tmpUsr);
                }

                // Find user and set the updated values a/r
                var staff = db.Users.OfType<Staff>().Where(u => u.Id == vm.Id).Single();
                staff.Rank = vm.Rank;
                staff.SuperiorID = vm.SuperiorID;
                staff.Subordinates = _subordinates;

                db.Entry(staff).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("StaffManagerIndex", "IMS", null);
            }
            return View(vm);
        }

        // GET: Propertys/Delete/5
        public ActionResult StaffManagerDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var staff = db.Users.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // POST: Propertys/Delete/5
        [HttpPost, ActionName("StaffManagerDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult StaffManagerDelete(string id)
        {
            var staff = db.Users.OfType<Staff>().Include(s => s.Subordinates).Include(su => su.Superior)
                .Where(u => u.Id == id).Single();

            foreach (var item in staff.Subordinates.ToList())
            {
                item.SuperiorID = "1f8a6942-2e52-480d-b0e1-1e4afb9ee33b";
                //item.Superior = (Staff)db.Users.Find("1f8a6942-2e52-480d-b0e1-1e4afb9ee33b");
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }

            db.Users.Remove(staff);
            db.SaveChanges();
            return RedirectToAction("StaffManagerIndex");
        }

        // GET: IMS/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: IMS/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: IMS/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: IMS/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: IMS/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: IMS/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: IMS/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
