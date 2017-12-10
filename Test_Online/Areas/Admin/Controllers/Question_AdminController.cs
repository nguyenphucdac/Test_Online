using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test_Online.Models;

namespace Test_Online.Areas.Admin.Controllers
{
    [Authorize(Roles = "4")]
    public class Question_AdminController : Controller
    {
        private Test_Online_DBEntities db = new Test_Online_DBEntities();
        public ActionResult Index(int? pageIndex)
        {
            try
            {
                int pageSize = 10;
                int pageNumber = (pageIndex ?? 1);
                var lstQuestion = db.Questions;
               
                ViewBag.lstSubject = db.Subjects;
                ViewBag.lstTopic = db.Topics;
                return View(lstQuestion.OrderBy(n => n.Id).ToPagedList(pageNumber, pageSize));
            }
            catch(Exception ex)
            {
                Console.WriteLine(" Question / Index Error is " + ex);
                return RedirectToAction("Index", "Question");
            }
        }

        public ActionResult Sort(int? pageIndex, int typeSort)
        {
           
                int pageSize = 10;
                int pageNumber = (pageIndex ?? 1);
                var lstQuestion = db.Questions;
                ViewBag.lstQuestion = lstQuestion;

                if (typeSort == 1)
                {
                    return PartialView(lstQuestion.OrderByDescending(n => n.Created_Time).ToPagedList(pageNumber, pageSize));
                }
                else if (typeSort == 2)
                {
                    return PartialView(lstQuestion.OrderBy(n => n.Created_Time).ToPagedList(pageNumber, pageSize));
                }
                else if (typeSort == 3)
                {
                    return PartialView(lstQuestion.OrderBy(n => n.View).ToPagedList(pageNumber, pageSize));
                }
                else if (typeSort == 4)
                {
                    return PartialView(lstQuestion.OrderBy(n => n.Id).ToPagedList(pageNumber, pageSize));
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

        public ActionResult SortFill(int typeSort, int subjectId, int topicId, int rank)
        {
            try
            {
                IEnumerable<Question> lstQuestion = db.Questions;

                // mã môn học khác 0
                if (subjectId != 0)
                {
                    if (topicId != 0)
                    {
                        if (rank != 0)
                        {
                            lstQuestion = db.Questions.Where(
                                n => n.Subject_Id == subjectId &&
                                n.Topic_Id == topicId &&
                                n.Rank_Id == rank
                            );
                        }
                        else
                        {
                            lstQuestion = db.Questions.Where(
                                n => n.Subject_Id == subjectId &&
                                n.Topic_Id == topicId
                            );
                        }
                    }
                    else
                    {
                        if (rank != 0)
                        {
                            lstQuestion = db.Questions.Where(
                                n => n.Subject_Id == subjectId &&
                                n.Rank_Id == rank
                            );
                        }
                        else
                        {
                            lstQuestion = db.Questions.Where(n => n.Subject_Id == subjectId
                            );
                        }
                    }
                }
                // mã môn học bằng 0
                else
                {
                    if (topicId != 0)
                    {
                        if (rank != 0)
                        {
                            lstQuestion = db.Questions.Where(
                                n => n.Topic_Id == topicId &&
                                n.Rank_Id == rank
                            );
                        }
                        else
                        {
                            lstQuestion = db.Questions.Where(n => n.Topic_Id == topicId);
                        }
                    }
                    else
                    {
                        if (rank != 0)
                        {
                            lstQuestion = db.Questions.Where(n => n.Rank_Id == rank);
                        }
                    }
                }

                if (typeSort == 1)
                {
                    return PartialView(lstQuestion.OrderByDescending(n => n.Created_Time));
                }
                else if (typeSort == 2)
                {
                    return PartialView(lstQuestion.OrderBy(n => n.Created_Time));
                }
                else if (typeSort == 3)
                {
                    return PartialView(lstQuestion.OrderBy(n => n.View));
                }
                else if (typeSort == 4)
                {
                    return PartialView(lstQuestion.OrderBy(n => n.Id));
                }
                else if (typeSort == 5)
                {
                    return PartialView(lstQuestion.OrderBy(n => n.Id));
                }
                else
                {
                    return PartialView(lstQuestion.OrderBy(n => n.Id));
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(" Question / Index Error is " + ex);
                return RedirectToAction("Index", "Question");
            }
        }

        public ActionResult Fill(int subjectId, int topicId, int rank)
        {
            try
            {
                IEnumerable<Question> lstQuestion = db.Questions;

                // mã môn học khác 0
                if (subjectId != 0)
                {
                    if (topicId != 0)
                    {
                        if (rank != 0)
                        {
                            lstQuestion = db.Questions.Where(
                                n=>n.Subject_Id == subjectId &&
                                n.Topic_Id == topicId &&
                                n.Rank_Id == rank
                            );
                        }
                        else
                        {
                            lstQuestion = db.Questions.Where(
                                n => n.Subject_Id == subjectId &&
                                n.Topic_Id == topicId
                            );
                        }
                    }
                    else
                    {
                        if (rank != 0)
                        {
                            lstQuestion = db.Questions.Where(
                                n => n.Subject_Id == subjectId &&
                                n.Rank_Id == rank
                            );
                        }
                        else
                        {
                            lstQuestion = db.Questions.Where(n => n.Subject_Id == subjectId
                            );
                        }
                    }
                }
                // mã môn học bằng 0
                else
                {
                    if (topicId != 0)
                    {
                        if (rank != 0)
                        {
                            lstQuestion = db.Questions.Where(
                                n => n.Topic_Id == topicId &&
                                n.Rank_Id == rank
                            );
                        }
                        else
                        {
                            lstQuestion = db.Questions.Where(n => n.Topic_Id == topicId);
                        }
                    }
                    else
                    {
                        if (rank != 0)
                        {
                            lstQuestion = db.Questions.Where(n=>n.Rank_Id == rank);
                        }
                    }
                }

                return PartialView(lstQuestion);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error is " + ex);
                return Content("Có lỗi xảy ra");
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
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Save(Question question, string solution, string an1, string an2, string an3, string an4, string isTrue)
        {
            try
            {
                //insert question to db
                question.Created_By = ((Member) Session["member"]).Id;
                question.Created_Time = DateTime.Now;
                db.Questions.Add(question);
                db.SaveChanges();

                Question lastQuestion = db.Questions.SingleOrDefault(n => n.Title == question.Title && n.Topic_Id == question.Topic_Id && n.Subject_Id == question.Subject_Id);

                // init and insert solution of question above
                Solution solutionDB = new Solution();
                solutionDB.Content = solution;
                solutionDB.Question_Id = lastQuestion.Id;
                solutionDB.Created_by = ((Member)Session["member"]).Id;
                solutionDB.Created_Time = DateTime.Now;
                db.Solutions.Add(solutionDB);
               

                //init and insert answer1 

                Answer answer1 = new Answer();
                answer1.Question_Id = lastQuestion.Id;
                answer1.Content = an1;

                Answer answer2 = new Answer();
                answer2.Question_Id = lastQuestion.Id;
                answer2.Content = an2;

                Answer answer3 = new Answer();
                answer3.Question_Id = lastQuestion.Id;
                answer3.Content = an3;

                Answer answer4 = new Answer();
                answer4.Question_Id = lastQuestion.Id;
                answer4.Content = an4;

               if(isTrue == "1")
                {
                    answer1.IsTrue = true;
                    answer2.IsTrue = false;
                    answer3.IsTrue = false;
                    answer4.IsTrue = false;
                }
               else if(isTrue == "2")
                {
                    answer1.IsTrue = false;
                    answer2.IsTrue = true;
                    answer3.IsTrue = false;
                    answer4.IsTrue = false;
                }
               else if(isTrue == "3")
                {
                    answer1.IsTrue = false;
                    answer2.IsTrue = false;
                    answer3.IsTrue = true;
                    answer4.IsTrue = false;
                }
                else
                {
                    answer1.IsTrue = false;
                    answer2.IsTrue = false;
                    answer3.IsTrue = false;
                    answer4.IsTrue = true;
                }

                db.Answers.Add(answer1);
                db.Answers.Add(answer2);
                db.Answers.Add(answer3);
                db.Answers.Add(answer4);

                db.SaveChanges();
                return RedirectToAction("Index", "Question_Admin");
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Question / Index Error is " + ex);
                return RedirectToAction("Index", "Question");
            }
        }

        public ActionResult Edit(int Id)
        {
            try
            {
                Question question = db.Questions.SingleOrDefault(n => n.Id == Id);
                ViewBag.Subject_Id = new SelectList(db.Subjects.OrderBy(n => n.Name), "Id", "Name", question.Subject_Id);
                ViewBag.Topic_Id = new SelectList(db.Topics.OrderBy(n => n.Name), "Id", "Name", question.Topic_Id);

                Solution solution = db.Solutions.SingleOrDefault(n => n.Question_Id == Id);
                ViewBag.solution = solution;

                ViewBag.lstAnswer = db.Answers.Where(n => n.Question_Id == Id);
                
                return View(question);
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Question / Index Error is " + ex);
                return RedirectToAction("Index", "Question");
            }
        }

        public ActionResult Update(Question question, string solution, string an1, string an2, string an3, string an4, string isTrue)
        {

            try
            {
                // update question
                db.Entry(question).State = System.Data.EntityState.Modified;

                // update solution
                Solution solutionDB = db.Solutions.SingleOrDefault(n => n.Question_Id == question.Id);
                solutionDB.Content = solution;
                db.Entry(solutionDB).State = System.Data.EntityState.Modified;

                //update answer
                var lstAnswer = db.Answers.Where(n => n.Question_Id == question.Id).OrderBy(n => n.Id);

                Answer answer1 = lstAnswer.ElementAt(0);
                answer1.Content = an1;

                Answer answer2 = lstAnswer.ElementAt(1);
                answer2.Content = an2;

                Answer answer3 = lstAnswer.ElementAt(2);
                answer3.Content = an3;

                Answer answer4 = lstAnswer.ElementAt(3);
                answer4.Content = an4;


                if (isTrue == "1")
                {
                    answer1.IsTrue = true;
                    answer2.IsTrue = false;
                    answer3.IsTrue = false;
                    answer4.IsTrue = false;
                }
                else if (isTrue == "2")
                {
                    answer1.IsTrue = false;
                    answer2.IsTrue = true;
                    answer3.IsTrue = false;
                    answer4.IsTrue = false;
                }
                else if (isTrue == "3")
                {
                    answer1.IsTrue = false;
                    answer2.IsTrue = false;
                    answer3.IsTrue = true;
                    answer4.IsTrue = false;
                }
                else
                {
                    answer1.IsTrue = false;
                    answer2.IsTrue = false;
                    answer3.IsTrue = false;
                    answer4.IsTrue = true;
                }

                db.Entry(answer1).State = System.Data.EntityState.Modified;
                db.Entry(answer2).State = System.Data.EntityState.Modified;
                db.Entry(answer3).State = System.Data.EntityState.Modified;
                db.Entry(answer4).State = System.Data.EntityState.Modified;

                db.SaveChanges();

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Question / Index Error is " + ex);
                return RedirectToAction("Index", "Question");
            }
        }

        public ActionResult Delete(int Id)
        {
            try
            {
                Question question = db.Questions.SingleOrDefault(n => n.Id == Id);
                db.Questions.Remove(question);
                Solution solution = db.Solutions.SingleOrDefault(n => n.Question_Id == Id);
                db.Solutions.Remove(solution);

                var lstanswer = db.Answers.Where(n => n.Question_Id == Id);
                for(int i = 0; i < lstanswer.Count(); i++)
                {
                    Answer answer = lstanswer.ElementAt(i);
                    db.Answers.Remove(answer);

                }

                return RedirectToAction("Index", "Question_Admin");
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Question / Index Error is " + ex);
                return RedirectToAction("Index", "Question");
            }
        }
        public ActionResult UploadImageToTiny(HttpPostedFileBase file)
        {
            file.SaveAs(file.FileName);
            return Json(new { location = "<url to that file>" });
        }
    }
}