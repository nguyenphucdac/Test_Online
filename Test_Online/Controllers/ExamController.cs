using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test_Online.Models;

namespace Test_Online.Controllers
{
    public class ExamController : Controller
    {
        Test_Online_DBEntities db = new Test_Online_DBEntities();
        public ActionResult Index()
        {
            return View();
        }

      
        public ActionResult ToTest(int subjectId)
        {
            try
            {
                ViewBag.lstSubject = db.Questions.Where(n => n.Subject_Id == subjectId).Take(30);
                return View();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in get type exam : " + ex);
                return RedirectToAction("Index", "Maintain");
            }
          
        }
        public ActionResult OptionTest(int subjectId)
        {
            try
            {
                Subject subject = db.Subjects.SingleOrDefault(n => n.Id == subjectId);
                return View(subject);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in OptionTst : " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
    }
}