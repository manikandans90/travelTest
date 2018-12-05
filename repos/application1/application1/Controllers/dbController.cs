using application1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace application1.Controllers
{
    public class dbController : Controller
    {
        // GET: db
        public ActionResult Index()
        {
            dummyEntities db = new dummyEntities();
            List<person> res = person.ToList();

            return View(res);
        }

        // GET: db/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: db/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: db/Create
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

        // GET: db/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: db/Edit/5
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

        // GET: db/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: db/Delete/5
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
