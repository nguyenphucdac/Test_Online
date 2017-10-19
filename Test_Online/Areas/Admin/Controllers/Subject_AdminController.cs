using System;
using System.Linq;
using System.Web.Mvc;
using Test_Online.Models;

namespace Test_Online.Areas.Admin.Controllers
{
    public class Subject_AdminController : Controller
    {
        Test_Online_DBEntities db = new Test_Online_DBEntities();
        public ActionResult Index()
        {
            try
            {
                var lstSubject = db.Subjects;
                return View(lstSubject);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Subject/ Index Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }

        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Save(Subject subject)
        {
            try
            {
                if (subject == null)
                {
                    return Content("<script>alert('Môn học không hợp lệ !!!')</script>");
                }
                db.Subjects.Add(subject);
                db.SaveChanges();
                return RedirectToAction("Index", "Subject_Admin");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Subject/Save Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
            
        }

        public ActionResult Edit(int Id)
        {
            try
            {
                Subject subject = db.Subjects.SingleOrDefault(n => n.Id == Id);
                if (subject == null)
                {
                    return Content("<script>alert('Môn học không hợp lệ !!!')</script>");
                }
                return View(subject);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Subject/Edit Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
          
        }

        public ActionResult Update(Subject subject)
        {
            try
            {
                if (subject == null)
                {
                    return Content("<script>alert('Môn học không hợp lệ !!!')</script>");
                }
                db.Entry(subject).State = System.Data.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Subject_Admin");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Subject/Edit Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
           
        }

        public ActionResult Delete(int Id)
        {
            try
            {
                Subject subject = db.Subjects.SingleOrDefault(n => n.Id == Id);
                if (subject == null)
                {
                    return Content("<script>alert('Môn học không hợp lệ !!!')</script>");
                }
                db.Subjects.Remove(subject);
                db.SaveChanges();
                return RedirectToAction("Index", "Subject_Admin");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Subject/Delete Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
    }
}