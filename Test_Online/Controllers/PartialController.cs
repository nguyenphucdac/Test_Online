using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test_Online.Models;

namespace Test_Online.Controllers
{
    public class PartialController : Controller
    {
        Test_Online_DBEntities db = new Test_Online_DBEntities();

        public ActionResult Banner()
        {
            try
            {
                var lstSubject = db.Subjects;
                return PartialView(lstSubject);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Partial / Banner Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
        public ActionResult SignIn()
        {
            return PartialView();
        }
        public ActionResult Menu()
        {
            try
            {
                var lstSubject = db.Subjects;
                return PartialView(lstSubject);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Partial / Banner Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
        public ActionResult BannerBottom()
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