using DeanAndSons.Models;
using DeanAndSons.Models.WAP;
using DeanAndSons.Models.WAP.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DeanAndSons.Controllers
{
    public class EventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // ********** Customer Views **********
        public ActionResult IndexCustomer(int? page, string searchString = null, string CategorySort = "0", string OrderSort = "0", string currentFilter = null)
        {
            // ********** Database Access **********
            var dbModel = db.Events.Include(i => i.Images)
                .Include(c => c.Contact);

            // ********** Search string **********
            if (!String.IsNullOrWhiteSpace(searchString))
            {
                dbModel = dbModel.Where(p => p.Title.Contains(searchString));
            }

            var dbModelList = dbModel.ToList();

            // ********** Paging and Sorting **********

            //Populate lists and add them to ViewBag for assignment to dropDowns in View.
            ViewBag.CategorySort = populateCategorySort();
            ViewBag.OrderSort = populateOrderSort();

            //Concatenate both sorting methods for use in select case.
            CategorySort = CategorySort + OrderSort;

            //Start of pagination addition
            ViewBag.CurrentSort = CategorySort;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            //Takes CategorySort var and matches it against case strings to determine how to order the table
            switch (CategorySort)
            {
                case "00":
                    //Alters dbModelList SQL to add order params to it, ditto for all of below.
                    dbModelList = dbModel.OrderBy(a => a.Title).ToList();
                    break;
                case "01":
                    dbModelList = dbModel.OrderByDescending(a => a.Title).ToList();
                    break;
                case "10":
                    dbModelList = dbModel.OrderBy(a => a.Created).ToList();
                    break;
                case "11":
                    dbModelList = dbModel.OrderByDescending(a => a.Created).ToList();
                    break;
                default:
                    dbModelList = dbModel.OrderBy(a => a.Title).ToList();
                    break;
            }

            var indexList = new List<EventIndexViewModel>();
            foreach (var item in dbModelList)
            {
                var vm = new EventIndexViewModel(item);

                indexList.Add(vm);
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);

            //If the user has called this action via AJAX (ie search field) then only update the partial view.
            if (Request.IsAjaxRequest())
            {
                return PartialView("_IndexList", indexList.ToPagedList(pageNumber, pageSize));
            }

            return View(indexList.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult DetailsCustomer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Event @event = db.Events.Include(s => s.StaffOwner)
                .Include(i => i.Images)
                .Include(c => c.Contact)
                .Single(se => se.EventID == id);

            var vm = new EventDetailsViewModel(@event);

            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(vm);
        }




        // ********** CRUD Views **********

        // GET: Events
        public ActionResult Index()
        {
            var events = db.Events.Include(s => s.StaffOwner);
            return View(events.ToList());
        }

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            ViewBag.StaffOwnerID = new SelectList(db.Users, "Id", "Forename");
            var vm = new EventCreateViewModel();
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var @event = new Event(vm);
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StaffOwnerID = new SelectList(db.Users, "Id", "Forename", vm.StaffOwnerID);
            return View(vm);
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            ViewBag.StaffOwnerID = new SelectList(db.Users, "Id", "Forename", @event.StaffOwnerID);
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventID,Title,Description,Created,LastModified,Deleted,StaffOwnerID")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StaffOwnerID = new SelectList(db.Users, "Id", "Forename", @event.StaffOwnerID);
            return View(@event);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //Populate category and sort drop down lists
        private List<SelectListItem> populateCategorySort()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Title", Value = "0", Selected = true });
            items.Add(new SelectListItem { Text = "Date Listed", Value = "1" });

            return items;
        }

        private List<SelectListItem> populateOrderSort()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Asc.", Value = "0", Selected = true });
            items.Add(new SelectListItem { Text = "Desc.", Value = "1" });

            return items;
        }
    }
}
