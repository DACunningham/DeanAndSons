using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeanAndSons.Controllers
{
    public class IMSController : Controller
    {
        // GET: IMS
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TeamViewer()
        {
            return View();
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
