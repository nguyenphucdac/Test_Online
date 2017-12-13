using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test_Online.Models;

namespace Test_Online.Areas.Admin.Controllers
{
    public class StatisticsController : Controller
    {
        // GET: Admin/statistics
        Test_Online_DBEntities db = new Test_Online_DBEntities();
        public ActionResult Index()
        {
            ViewBag.CountMemBer = db.Members.Count();
            ViewBag.visituser = HttpContext.Application["visit"].ToString(); // thống kế số lượt viếng thăm trang web
            ViewBag.onlineuser = HttpContext.Application["online"].ToString(); // thống kế số người đang online

            return View();
        }
    }
}