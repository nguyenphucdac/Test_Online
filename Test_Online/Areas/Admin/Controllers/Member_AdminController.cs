using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test_Online.Models;

namespace Test_Online.Areas.Admin.Controllers
{
    [Authorize(Roles = "6")]
    public class Member_AdminController : Controller
    {
        private Test_Online_DBEntities db = new Test_Online_DBEntities();
        public ActionResult Index()
        {
            try
            {
                var lstMember = db.Members;
                return View(lstMember);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Member Controller method Index, Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
           
        }
        public ActionResult Edit(int Id)
        {
            try
            {
                Member member = db.Members.SingleOrDefault(n => n.Id == Id);
                if (member == null)
                {
                    return Content("<script>alert('Thành viên không hợp lệ !!!')</script>");
                }
                ViewBag.Type_Member_Id = new SelectList(db.Type_Member.OrderBy(n => n.Name), "Id", "Name", member.Type_Member_Id);
                return View(member);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Member Controller method Edit, Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }

        public ActionResult Update(Member member)
        {
            try
            {
                if (member == null)
                {
                    return Content("<script>alert('Thành viên không hợp lệ !!!')</script>");
                }
                db.Entry(member).State = System.Data.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Member_Admin");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Member Controller method update, Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }

        public ActionResult Delete(int Id)
        {
            try
            {
                Member member = db.Members.SingleOrDefault(n => n.Id == Id);
                if (member == null)
                {
                    return Content("<script>alert('Thành viên không hợp lệ !!!')</script>");
                }
                db.Members.Remove(member);
                db.SaveChanges();
                return RedirectToAction("Index", "Member_Admin");
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in Member Controller method Delete, Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
            
        }
    }
}