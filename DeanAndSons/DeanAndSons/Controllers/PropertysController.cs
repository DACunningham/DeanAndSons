using DeanAndSons.Models;
using DeanAndSons.Models.WAP.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace DeanAndSons.Controllers
{
    public class PropertysController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Propertys
        public ActionResult Index(int? page, string searchString = null, string CategorySort = "0", string OrderSort = "0", string currentFilter = null,
            string Location = null, string MinPrice = "50000", string Beds = "1", string Radius = null, string MaxPrice = "1000000", string Age = "0")
        {
            var _MinPrice = Int32.Parse(MinPrice);
            var _MaxPrice = Int32.Parse(MaxPrice);
            var _Beds = UInt16.Parse(Beds);
            var _Age = Convert.ToUInt16(Age);

            // ********** Database Access **********
            //var dbModel = new List<Property>();
            var dbModel = db.Propertys.Include(p => p.Contact).Include(p => p.Images);

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

            var dbModelList = dbModel.ToList();

            // ********** Paging and Sorting **********

            //Populate lists and add them to ViewBag for assignment to dropDowns in View.
            ViewBag.Location = populateLocation();
            ViewBag.Radius = populateRadius();
            ViewBag.MinPrice = populateMinPrice();
            ViewBag.MaxPrice = populateMaxPrice();
            ViewBag.Beds = populateBeds();
            ViewBag.Age = populateAge();
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
                case "20":
                    dbModelList = dbModel.OrderBy(a => a.Price).ToList();
                    break;
                case "21":
                    dbModelList = dbModel.OrderByDescending(a => a.Price).ToList();
                    break;
                default:
                    dbModelList = dbModel.OrderBy(a => a.Title).ToList();
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

        public ActionResult IndexL()
        {
            // ********** Database Access **********
            var dbModel = db.Propertys.ToList();

            //var indexList = new List<PropertyIndexViewModel>();
            //var dbModelList = dbModel.ToList();

            //foreach (var item in dbModelList)
            //{
            //    var vm = new PropertyIndexViewModel(item);

            //    indexList.Add(vm);
            //}

            return View(dbModel);
        }

        // GET: Propertys/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

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
        public ActionResult Create()
        {
            ViewBag.StaffOwnerID = new SelectList(db.Users, "Id", "Forename");
            PropertyCreateViewModel vm = new PropertyCreateViewModel();
            return View(vm);
        }

        // POST: Propertys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PropertyCreateViewModel vm)
        {
            if (ModelState.IsValid)

            {
                var property = new Property(vm);

                db.Propertys.Add(property);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StaffOwnerID = new SelectList(db.Users, "Id", "Forename", vm.StaffOwnerID);
            return View(vm);
        }

        // GET: Propertys/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Property property = db.Propertys.Find(id);
            if (property == null)
            {
                return HttpNotFound();
            }
            return View(property);
        }

        // POST: Propertys/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PropertyID,Title,Description,Type,Price")] Property property)
        {
            if (ModelState.IsValid)
            {
                db.Entry(property).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(property);
        }

        // GET: Propertys/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Property property = db.Propertys.Find(id);
            if (property == null)
            {
                return HttpNotFound();
            }
            return View(property);
        }

        // POST: Propertys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Property property = db.Propertys.Find(id);
            db.Propertys.Remove(property);
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

        public ActionResult MoreImages(PropertyCreateViewModel vm)
        {
            return PartialView("_ImageUpload", vm);
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

            items.Add(new SelectListItem { Text = "1 Mile", Value = "0" });
            items.Add(new SelectListItem { Text = "2 Miles", Value = "1" });
            items.Add(new SelectListItem { Text = "5 Miles", Value = "2" });
            items.Add(new SelectListItem { Text = "10 Miles", Value = "3" });
            items.Add(new SelectListItem { Text = "15 Miles", Value = "4" });
            items.Add(new SelectListItem { Text = "20 Miles", Value = "5" });
            items.Add(new SelectListItem { Text = "30 Miles", Value = "6", Selected = true });

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
            items.Add(new SelectListItem { Text = "1,000,000", Value = "1000000", Selected = true });

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
