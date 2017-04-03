using DeanAndSons.Models;
using DeanAndSons.Models.IMS.ViewModels;
using DeanAndSons.Models.WAP;
using DeanAndSons.Models.WAP.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace DeanAndSons.Controllers
{
    public class ServicesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // ********** Customer Views **********
        public ActionResult IndexCustomer()
        {
            var services = db.Services.Include(i => i.Images).Include(s => s.StaffOwner).ToList();
            var vmList = new List<ServiceIndexViewModel>();

            foreach (var item in services)
            {
                var vm = new ServiceIndexViewModel();

                vm.Title = item.Title;
                vm.SubTitle = item.SubTitle;
                vm.ServiceID = item.ServiceID;
                vm.Image = item.Images.Single(i => i.Type == ImageType.ServiceHeader);

                vmList.Add(vm);
            }

            return View(vmList);
        }

        public ActionResult IndexIMS(string searchString)
        {
            // ********** Database Access **********
            var dbModel = db.Services.Include(e => e.StaffOwner);

            if (!String.IsNullOrWhiteSpace(searchString))
            {
                dbModel = dbModel.Where(p => p.Title.Contains(searchString));
            }

            //If the user has called this action via AJAX (ie search field) then only update the partial view.
            if (Request.IsAjaxRequest())
            {
                return PartialView("_IndexIMS", dbModel);
            }

            return View(dbModel);
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

        // GET: Services
        public ActionResult Index()
        {
            var services = db.Services.Include(s => s.StaffOwner);
            return View(services.ToList());
        }

        // GET: Services/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // GET: Events/Create
        public ActionResult CreateIMS()
        {
            ViewBag.StaffOwnerID = new SelectList(db.Users, "Id", "Forename");
            var vm = new ServiceCreateIMSViewModel();
            return View(vm);
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateIMS(ServiceCreateIMSViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var service = new Service(vm);
                db.Services.Add(service);
                db.SaveChanges();
                return RedirectToAction("IndexIMS");
            }

            ViewBag.StaffOwnerID = new SelectList(db.Users, "Id", "Forename", vm.StaffOwnerID);
            return View(vm);
        }

        //// GET: Services/Create
        //public ActionResult Create()
        //{
        //    ViewBag.StaffOwnerID = new SelectList(db.Users, "Id", "Forename");
        //    var vm = new ServiceCreateIMSViewModel();
        //    return View(vm);
        //}

        //// POST: Services/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(ServiceCreateIMSViewModel vm)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var service = new Service(vm);
        //        db.Services.Add(service);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.StaffOwnerID = new SelectList(db.Users, "Id", "Forename", vm.StaffOwnerID);
        //    return View(vm);
        //}

        // GET: Services/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            ViewBag.StaffOwnerID = new SelectList(db.Users, "Id", "Forename", service.StaffOwnerID);
            return View(service);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ServiceID,Title,Description,StaffOwnerID")] Service service)
        {
            if (ModelState.IsValid)
            {
                //Update last updated field
                service.LastModified = DateTime.Now;

                db.Entry(service).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StaffOwnerID = new SelectList(db.Users, "Id", "Forename", service.StaffOwnerID);
            return View(service);
        }

        // GET: Services/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Service service = db.Services.Find(id);
            db.Services.Remove(service);
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
