using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test_Online.Models;

namespace Test_Online.Controllers
{
    public class QuestionController : Controller
    {
        Test_Online_DBEntities db = new Test_Online_DBEntities();
        public ActionResult Index(int topicId)
        {
            try
            {
                Topic topic = db.Topics.SingleOrDefault(n => n.Id == topicId);
                ViewBag.nameTopic = topic.Name;
                ViewBag.lstQuestion = db.Questions.Where(n => n.Topic_Id == topicId);

                return View();
            }
            catch(Exception ex)
            {
                Console.WriteLine("System error in Question controller errorr is : " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
        public ActionResult QuestionDetail(int questionId)
        {
            try
            {

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("System error in Question controller errorr is : " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
        public ActionResult RateQuestion(int questionId)
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("System error in Question controller errorr is : " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
        public ActionResult CommentQuestion(int questionId, String content)
        {
            try
            {
             
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("System error in Question controller errorr is : " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
    }

}