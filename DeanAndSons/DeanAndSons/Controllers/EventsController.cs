using DeanAndSons.Models;
using DeanAndSons.Models.CMS.ViewModels;
using DeanAndSons.Models.IMS.ViewModels;
using DeanAndSons.Models.WAP;
using DeanAndSons.Models.WAP.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
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
    [Authorize(Roles = "Admin, Staff")]
    public class EventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // ********** Customer Views **********
        [AllowAnonymous]
        public ActionResult IndexCustomer(int? page, string searchString = null, string CategorySort = "0", string OrderSort = "0", string currentFilter = null)
        {
            // ********** Database Access **********
            var dbModel = db.Events
                .Include(i => i.Images)
                .Include(c => c.Contact)
                .Where(p => p.Deleted != true);

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

        public ActionResult IndexIMS(string searchString)
        {
            // ********** Database Access **********
            var dbModel = db.Events.Include(e => e.StaffOwner);

            if (!User.IsInRole("Admin"))
            {
                var _currentUser = CurrentUser();
                dbModel = dbModel.Where(p => p.StaffOwnerID == _currentUser.Id);
            }

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
            var dbModel = db.Events.Include(e => e.StaffOwner);

            if (!User.IsInRole("Admin"))
            {
                var _currentUser = CurrentUser();
                dbModel = dbModel.Where(p => p.StaffOwnerID == _currentUser.Id);
            }

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

        [AllowAnonymous]
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
        [Authorize(Roles = "Admin, Staff")]
        public ActionResult CreateIMS()
        {
            ViewBag.StaffOwnerID = new SelectList(db.Users, "Id", "Forename");
            var vm = new EventCreateIMSViewModel();
            return View(vm);
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Staff")]
        public ActionResult CreateIMS(EventCreateIMSViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var @event = new Event(vm);
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("IndexIMS");
            }

            ViewBag.StaffOwnerID = new SelectList(db.Users, "Id", "Forename", vm.StaffOwnerID);
            return View(vm);
        }

        // GET: Events/Edit/5
        [Authorize(Roles = "Admin, Staff")]
        public ActionResult EditIMS(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Event @event = db.Events.Include(a => a.Contact)
                .Single(e => e.EventID == id);
            var vm = new EventEditIMSViewModel(@event);
            //Populate drop down lists with data from DB
            vm.StaffOwner = new SelectList(db.Users.OfType<Staff>(), "Id", "Forename", vm.StaffOwnerID);

            if (@event == null)
            {
                return HttpNotFound();
            }

            return View(vm);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Staff")]
        public ActionResult EditIMS(EventEditIMSViewModel vm)
        {
            if (ModelState.IsValid)
            {
                //Get object to edit from DB
                var _obj = db.Events.Find(vm.EventID);
                //Apply view model properties to EF tracked DB object
                _obj.ApplyEditIMS(vm);

                db.Entry(_obj).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexIMS");
            }

            ViewBag.StaffOwnerID = new SelectList(db.Users.OfType<Staff>(), "Id", "Forename", vm.StaffOwnerID);
            return View(vm);
        }

        // GET: Propertys/Edit/5
        [Authorize(Roles = "Admin, Staff")]
        public ActionResult EditCMS(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var @event = db.Events.Include(i => i.Images)
                .Include(s => s.StaffOwner)
                .Include(a => a.Contact)
                .Single(p => p.EventID == id);

            var vm = new EventEditCMSViewModel(@event);

            if (@event == null)
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
        [Authorize(Roles = "Admin, Staff")]
        public ActionResult EditCMS(EventEditCMSViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var @event = db.Events.Include(i => i.Images).Single(p => p.EventID == vm.EventID);
                var _hasNewImage = -1;

                @event.Title = vm.Title;
                @event.Description = vm.Description;

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
                    foreach (var item in @event.Images.ToList())
                    {
                        @event.removeImage(item);
                        db.Images.Remove(item);
                    }

                    //Add images to property.  Will only add images if there is an image in the list and if there is an image in the list it would have
                    //already been emptied by the foreach loop above.
                    @event.Images = @event.addImages(vm.Images);
                }

                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexCMS");
            }

            var ev = db.Events.Include(i => i.Images)
                .Include(s => s.StaffOwner)
                .Include(a => a.Contact)
                .Single(p => p.EventID == vm.EventID);
            vm.ImagesProp = ev.Images;

            return View(vm);
        }

        //// GET: Events/Delete/5
        //[Authorize(Roles = "Admin")]
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Event @event = db.Events.Find(id);
        //    if (@event == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(@event);
        //}

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
            db.SaveChanges();
            return RedirectToAction("IndexIMS");
        }

        /// <summary>
        /// Sets DB flag to deleted but doesn't physically remove the item
        /// </summary>
        /// <param name="id">ID of item to set delete flag</param>
        /// <returns></returns>
        [HttpPost, ActionName("DeleteLogical")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Staff")]
        public ActionResult DeleteLogical(int id)
        {
            Event @event = db.Events.Find(id);
            @event.Deleted = true;
            @event.LastModified = DateTime.Now;

            db.Entry(@event).State = EntityState.Modified;
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

        private ApplicationUser CurrentUser()
        {
            return System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
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
