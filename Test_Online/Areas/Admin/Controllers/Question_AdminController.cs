using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test_Online.Models;

namespace Test_Online.Areas.Admin.Controllers
{
    public class Question_AdminController : Controller
    {
        private Test_Online_DBEntities db = new Test_Online_DBEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Save(Question question)
        {
            return View();
        }

        public ActionResult Edit(int Id)
        {
            return View();
        }

        public ActionResult Update(Question question)
        {
            return View();
        }

        public ActionResult Delete(int Id)
        {
            return View();
        }
    }
}