using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ToanThangEdu.Models;

namespace ToanThangEdu.Controllers
{
    public class UserSubjectSchedulesController : Controller
    {
        private ToanThangEducationDBEntities db = new ToanThangEducationDBEntities();

        // GET: UserSubjectSchedules
        public ActionResult Index()
        {
            var list = new List<string>();
            list.Add("Sáng"); list.Add("Chiều"); list.Add("Tối");
            ViewBag.time = list;
            var dayslist = new List<string>();
            dayslist.Add("Thứ Hai"); dayslist.Add("Thứ Ba"); dayslist.Add("Thứ Tư"); dayslist.Add("Thứ Năm"); dayslist.Add("Thứ Sáu"); dayslist.Add("Thứ Bảy"); dayslist.Add("Chủ Nhật");
            ViewBag.DaysOfWeek = dayslist;
            ViewBag.weekdaysList = new List<string>();
            return View(db.Periods.ToList());
        }

        // GET: UserSubjectSchedules/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var studentList = db.StudierInSchedules.Where(x=>x.SubjectSchedule.ClassId==id);
        //    if (studentList == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(studentList);
        //}
        
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
