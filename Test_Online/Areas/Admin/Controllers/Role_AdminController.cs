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
            try
            {
                var lstRole = db.Roles;
                return View(lstRole);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Role/Index Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Save(Role role)
        {
            try
            {
                if (role == null)
                {
                    return Content("<script>alert('Quyền không hợp lệ !!!')</script>");
                }
                db.Roles.Add(role);
                db.SaveChanges();
                return RedirectToAction("Index", "Role_Admin");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Role/Save Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }

        public ActionResult Edit(int Id)
        {
            try
            {
                Role role = db.Roles.SingleOrDefault(n => n.Id == Id);
                if (role == null)
                {
                    return Content("<script>alert('Quyền không hợp lệ !!!')</script>");
                }
                return View(role);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Role/Edit Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }

        public ActionResult Update(Role role)
        {
            try
            {
                if (role == null)
                {
                    return Content("<script>alert('Quyền không hợp lệ !!!')</script>");
                }
                db.Entry(role).State = System.Data.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Role_Admin");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Role/Update Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }

        public ActionResult Delete(int Id)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine("Role/Delete Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
    }
}