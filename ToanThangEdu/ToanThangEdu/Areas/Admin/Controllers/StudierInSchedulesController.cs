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
    public class StudierInSchedulesController : Controller
    {
        private ToanThangEducationDBEntities db = new ToanThangEducationDBEntities();

        // GET: Admin/StudierInSchedules
        public ActionResult Index()
        {
            var studierInSchedules = db.StudierInSchedules.Include(s => s.SubjectSchedule);
            return View(studierInSchedules.ToList());
        }
        // GET: Admin/StudierInSchedules/Create
        public ActionResult Create()
        {
            ViewBag.SubjectScheduleId = new SelectList(db.SubjectSchedules, "Id", "ScheduleName");
            return View();
        }

        // POST: Admin/StudierInSchedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SubjectScheduleId,StudierId")] StudierInSchedule studierInSchedule)
        {
            if (ModelState.IsValid)
            {
                db.StudierInSchedules.Add(studierInSchedule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SubjectScheduleId = new SelectList(db.SubjectSchedules, "Id", "ScheduleName", studierInSchedule.SubjectScheduleId);
            return View(studierInSchedule);
        }

        // GET: Admin/StudierInSchedules/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudierInSchedule studierInSchedule = db.StudierInSchedules.Find(id);
            if (studierInSchedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubjectScheduleId = new SelectList(db.SubjectSchedules, "Id", "ScheduleName", studierInSchedule.SubjectScheduleId);
            return View(studierInSchedule);
        }

        // POST: Admin/StudierInSchedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SubjectScheduleId,StudierId")] StudierInSchedule studierInSchedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studierInSchedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SubjectScheduleId = new SelectList(db.SubjectSchedules, "Id", "ScheduleName", studierInSchedule.SubjectScheduleId);
            return View(studierInSchedule);
        }

        // GET: Admin/StudierInSchedules/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudierInSchedule studierInSchedule = db.StudierInSchedules.Find(id);
            if (studierInSchedule == null)
            {
                return HttpNotFound();
            }
            return View(studierInSchedule);
        }

        // POST: Admin/StudierInSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            StudierInSchedule studierInSchedule = db.StudierInSchedules.Find(id);
            db.StudierInSchedules.Remove(studierInSchedule);
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
