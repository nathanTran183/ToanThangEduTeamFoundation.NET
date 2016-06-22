using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ToanThangEdu.Areas.Admin.Models;
using ToanThangEdu.Models;
using PagedList;

namespace ToanThangEdu.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManagerTeacherController : Controller
    {
        ToanThangEducationDBEntities db = new ToanThangEducationDBEntities();
        public ManagerTeacherController()
        {
        }

        public ManagerTeacherController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        public List<TeacherCustomizeModel> listTeacherPublic = new List<TeacherCustomizeModel>();

        //
        // GET: /Users/
        public ActionResult Index(int? page)
        {
            if (listTeacherPublic.Count == 0)
            {
                var listTeacherDB = db.Teachers.ToList();
                var listTeacherRegisterDB = db.RegisterTeachers.ToList();
                //List<TeacherCustomizeModel> listTeacher = new List<TeacherCustomizeModel>();
                foreach (var item in listTeacherDB)
                {
                    var teacher = new TeacherCustomizeModel();
                    //teacher.Id = item.Id.ToString();
                    teacher.ImageURL = item.ImageURL;
                    teacher.TeacherType = item.TeacherType;
                    teacher.Description = item.Description;
                    teacher.Detail = item.Detail;
                    teacher.Name = item.Name;
                    listTeacherPublic.Add(teacher);
                }
                foreach (var teacherR in listTeacherRegisterDB)
                {
                    var teacher = new TeacherCustomizeModel();
                    teacher.Id = teacherR.Id;
                    teacher.Email = teacherR.Email;
                    teacher.FullName = teacherR.Fullname;
                    if (teacherR.Gender == 1)
                        teacher.Gender = "Name";
                    teacher.Gender = "Nữ";
                    teacher.Tuition = teacherR.Tuition;
                    teacher.YearOfBirth = teacherR.YearOfBirth;
                    teacher.MoreContent = teacherR.MoreContent;
                    teacher.PhoneNumber = teacherR.PhoneNumber;
                    teacher.TeachArea = teacherR.TeachArea;
                    teacher.TeachContent = teacherR.TeachContent;
                    listTeacherPublic.Add(teacher);
                }
                //listTeacherPublic = listTeacher;
            }

            //ViewBag.ListTeacher = listTeacherPublic;
            int pageZise = 10;
            int pageNumber = (page ?? 1);
            return View(listTeacherPublic.ToPagedList(pageNumber,pageZise));
        }

        public async Task<ActionResult> Create()
        {
            //Get the list of Roles
            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Create(HttpPostedFileBase fileUpload,string Name,string Detail, string Description, string TeacherType)
        {
            Regex imageFilenameRegex = new Regex(@"(.*?)\.(jpg|jpeg|png|gif)$");
            if (fileUpload == null)
            {
                ViewBag.ThongBao = "Input Image";
                return View();
            }

            if (Name != null || TeacherType != null || Detail != null || Description != null )
            {
                if (!imageFilenameRegex.IsMatch(fileUpload.FileName.ToLower()))
                {
                    ViewBag.ThongBao = "Input Image";
                    return View();
                }
                if (fileUpload != null && fileUpload.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName.ToLower());
                    if (fileName != null)
                    {
                        DateTime currentDate = DateTime.Now;
                        var currentDateTick = currentDate.Ticks;
                        var path = Path.Combine(Server.MapPath("~/images/Teacher"), currentDateTick + "_" + fileName);

                        fileUpload.SaveAs(path);

                        Teacher teacher = new Teacher();
                        teacher.Description = Description;
                        teacher.Detail = Detail;
                        teacher.Name = Name;
                        teacher.TeacherType = TeacherType;
                        teacher.ImageURL = currentDateTick + "_" + fileName;
                        db.Teachers.Add(teacher);
                        db.SaveChanges();
                    }
                   
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(string id)
        {
            TeacherCustomizeModel teacherCus = new TeacherCustomizeModel();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id.Split('-').Count() > 2)
            {
                //var registerTeacher = db.RegisterTeachers.FirstOrDefault(x => x.Id == id);
                //teacherCus.Id = registerTeacher.Id;
                //teacherCus.FullName = registerTeacher.Fullname;
                //teacherCus.PhoneNumber = registerTeacher.PhoneNumber;
                //teacherCus.TeachArea = registerTeacher.TeachArea;
                //teacherCus.TeachContent = registerTeacher.TeachContent;
                //teacherCus.TeachLevel = registerTeacher.TeachLevel;
                //teacherCus.Tuition = registerTeacher.Tuition;
                //teacherCus.YearOfBirth = registerTeacher.YearOfBirth;
                //teacherCus.MoreContent = registerTeacher.MoreContent;
                //if (registerTeacher.Gender == 1)
                //    teacherCus.Gender = "Name";
                //teacherCus.Gender = "Nữ";
                //teacherCus.Email = registerTeacher.Email;
                //ViewBag.IsRegisterTeacher = true;
                //ViewBag.Teacher = teacherCus;
            }
            else
            {
                var idConvert = Convert.ToInt64(id);
                var teacher = db.Teachers.FirstOrDefault(x => x.Id == idConvert);
                //teacherCus.Id = teacher.Id.ToString();
                teacherCus.Name = teacher.Name;
                teacherCus.ImageURL = teacher.ImageURL;
                teacherCus.TeacherType = teacher.TeacherType;
                teacherCus.Description = teacher.Description;
                teacherCus.Detail = teacher.Detail;
                ViewBag.IsRegisterTeacher = false;
                ViewBag.Teacher = teacherCus;
            }
            

            return View();
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(string Id, string MoreContent, string FullName, int? Gender , 
            string PhoneNumber,string TeachArea,string TeachContent,string TeachLevel,int? Tuition,int? YearOfBirth
            ,string Name, HttpPostedFileBase fileUpload, string TeacherType, string Description, string Detail)
        {
            if (Id.Split('-').Count() > 2)
            {
                //var registerTeacher = db.RegisterTeachers.FirstOrDefault(x => x.Id == Id);
                //registerTeacher.Fullname = FullName;
                //registerTeacher.MoreContent = MoreContent;
                //registerTeacher.Gender = Gender;
                //registerTeacher.PhoneNumber = PhoneNumber;
                //registerTeacher.TeachArea = TeachArea;
                //registerTeacher.TeachContent = TeachContent;
                //registerTeacher.TeachLevel = TeachLevel;
                //registerTeacher.Tuition = Tuition;
                //registerTeacher.YearOfBirth = YearOfBirth;
                //db.SaveChanges();
            }
            else
            {
                Regex imageFilenameRegex = new Regex(@"(.*?)\.(jpg|jpeg|png|gif)$");
                if (fileUpload == null)
                {
                    ViewBag.ThongBao = "Input Image";
                    return View();
                }

                if (Name != null || TeacherType != null || Detail != null || Description != null)
                {
                    if (!imageFilenameRegex.IsMatch(fileUpload.FileName.ToLower()))
                    {
                        ViewBag.ThongBao = "Input Image";
                        return View();
                    }
                    if (fileUpload != null && fileUpload.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(fileUpload.FileName.ToLower());
                        if (fileName != null)
                        {
                            DateTime currentDate = DateTime.Now;
                            var currentDateTick = currentDate.Ticks;
                            var path = Path.Combine(Server.MapPath("~/images/Teacher"), currentDateTick + "_" + fileName);

                            fileUpload.SaveAs(path);
                            var idConvert = Convert.ToInt64(Id);
                            var teacher = db.Teachers.FirstOrDefault(x => x.Id == idConvert);
                            teacher.Description = Description;
                            teacher.Detail = Detail;
                            teacher.Name = Name;
                            teacher.TeacherType = TeacherType;
                            teacher.ImageURL = currentDateTick + "_" + fileName;

                            db.SaveChanges();
                        }

                    }
                }
            }
            return RedirectToAction("Index");
        }


        //public async Task<ActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var user = await UserManager.FindByIdAsync(id);
        //    if (user == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(user);
        //}


        public ActionResult Delete(string id)
        {
            TeacherCustomizeModel teacherCus = new TeacherCustomizeModel();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id.Split('-').Count() > 2)
            {
                //var registerTeacher = db.RegisterTeachers.FirstOrDefault(x => x.Id == id);
                //teacherCus.Id = registerTeacher.Id;
                //teacherCus.FullName = registerTeacher.Fullname;
                //teacherCus.PhoneNumber = registerTeacher.PhoneNumber;
                //teacherCus.TeachArea = registerTeacher.TeachArea;
                //teacherCus.TeachContent = registerTeacher.TeachContent;
                //teacherCus.TeachLevel = registerTeacher.TeachLevel;
                //teacherCus.Tuition = registerTeacher.Tuition;
                //teacherCus.YearOfBirth = registerTeacher.YearOfBirth;
                //teacherCus.MoreContent = registerTeacher.MoreContent;
                //if (registerTeacher.Gender == 1)
                //    teacherCus.Gender = "Name";
                //teacherCus.Gender = "Nữ";
                //teacherCus.Email = registerTeacher.Email;
                //ViewBag.IsRegisterTeacher = true;
                //ViewBag.Teacher = teacherCus;
                
            }
            else
            {
                var idConvert = Convert.ToInt64(id);
                var teacher = db.Teachers.FirstOrDefault(x => x.Id == idConvert);
                //teacherCus.Id = teacher.Id.ToString();
                teacherCus.Name = teacher.Name;
                teacherCus.ImageURL = teacher.ImageURL;
                teacherCus.TeacherType = teacher.TeacherType;
                teacherCus.Description = teacher.Description;
                teacherCus.Detail = teacher.Detail;
                ViewBag.IsRegisterTeacher = false;
                ViewBag.Teacher = teacherCus;
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteTeacher(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            if (id.Split('-').Count() < 2)
            {
                var intID = Convert.ToInt32(id);
                Teacher registerStudier = db.Teachers.Find(intID);
                db.Teachers.Remove(registerStudier);
                db.SaveChanges();
            }
            else
            {
                var user = await UserManager.FindByIdAsync("946fb262-711e-42fc-969c-78fb039dfb98");
                if (user == null)
                {
                    //return HttpNotFound();
                    Response.Redirect("/Admin/ManagerTeacher");
                    return null;
                }
                if (user.UserName == User.Identity.Name)
                {
                    Response.Redirect("/Admin/ManagerTeacher");
                    return null;
                }
                var result = await UserManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
            }

            return RedirectToAction("Index");
        }


    }
}