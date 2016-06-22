using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ToanThangEdu.Models;
using PagedList;

namespace ToanThangEdu.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManagerStudentController : Controller
    {
        private ToanThangEducationDBEntities db = new ToanThangEducationDBEntities();

        // GET: Admin/RegisterStudiers
        public ActionResult Index(int? page)
        {
            var student = db.RegisterStudiers.ToList().OrderByDescending(x=>x.Id);
            int pageZise = 10;
            int pageNumber = (page ?? 1);
            return View(student.ToList().ToPagedList(pageNumber,pageZise));
        }

        // GET: Admin/RegisterStudiers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/RegisterStudiers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(string FullName, string PhoneNumber, string Email, string Address, int? Tuition, string TeacherLevel, string StudyContent, string MoreContent)
        {
            var registerStudier = new RegisterStudier(); 
            if (Email != "" && FullName != "" && PhoneNumber != "" && Address != "" && Tuition > 0 && TeacherLevel != "" && StudyContent != "" && MoreContent != "")
            {
                registerStudier.FullName = FullName;
                registerStudier.PhoneNumber = PhoneNumber;
                registerStudier.Email = Email;
                registerStudier.Address = Address;
                registerStudier.Tuition = Tuition;
                registerStudier.TeacherLevel = TeacherLevel;
                registerStudier.StudyContent = StudyContent;
                registerStudier.MoreContent = MoreContent;
                db.RegisterStudiers.Add(registerStudier);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: Admin/RegisterStudiers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegisterStudier registerStudier = db.RegisterStudiers.Find(id);
            if (registerStudier == null)
            {
                return HttpNotFound();
            }
            return View(registerStudier);
        }

        // POST: Admin/RegisterStudiers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit( int? Id, string FullName, string PhoneNumber, string Email, string Address, int? Tuition, string TeacherLevel, string StudyContent, string MoreContent )
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var intId = Convert.ToInt64(Id);
            var registerStudier = db.RegisterStudiers.FirstOrDefault(x => x.Id == intId);
            if (Email != "" && FullName != "" && PhoneNumber != "" && Address != "" && Tuition > 0 && TeacherLevel != "" && StudyContent != "" && MoreContent != "")
            {
                registerStudier.FullName = FullName;
                registerStudier.PhoneNumber = PhoneNumber;
                registerStudier.Email = Email;
                registerStudier.Address = Address;
                registerStudier.Tuition = Tuition;
                registerStudier.TeacherLevel = TeacherLevel;
                registerStudier.StudyContent = StudyContent;
                registerStudier.MoreContent = MoreContent; 
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(registerStudier);
        }

        // GET: Admin/RegisterStudiers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegisterStudier registerStudier = db.RegisterStudiers.Find(id);
            if (registerStudier == null)
            {
                return HttpNotFound();
            }
            return View(registerStudier);
        }

        // POST: Admin/RegisterStudiers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            RegisterStudier registerStudier = db.RegisterStudiers.Find(id);
            db.RegisterStudiers.Remove(registerStudier);
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
