using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test_Online.Models;

namespace Test_Online.Areas.Admin.Controllers
{
    public class Role_AdminController : Controller
    {
        Test_Online_DBEntities db = new Test_Online_DBEntities();
        public ActionResult Index()
        {
            var lstRole = db.Roles;
            return View(lstRole);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Save(Role role)
        {
            if (role == null)
            {
                return Content("<script>alert('Quyền không hợp lệ !!!')</script>");
            }
            db.Roles.Add(role);
            db.SaveChanges();
            return RedirectToAction("Index", "Role_Admin");
        }

        public ActionResult Edit(int Id)
        {
            Role role = db.Roles.SingleOrDefault(n => n.Id == Id);
            if (role == null)
            {
                return Content("<script>alert('Quyền không hợp lệ !!!')</script>");
            }
            return View(role);
        }

        public ActionResult Update(Role role)
        {
            if (role == null)
            {
                return Content("<script>alert('Quyền không hợp lệ !!!')</script>");
            }
            db.Entry(role).State = System.Data.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "TypeMember_Admin");
        }

        public ActionResult Delete(int Id)
        {
            Role role = db.Roles.SingleOrDefault(n => n.Id == Id);
            if (role == null)
            {
                return Content("<script>alert('Quyền không hợp lệ !!!')</script>");
            }
            db.Roles.Remove(role);
            db.SaveChanges();
            return RedirectToAction("Index", "Role_Admin");
        }
    }
}