﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaVentaMusical.Controllers
{
    public class AlbumesController : Controller
    {
        // GET: Albumes
        public ActionResult Index()
        {
            return View();
        }

        // GET: Albumes/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Albumes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Albumes/Create
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

        // GET: Albumes/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Albumes/Edit/5
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

        // GET: Albumes/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Albumes/Delete/5
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
