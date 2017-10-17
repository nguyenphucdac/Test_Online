using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Test_Online.Areas.Admin.Controllers
{
    public class Partial_AdminController : Controller
    {
        // GET: Admin/Partial_Admin
        public ActionResult BannerMenu()
        {
            return PartialView();
        }
    }
}