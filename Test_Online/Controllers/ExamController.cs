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
        public ActionResult OptionTest(int subjectId)
        {
            try
            {
                Subject subject = db.Subjects.SingleOrDefault(n => n.Id == subjectId);
                ViewBag.lstTopic = db.Topics.Where(n => n.Subject_Id == subjectId);

                return View(subject);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in OptionTst : " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
        public ActionResult CreateTest1()
        {
            try
            {
                if(Session["member"] != null)
                {
                    Member member = (Member)Session["member"];

                }
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in CreateTest : " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
        public ActionResult CreateTest(int subjectId, int topicId, int rank)
        {
            try
            {
                if(topicId == 0)
                {
                    ViewBag.lstQuestion = db.Questions.Where(n => n.Subject_Id == subjectId);
                }
                else
                {
                    ViewBag.lstQuestion = db.Questions.Where(
                        n => n.Subject_Id == subjectId
                        && n.Topic_Id == topicId
                    );
                }
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in OptionTst : " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
        
        public ActionResult ResultTest(IEnumerable<Answer> lstAnswer)
        {
            try
            {
                float score = 0f;
                int numberTrue = 0;
                if(lstAnswer != null)
                {
                    for (int i = 0; i < lstAnswer.Count(); i++)
                    {
                        int id = lstAnswer.ElementAt(i).Id;
                        Answer answer = db.Answers.SingleOrDefault(n => n.Id == id);

                        if (answer.IsTrue == true)
                        {
                            score += 0.5f;
                            numberTrue++;
                        }
                        else
                        {

                        }
                    }
                }
                ViewBag.Score = score;
                ViewBag.NumBerTrue = numberTrue;
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in get type exam : " + ex);
                return RedirectToAction("Index", "Maintain");
            }

        }
    }
}