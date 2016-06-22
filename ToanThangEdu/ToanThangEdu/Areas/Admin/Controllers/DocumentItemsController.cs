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
    [Authorize(Roles = "Admin, Teacher")]
    public class DocumentItemsController : Controller
    {
        private ToanThangEducationDBEntities db = new ToanThangEducationDBEntities();

        // GET: Admin/DocumentItems
        public ActionResult Index(int? page)
        {
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            var documentItems = db.DocumentItems.Include(d => d.DocumentsCategory).ToList();
            return View(documentItems.ToPagedList(pageNumber, pageSize));
        }       
        public FileResult PDFDisplay(int? id)
        {            
            var document = db.DocumentItems.Find(id);
            string filepath = Server.MapPath("~/images/Document/"+ document.DocumentURL);            
            return File(filepath, "application/pdf",document.DocumentURL);
        }

        public FileResult DisplayPDF(long? id)
        {
            DocumentItem documentItem = db.DocumentItems.Find(id);
            return File("~/images/Document/" + documentItem.DocumentURL, "application/pdf");
        }
        // GET: Admin/DocumentItems/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentItem documentItem = db.DocumentItems.Find(id);
            if (documentItem == null)
            {
                return HttpNotFound();
            }
            return View(documentItem);
        }

        // GET: Admin/DocumentItems/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.DocumentsCategories, "Id", "Name");
            return View();
        }

        // POST: Admin/DocumentItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,DocumentContent,DocumentURL,CategoryId")] DocumentItem documentItem, HttpPostedFileBase fileUpload)
        {
            Regex imageFilenameRegex = new Regex(@"(.*?)\.(pdf|doc|docx|rar)$");
            //Regex imageFilenameRegex = new Regex(@"(.*?)\.(pdf)$");
            if (fileUpload == null)
            {
                ViewBag.ThongBao = "Chưa có file đính kèm";
                ViewBag.CategoryId = new SelectList(db.DocumentsCategories, "Id", "Name", documentItem.CategoryId);
                return View();
            }
            if (!imageFilenameRegex.IsMatch(fileUpload.FileName.ToLower()))
            {
                //ViewBag.ThongBao = "File tải lên phải có định dạng pdf.";
                ViewBag.ThongBao = "File tải lên phải có định dạng pdf,doc hoặc docx.";
                ViewBag.CategoryId = new SelectList(db.DocumentsCategories, "Id", "Name", documentItem.CategoryId);
                return View();
            }
            if (fileUpload.ContentLength > 0)
            {
                var fileName = Path.GetFileName(fileUpload.FileName.ToLower());
                if (fileName != null)
                {
                    DateTime currentDate = DateTime.Now;
                    var currentDateTick = currentDate.Ticks;             
                    var path = Path.Combine(Server.MapPath("~/images/Document"), currentDateTick + "_" + fileName);                    
                    fileUpload.SaveAs(path);
                    documentItem.DocumentURL = currentDateTick + "_" + fileName;
                }
                
            }
            if (ModelState.IsValid)
            {
                db.DocumentItems.Add(documentItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.DocumentsCategories, "Id", "Name", documentItem.CategoryId);
            return View(documentItem);
        }

        // GET: Admin/DocumentItems/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentItem documentItem = db.DocumentItems.Find(id);
            Session["document"] = documentItem;
            if (documentItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.DocumentsCategories, "Id", "Name", documentItem.CategoryId);
            return View(documentItem);
        }

        // POST: Admin/DocumentItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(long? Id, string Name, long? CategoryId, string DocumentContent, HttpPostedFileBase fileUpload)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DocumentItem documentItem = db.DocumentItems.Find(Id);

            //Regex imageFilenameRegex = new Regex(@"(.*?)\.(pdf)$");
            Regex imageFilenameRegex = new Regex(@"(.*?)\.(pdf|doc|docx)$");
            if (fileUpload == null)
            {
                ViewBag.ThongBao = "Chưa có file đính kèm";
                ViewBag.CategoryId = new SelectList(db.DocumentsCategories, "Id", "Name", documentItem.CategoryId);
                return View();
            }
            if (!imageFilenameRegex.IsMatch(fileUpload.FileName.ToLower()))
            {
                //ViewBag.ThongBao = "File tải lên phải có định dạng pdf.";
                ViewBag.ThongBao = "File tải lên phải có định dạng pdf, doc hoặc docx.";
                ViewBag.CategoryId = new SelectList(db.DocumentsCategories, "Id", "Name", documentItem.CategoryId);
                return View();
            }
            if (fileUpload.ContentLength > 0)
            {
                var fileName = Path.GetFileName(fileUpload.FileName.ToLower());
                if (fileName != null)
                {
                    DateTime currentDate = DateTime.Now;
                    var currentDateTick = currentDate.Ticks;
                    var path = Path.Combine(Server.MapPath("~/images/Document"), currentDateTick + "_" + fileName);
                    fileUpload.SaveAs(path);
                    documentItem.DocumentURL = currentDateTick + "_" + fileName;
                }
                
            }
            if (Name != null && DocumentContent != null)
            {
                documentItem.Name = Name;
                documentItem.DocumentContent = DocumentContent;
                documentItem.CategoryId = CategoryId;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            Session["document"] = null;
            ViewBag.CategoryId = new SelectList(db.DocumentsCategories, "Id", "Name", documentItem.CategoryId);
            return View(documentItem);
        }

        // GET: Admin/DocumentItems/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentItem documentItem = db.DocumentItems.Find(id);
            if (documentItem == null)
            {
                return HttpNotFound();
            }
            return View(documentItem);
        }

        // POST: Admin/DocumentItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            DocumentItem documentItem = db.DocumentItems.Find(id);
            db.DocumentItems.Remove(documentItem);
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
