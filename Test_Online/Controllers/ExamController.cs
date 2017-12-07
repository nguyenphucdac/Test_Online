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
        
        public ActionResult CreateTest(int subjectId, int topicId, int rank)
        {
            try
            {
                if(topicId == 0)
                {
                    if(rank == 0)
                    {
                        ViewBag.lstQuestion = db.Questions.Where(n => n.Subject_Id == subjectId);
                    }
                    else
                    {
                        ViewBag.lstQuestion = db.Questions.Where(
                            n => n.Subject_Id == subjectId &&
                            n.Rank_Id == rank
                            );
                    }
                }
                else
                {
                    if(rank == 0)
                    {
                        ViewBag.lstQuestion = db.Questions.Where(
                            n => n.Subject_Id == subjectId && 
                            n.Topic_Id == topicId
                            );
                    }
                    else
                    {
                        ViewBag.lstQuestion = db.Questions.Where(
                            n => n.Subject_Id == subjectId &&
                            n.Topic_Id == topicId &&
                            n.Rank_Id == rank
                            );
                    }
                }


                ViewBag.rank = rank;

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in OptionTst : " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
        
        public ActionResult AutoCreateTest(int subjectId)
        {
            try
            {
                if(Session["member"] == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                Member member = (Member)Session["member"];
                IEnumerable<History> history = db.Histories.Where(n => n.Member_Id == member.Id && n.isTrue == false);
                IEnumerable<Question> question = db.Questions.Where(n => n.Subject_Id == subjectId);
                List<Question> lstQuestionToTest = new List<Question>();

                if(history.Any())
                {
                    for (int i = 0; i < history.Count(); i++)
                    {
                        int questionId = history.ElementAt(i).Question_Id;
                        int topicId = history.ElementAt(i).Question.Topic_Id;
                        IEnumerable<Question> lstSubQuestion = db.Questions.Where(
                                n=>n.Subject_Id == subjectId &&
                                n.Id != questionId &&
                                n.Topic_Id == topicId
                            );

                        
                        if (lstSubQuestion.Any())
                        {
                            for(int j = 0; j < lstSubQuestion.Count(); j++)
                            {
                                if (lstQuestionToTest.Count >= 20)
                                    break;
                                lstQuestionToTest.Add(lstSubQuestion.ElementAt(j));
                            }
                        }
                        for (int k = 0; k < history.Count(); k++)
                        {
                            if (history.ElementAt(k) != null && history.ElementAt(k).Question.Topic_Id == topicId)
                            {
                                history.ElementAt(k).Question.Topic_Id = -1;
                            }
                        }

                    }

                    for(int i = 0; i < question.Count(); i++)
                    {
                        if (lstQuestionToTest.Count >= 20)
                        {
                            break;
                        }
                        for (int j = 0; j < lstQuestionToTest.Count(); j++)
                        {
                            if(lstQuestionToTest.Count >= 20)
                            {
                                break;
                            }
                            if(lstQuestionToTest.ElementAt(j).Id != question.ElementAt(i).Id)
                            {
                                lstQuestionToTest.Add(question.ElementAt(i));
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }

                    ViewBag.lstQuestion = lstQuestionToTest;
                }
                else
                {
                    ViewBag.lstQuestion = db.Questions.Where(n => n.Subject_Id == subjectId);
                }

                return View();
            }catch(Exception ex)
            {
                Console.WriteLine("Error in Exam/Autocreate test error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }

        public ActionResult ResultTest(IEnumerable<Answer> lstAnswer, int subjectId, int topicId,int rank)
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
                        
                        if(Session["member"] != null)
                        {
                            Member member = (Member) Session["member"];

                            History check = db.Histories.SingleOrDefault(n => n.Member_Id == member.Id && n.Question_Id == answer.Question_Id);

                            if (check == null)
                            {
                                History historyOb = new History();
                                historyOb.Member_Id = member.Id;
                                historyOb.Question_Id = answer.Question_Id;
                                historyOb.isTrue = answer.IsTrue;
                                historyOb.Created_Time = DateTime.Now;

                                db.Histories.Add(historyOb);
                                db.SaveChanges();
                            }
                        }
                        
                    }
                }

                if (topicId == 0)
                {
                    if (rank == 0)
                    {
                        ViewBag.lstQuestion = db.Questions.Where(n => n.Subject_Id == subjectId);
                    }
                    else
                    {
                        ViewBag.lstQuestion = db.Questions.Where(
                            n => n.Subject_Id == subjectId &&
                            n.Rank_Id == rank
                            );
                    }
                }
                else
                {
                    if (rank == 0)
                    {
                        ViewBag.lstQuestion = db.Questions.Where(
                            n => n.Subject_Id == subjectId &&
                            n.Topic_Id == topicId
                            );
                    }
                    else
                    {
                        ViewBag.lstQuestion = db.Questions.Where(
                            n => n.Subject_Id == subjectId &&
                            n.Topic_Id == topicId &&
                            n.Rank_Id == rank
                            );
                    }
                }

                ViewBag.Score = score;
                ViewBag.NumBerTrue = numberTrue;
                ViewBag.lstAnswer = lstAnswer;
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in get type exam : " + ex);
                return RedirectToAction("Index", "Maintain");
            }

        }

        public ActionResult ResultTestAuto(IEnumerable<Answer> lstAnswer, int subjectId)
        {
            try
            {
                Member member = (Member)Session["member"];
                float score = 0f;
                int numberTrue = 0;
                if (lstAnswer != null)
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

                        if (Session["member"] != null)
                        {

                            History check = db.Histories.SingleOrDefault(n => n.Member_Id == member.Id && n.Question_Id == answer.Question_Id);

                            if(check == null)
                            {
                                History historyOb = new History();
                                historyOb.Member_Id = member.Id;
                                historyOb.Question_Id = answer.Question_Id;
                                historyOb.isTrue = answer.IsTrue;
                                historyOb.Created_Time = DateTime.Now;

                                db.Histories.Add(historyOb);
                                db.SaveChanges();
                            }
                        }

                    }
                }

                
                IEnumerable<History> history = db.Histories.Where(n => n.Member_Id == member.Id && n.isTrue == false);
                IEnumerable<Question> question = db.Questions.Where(n => n.Subject_Id == subjectId);
                List<Question> lstQuestionToTest = new List<Question>();

                if (history.Any())
                {
                    for (int i = 0; i < history.Count(); i++)
                    {
                        int questionId = history.ElementAt(i).Question_Id;
                        int topicId = history.ElementAt(i).Question.Topic_Id;
                        IEnumerable<Question> lstSubQuestion = db.Questions.Where(
                                n => n.Subject_Id == subjectId &&
                                n.Id != questionId &&
                                n.Topic_Id == topicId
                            );


                        if (lstSubQuestion.Any())
                        {
                            for (int j = 0; j < lstSubQuestion.Count(); j++)
                            {
                                if (lstQuestionToTest.Count >= 20)
                                    break;
                                lstQuestionToTest.Add(lstSubQuestion.ElementAt(j));
                            }
                        }
                        for (int k = 0; k < history.Count(); k++)
                        {
                            if (history.ElementAt(k) != null && history.ElementAt(k).Question.Topic_Id == topicId)
                            {
                                history.ElementAt(k).Question.Topic_Id = -1;
                            }
                        }

                    }

                    for (int i = 0; i < question.Count(); i++)
                    {
                        if (lstQuestionToTest.Count >= 20)
                        {
                            break;
                        }
                        for (int j = 0; j < lstQuestionToTest.Count(); j++)
                        {
                            if (lstQuestionToTest.Count >= 20)
                            {
                                break;
                            }
                            if (lstQuestionToTest.ElementAt(j).Id != question.ElementAt(i).Id)
                            {
                                lstQuestionToTest.Add(question.ElementAt(i));
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }

                    ViewBag.lstQuestion = lstQuestionToTest;
                }
                else
                {
                    ViewBag.lstQuestion = db.Questions.Where(n => n.Subject_Id == subjectId);
                }

                ViewBag.lstAnswer = lstAnswer;
                ViewBag.Score = score;
                ViewBag.NumBerTrue = numberTrue;

                return PartialView();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in OptionTst : " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }

        public ActionResult GetAnswer(int subjectId, int topicId)
        {
            try
            {
                if (topicId == 0)
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


                return PartialView();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in OptionTst : " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
    }
}