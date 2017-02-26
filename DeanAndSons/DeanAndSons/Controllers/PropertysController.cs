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
        public ActionResult Index(int? page, string searchString = null, string CategorySort = null, string OrderSort = null, string currentFilter = null)
        {
            // ********** Database Access **********

            var dbModel = new List<Property>();

            if (!String.IsNullOrWhiteSpace(searchString))
            {
                dbModel = db.Propertys.Include(p => p.Contact).Include(p => p.Images)
                    .Where(p => p.Title.Contains(searchString)).ToList();
            }
            else
            {
                dbModel = db.Propertys.Include(p => p.Contact).Include(p => p.Images).ToList();
            }

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
                    //Alters iavmList SQL to add order params to it, ditto for all of below.
                    dbModel = dbModel.OrderBy(a => a.Title).ToList();
                    break;
                case "01":
                    dbModel = dbModel.OrderByDescending(a => a.Title).ToList();
                    break;
                case "10":
                    dbModel = dbModel.OrderBy(a => a.Created).ToList();
                    break;
                case "11":
                    dbModel = dbModel.OrderByDescending(a => a.Created).ToList();
                    break;
                default:
                    dbModel = dbModel.OrderBy(a => a.Title).ToList();
                    break;
            }

            var indexList = new List<PropertyIndexViewModel>();
            foreach (var item in dbModel)
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

        // GET: Propertys/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Property property = db.Propertys.Include(i => i.Images).Include(c => c.Contact).Single(p => p.PropertyID == id);

            if (property == null)
            {
                return HttpNotFound();
            }

            //.Include(i => i.Images).Include(c => c.Contact)
            var vm = new PropertyDetailsViewModel(property);

            return View(vm);
        }

        // GET: Propertys/Create
        public ActionResult Create()
        {
            PropertyCreateViewModel vm = new PropertyCreateViewModel();

            return View(vm);
        }

        // POST: Propertys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "PropertyID,Title,Description,Type,Price,PropertyNo,Street,Town,PostCode,TelephoneNo,Email,Images")]*/ PropertyCreateViewModel vm)
        {
            if (ModelState.IsValid)

            {
                var property = new Property(vm);

                db.Propertys.Add(property);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

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
