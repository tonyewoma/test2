using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using test2.Models;

namespace test2.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        //public ActionResult Index()
        //{
        //    return View();
        //}


        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Products
        //public ViewResult Index(string sortOrder, string searchString)
        //{
        //    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        //    ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
        //    var Products = from s in db.Products
        //                   select s;
        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        Products = Products.Where(s => s.Name.Contains(searchString) 
        //                               || s.Description.Contains(searchString));
        //    }
        //    switch (sortOrder)
        //    {
        //        case "name_desc":
        //            Products = Products.OrderByDescending(s => s.Name);
        //            break;
        //        case "Date":
        //            Products = Products.OrderBy(s => s.Description);
        //            break;
        //        case "date_desc":
        //            Products = Products.OrderByDescending(s => s.Description);
        //            break;
        //        default:
        //            Products = Products.OrderBy(s => s.Name);
        //            break;
        //    }

        //    return View(Products.ToList());
        //}


        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Description,Price,PhoneNumber,ImageUrl")] Product product, HttpPostedFileBase file)
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

                product.ImageUrl = imageLocation;
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
            //if (ModelState.IsValid)
            //{
            //    db.Products.Add(product);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //return View(product);
        }

        // GET: Products/Edit/5

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
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Description,price,PhoneNumber,ImageUrl")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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