﻿using DeanAndSons.Models;
using DeanAndSons.Models.CMS.ViewModels;
using DeanAndSons.Models.IMS.ViewModels;
using DeanAndSons.Models.WAP.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DeanAndSons.Controllers
{
    [Authorize]
    public class PropertysController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Propertys
        [AllowAnonymous]
        public ActionResult Index(int? page, string searchString = null, string CategorySort = "0", string OrderSort = "0", string currentFilter = null,
            string Location = "Bath", string MinPrice = "50000", string Beds = "1", string Radius = "30", string MaxPrice = "100000000", string Age = "0", string SaleState = "0")
        {
            var _MinPrice = Int32.Parse(MinPrice);
            var _MaxPrice = Int32.Parse(MaxPrice);
            var _Beds = UInt16.Parse(Beds);
            var _Age = Convert.ToUInt16(Age);
            var _Radius = Convert.ToUInt16(Radius);
            var _SaleState = Convert.ToUInt16(SaleState);

            //Get current logged in user object and check if it is of type customer and set this as a viewbag var.
            var _currentUser = CurrentUser();
            ViewBag.IsCustomer = ApplicationUser.IsCustomer(_currentUser);

            // ********** Database Access **********
            //var dbModel = new List<Property>();
            var dbModel = db.Propertys
                .Include(p => p.Contact)
                .Include(p => p.Images)
                .Where(p => p.Deleted != true);

            if (!String.IsNullOrWhiteSpace(searchString))
            {
                dbModel = dbModel
                    .Where(p => p.Title.Contains(searchString))
                    .Where(pm => pm.Price >= _MinPrice && pm.Price <= _MaxPrice)
                    .Where(b => b.NoBedRms > _Beds);
            }
            else
            {
                dbModel = dbModel
                        .Where(pm => pm.Price >= _MinPrice && pm.Price <= _MaxPrice)
                        .Where(b => b.NoBedRms >= _Beds);
            }

            if (_Age != 0)
            {
                dbModel = dbModel.Where(a => a.Age == (PropertyAge)_Age);
            }

            if (_SaleState != 0)
            {
                dbModel = dbModel.Where(a => a.SaleState == (SaleState)_SaleState);
            }
            else
            {
                dbModel = dbModel.Where(a => a.SaleState == Models.SaleState.Any
                || a.SaleState == Models.SaleState.ForSale
                || a.SaleState == Models.SaleState.ToLet
                || a.SaleState == Models.SaleState.UnderOffer);
            }

            var dbModelList = dbModel.ToList();

            // ********** Distance Calculations **********
            var sourceLocation = Contact.GetLocationLatLng(Location);

            foreach (var item in dbModelList.ToList())
            {
                var itemContact = item.getContact(item.Contact);
                var destLocation = new GeoCoordinate(itemContact.Lat, itemContact.Long);
                //Get distance between two positions in meters & convert to miles
                var itemDistanceSource = sourceLocation.GetDistanceTo(destLocation);
                itemDistanceSource = (itemDistanceSource / 1000) * 0.621371192;

                if (itemDistanceSource > _Radius)
                {
                    dbModelList.Remove(item);
                }
            }

            // ********** Paging and Sorting **********

            //Populate lists and add them to ViewBag for assignment to dropDowns in View.
            ViewBag.Location = populateLocation();
            ViewBag.Radius = populateRadius();
            ViewBag.MinPrice = populateMinPrice();
            ViewBag.MaxPrice = populateMaxPrice();
            ViewBag.Beds = populateBeds();
            ViewBag.Age = populateAge();
            ViewBag.SaleState = populateSaleState();
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
                    dbModelList = dbModelList.OrderBy(a => a.Title).ToList();
                    break;
                case "01":
                    dbModelList = dbModelList.OrderByDescending(a => a.Title).ToList();
                    break;
                case "10":
                    dbModelList = dbModelList.OrderBy(a => a.Created).ToList();
                    break;
                case "11":
                    dbModelList = dbModelList.OrderByDescending(a => a.Created).ToList();
                    break;
                case "20":
                    dbModelList = dbModelList.OrderBy(a => a.Price).ToList();
                    break;
                case "21":
                    dbModelList = dbModelList.OrderByDescending(a => a.Price).ToList();
                    break;
                default:
                    dbModelList = dbModelList.OrderBy(a => a.Title).ToList();
                    break;
            }

            var indexList = new List<PropertyIndexViewModel>();
            foreach (var item in dbModelList)
            {
                var vm = new PropertyIndexViewModel(item);

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

        [Authorize(Roles = "Admin, Staff")]
        public ActionResult IndexL(string searchString)
        {
            // ********** Database Access **********
            var dbModel = db.Propertys.AsQueryable();

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
                return PartialView("_IndexLList", dbModel);
            }

            return View(dbModel);
        }

        [Authorize(Roles = "Admin, Staff")]
        public ActionResult IndexIMS(string searchString)
        {
            // ********** Database Access **********
            var dbModel = db.Propertys.Include(b => b.Buyer).Include(s => s.Seller);

            if (!User.IsInRole("Admin"))
            {
                var _currentUser = CurrentUser();
                dbModel = dbModel.Where(p => p.StaffOwnerID == _currentUser.Id);
            }

            if (!String.IsNullOrWhiteSpace(searchString))
            {
                dbModel = dbModel.Where(p => p.Title.Contains(searchString));
            }

            var dbModelList = dbModel.ToList();

            //If the user has called this action via AJAX (ie search field) then only update the partial view.
            if (Request.IsAjaxRequest())
            {
                return PartialView("_IndexIMSList", dbModelList);
            }

            return View(dbModelList);
        }

        // GET: Propertys/Edit/5
        [Authorize(Roles = "Admin, Staff")]
        public ActionResult EditCMS(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Property property = db.Propertys.Include(i => i.Images).Single(p => p.PropertyID == id);

            PropertyEditCMSViewModel vm = new PropertyEditCMSViewModel(property);

            if (property == null)
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
        public ActionResult EditCMS(PropertyEditCMSViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var property = db.Propertys.Include(i => i.Images).Single(p => p.PropertyID == vm.PropertyID);
                var _hasNewImage = -1;

                property.Title = vm.Title;
                property.Description = vm.Description;

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
                    foreach (var item in property.Images.ToList())
                    {
                        property.removeImage(item);
                        db.Images.Remove(item);
                    }

                    //Add images to property.  Will only add images if there is an image in the list and if there is an image in the list it would have
                    //already been emptied by the foreach loop above.
                    property.Images = property.addImages(vm.Images);
                }

                db.Entry(property).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexL");
            }

            Property prop = db.Propertys.Include(i => i.Images).Single(p => p.PropertyID == vm.PropertyID);
            vm.ImagesProp = prop.Images;
            return View(vm);
        }

        [AllowAnonymous]
        // GET: Propertys/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Get current logged in user object and check if it is of type customer and set this as a viewbag var.
            var _currentUser = CurrentUser();
            ViewBag.IsCustomer = ApplicationUser.IsCustomer(_currentUser);

            Property property = db.Propertys.Include(i => i.Images)
                .Include(c => c.Contact)
                .Include(s => s.StaffOwner)
                .Single(p => p.PropertyID == id);

            if (property == null)
            {
                return HttpNotFound();
            }

            var vm = new PropertyDetailsViewModel(property);

            return View(vm);
        }

        // GET: Propertys/Create
        [Authorize(Roles = "Admin, Staff")]
        public ActionResult CreateIMS()
        {
            //Populate drop down lists with data from DB
            ViewBag.BuyerID = new SelectList(db.Users.OfType<Customer>(), "Id", "Forename");
            ViewBag.SellerID = new SelectList(db.Users.OfType<Customer>(), "Id", "Forename");
            ViewBag.StaffOwnerID = new SelectList(db.Users.OfType<Staff>(), "Id", "Forename");

            PropertyCreateIMSViewModel vm = new PropertyCreateIMSViewModel();

            return View(vm);
        }

        // POST: Propertys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Staff")]
        public ActionResult Create(PropertyCreateIMSViewModel vm)
        {
            if (ModelState.IsValid)

            {
                var property = new Property(vm);

                db.Propertys.Add(property);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //Populate drop down lists with data from DB
            ViewBag.BuyerID = new SelectList(db.Users.OfType<Customer>(), "Id", "Forename", vm.BuyerID);
            ViewBag.SellerID = new SelectList(db.Users.OfType<Customer>(), "Id", "Forename", vm.SellerID);
            ViewBag.StaffOwnerID = new SelectList(db.Users.OfType<Staff>(), "Id", "Forename", vm.StaffOwnerID);

            return View(vm);
        }

        // GET: Propertys/Edit/5
        [Authorize(Roles = "Admin, Staff")]
        public ActionResult EditIMS(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Property property = db.Propertys.Find(id);
            var vm = new PropertyEditIMSViewModel(property);
            vm.Buyer = new SelectList(db.Users.OfType<Customer>(), "Id", "Forename", vm.BuyerID);
            vm.Seller = new SelectList(db.Users.OfType<Customer>(), "Id", "Forename", vm.SellerID);
            vm.StaffOwner = new SelectList(db.Users.OfType<Staff>(), "Id", "Forename", vm.StaffOwnerID);

            if (property == null)
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
        public ActionResult EditIMS(PropertyEditIMSViewModel vm)
        {
            if (ModelState.IsValid)
            {
                //Get object to edit from DB
                var _obj = db.Propertys.Find(vm.PropertyID);
                //Apply view model properties to EF tracked DB object
                _obj.ApplyEditIMS(vm);

                db.Entry(_obj).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexIMS");
            }

            vm.Buyer = new SelectList(db.Users.OfType<Customer>(), "Id", "Forename", vm.BuyerID);
            vm.Seller = new SelectList(db.Users.OfType<Customer>(), "Id", "Forename", vm.SellerID);
            vm.StaffOwner = new SelectList(db.Users.OfType<Staff>(), "Id", "Forename", vm.StaffOwnerID);
            return View(vm);
        }

        // POST: Propertys/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Property property = db.Propertys.Find(id);
            db.Propertys.Remove(property);
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
            Property property = db.Propertys.Find(id);
            property.Deleted = true;
            property.LastModified = DateTime.Now;
            db.Entry(property).State = EntityState.Modified;
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
        private List<SelectListItem> populateLocation()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Bath", Value = "Bath", Selected = true });
            items.Add(new SelectListItem { Text = "Chippenham", Value = "Chippenham" });
            items.Add(new SelectListItem { Text = "Corsham", Value = "Corsham" });
            items.Add(new SelectListItem { Text = "Devizes", Value = "Devizes" });
            items.Add(new SelectListItem { Text = "Melksham", Value = "Melksham" });
            items.Add(new SelectListItem { Text = "Swindon", Value = "Swindon" });
            items.Add(new SelectListItem { Text = "Trowbridge", Value = "Trowbridge" });

            return items;
        }

        private List<SelectListItem> populateRadius()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "1 Mile", Value = "1" });
            items.Add(new SelectListItem { Text = "2 Miles", Value = "2" });
            items.Add(new SelectListItem { Text = "5 Miles", Value = "5" });
            items.Add(new SelectListItem { Text = "10 Miles", Value = "10" });
            items.Add(new SelectListItem { Text = "15 Miles", Value = "15" });
            items.Add(new SelectListItem { Text = "20 Miles", Value = "20" });
            items.Add(new SelectListItem { Text = "30 Miles", Value = "30", Selected = true });

            return items;
        }
        
        private List<SelectListItem> populateMinPrice()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "50,000", Value = "50000", Selected = true });
            items.Add(new SelectListItem { Text = "100,000", Value = "100000" });
            items.Add(new SelectListItem { Text = "200,000", Value = "200000" });
            items.Add(new SelectListItem { Text = "300,000", Value = "300000" });
            items.Add(new SelectListItem { Text = "400,000", Value = "400000" });
            items.Add(new SelectListItem { Text = "500,000", Value = "500000" });
            items.Add(new SelectListItem { Text = "600,000", Value = "600000" });
            items.Add(new SelectListItem { Text = "700,000", Value = "700000" });
            items.Add(new SelectListItem { Text = "800,000", Value = "800000" });
            items.Add(new SelectListItem { Text = "900,000", Value = "900000" });
            items.Add(new SelectListItem { Text = "1,000,000", Value = "1000000" });

            return items;
        }

        private List<SelectListItem> populateMaxPrice()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "50,000", Value = "50000" });
            items.Add(new SelectListItem { Text = "100,000", Value = "100000" });
            items.Add(new SelectListItem { Text = "200,000", Value = "200000" });
            items.Add(new SelectListItem { Text = "300,000", Value = "300000" });
            items.Add(new SelectListItem { Text = "100,000", Value = "400000" });
            items.Add(new SelectListItem { Text = "500,000", Value = "500000" });
            items.Add(new SelectListItem { Text = "600,000", Value = "600000" });
            items.Add(new SelectListItem { Text = "700,000", Value = "700000" });
            items.Add(new SelectListItem { Text = "800,000", Value = "800000" });
            items.Add(new SelectListItem { Text = "900,000", Value = "900000" });
            items.Add(new SelectListItem { Text = "1,000,000", Value = "1000000" });
            items.Add(new SelectListItem { Text = "100,000,000", Value = "100000000", Selected = true });

            return items;
        }

        private List<SelectListItem> populateBeds()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "1", Value = "1", Selected = true });
            items.Add(new SelectListItem { Text = "2", Value = "2" });
            items.Add(new SelectListItem { Text = "3", Value = "3" });
            items.Add(new SelectListItem { Text = "4", Value = "4" });
            items.Add(new SelectListItem { Text = "5+", Value = "5" });

            return items;
        }

        private List<SelectListItem> populateAge()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            
            items.Add(new SelectListItem { Text = "Any", Value = "0", Selected = true });
            items.Add(new SelectListItem { Text = "Older", Value = "1" });
            items.Add(new SelectListItem { Text = "Modern", Value = "2" });
            items.Add(new SelectListItem { Text = "New Build", Value = "4" });
            items.Add(new SelectListItem { Text = "Post War", Value = "8" });
            items.Add(new SelectListItem { Text = "Pre-War", Value = "16" });

            return items;
        }

        private List<SelectListItem> populateSaleState()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Any", Value = "0", Selected = true });
            items.Add(new SelectListItem { Text = "For Sale", Value = "1" });
            items.Add(new SelectListItem { Text = "Under Offer", Value = "2" });
            items.Add(new SelectListItem { Text = "To Let", Value = "8" });

            return items;
        }

        private List<SelectListItem> populateCategorySort()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Title", Value = "0", Selected = true });
            items.Add(new SelectListItem { Text = "Date Listed", Value = "1" });
            items.Add(new SelectListItem { Text = "Price", Value = "2" });

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
