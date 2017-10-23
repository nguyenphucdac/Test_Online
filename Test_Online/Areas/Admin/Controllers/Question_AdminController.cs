using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test_Online.Models;

namespace Test_Online.Areas.Admin.Controllers
{
    public class Question_AdminController : Controller
    {
        private Test_Online_DBEntities db = new Test_Online_DBEntities();
        public ActionResult Index()
        {
            try
            {
                var lstQuestion = db.Questions;
                return View(lstQuestion);
            }
            catch(Exception ex)
            {
                Console.WriteLine(" Question / Index Error is " + ex);
                return RedirectToAction("Index", "Question");
            }
        }

        public ActionResult Create()
        {
            try
            {
                ViewBag.Subject_Id = new SelectList(db.Subjects.OrderBy(n => n.Name), "Id", "Name");
                ViewBag.Topic_Id = new SelectList(db.Topics.OrderBy(n => n.Name), "Id", "Name");
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Question / Index Error is " + ex);
                return RedirectToAction("Index", "Question");
            }
        }

        public ActionResult Save(Question question)
        {
            return View();
        }

        public ActionResult Edit(int Id)
        {
            return View();
        }

        public ActionResult Update(Question question)
        {
            return View();
        }

        public ActionResult Delete(int Id)
        {
            return View();
        }
        public ActionResult UploadImageToTiny(HttpPostedFileBase file)
        {
            file.SaveAs(file.FileName);
            return Json(new { location = "<url to that file>" });
        }
    }
}