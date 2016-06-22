using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using ToanThangEdu.Models;

namespace ToanThangEdu.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TeachersController : Controller
    {
        private ToanThangEducationDBEntities db = new ToanThangEducationDBEntities();

        // GET: Admin/Teachers
        public ActionResult Index(int? page)
        {
            var teachers = db.Teachers.OrderByDescending(x => x.Id);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(teachers.ToList().ToPagedList(pageNumber, pageSize));
        }
        
        // GET: Admin/Teachers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Teachers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase fileUpload, string Name, string Detail, string Description, string TeacherType)
        {
            Regex imageFilenameRegex = new Regex(@"(.*?)\.(jpg|jpeg|png|gif)$");
            if (fileUpload == null)
            {
                ViewBag.ThongBao = "Chưa có hình ảnh đính kèm";
                return View();
            }

            if (Name != null || TeacherType != null || Detail != null || Description != null)
            {
                if (!imageFilenameRegex.IsMatch(fileUpload.FileName.ToLower()))
                {
                    ViewBag.ThongBao = "Chưa có hình ảnh đính kèm";
                    return View();
                }
                if (fileUpload != null && fileUpload.ContentLength > 0)
                {
                    DateTime currentDate = DateTime.Now;
                    var fileName = currentDate.Ticks + "_" + Path.GetFileName(fileUpload.FileName);
                    if (fileName != null)
                    {

                        var path = Path.Combine(Server.MapPath("~/images/Teacher"), fileName);
                        fileUpload.SaveAs(path);

                        Teacher teacher = new Teacher();
                        teacher.Description = Description;
                        teacher.Detail = Detail;
                        teacher.Name = Name;
                        teacher.TeacherType = TeacherType;
                        teacher.ImageURL = fileName;
                        db.Teachers.Add(teacher);
                        db.SaveChanges();
                    }

                }
            }

            return RedirectToAction("Index");
        }

        // GET: Admin/Teachers/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Find(id);
            Session["previousTeacher"] = teacher;
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // POST: Admin/Teachers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int? Id, string Name, HttpPostedFileBase fileUpload, string TeacherType, string Description, string Detail)
        {
            Teacher teacher = db.Teachers.Find(Id);
            var temp = (Teacher)Session["previousTeacher"];
            teacher.ImageURL = temp.ImageURL;
            Regex imageFilenameRegex = new Regex(@"(.*?)\.(jpg|jpeg|png|gif)$");
            if (fileUpload != null)
            {
                if (!imageFilenameRegex.IsMatch(fileUpload.FileName.ToLower()))
                {
                    ViewBag.ThongBao = "Chưa có hình ảnh đính kèm";
                    return View(teacher);
                }
                if (fileUpload.ContentLength > 0)
                {
                    DateTime currentDate = DateTime.Now;
                    var fileName = currentDate.Ticks + "_" + Path.GetFileName(fileUpload.FileName);
                    if (fileName != null)
                    {

                        var path = Path.Combine(Server.MapPath("~/images/Teacher"), fileName);
                        fileUpload.SaveAs(path);
                        teacher.ImageURL = fileName;
                    }
                }
            }
           
            if (Name != null && TeacherType != null && Detail != null && Description != null)
            {
                        teacher.Description = Description;
                        teacher.Detail = Detail;
                        teacher.Name = Name;
                        teacher.TeacherType = TeacherType;                       

                        db.SaveChanges();
                        return RedirectToAction("Index");
                   
            }
                return View(teacher);
        }

        // GET: Admin/Teachers/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // POST: Admin/Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Teacher teacher = db.Teachers.Find(id);
            db.Teachers.Remove(teacher);
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
