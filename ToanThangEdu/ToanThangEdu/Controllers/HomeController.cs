using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToanThangEdu.Models;

namespace ToanThangEdu.Controllers
{
    
    public class HomeController : Controller
    {
        ToanThangEducationDBEntities db = new ToanThangEducationDBEntities();
        public ActionResult Index()
        {
            if (Session["User_Name"]== null)
            {
                Session["User_Name"] = "";
                Session["User_Email"] = "";
            }
            var slide = db.News.Where(x=>x.NewsCategory.MenuId==2).OrderByDescending(x => x.NewsDate).Take(3).Select(x => new NewsCustomize {                
                id = x.Id,
                title = x.Title,
                newsContent = x.NewsContent,
                creator = x.CreatorName,
                image = x.ImageURL
             }).ToList();

            var newest = db.News.OrderByDescending(x=>x.NewsDate).First();

            ViewData["item"] = db.Teachers.OrderByDescending(x => x.Id).ToList();
            ViewData["news"] = db.News.OrderByDescending(x => x.NewsDate).Where(x=>x.NewsCategory.MenuId==2).Take(3).ToList();
            ViewData["about"] = db.News.OrderByDescending(x => x.NewsDate).Where(x=>x.NewsCategory.MenuId==1).Take(3).ToList();
            ViewData["itemDocuments"] = db.DocumentItems.Take(8).OrderByDescending(x=>x.Id).ToList();
            ViewBag.New = newest;
            ViewBag.Slide = slide;


            var path = Server.MapPath(("~/images/Gallary"));
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(path);
            int count = dir.GetFiles().Length;

            string[] fileNameUploadArr = new string[count];

            DirectoryInfo info = new DirectoryInfo(path);
            FileInfo[] files = info.GetFiles().OrderBy(x => x.CreationTime).Take(12).ToArray();
            int temp = 0;
            foreach (FileInfo file in files)
            {
                fileNameUploadArr[temp] = file.Name;
                temp++;
            }
            ViewBag.FileName = fileNameUploadArr;



            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}