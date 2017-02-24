using DeanAndSons.Models;
using DeanAndSons.Models.IMS.ViewModels;
using Microsoft.AspNet.Identity;
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
            //var a = db.Users.OfType<Staff>().Where(s => s.SuperiorID == null);

            //Get logged in user ID then use this ID to get ApplicationUser instance
            var usrID = User.Identity.GetUserId();
            var appUser = db.Users.Where(usr => usr.Id == usrID).OfType<Staff>()
                .Include(s => s.Subordinates).Include(su => su.Superior).Single();

            var vm = new HierarchyIndexViewModel(appUser);

            return View(vm);
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
