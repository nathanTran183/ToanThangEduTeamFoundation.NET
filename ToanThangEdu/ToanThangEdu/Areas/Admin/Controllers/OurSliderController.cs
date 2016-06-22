using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ToanThangEdu.Areas.Admin.Controllers
{
    public class OurSliderController : Controller
    {
        // GET: Admin/OurSlider
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var path = Server.MapPath(("~/images/Gallary"));
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(path);
            int count = dir.GetFiles().Length;

            string[] fileNameUploadArr = new string[count];
            
            DirectoryInfo info = new DirectoryInfo(path);
            FileInfo[] files = info.GetFiles().OrderByDescending(x => x.CreationTime).Take(44).ToArray();
            int temp = 0;
            foreach (FileInfo file in files)
            {
                fileNameUploadArr[temp] = file.Name;
                temp++;
            }
            ViewBag.FileName = fileNameUploadArr;
            
            return View();

        }



        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase fileUpload)
        {

            Regex imageFilenameRegex = new Regex(@"(.*?)\.(jpg|jpeg|png|gif)$");
            if (fileUpload == null)
            {
                ViewBag.ThongBao = "Chưa có hình ảnh đính kèm";
                return View();
            }
            if (!imageFilenameRegex.IsMatch(fileUpload.FileName.ToLower()))
            {
                ViewBag.ThongBao = "File phải có định dạng hình ảnh";
                return View();
            }
            if (fileUpload != null && fileUpload.ContentLength > 0)
            {
                var fileName = Path.GetFileName(fileUpload.FileName);
                if (fileName != null)
                {
                    DateTime currentDate = DateTime.Now;
                    var path = Path.Combine(Server.MapPath("~/images/Gallary"), currentDate.Ticks + "_" + fileName);

                    fileUpload.SaveAs(path);

                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string fileName)
        {
            if (fileName != null)
            {
                string fullPath = Request.MapPath("~/images/Gallary/" + fileName);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

            return RedirectToAction("Index", "OurSlider", new { area = "Admin" });
        }
    }
}