using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test_Online.Models;

namespace Test_Online.Areas.Admin.Controllers
{
    [Authorize(Roles = "3")]
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
        public ActionResult Save(Subject subject, HttpPostedFileBase image)
        {
            try
            {
                if (subject == null)
                {
                    return Content("<script>alert('Môn học không hợp lệ !!!')</script>");
                }
                if (image != null && image.ContentLength > 0)
                {
                    var imageName = Path.GetFileName(image.FileName);
                    var imagePath = Path.Combine(Server.MapPath("~/Content/common/images"), imageName);

                    if (System.IO.File.Exists(imagePath))
                    {
                        subject.Image = image.FileName;
                    }
                    else
                    {
                        image.SaveAs(imagePath);
                        subject.Image = image.FileName;
                    }
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

        public ActionResult Update(Subject subject, HttpPostedFileBase image, string imageName)
        {
            try
            {
                if (subject == null)
                {
                    return Content("<script>alert('Môn học không hợp lệ !!!')</script>");
                }
                if (image == null || imageName == Path.GetFileName(image.FileName))
                {
                    subject.Image = imageName;
                }
                else
                {
                    var path = Path.Combine(Server.MapPath("~/Content/common/images"), Path.GetFileName(image.FileName));
                    image.SaveAs(path);
                    subject.Image = image.FileName;
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