using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ToanThangEdu.Models;
using PagedList;

namespace ToanThangEdu.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class NewsController : Controller
    {
        private ToanThangEducationDBEntities db = new ToanThangEducationDBEntities();

        // GET: Admin/News
        public ActionResult Index(int? page)
        {
            var news = db.News.Include(n => n.NewsCategory).OrderByDescending(x => x.NewsDate);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(news.ToList().ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/News/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // GET: Admin/News/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.NewsCategories, "Id", "Name");
            return View();
        }

        // POST: Admin/News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(News news, HttpPostedFileBase fileUpload)
        {
            Regex imageFilenameRegex = new Regex(@"(.*?)\.(jpg|jpeg|png|gif)$");
            if (fileUpload == null)
            {
                ViewBag.ThongBao = "Chưa có hình ảnh đính kèm";
                ViewBag.CategoryId = new SelectList(db.NewsCategories, "Id", "Name", news.CategoryId);
                return View();
            }
            if (!imageFilenameRegex.IsMatch(fileUpload.FileName.ToLower()))
            {
                ViewBag.ThongBao = "file phải có định dạng hình ảnh";
                ViewBag.CategoryId = new SelectList(db.NewsCategories, "Id", "Name", news.CategoryId);
                return View();
            }
            if (fileUpload != null && fileUpload.ContentLength > 0)
            {
                var fileName = Path.GetFileName(fileUpload.FileName.ToLower());
                if (fileName != null)
                {
                    DateTime currentDate = DateTime.Now;
                    var currentDateTick = currentDate.Ticks;
                    var path = Path.Combine(Server.MapPath("~/images/News"), currentDateTick + "_" + fileName);
                    var name = currentDateTick + "_" + fileName;
                    fileUpload.SaveAs(path);
                    news.ImageURL = name;
                }
               
            }
            if (ModelState.IsValid)
            {
                news.NewsDate = DateTime.Now;
                db.News.Add(news);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.NewsCategories, "Id", "Name", news.CategoryId);
            return View(news);
        }

        // GET: Admin/News/Edit/5
        public ActionResult Edit(long? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            Session["previousNews"] = news;
            if (news == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.NewsCategories, "Id", "Name", news.CategoryId);
            return View(news);
        }

        // POST: Admin/News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,Title,NewsContent,CategoryId,CreatorName")] News news, HttpPostedFileBase fileUpload)
        {
            Regex imageFilenameRegex = new Regex(@"(.*?)\.(jpg|jpeg|png|gif)$");
            var temp = (News) Session["previousNews"];
            news.ImageURL = temp.ImageURL;
            news.NewsDate = temp.NewsDate;
            if (fileUpload != null)
            {
                if (!imageFilenameRegex.IsMatch(fileUpload.FileName.ToLower()))
                {
                    ViewBag.ThongBao = "file phải có định dạng hình ảnh";
                    ViewBag.CategoryId = new SelectList(db.NewsCategories, "Id", "Name", news.CategoryId);
                    return View();
                }
                if (fileUpload.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName.ToLower());
                    if (fileName != null)
                    {
                        DateTime currentDate = DateTime.Now;
                        var currentDateTick = currentDate.Ticks;
                        var path = Path.Combine(Server.MapPath("~/images/News"), currentDateTick + "_" + fileName);
                        var name = currentDateTick + "_" + fileName;
                        fileUpload.SaveAs(path);
                        news.ImageURL = name;
                    }
                    
                }
            }
            
            if (ModelState.IsValid)
            {
                db.Entry(news).State = EntityState.Modified;
                db.SaveChanges();
                Session["previousNews"] = null;
                return RedirectToAction("Index");
            }
            Session["previousNews"] = null;
            ViewBag.CategoryId = new SelectList(db.NewsCategories, "Id", "Name", news.CategoryId);
            return View(news);
        }

        // GET: Admin/News/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: Admin/News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            News news = db.News.Find(id);
            db.News.Remove(news);
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
        public ActionResult Test()
        {
            var news = db.News.Include(n => n.NewsCategory).OrderBy(x=>x.NewsCategory.MenuId);            
            return View(news.ToList());
        }
    }
}