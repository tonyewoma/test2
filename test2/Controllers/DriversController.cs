using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using test2.Models;
using System.IO;
using test2.Migrations;

namespace farmgate.Controllers
{
    public class DriversController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Drivers
        public ActionResult Index()
        {
            return View(db.Drivers.ToList());
        }

        // GET: Drivers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Driver drivers = db.Drivers.Find(id);
            if (drivers == null)
            {
                return HttpNotFound();
            }
            return View(drivers);
        }

        // GET: Drivers/Create

        public ActionResult Create()
        {
            return View();
        }

        // POST: Drivers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,Route,PhoneNumber,ImageUrl")] Driver drivers, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                string imageLocation = "";
                if ((file == null || file.ContentLength < 1))
                {
                    ViewBag.Msg = "Please select an image";
                    return View();
                }
                if (!SaveImg(file, out imageLocation))
                {
                    ViewBag.Msg = "An error occured while saving the image";
                    return View();
                }

                drivers.ImageUrl = imageLocation;
                db.Drivers.Add(drivers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(drivers);
        }

        public bool SaveImg(HttpPostedFileBase file, out string imageLocation)
        {
            imageLocation = "";
            string serverPath = Server.MapPath("~/Images");
            if ((file == null || file.ContentLength < 1))
            {
                //throw an exception showing that no file is present
            }

            var imageString = file.ToString();
            var allowedExtensions = new[]
            {
            ".jpg", ".png", ".jpg", ".jpeg"
             };

            var fileName = Path.GetFileName(file.FileName); //eg myImage.jpg
            var extension = Path.GetExtension(file.FileName);    //eg .jpg

            if (allowedExtensions.Contains(extension.ToLower()))
            {
                string ordinaryFileName = Path.GetFileNameWithoutExtension(file.FileName);
                string myFile = ordinaryFileName + "_" + Guid.NewGuid() + extension;
                var path = Path.Combine(serverPath, myFile);
                file.SaveAs(path);

                string relativePath = "~/Images/" + myFile;
                imageLocation = relativePath;
                return true;
                //return a success message here
            }
            else
            {
                //file save error
                return false;
            }
        }

        // GET: Drivers/Edit/5

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Driver drivers = db.Drivers.Find(id);
            if (drivers == null)
            {
                return HttpNotFound();
            }
            return View(drivers);
        }

        // POST: Drivers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,Route,PhoneNumber,ImageUrl")] Drivers drivers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(drivers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(drivers);
        }

        // GET: Drivers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Driver drivers = db.Drivers.Find(id);
            if (drivers == null)
            {
                return HttpNotFound();
            }
            return View(drivers);
        }

        // POST: Drivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Driver drivers = db.Drivers.Find(id);
            db.Drivers.Remove(drivers);
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


    }
}
