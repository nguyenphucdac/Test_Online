using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Test_Online.Models;

namespace Test_Online.Areas.Admin.Controllers
{
    [Authorize(Roles = "6")]
    public class TypeMember_AdminController : Controller
    {
        private Test_Online_DBEntities db = new Test_Online_DBEntities();

        public ActionResult Index()
        {
            try
            {
                var lstTypeMember = db.Type_Member;
                return View(lstTypeMember);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Save(Type_Member typeMember)
        {
            try
            {
                if (typeMember == null)
                {
                    return Content("<script>alert('Loại thành viên không hợp lệ !!!')</script>");
                }
                db.Type_Member.Add(typeMember);
                db.SaveChanges();
                return RedirectToAction("Index", "TypeMember_Admin");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }

        public ActionResult Edit(int Id)
        {
            try
            {
                Type_Member typeMember = db.Type_Member.SingleOrDefault(n => n.Id == Id);
                if (typeMember == null)
                {
                    return Content("<script>alert('Loại thành viên không hợp lệ !!!')</script>");
                }
                return View(typeMember);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }

        public ActionResult Update(Type_Member typeMember)
        {
            try
            {
                if (typeMember == null)
                {
                    return Content("<script>alert('Loại thành viên không hợp lệ !!!')</script>");
                }
                db.Entry(typeMember).State = System.Data.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "TypeMember_Admin");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }

        public ActionResult Delete(int Id)
        {
            try
            {
                Type_Member typeMember = db.Type_Member.SingleOrDefault(n => n.Id == Id);
                if (typeMember == null)
                {
                    return Content("<script>alert('Loại thành viên không hợp lệ !!!')</script>");
                }
                db.Type_Member.Remove(typeMember);
                db.SaveChanges();
                return RedirectToAction("Index", "TypeMember_Admin");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
        public ActionResult ChooseRole(int id)
        {
            try
            {
                Type_Member typeMember = db.Type_Member.SingleOrDefault(n => n.Id == id);
                if(typeMember == null)
                {
                    return Content("<script>alert('Loại thành viên không hợp lệ !!!')</script>");
                }
                ViewBag.Role = db.Roles;
                ViewBag.RoleMember = db.Role_Member.Where(n => n.Type_Member_Id == id);
                return View(typeMember);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
        public ActionResult AcceptRole(int idTypeMember, IEnumerable<Role_Member> lstRoleMember)
        {
            try
            {
                var lstRoleOld = db.Role_Member.Where(n => n.Type_Member_Id == idTypeMember);

                if (lstRoleOld != null)
                {
                    foreach (var item in lstRoleOld)
                    {
                        db.Role_Member.Remove(item);
                    }
                }
                if (lstRoleMember != null)
                {
                    foreach (var item in lstRoleMember)
                    {
                        item.Type_Member_Id = idTypeMember;
                        db.Role_Member.Add(item);
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index", "TypeMember_Admin");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
    }
}