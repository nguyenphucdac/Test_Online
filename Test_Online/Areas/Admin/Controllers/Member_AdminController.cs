using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test_Online.Models;

namespace Test_Online.Areas.Admin.Controllers
{
    public class Member_AdminController : Controller
    {
        private Test_Online_DBEntities db = new Test_Online_DBEntities();
        public ActionResult Index()
        {
            var lstMember = db.Members;
            return View(lstMember);
        }
        public ActionResult Delete(int Id)
        {
            Member member = db.Members.SingleOrDefault(n => n.Id == Id);
            if (member == null)
            {
                return Content("<script>alert('Môn học không hợp lệ !!!')</script>");
            }
            db.Members.Remove(member);
            db.SaveChanges();
            return RedirectToAction("Index", "Member_Admin");
        }
    }
}