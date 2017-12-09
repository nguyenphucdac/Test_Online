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
                int sumQuestion = 10;
                List<Question> lstQuestion = new List<Question>();
                // chủ đề tổng hợp và có độ khó các câu hỏi ngẫu nhiên
                if(topicId == 0)
                {
                    
                        IEnumerable<Topic> lstTopic = db.Topics.Where(n => n.Subject_Id == subjectId);
                        
                        for (int i = 0; i < lstTopic.Count(); i++)
                        {
                            double? percen = lstTopic.ElementAt(i).Percen;
                            int numberQuestion = (int) (percen * sumQuestion);
                            int topicIdItem = lstTopic.ElementAt(i).Id;
                            IEnumerable<Question> lstQuestionOfTopic = db.Questions.Where(n => n.Topic_Id == topicIdItem);

                            int count = 0;
                            while(count < numberQuestion)
                            {
                                Random random = new Random();
                                int position = random.Next(0, lstQuestionOfTopic.Count() - 1);
                                if(lstQuestionOfTopic.ElementAt(position).Rank_Id != -1)
                                {
                                    if(lstQuestion.Count >= sumQuestion)
                                    {
                                        break;
                                    }
                                    lstQuestion.Add(lstQuestionOfTopic.ElementAt(position));
                                    count++;
                                    lstQuestionOfTopic.ElementAt(position).Rank_Id = -1;
                                }
                            }
                            if (lstQuestion.Count >= sumQuestion)
                            {
                                break;
                            }
                        }
                        
                }
                
                // chủ đề xác định
                else
                {
                    // độ khó ngẫu nhiên
                    if(rank == 0)
                    {
                        IEnumerable<Question> lstQuestionOfTopic = db.Questions.Where(n => n.Topic_Id == topicId);
                        
                        while (lstQuestion.Count < sumQuestion)
                        {
                            Random random = new Random();
                            int position = random.Next(0, lstQuestionOfTopic.Count() - 1);
                            if (lstQuestionOfTopic.ElementAt(position).Rank_Id != -1)
                            {
                                lstQuestion.Add(lstQuestionOfTopic.ElementAt(position));

                                // đánh dấu là đã được chọn
                                lstQuestionOfTopic.ElementAt(position).Rank_Id = -1;
                            }
                        }

                    }
                    // độ khó xác định
                    else
                    {
                        IEnumerable<Question> lstQuestionOfTopic = db.Questions.Where(
                            n => n.Topic_Id == topicId && 
                            n.Rank_Id == rank
                            ).Take(sumQuestion);
                        
                        for(int i = 0; i < lstQuestionOfTopic.Count(); i++)
                        {
                            lstQuestion.Add(lstQuestionOfTopic.ElementAt(i));
                        }
                    }
                }

                ViewBag.lstQuestion = lstQuestion;
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

                int sumQuestion = 10;
                Member member = (Member)Session["member"];
                IEnumerable<History> history = db.Histories.Where(n => n.Member_Id == member.Id && n.isTrue == false);
                
                List<Question> lstQuestion = new List<Question>();

                // kiểm tra lịch sử làm bài của người dùng nếu tồn tại lịch sử làm bài
                if (history.Any())
                {
                    for (int i = 0; i < history.Count(); i++)
                    {
                        if (history.ElementAt(i).isTrue == false)
                        {
                            int questionId = history.ElementAt(i).Question_Id;
                            Question question = db.Questions.SingleOrDefault(n => n.Id == questionId && n.Subject_Id == subjectId);

                            if(lstQuestion.Count >= sumQuestion)
                            {
                                break;
                            }
                            if(question != null)
                            {
                                lstQuestion.Add(question);
                            }
                        }
                    }
                    if(lstQuestion.Count < sumQuestion)
                    {
                        IEnumerable<Question> lstQ = db.Questions.Where(n => n.Subject_Id == subjectId);
                        while(lstQuestion.Count < sumQuestion)
                        {
                            Random random = new Random();
                            int position = random.Next(1, lstQ.Count() - 1);
                            Question question = lstQ.ElementAt(position);

                            if(!checkExits(lstQuestion, question))
                            {
                                lstQuestion.Add(question);
                            }
                        }
                    }
                    ViewBag.lstQuestion = lstQuestion;
                }
                // nếu chưa có lích sử làm bài
                else
                {
                    RedirectToAction("CreateTest", "Exam", new { @subjectId = subjectId, @topicId = 0, @rank = 0 });
                }

                return View();
            }catch(Exception ex)
            {
                Console.WriteLine("Error in Exam/Autocreate test error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }

        public ActionResult ResultTest(List<Answer> lstAnswer, string lstId)
        {
            try
            {
                float score = 0f;
                int numberTrue = 0;
                List<Question> lstQuestion = new List<Question>();

                string[] arrQuestionId = lstId.Split('@');
                for(int i = 0; i < arrQuestionId.Length - 1; i++)
                {
                    int id = Int32.Parse(arrQuestionId[i]);
                    Question question = db.Questions.SingleOrDefault(n => n.Id == id);
                    lstQuestion.Add(question);
                }

                if (lstAnswer.Count() != 0)
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
                        // lưu lịch sử làm bài nếu người dùng đã đăng nhập
                        if (Session["member"] != null)
                        {
                            Member member = (Member) Session["member"];
                            History historyCheckExits = db.Histories.SingleOrDefault(n => n.Member_Id == member.Id && n.Question_Id == answer.Question_Id);

                            if(historyCheckExits != null)
                            {
                                historyCheckExits.isTrue = answer.IsTrue;
                                historyCheckExits.Created_Time = DateTime.Now;

                                db.Entry(historyCheckExits).State = System.Data.EntityState.Modified;
                                db.SaveChanges();
                            }
                            else
                            {
                                History history = new History();
                                history.Member_Id = member.Id;
                                history.Question_Id = answer.Question_Id;
                                history.isTrue = answer.IsTrue;
                                history.Created_Time = DateTime.Now;
                            }
                        }
                        
                    }
                }

                ViewBag.Score = score;
                ViewBag.NumBerTrue = numberTrue;
                ViewBag.lstAnswer = lstAnswer;
                ViewBag.lstQuestion = lstQuestion;

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in get type exam : " + ex);
                return RedirectToAction("Index", "Maintain");
            }

        }

        public ActionResult ResultTestAuto(List<Answer> lstAnswer, string lstId)
        {
            try
            {
               

                return PartialView();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in OptionTst : " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }

        public Boolean checkExits(List<Question> lstQuestion, Question question)
        {
            for(int i = 0; i < lstQuestion.Count; i++)
            {
                if(question.Id == lstQuestion.ElementAt(i).Id)
                {
                    return true;
                }
            }
            return false;
        }
        //public ActionResult GetAnswer(int subjectId, int topicId)
        //{
        //    try
        //    {
        //        if (topicId == 0)
        //        {
        //            ViewBag.lstQuestion = db.Questions.Where(n => n.Subject_Id == subjectId);
        //        }
        //        else
        //        {
        //            ViewBag.lstQuestion = db.Questions.Where(
        //                n => n.Subject_Id == subjectId
        //                && n.Topic_Id == topicId
        //            );
        //        }


        //        return PartialView();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error in OptionTst : " + ex);
        //        return RedirectToAction("Index", "Maintain");
        //    }
        //}
    }
}