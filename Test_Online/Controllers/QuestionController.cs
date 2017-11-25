using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test_Online.Models;
using PagedList;

namespace Test_Online.Controllers
{
    public class QuestionController : Controller
    {
        Test_Online_DBEntities db = new Test_Online_DBEntities();
        public ActionResult Index(int topicId, int? pageIndex)
        {
            try
            {
                int pageSize = 5;
                int pageNumber = (pageIndex ?? 1);

                Topic topic = db.Topics.SingleOrDefault(n => n.Id == topicId);
                ViewBag.nameTopic = topic.Name;
                ViewBag.topicId = topic.Id;
                var lstQuestion = db.Questions.Where(n => n.Topic_Id == topicId);
                ViewBag.lstQuestion = lstQuestion;

                return View(lstQuestion.OrderBy(n => n.Id).ToPagedList(pageNumber, pageSize));
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
                ViewBag.question = db.Questions.SingleOrDefault(n => n.Id == questionId);
                ViewBag.lstAnswer = db.Answers.Where(n => n.Question_Id == questionId);
                ViewBag.lstSolution = db.Solutions.Where(n => n.Question_Id == questionId);

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("System error in Question controller errorr is : " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
       
        public ActionResult RattingQuestion(int questionId, int valueRate)
        {
            try
            {
                if (Session["member"] != null)
                {
                    Member member = (Member) Session["member"];
                    Rate_Question rateQuestion = new Rate_Question();
                    rateQuestion.Question_Id = questionId;
                    rateQuestion.Rate = valueRate;
                    rateQuestion.Member_Id = member.Id;

                    db.Rate_Question.Add(rateQuestion);
                    db.SaveChanges();
                }
                
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