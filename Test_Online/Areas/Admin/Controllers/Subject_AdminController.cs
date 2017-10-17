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
            var lstSubject = db.Subjects;
            return View(lstSubject);
        }

        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Save(Subject subject)
        {
            if(subject == null)
            {
                return Content("<script>alert('Môn học không hợp lệ !!!')</script>");
            }
            db.Subjects.Add(subject);
            db.SaveChanges();
            return RedirectToAction("Index", "Subject_Admin");
        }

        public ActionResult Edit(int Id)
        {
            Subject subject = db.Subjects.SingleOrDefault(n => n.Id == Id); 
            if(subject == null)
            {
                return Content("<script>alert('Môn học không hợp lệ !!!')</script>");
            }
            return View(subject);
        }

        public ActionResult Update(Subject subject)
        {
            if (subject == null)
            {
                return Content("<script>alert('Môn học không hợp lệ !!!')</script>");
            }
            db.Entry(subject).State = System.Data.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Subject_Admin");
        }

        public ActionResult Delete(int Id)
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
    }
}