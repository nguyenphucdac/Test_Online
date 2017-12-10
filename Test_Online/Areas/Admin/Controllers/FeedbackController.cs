using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test_Online.Models;

namespace Test_Online.Areas.Admin.Controllers
{
    public class FeedbackController : Controller
    {
        // GET: Admin/Feedback
        Test_Online_DBEntities db = new Test_Online_DBEntities();
        public ActionResult Index()
        {
            var lstFeedback = db.advices;
            return View(lstFeedback);
        }
        public ActionResult CreatedFeedback(int id)
        {
            advice advice = db.advices.SingleOrDefault(n => n.Id == id);
            return View(advice);
        }
        public ActionResult SendMessage(int id, string message)
        {

            advice advice = db.advices.SingleOrDefault(n => n.Id == id);
            NotificationController.SendMessageToPerson(message, advice.Member.Email);
            return Content("<script>alert('đã gửi tới người dùng');</script>");
        }
    }
}