using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DegreeMgmt.Models.Entities;
using DegreeMgmt.Models;
using System.Collections.Generic;

namespace DegreeMgmt.Controllers
{
    public class PersonController : Controller
    {
        private EfDbContext dbContext = new EfDbContext();
        private int pageSize = 5;

        //
        // GET: /Person/

        public ActionResult Index(int page = 1)
        {
            if (page <= 0)
                page = 1;
            int maxPage = (int)Math.Ceiling(dbContext.Persons.Count() / (float)pageSize);
            if (page > maxPage)
                page = maxPage;

            PersonListViewModel model = new PersonListViewModel
            {
                Persons = dbContext.Persons.OrderBy(x => x.ID).Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                PagingInfo = new PagingInfo { 
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = dbContext.Persons.Count()
                }
            };
            return View(model);
        }

        //
        // GET: /Person/Details/5

        public ActionResult Details(int id = 0)
        {
            Person person = dbContext.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            
            person.Degrees.AddRange(FindDegreesForPerson(id));
            return View(person);
        }

#if false

        //
        // GET: /Person/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Person/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Person person)
        {
            if (ModelState.IsValid)
            {
                db.Persons.Add(person);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(person);
        }

#endif

        //
        // GET: /Person/Edit/5
        public ActionResult Edit(int id = 0)
        {
            // Person degree = dbContext.Persons.Find(id);
            Person person = dbContext.Persons.FirstOrDefault(x => x.ID == id);
            if (person == null)
            {
                return HttpNotFound();
            }

            person.Degrees.AddRange(FindDegreesForPerson(id));
            return View(person);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="degree"></param>
        /// <param name="image">仅当该参数名称为 image 时，图片才能上传成功</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Person person, HttpPostedFileBase image = null)
        {
            int year = person.Birthday.Year;
            if (year < 1900 || year > DateTime.Now.Year)
                ModelState.AddModelError("Birthday", "出生日期范围不正确");

            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    person.ImageMimeType = image.ContentType;
                    person.Portrait = new byte[image.ContentLength];
                    image.InputStream.Read(person.Portrait, 0, image.ContentLength);
                }

                // dbContext.Entry(degree).State = EntityState.Modified;
                // dbContext.SaveChanges();
                SavePerson(dbContext, person);
                TempData["message"] = string.Format("已保存{0}的信息", person.Name);
                return RedirectToAction("Index");
            }
            return View(person);
        }

        public ViewResult Create()
        {
            return View("Edit", new Person());
        }

        //
        // GET: /Person/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Person person = dbContext.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        //
        // POST: /Person/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Person person = dbContext.Persons.Find(id);
            if (null == person)
                return HttpNotFound();

            // delete one's associated degrees when deleting the individual
            var degrees = dbContext.Degrees.Where(x => x.HoldByWhom == person.ID);
            if (null != degrees && degrees.Count() > 0)
            {
                foreach (var item in degrees)
                    dbContext.Degrees.Remove(item);
            }

            dbContext.Persons.Remove(person);
            dbContext.SaveChanges();
            TempData["message"] = string.Format("已保存{0}的信息", person.Name);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            dbContext.Dispose();
            base.Dispose(disposing);
        }

        public FileContentResult GetImage(int id)
        {
            Person person = dbContext.Persons.FirstOrDefault(p => p.ID == id);
            if (person != null)
            {
                return File(person.Portrait, person.ImageMimeType);
            }
            else
            {
                return null;
            }
        }

        #region Helper Methods

        private void SavePerson(EfDbContext dbContext, Person person)
        {
            if (null == person)
                return;

            Person dbEntry = dbContext.Persons.Find(person.ID);
            if (null == dbEntry)
            {
                dbContext.Persons.Add(person);
            }
            else
            {
                dbEntry.Name = person.Name;
                dbEntry.Gender = person.Gender;
                dbEntry.Birthday = person.Birthday;
                dbEntry.IDNumber = person.IDNumber;
                dbEntry.Birthplace = person.Birthplace;
                dbEntry.MobilePhone = person.MobilePhone;
                dbEntry.Email = person.Email;
                if (null != person.Portrait)
                {
                    dbEntry.Portrait = person.Portrait;
                    dbEntry.ImageMimeType = person.ImageMimeType;
                }
            }
            dbContext.SaveChanges();
        }

        private List<Degree> FindDegreesForPerson(int personId)
        {
            return dbContext.Degrees.Where(x => x.HoldByWhom == personId).ToList();
        }

        #endregion
    }
}