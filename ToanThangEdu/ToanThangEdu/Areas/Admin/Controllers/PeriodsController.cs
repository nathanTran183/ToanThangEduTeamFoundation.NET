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
    public class ThoiKhoaBieuRow
    {
        public int Id;
        public string MônHọc;
        public string Thứ;
        public string Buổi;
        public string Thờigian;
    }
    public class ThoiGianCls
    {
        public int Id;
        public string Thứ;
        public string Buổi;
        public string Thờigian;
    }

    public class ThoiKhoaBieuMaster
    {
        public string MonHoc;
        public List<ThoiGianCls> listThoiGian;
    }
    [Authorize(Roles = "Admin")]
    public class PeriodsController : Controller
    {
        private ToanThangEducationDBEntities db = new ToanThangEducationDBEntities();

        // GET: Admin/Periods
        public ActionResult Index()
        {
            var ThoiKhoaBieu = new List<ThoiKhoaBieuRow>();
            foreach (var item in db.Periods)
            {

                var monHocs = (List<string>)item.Subjects.Split(',').ToList();
                var lstMonHoc = new List<string>();
                foreach (var itemChild in monHocs)
                {
                    ThoiKhoaBieuRow thoiKhoaBieu = new ThoiKhoaBieuRow();
                    thoiKhoaBieu.Id = item.Id;
                    thoiKhoaBieu.MônHọc = itemChild;
                    thoiKhoaBieu.Thứ = item.DayOfWeek;
                    thoiKhoaBieu.Buổi = item.Time;
                    thoiKhoaBieu.Thờigian = item.DistanceOfTime;
                    ThoiKhoaBieu.Add(thoiKhoaBieu);
                }

            }
            var ThoiKhoaBieuOrder = new List<ThoiKhoaBieuRow>();
            ThoiKhoaBieuOrder = ThoiKhoaBieu.OrderBy(x => x.MônHọc).ToList();

            var thoiKhoaBieuMaster = new List<ThoiKhoaBieuMaster>();
            ThoiKhoaBieuMaster thoiKhoaBieuMasterTmp = new ThoiKhoaBieuMaster();
            List<ThoiGianCls> listThoiGianClsTmp = new List<ThoiGianCls>();
            foreach (var item in ThoiKhoaBieuOrder)
            {
                List<ThoiGianCls> listThoiGianCls = new List<ThoiGianCls>();
                ThoiKhoaBieuMaster tkbMaster = new ThoiKhoaBieuMaster();

                ThoiGianCls thoiGianCls = new ThoiGianCls();
                thoiGianCls.Id = item.Id;
                thoiGianCls.Buổi = item.Buổi;
                thoiGianCls.Thờigian = item.Thờigian;
                thoiGianCls.Thứ = item.Thứ;

                tkbMaster.MonHoc = item.MônHọc;
                listThoiGianCls.Add(thoiGianCls);
                tkbMaster.listThoiGian = listThoiGianCls;
                if (thoiKhoaBieuMasterTmp.MonHoc == item.MônHọc)
                {
                    var temp = thoiKhoaBieuMaster.Find(x => x.MonHoc == item.MônHọc);
                    temp.listThoiGian.Add(thoiGianCls);
                }
                else
                {
                    thoiKhoaBieuMaster.Add(tkbMaster);
                }
                thoiKhoaBieuMasterTmp = tkbMaster;
            }
            ViewBag.ThoiKhoaBieuMaster = thoiKhoaBieuMaster;

            return View();
        }

        // GET: Admin/Periods/Create
        public ActionResult Create()
        {
            ViewBag.teacher = db.Teachers.ToList();
            return View();
        }

        // POST: Admin/Periods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Time,DistanceOfTime,DayOfWeek,Subjects")] Period period)
        {
            if (ModelState.IsValid)
            {
                db.Periods.Add(period);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(period);
        }

        // GET: Admin/Periods/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Period period = db.Periods.Find(id);
            if (period == null)
            {
                return HttpNotFound();
            }
            ViewBag.teacher = db.Teachers.ToList();
            return View(period);
        }

        // POST: Admin/Periods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Time,DistanceOfTime,DayOfWeek,Subjects")] Period period)
        {
            if (ModelState.IsValid)
            {
                db.Entry(period).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(period);
        }

        // GET: Admin/Periods/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Period period = db.Periods.Find(id);
            if (period == null)
            {
                return HttpNotFound();
            }
            return View(period);
        }

        // POST: Admin/Periods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Period period = db.Periods.Find(id);
            db.Periods.Remove(period);
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
