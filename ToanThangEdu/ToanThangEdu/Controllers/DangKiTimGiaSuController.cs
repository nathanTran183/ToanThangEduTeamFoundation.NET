using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToanThangEdu.Models;

namespace ToanThangEdu.Controllers
{
    public class DangKiTimGiaSuController : Controller
    {
        ToanThangEducationDBEntities db = new ToanThangEducationDBEntities();
        public ActionResult DangKiTimGiaSu()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKiTimGiaSu(string FullName, string Email, string PhoneNumber, int Tuition, string Address, string TeacherLevel, string StudyContent, int NumberOfSession, string MoreContent)
        {
            if (Email != "" && FullName!="" && PhoneNumber != null && Tuition > 0 && Address != "" && TeacherLevel != "" && TeacherLevel != null && StudyContent != null && NumberOfSession > 0 && MoreContent != null)
            {
                var registerStudentDB = new RegisterStudier();
                //TryUpdateModel(registerTeacherDB);
                registerStudentDB.Email = Email;
                registerStudentDB.FullName = FullName;
                registerStudentDB.PhoneNumber = PhoneNumber;
                registerStudentDB.TeacherLevel = TeacherLevel;
                registerStudentDB.StudyContent = StudyContent;
                registerStudentDB.Address = Address;
                registerStudentDB.Tuition = Tuition;
                registerStudentDB.NumberOfSession = NumberOfSession;
                registerStudentDB.MoreContent = MoreContent;
                db.RegisterStudiers.Add(registerStudentDB);
                db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            return View();
        }


        public ActionResult DangKiLamGiaSu()
        {
            return View();
        }


        [HttpPost]
        public ActionResult DangKiLamGiaSu(int Tuition, string Email, string TeachArea, int YearOfBirth, int Gender, string FullName, string PhoneNumber, string TeachContent, string MoreContent)
        {
            if (Email != "" && Tuition > 0 && FullName != "" && Gender >= 0 && MoreContent != "" && PhoneNumber != "" && TeachContent != "" && TeachArea != "" && YearOfBirth > 0)
            {
                RegisterTeacher registerTeach = new RegisterTeacher();
                registerTeach.Email = Email;
                registerTeach.Fullname = FullName;
                registerTeach.Gender = Gender;
                registerTeach.Tuition = Tuition;
                registerTeach.MoreContent = MoreContent;
                registerTeach.PhoneNumber = PhoneNumber;
                registerTeach.TeachArea = TeachArea;
                registerTeach.TeachContent = TeachContent;
                registerTeach.YearOfBirth = YearOfBirth;
                db.RegisterTeachers.Add(registerTeach);
                
                db.SaveChanges();
                
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}