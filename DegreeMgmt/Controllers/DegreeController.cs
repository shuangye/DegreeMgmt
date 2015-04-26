using System;
using System.Web.Mvc;
using DegreeMgmt.Models.Entities;
using DegreeMgmt.Models;

namespace DegreeMgmt.Controllers
{
    public class DegreeController : Controller
    {
        private EfDbContext dbContext = new EfDbContext();
     
        public ActionResult Details(int id = 0)
        {
            Degree degree = dbContext.Degrees.Find(id);
            if (degree == null)
            {
                return HttpNotFound();
            }
            return View(degree);
        }

        public ViewResult Create(int personId, String returnUrl)
        {
            ViewBag.PersonId = personId;
            ViewBag.ReturnUrl = returnUrl;            
            return View("Edit", new Degree());
        }


        public ActionResult Edit(int id = 0, int personId = 0, String returnUrl = null)
        {
            ViewBag.PersonId = personId;
            ViewBag.ReturnUrl = returnUrl;  
            Degree degree = dbContext.Degrees.Find(id);
            if (degree == null)
            {
                return HttpNotFound();
            }
            return View(degree);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Degree degree, int personId, String returnUrl)
        {
            ViewBag.PersonId = personId;
            ViewBag.ReturnUrl = returnUrl; 

            int year = degree.AcquiredDate.Year;
            if (year < 1900 || year > DateTime.Now.Year)
                ModelState.AddModelError("AcquiredDate", "获得学位的日期范围不正确");

            if (ModelState.IsValid)
            {
                // dbContext.Entry(degree).State = EntityState.Modified;
                // dbContext.SaveChanges();
                degree.HoldByWhom = personId;
                SaveDegree(dbContext, degree);                
                TempData["message"] = "学位信息已保存";

                if (!String.IsNullOrWhiteSpace(returnUrl))
                    return Redirect(returnUrl);
            }
            
            return View(degree);
        }

      
        public ActionResult Delete(int id = 0, int personId = 0, String returnUrl = null)
        {
            ViewBag.PersonId = personId;
            ViewBag.ReturnUrl = returnUrl;  
            Degree degree = dbContext.Degrees.Find(id);
            if (degree == null)
            {
                return HttpNotFound();
            }
            return View(degree);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int personId = 0, String returnUrl = null)
        {
            Degree degree = dbContext.Degrees.Find(id);
            dbContext.Degrees.Remove(degree);
            dbContext.SaveChanges();
            TempData["message"] = "学位已删除";

            if (!String.IsNullOrWhiteSpace(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Person");
        }

        protected override void Dispose(bool disposing)
        {
            dbContext.Dispose();
            base.Dispose(disposing);
        }

        #region Helper Methods

        private void SaveDegree(EfDbContext dbContext, Degree degree)
        {
            if (null == degree)
                return;

            Degree dbEntry = dbContext.Degrees.Find(degree.ID);
            if (null == dbEntry)
            {
                dbContext.Degrees.Add(degree);
            }
            else
            {
                dbEntry.HoldByWhom = degree.HoldByWhom;
                dbEntry.School = degree.School;
                dbEntry.Academy = degree.Academy;
                dbEntry.Major = degree.Major;
                dbEntry.Edu = degree.Edu;
                dbEntry.Class = degree.Class;
                dbEntry.AcquiredDate = degree.AcquiredDate;
            }
            dbContext.SaveChanges();
        }
        
        #endregion
    }
}