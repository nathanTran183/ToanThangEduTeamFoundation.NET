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
    public class SubjectSchedulesController : Controller
    {
        private ToanThangEducationDBEntities db = new ToanThangEducationDBEntities();

        // GET: Admin/SubjectSchedules
        public ActionResult Index()
        {
            //var list = new List<string>();
            //list.Add("Sáng"); list.Add("Chiều"); list.Add("Tối");
            //ViewBag.time = list;
            //var dayslist = new List<string>();
            //dayslist.Add("Thứ Hai"); dayslist.Add("Thứ Ba"); dayslist.Add("Thứ Tư"); dayslist.Add("Thứ Năm"); dayslist.Add("Thứ Sáu"); dayslist.Add("Thứ Bảy"); dayslist.Add("Chủ Nhật");
            //ViewBag.DaysOfWeek = dayslist;
            //ViewBag.weekdaysList = new List<string>();
            //var subjectSchedules = db.SubjectSchedules.Include(s => s.Class).Include(s => s.Period).Include(s => s.Subject).Include(s => s.Teacher).OrderBy(x => x.Period.DistanceOfTime);
            //return View(subjectSchedules.ToList());
            return View();
        }
        // GET: Admin/SubjectSchedules/Create
        public ActionResult Create()
        {
            
            ViewBag.ClassId = new SelectList(db.Classes, "Id", "ClassName");
            ViewBag.PeriodId = new SelectList(db.Periods, "Id", "Time");
            ViewBag.SubjectId = new SelectList(db.Subjects, "Id", "Subject1");
            ViewBag.TeacherId = new SelectList(db.Teachers, "Id", "Name");
            return View();
        }

        // POST: Admin/SubjectSchedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ScheduleName,PeriodId,TeacherId,ClassId,SubjectId")] SubjectSchedule subjectSchedule)
        {
            if (ModelState.IsValid)
            {
                db.SubjectSchedules.Add(subjectSchedule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClassId = new SelectList(db.Classes, "Id", "ClassName", subjectSchedule.ClassId);
            ViewBag.PeriodId = new SelectList(db.Periods, "Id", "Time", subjectSchedule.PeriodId);
            ViewBag.SubjectId = new SelectList(db.Subjects, "Id", "Subject1", subjectSchedule.SubjectId);
            ViewBag.TeacherId = new SelectList(db.Teachers, "Id", "Name", subjectSchedule.TeacherId);
            return View(subjectSchedule);
        }

        // GET: Admin/SubjectSchedules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubjectSchedule subjectSchedule = db.SubjectSchedules.Find(id);
            if (subjectSchedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassId = new SelectList(db.Classes, "Id", "ClassName", subjectSchedule.ClassId);
            ViewBag.PeriodId = new SelectList(db.Periods, "Id", "Time", subjectSchedule.PeriodId);
            ViewBag.SubjectId = new SelectList(db.Subjects, "Id", "Subject1", subjectSchedule.SubjectId);
            ViewBag.TeacherId = new SelectList(db.Teachers, "Id", "Name", subjectSchedule.TeacherId);
            return View(subjectSchedule);
        }

        // POST: Admin/SubjectSchedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ScheduleName,PeriodId,TeacherId,ClassId,SubjectId")] SubjectSchedule subjectSchedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subjectSchedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassId = new SelectList(db.Classes, "Id", "ClassName", subjectSchedule.ClassId);
            ViewBag.PeriodId = new SelectList(db.Periods, "Id", "Time", subjectSchedule.PeriodId);
            ViewBag.SubjectId = new SelectList(db.Subjects, "Id", "Subject1", subjectSchedule.SubjectId);
            ViewBag.TeacherId = new SelectList(db.Teachers, "Id", "Name", subjectSchedule.TeacherId);
            return View(subjectSchedule);
        }

        // GET: Admin/SubjectSchedules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubjectSchedule subjectSchedule = db.SubjectSchedules.Find(id);
            if (subjectSchedule == null)
            {
                return HttpNotFound();
            }
            return View(subjectSchedule);
        }

        // POST: Admin/SubjectSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubjectSchedule subjectSchedule = db.SubjectSchedules.Find(id);
            db.SubjectSchedules.Remove(subjectSchedule);
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
