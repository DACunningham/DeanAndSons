using DeanAndSons.Models;
using DeanAndSons.Models.WAP.ViewModels;
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
        public ActionResult Index()
        {
            var dbModel = db.Propertys.Include(p => p.Contact).Include(p => p.Images).ToList();
            var indexList = new List<PropertyIndexViewModel>();

            foreach (var item in dbModel)
            {
                var vm = new PropertyIndexViewModel(item);

                indexList.Add(vm);
            }

            return View(indexList);
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
    }
}
