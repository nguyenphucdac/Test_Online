using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Test_Online.Controllers
{
    public class PartialController : Controller
    {
        // GET: Partial
        public ActionResult Banner()
        {
            return PartialView();
        }
        public ActionResult SlideBar()
        {
            return PartialView();
        }
        public ActionResult Footer()
        {
            return PartialView();
        }
    }
}