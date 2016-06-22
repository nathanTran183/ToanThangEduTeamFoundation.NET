using Microsoft.AspNet.Identity.Owin;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ToanThangEdu.Models;

namespace ToanThangEdu.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ConfirmAccountController : Controller
    {
        private ToanThangEducationDBEntities db = new ToanThangEducationDBEntities();
        public ConfirmAccountController()
        {
        }

        public ConfirmAccountController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
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

        public ActionResult Index(int? page)
        {
            var users = db.AspNetUsers.OrderByDescending(x => x.UserName).Where(x => x.Activated != true && x.Id != "9c8297f6-6207-4f50-82d9-75cbbab46695");
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(users.ToList().ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.AspNetUsers.Where(x => x.Id == id).FirstOrDefault();
            user.Activated = true;
            db.SaveChanges();
            return RedirectToAction("Index", "Admin/ConfirmAccount");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                if (user.UserName == User.Identity.Name)
                {
                    Response.Redirect("/Admin/ConfirmAccount");
                    return null;
                }
                var result = await UserManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index","Admin/ConfirmAccount");
            }
            return View();
        }
    }
}