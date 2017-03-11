using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DeanAndSons.Models;
using DeanAndSons.Models.WAP;
using DeanAndSons.Models.WAP.ViewModels;

namespace DeanAndSons.Controllers
{
    public class EventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // ********** Customer Views **********
        public ActionResult IndexCustomer()
        {
            var events = db.Events.Include(i => i.Images)
                .Include(s => s.StaffOwner)
                .Include(c => c.Contact)
                .ToList();

            var vmList = new List<EventIndexViewModel>();

            foreach (var item in events)
            {
                var vm = new EventIndexViewModel();

                vm.Title = item.Title;
                vm.EventID = item.EventID;
                vm.Image = item.Images.Single(i => i.Type == ImageType.EventHeader);

                vmList.Add(vm);
            }

            return View(vmList);
        }

        public ActionResult DetailsCustomer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Service service = db.Services.Include(s => s.StaffOwner)
                .Include(i => i.Images)
                .Single(se => se.ServiceID == id);

            var vm = new ServiceDetailsViewModel(service);

            if (service == null)
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
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EventID,Title,Description,Created,LastModified,Deleted,StaffOwnerID")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StaffOwnerID = new SelectList(db.Users, "Id", "Forename", @event.StaffOwnerID);
            return View(@event);
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
    }
}
