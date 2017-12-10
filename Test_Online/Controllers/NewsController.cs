using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test_Online.Models;
using PagedList;

namespace Test_Online.Controllers
{
    public class NewsController : Controller
    {
        Test_Online_DBEntities db = new Test_Online_DBEntities();
        public ActionResult Index(int? pageIndex)
        {
            try
            {
                int pageSize = 10;
                int pageNumber = (pageIndex ?? 1);
                var lstNews = db.News;
                return View(lstNews.OrderBy(n=>n.Id).ToPagedList(pageNumber, pageSize));
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
                News news = db.News.SingleOrDefault(n => n.Id == newsId);
                news.View += 1;
                db.Entry(news).State = System.Data.EntityState.Modified;
                db.SaveChanges();

                ViewBag.News = news;
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