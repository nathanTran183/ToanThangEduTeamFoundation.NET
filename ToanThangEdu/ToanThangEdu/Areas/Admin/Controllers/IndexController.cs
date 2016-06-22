using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToanThangEdu.Models;

namespace ToanThangEdu.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, Teacher")]
    public class IndexController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            ToanThangEducationDBEntities db = new ToanThangEducationDBEntities();
            ViewBag.numUsers = db.AspNetUsers.Where(x=>x.Activated == true).Count();
            ViewBag.numUsersConfirm = db.AspNetUsers.Where(x => x.Activated != true).Count();
            ViewBag.numRegisterStudiers = db.RegisterStudiers.Count();
            ViewBag.numNews = db.News.Count();
            ViewBag.numTeachers = db.Teachers.Count();
            ViewBag.nummRegisterTeacher = db.RegisterTeachers.Count();

            return View();
        }
        
    }
}