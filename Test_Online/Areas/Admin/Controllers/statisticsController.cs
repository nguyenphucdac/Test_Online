using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Test_Online.Areas.Admin.Controllers
{
    public class StatisticsController : Controller
    {
        // GET: Admin/statistics
        public ActionResult Index()
        {
            ViewBag.visituser = HttpContext.Application["visit"].ToString(); // thống kế số lượt viếng thăm trang web
            ViewBag.onlineuser = HttpContext.Application["online"].ToString(); // thống kế số người đang online
            return View();
        }
    }
}