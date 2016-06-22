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
    public class Menu
    {
        public Int64 Id;
        public string PaMenu;
        public string ChiMenu;
    }

    public class MenuChild
    {
        public Int64 Id;
        public string ChiMenu;
    }

    public class MenuInView
    {
        public Int64 Id;
        public string PaMenu;
        public List<MenuChild> ChiMenu;
    }
    [Authorize(Roles = "Admin")]
    public class DocumentsCategoriesController : Controller
    {
        private ToanThangEducationDBEntities db = new ToanThangEducationDBEntities();

        public List<Menu> GetListRoot()
        {
            var listRoot = new List<Menu>();
            foreach (var item in db.DocumentsCategories.OrderBy(x=>x.Id))
            {
                if (item.RootId == 0)
                {
                    Menu tmp = new Menu();
                    tmp.Id = item.Id;
                    tmp.PaMenu = item.Name;
                    listRoot.Add(tmp);
                }
            }
            Menu temp = new Menu();
            temp.Id = 0;
            temp.PaMenu = "Thể loại gốc";
            listRoot.Add(temp);
            return listRoot;
        }
        // GET: Admin/DocumentsCategories
        public ActionResult Index()
        {
            var listMenu = new List<Menu>();
            foreach (var item in db.DocumentsCategories.OrderBy(x=>x.Id))
            {
                var menuTmp = new Menu();
                menuTmp.Id = item.Id;
                if (item.RootId != 0)
                {
                    var tmp = (DocumentsCategory)db.DocumentsCategories.Where(x => x.Id == item.RootId).FirstOrDefault();
                    if (tmp == null)
                    {
                        menuTmp.PaMenu = "Thể loại gốc đã được xóa";
                    }
                    else
                    {
                        menuTmp.PaMenu = tmp.Name;
                    }                       
                    
                }
                else
                {
                    menuTmp.PaMenu = item.Name;
                }
                
                menuTmp.ChiMenu = item.Name;
                listMenu.Add(menuTmp);
            }
            ViewBag.Menu = listMenu;
            return View();
        }

        // GET: Admin/DocumentsCategories/Create
        public ActionResult Create()
        {
            ViewBag.RootList = GetListRoot();
            return View();
        }

        // POST: Admin/DocumentsCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string Name, int rootId)
        {
            if (Name != null)
            {
                DocumentsCategory documentsCategory = new DocumentsCategory();
                documentsCategory.Name = Name;
                documentsCategory.RootId = rootId;

                db.DocumentsCategories.Add(documentsCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View();
        }

        // GET: Admin/DocumentsCategories/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentsCategory documentsCategory = db.DocumentsCategories.Find(id);
            if (documentsCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.RootList = GetListRoot();
            return View(documentsCategory);
        }

        // POST: Admin/DocumentsCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int Id, int rootId , string Name)
        {
            if (Name != null)
            {
                var tmp = db.DocumentsCategories.Where(x => x.Id == Id).FirstOrDefault();
                tmp.Name = Name;
                tmp.RootId = rootId;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RootList = GetListRoot();
            return View();
        }

        // GET: Admin/DocumentsCategories/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentsCategory documentsCategory = db.DocumentsCategories.Find(id);
            if (documentsCategory == null)
            {
                return HttpNotFound();
            }
            return View(documentsCategory);
        }

        // POST: Admin/DocumentsCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            DocumentsCategory documentsCategory = db.DocumentsCategories.Find(id);
            db.DocumentsCategories.Remove(documentsCategory);
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
