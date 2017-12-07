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

            return View();
        }
    }
}