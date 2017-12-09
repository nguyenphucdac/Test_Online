using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test_Online.Models;

namespace Test_Online.Controllers
{
    public class SearchController : Controller
    {
        Test_Online_DBEntities db = new Test_Online_DBEntities();
        public ActionResult Search(string keyWord)
        {
            ViewBag.lstDocument = db.Documents.Where(n => n.Link.Contains(keyWord.ToLower()));
            ViewBag.lstQuestion = db.Questions.Where(n => n.Title.Contains(keyWord.ToLower()));
            ViewBag.lstNews = db.News.Where(
                n => n.title.Contains(keyWord.ToLower()) &&
                n.Content.Contains(keyWord.ToLower())
                );

            ViewBag.lstSubject = db.Subjects;
            ViewBag.lstTopic = db.Topics;
            ViewBag.keyWord = keyWord;

            return View();
        }
        public ActionResult Fill(int typeInfo, int subjectId, int topicId, int rank, string keyWord)
        {
            try
            {
                ViewBag.lstData = null;
                if(typeInfo == 1)
                {
                    ViewBag.lstData = db.Documents.Where(n => n.Link.Contains(keyWord.ToLower()));
                }
                else if(typeInfo == 2)
                {
                    if(subjectId != 0)
                    {
                        if (topicId != 0)
                        {
                            if (rank != 0)
                            {
                                ViewBag.lstData = db.Questions.Where(
                                    n => n.Title.Contains(keyWord.ToLower()) &&
                                    n.Subject_Id == subjectId &&
                                    n.Topic_Id == topicId &&
                                    n.Rank_Id == rank
                                );
                            }
                            else
                            {
                                ViewBag.lstData = db.Questions.Where(
                                    n => n.Title.Contains(keyWord.ToLower()) &&
                                    n.Subject_Id == subjectId &&
                                    n.Topic_Id == topicId
                                );
                            }
                        }
                        else
                        {
                            if (rank != 0)
                            {
                                ViewBag.lstData = db.Questions.Where(
                                    n => n.Title.Contains(keyWord.ToLower()) &&
                                    n.Subject_Id == subjectId &&
                                    n.Topic_Id == topicId &&
                                    n.Rank_Id == rank
                                );
                            }
                            else
                            {
                                ViewBag.lstData = db.Questions.Where(
                                    n => n.Title.Contains(keyWord.ToLower()) &&
                                    n.Subject_Id == subjectId &&
                                    n.Topic_Id == topicId
                                );
                            }
                        }
                    }
                    else
                    {
                        if (topicId != 0)
                        {
                            if (rank != 0)
                            {
                                ViewBag.lstData = db.Questions.Where(
                                    n => n.Title.Contains(keyWord.ToLower()) &&
                                    n.Topic_Id == topicId &&
                                    n.Rank_Id == rank
                                );
                            }
                            else
                            {
                                ViewBag.lstData = db.Questions.Where(
                                    n => n.Title.Contains(keyWord.ToLower()) &&
                                    n.Topic_Id == topicId
                                );
                            }
                        }
                        else
                        {
                            if (rank != 0)
                            {
                                ViewBag.lstData = db.Questions.Where(
                                    n => n.Title.Contains(keyWord.ToLower()) &&
                                    n.Subject_Id == subjectId &&
                                    n.Topic_Id == topicId &&
                                    n.Rank_Id == rank
                                );
                            }
                            else
                            {
                                ViewBag.lstData = db.Questions.Where(
                                    n => n.Title.Contains(keyWord.ToLower()) &&
                                    n.Subject_Id == subjectId &&
                                    n.Topic_Id == topicId
                                );
                            }
                        }
                    }
                }
                else if(typeInfo == 3)
                {
                    ViewBag.lstData = db.News.Where(n => n.title.Contains(keyWord.ToLower()));
                }
                else
                {
                    return RedirectToAction("Search", "Search", new { @keyWord = keyWord });
                }

                ViewBag.typeInfo = typeInfo;
                return PartialView();
            }catch(Exception ex)
            {
                Console.WriteLine("Error is " + ex);
                return Content("");
            }
        }
    }
}