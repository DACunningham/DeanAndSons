﻿using DeanAndSons.Models;
using DeanAndSons.Models.CMS.ViewModels;
using DeanAndSons.Models.IMS.ViewModels;
using DeanAndSons.Models.WAP;
using DeanAndSons.Models.WAP.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DeanAndSons.Controllers
{
    public class ServicesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // ********** Customer Views **********
        //public ActionResult IndexCustomer(int? page, string searchString = null, string CategorySort = "0", string OrderSort = "0", string currentFilter = null)
        //{
        //    // ********** Database Access **********
        //    var dbModel = db.Services.Include(i => i.Images);

        //    // ********** Search string **********
        //    //if (!String.IsNullOrWhiteSpace(searchString))
        //    //{
        //    //    dbModel = dbModel.Where(p => p.Title.Contains(searchString));
        //    //}

        //    var dbModelList = dbModel.ToList();

        //    //// ********** Paging and Sorting **********

        //    ////Populate lists and add them to ViewBag for assignment to dropDowns in View.
        //    //ViewBag.CategorySort = populateCategorySort();
        //    //ViewBag.OrderSort = populateOrderSort();

        //    ////Concatenate both sorting methods for use in select case.
        //    //CategorySort = CategorySort + OrderSort;

        //    ////Start of pagination addition
        //    //ViewBag.CurrentSort = CategorySort;

        //    //if (searchString != null)
        //    //{
        //    //    page = 1;
        //    //}
        //    //else
        //    //{
        //    //    searchString = currentFilter;
        //    //}

        //    //ViewBag.CurrentFilter = searchString;

        //    ////Takes CategorySort var and matches it against case strings to determine how to order the table
        //    //switch (CategorySort)
        //    //{
        //    //    case "00":
        //    //        //Alters dbModelList SQL to add order params to it, ditto for all of below.
        //    //        dbModelList = dbModel.OrderBy(a => a.Title).ToList();
        //    //        break;
        //    //    case "01":
        //    //        dbModelList = dbModel.OrderByDescending(a => a.Title).ToList();
        //    //        break;
        //    //    case "10":
        //    //        dbModelList = dbModel.OrderBy(a => a.Created).ToList();
        //    //        break;
        //    //    case "11":
        //    //        dbModelList = dbModel.OrderByDescending(a => a.Created).ToList();
        //    //        break;
        //    //    default:
        //    //        dbModelList = dbModel.OrderBy(a => a.Title).ToList();
        //    //        break;
        //    //}

        //    var indexList = new List<ServiceIndexViewModel>();
        //    foreach (var item in dbModelList)
        //    {
        //        var vm = new ServiceIndexViewModel(item);

        //        indexList.Add(vm);
        //    }

        //    //int pageSize = 3;
        //    //int pageNumber = (page ?? 1);

        //    //If the user has called this action via AJAX (ie search field) then only update the partial view.
        //    //if (Request.IsAjaxRequest())
        //    //{
        //    //    return PartialView("_IndexList", indexList.ToPagedList(pageNumber, pageSize));
        //    //}

        //    return View(indexList);
        //    //return View(indexList.ToPagedList(pageNumber, pageSize));
        //}

        // ********** Customer Views **********
        public ActionResult IndexCustomer()
        {
            // ********** Database Access **********
            var dbModel = db.Services.Include(i => i.Images).Include(s => s.StaffOwner).ToList();

            var indexList = new List<ServiceIndexViewModel>();
            foreach (var item in dbModel)
            {
                var vm = new ServiceIndexViewModel(item);

                indexList.Add(vm);
            }

            return View(indexList);

            //var services = db.Services.Include(i => i.Images).Include(s => s.StaffOwner).ToList();
            //var vmList = new List<ServiceIndexViewModel>();

            //foreach (var item in services)
            //{
            //    var vm = new ServiceIndexViewModel();

            //    vm.Title = item.Title;
            //    vm.SubTitle = item.SubTitle;
            //    vm.ServiceID = item.ServiceID;
            //    vm.Image = item.Images.Single(i => i.Type == ImageType.ServiceHeader);

            //    vmList.Add(vm);
            //}

            //return View(vmList);
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

        public ActionResult IndexCMS(string searchString)
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
                return PartialView("_IndexCMS", dbModel);
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

        // GET: Services/Create
        public ActionResult CreateIMS()
        {
            ViewBag.StaffOwnerID = new SelectList(db.Users, "Id", "Forename");
            var vm = new ServiceCreateIMSViewModel();
            return View(vm);
        }

        // POST: Services/Create
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

        // GET: Propertys/Edit/5
        public ActionResult EditCMS(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var service = db.Services.Include(i => i.Images)
                .Include(s => s.StaffOwner)
                .Single(p => p.ServiceID == id);

            var vm = new ServiceEditCMSViewModel(service);

            if (service == null)
            {
                return HttpNotFound();
            }

            return View(vm);
        }

        // POST: Propertys/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCMS(ServiceEditCMSViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var service = db.Services.Include(i => i.Images).Single(p => p.ServiceID == vm.ServiceID);
                var _hasNewImage = -1;

                service.Title = vm.Title;
                service.Description = vm.Description;

                //Checks if any of the image objs has a valid file uploaded
                foreach (var item in vm.Images)
                {
                    if (item != null && item.ContentLength != 0)
                    {
                        _hasNewImage = 1;
                        break;
                    }
                }

                //If image obj has valid file uploaded clear original image list and add new images.
                if (_hasNewImage == 1)
                {
                    //Remove all images from file store and DB
                    foreach (var item in service.Images.ToList())
                    {
                        service.removeImage(item);
                        db.Images.Remove(item);
                    }

                    //Add images to property.  Will only add images if there is an image in the list and if there is an image in the list it would have
                    //already been emptied by the foreach loop above.
                    service.Images = service.addImages(vm.Images);
                }

                db.Entry(service).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexCMS");
            }
            return View(vm);
        }

        // GET: Services/Edit/5
        public ActionResult EditIMS(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Service service = db.Services.Single(e => e.ServiceID == id);
            var vm = new ServiceEditIMSViewModel(service);
            //Populate drop down lists with data from DB
            vm.StaffOwner = new SelectList(db.Users.OfType<Staff>(), "Id", "Forename", vm.StaffOwnerID);

            if (service == null)
            {
                return HttpNotFound();
            }

            return View(vm);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditIMS(ServiceEditIMSViewModel vm)
        {
            if (ModelState.IsValid)
            {
                //Get object to edit from DB
                var _obj = db.Services.Find(vm.ServiceID);
                //Apply view model properties to EF tracked DB object
                _obj.ApplyEditIMS(vm);

                db.Entry(_obj).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexIMS");
            }

            ViewBag.StaffOwnerID = new SelectList(db.Users.OfType<Staff>(), "Id", "Forename", vm.StaffOwnerID);
            return View(vm);
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

        /// <summary>
        /// Sets DB flag to deleted but doesn't physically remove the item
        /// </summary>
        /// <param name="id">ID of item to set delete flag</param>
        /// <returns></returns>
        [HttpPost, ActionName("DeleteLogical")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteLogical(int id)
        {
            var service = db.Services.Find(id);
            service.Deleted = true;
            service.LastModified = DateTime.Now;

            db.Entry(service).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("IndexIMS");
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
