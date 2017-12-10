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
                int pageSize = 10;
                int pageNumber = (pageIndex ?? 1);

                Topic topic = db.Topics.SingleOrDefault(n => n.Id == topicId);
                ViewBag.nameTopic = topic.Name;
                ViewBag.topicId = topic.Id;
                var lstQuestion = db.Questions.Where(n => n.Topic_Id == topicId);
                ViewBag.lstQuestion = lstQuestion;

                ViewBag.lstRateQuestion = db.Rate_Question;

                return View(lstQuestion.OrderBy(n => n.Id).ToPagedList(pageNumber, pageSize));
            }
            catch(Exception ex)
            {
                Console.WriteLine("System error in Question controller errorr is : " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
        public ActionResult Sort(int topicId, int? pageIndex, int typeSort)
        {
            try
            {
                int pageSize = 10;
                int pageNumber = (pageIndex ?? 1);

                Topic topic = db.Topics.SingleOrDefault(n => n.Id == topicId);
                ViewBag.nameTopic = topic.Name;
                ViewBag.topicId = topic.Id;
                var lstQuestion = db.Questions.Where(n => n.Topic_Id == topicId);
                ViewBag.lstQuestion = lstQuestion;

                ViewBag.lstRateQuestion = db.Rate_Question;
                ViewBag.typeSort = typeSort;

                if (typeSort == 1)
                {
                    return PartialView(lstQuestion.OrderByDescending(n => n.Created_Time).ToPagedList(pageNumber, pageSize));
                }
                else if(typeSort == 2)
                {
                    return PartialView(lstQuestion.OrderBy(n => n.Created_Time).ToPagedList(pageNumber, pageSize));
                }
                else if (typeSort == 3)
                {
                    return PartialView(lstQuestion.OrderBy(n => n.Rank_Id).ToPagedList(pageNumber, pageSize));
                }
                else if (typeSort == 4)
                {
                    return PartialView(lstQuestion.OrderBy(n => n.View).ToPagedList(pageNumber, pageSize));
                }
                else if (typeSort == 5)
                {
                    return PartialView(lstQuestion.OrderBy(n => n.Id).ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    return PartialView(lstQuestion.OrderBy(n => n.Id).ToPagedList(pageNumber, pageSize));
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("System error in Question controller errorr is : " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
        public ActionResult QuestionDetail(int questionId)
        {
            try
            {
                Question question = db.Questions.SingleOrDefault(n => n.Id == questionId);
                question.View += 1;
                db.Entry(question).State = System.Data.EntityState.Modified;
                db.SaveChanges();

                ViewBag.question = question;
                ViewBag.lstAnswer = db.Answers.Where(n => n.Question_Id == questionId);
                ViewBag.lstSolution = db.Solutions.Where(n => n.Question_Id == questionId);
                ViewBag.lstMemberRated = db.Rate_Question.Where(n=>n.Question_Id == questionId);
                
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("System error in Question controller errorr is : " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
       
        public ActionResult RattingQuestion(int questionId, int ratting)
        {
            try
            {
                if (Session["member"] != null)
                {
                    Member member = (Member) Session["member"];
                    Rate_Question rateQuestion = new Rate_Question();
                    rateQuestion.Question_Id = questionId;
                    rateQuestion.Rate = ratting;
                    rateQuestion.Member_Id = member.Id;

                    db.Rate_Question.Add(rateQuestion);
                    db.SaveChanges();
                }

                return RedirectToAction("QuestionDetail", "Question", new { @questionId = questionId});
            }
            catch (Exception ex)
            {
                Console.WriteLine("System error in Question controller errorr is : " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
        
    }

}