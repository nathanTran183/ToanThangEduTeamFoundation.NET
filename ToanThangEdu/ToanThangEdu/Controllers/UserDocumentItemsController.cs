using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ToanThangEdu.Models;

namespace ToanThangEdu.Controllers
{
    [Authorize(Roles = "Student, Admin, Teacher")]
    public class UserDocumentItemsController : Controller
    {
        private ToanThangEducationDBEntities db = new ToanThangEducationDBEntities();

        // GET: UserDocumentItems
        [AllowAnonymous]
        public ActionResult Index(int? page,int? id)
        {
            ViewData["item"] = db.DocumentsCategories.ToList();
            int pageSize = 10;
            int pageNumber = (page ?? 1);            
            if (id != null)
            {
                var documentItems1 = db.DocumentItems.Include(d => d.DocumentsCategory).Where(x=>x.CategoryId==id).ToList();
                ViewBag.Title = db.DocumentsCategories.Where(x=>x.Id==id).Select(xy=>xy.Name).FirstOrDefault().ToString() + " - Thể loại tài liệu ";
                return View(documentItems1.ToPagedList(pageNumber, pageSize));
            }
            var documentItems = db.DocumentItems.Include(d => d.DocumentsCategory).ToList();
            ViewBag.Title = "Tài Liệu - Trung tâm Toàn Thắng";
            return View(documentItems.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult GetPdf(string fileName)
        {
            var fileStream = new FileStream("~/images/Document/" + fileName,
                                             FileMode.Open,
                                             FileAccess.Read
                                           );
            var fsResult = new FileStreamResult(fileStream, "application/pdf");
            return fsResult;
        }

        [AllowAnonymous]
        public FileResult DisplayPDF(long? id)
        {
            DocumentItem documentItem = db.DocumentItems.Find(id);
            return File("~/images/Document/"+documentItem.DocumentURL, "application/pdf");
        }

        // GET: UserDocumentItems/Details/5
        public FileResult Download(long? id)
        {
            var document = db.DocumentItems.Find(id);
            string filepath = Server.MapPath("~/images/Document/" + document.DocumentURL);
            return File(filepath, "application/pdf", document.DocumentURL);
        }

        [AllowAnonymous]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentItem documentItem = db.DocumentItems.Find(id);
            ViewData["item"] = db.DocumentsCategories.ToList();            
            if (documentItem == null)
            {
                return HttpNotFound();
            }
            return View(documentItem);
        }

        // GET: UserDocumentItems/Create
       

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
