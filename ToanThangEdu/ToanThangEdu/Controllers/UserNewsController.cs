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

namespace ToanThangEdu.Controllers
{
    public class UserNewsController : Controller
    {
        private ToanThangEducationDBEntities db = new ToanThangEducationDBEntities();

        // GET: UserNews
        public ActionResult Index(long? id, int? page)
        {
            ViewData["item"] = db.Teachers.OrderByDescending(x => x.Id).ToList();
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            if (id != null)
            {
                var newsCat = db.News.Include(n => n.NewsCategory).Where(x => x.CategoryId == id).OrderByDescending(x => x.NewsDate);
                ViewBag.Title = db.NewsCategories.Where(x => x.Id == id).Select(y => y.Name).FirstOrDefault().ToString() + " - Trung tâm Toàn Thắng"; ;
                return View(newsCat.ToList().ToPagedList(pageNumber, pageSize));
            }
            var news = db.News.Include(n => n.NewsCategory).Where(x=>x.NewsCategory.MenuId==2).OrderByDescending(x => x.NewsDate);
            ViewBag.Title = "Tin Tức - Trung tâm Toàn Thắng";
            return View(news.ToList().ToPagedList(pageNumber, pageSize));
        }
        public ActionResult About(long? id, int? page)
        {
            ViewData["item"] = db.Teachers.OrderByDescending(x => x.Id).ToList();
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            if (id != null)
            {
                var newsCat = db.News.Include(n => n.NewsCategory).Where(x => x.CategoryId == id).OrderByDescending(x => x.NewsDate);
                ViewBag.Title = db.NewsCategories.Where(x => x.Id == id).Select(y => y.Name).FirstOrDefault().ToString() + " - Trung tâm Toàn Thắng"; ;
                return View(newsCat.ToList().ToPagedList(pageNumber, pageSize));
            }
            var news = db.News.Include(n => n.NewsCategory).Where(x => x.NewsCategory.MenuId == 1).OrderByDescending(x => x.NewsDate);
            ViewBag.Title = "Giới Thiệu - Trung tâm Toàn Thắng";
            return View(news.ToList().ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Program(long? id, int? page)
        {
            ViewData["item"] = db.Teachers.OrderByDescending(x => x.Id).ToList();
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            if (id != null)
            {
                var newsCat = db.News.Include(n => n.NewsCategory).Where(x => x.CategoryId == id).OrderByDescending(x => x.NewsDate);
                ViewBag.Title = db.NewsCategories.Where(x => x.Id == id).Select(y => y.Name).FirstOrDefault().ToString() + " - Trung tâm Toàn Thắng";
                return View(newsCat.ToList().ToPagedList(pageNumber, pageSize));
            }
            var news = db.News.Include(n => n.NewsCategory).Where(x => x.NewsCategory.MenuId == 3).OrderByDescending(x => x.NewsDate);
            ViewBag.Title = "Đào Tạo - Trung tâm Toàn Thắng";
            return View(news.ToList().ToPagedList(pageNumber, pageSize));
        }

        // GET: UserNews/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            ViewData["item"] = db.News.Include(n => n.NewsCategory).OrderByDescending(x => x.NewsDate).Take(5).ToList();
            //ViewData["item"] = news;
            //var list = db.News.OrderByDescending(x => x.NewsDate).Take(5).ToList();
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
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
