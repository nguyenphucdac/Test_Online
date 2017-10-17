﻿using System.Web.Mvc;
using Test_Online.Models;

namespace Test_Online.Areas.Admin.Controllers
{
    public class News_AdminController : Controller
    {
        private Test_Online_DBEntities db = new Test_Online_DBEntities();

        public ActionResult Index()
        {
            var lstNews = db.News;
            return View(lstNews);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Save(News news)
        {
            return View();
        }

        public ActionResult Edit(int Id)
        {
            return View();
        }

        public ActionResult Update(News news)
        {
            return View();
        }

        public ActionResult Delete(int Id)
        {
            return View();
        }
    }
}