using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WendtEquipmentTracking.App.Controllers
{
    public class BillOfLandingController : Controller
    {
        // GET: BillOfLanding
        public ActionResult Index()
        {
            return View();
        }

        // GET: BillOfLanding/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BillOfLanding/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BillOfLanding/Create
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

        // GET: BillOfLanding/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BillOfLanding/Edit/5
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

        // GET: BillOfLanding/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BillOfLanding/Delete/5
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
