using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ToanThangEdu.Models;

namespace ToanThangEdu.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RegisterTeachersController : Controller
    {
        private ToanThangEducationDBEntities db = new ToanThangEducationDBEntities();

        // GET: Admin/RegisterTeachers
        public ActionResult Index(int? page)
        {
            var teachers = db.RegisterTeachers.OrderByDescending(x => x.Id);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(teachers.ToList().ToPagedList(pageNumber, pageSize));
        }
        // GET: Admin/RegisterTeachers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/RegisterTeachers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int Tuition, string Email, string TeachArea, int YearOfBirth, int Gender, string FullName, string PhoneNumber, string TeachContent, string MoreContent)
        {
            if (Email != "" && Tuition > 0 && FullName != "" && Gender >= 0 && MoreContent != "" && PhoneNumber != "" && TeachContent != "" && TeachArea != "" && YearOfBirth > 0)
            {
                RegisterTeacher registerTeach = new RegisterTeacher();
                registerTeach.Email = Email;
                registerTeach.Fullname = FullName;
                registerTeach.Gender = Gender;
                registerTeach.Tuition = Tuition;
                registerTeach.MoreContent = MoreContent;
                registerTeach.PhoneNumber = PhoneNumber;
                registerTeach.TeachArea = TeachArea;
                registerTeach.TeachContent = TeachContent;
                registerTeach.YearOfBirth = YearOfBirth;
                db.RegisterTeachers.Add(registerTeach);

                db.SaveChanges();

                return RedirectToAction("Index", "RegisterTeachers");
            }

            return View();
        }

        // GET: Admin/RegisterTeachers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegisterTeacher registerTeacher = db.RegisterTeachers.Find(id);
            if (registerTeacher == null)
            {
                return HttpNotFound();
            }

            return View(registerTeacher);
        }

        // POST: Admin/RegisterTeachers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? Id, string MoreContent, string FullName, int? Gender,
            string PhoneNumber, string TeachArea, string TeachContent, int? Tuition, int? YearOfBirth, string Email)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var intId = Convert.ToInt64(Id);
            var registerTeacher = db.RegisterTeachers.FirstOrDefault(x => x.Id == intId);
            if (Email != "" && Tuition > 0 && FullName != "" && Gender >= 0 && MoreContent != "" && PhoneNumber != "" && TeachContent != "" && TeachArea != "" && YearOfBirth > 0)
            {
                
                registerTeacher.Fullname = FullName;
                registerTeacher.MoreContent = MoreContent;
                registerTeacher.Gender = Gender;
                registerTeacher.PhoneNumber = PhoneNumber;
                registerTeacher.TeachArea = TeachArea;
                registerTeacher.TeachContent = TeachContent;
                registerTeacher.Tuition = Tuition;
                registerTeacher.YearOfBirth = YearOfBirth;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(registerTeacher);
        }

        // GET: Admin/RegisterTeachers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegisterTeacher registerTeacher = db.RegisterTeachers.Find(id);
            if (registerTeacher == null)
            {
                return HttpNotFound();
            }
            return View(registerTeacher);
        }

        // POST: Admin/RegisterTeachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            RegisterTeacher registerTeacher = db.RegisterTeachers.Find(id);
            db.RegisterTeachers.Remove(registerTeacher);
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
