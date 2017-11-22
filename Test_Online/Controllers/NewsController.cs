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
        public ActionResult NewsDetail()
        {
            return View();
        }
    }
}