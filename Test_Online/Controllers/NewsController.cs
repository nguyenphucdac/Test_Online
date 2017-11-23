using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test_Online.Models;

namespace Test_Online.Controllers
{
    public class NewsController : Controller
    {
        Test_Online_DBEntities db = new Test_Online_DBEntities();
        public ActionResult Index()
        {
            try
            {
                ViewBag.lstNews = db.News;
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("System error in new Controller error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
        public ActionResult NewsDetail(int newsId)
        {
            try
            {
                ViewBag.News = db.News.SingleOrDefault(n => n.Id == newsId);
                ViewBag.lstNews = db.News;
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("System error in new Controller error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
    }
}