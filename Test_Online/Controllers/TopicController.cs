using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test_Online.Models;

namespace Test_Online.Controllers
{
    public class TopicController : Controller
    {
        Test_Online_DBEntities db = new Test_Online_DBEntities();
        public ActionResult Index(int subjectId)
        {
            try
            {
                ViewBag.lstTopic = db.Topics.Where(n => n.Subject_Id == subjectId);
                Subject subject = db.Subjects.SingleOrDefault(n => n.Id == subjectId);

                ViewBag.nameSubject = subject.Name;

                return View();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in Topic Controller error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
    }
}