using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test_Online.Models;

namespace Test_Online.Areas.Admin.Controllers
{
    public class Topic_AdminController : Controller
    {
        private Test_Online_DBEntities db = new Test_Online_DBEntities();
        public ActionResult Index()
        {
            try
            {
                var lstTopic = db.Topics;
                return View(lstTopic);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Topic/ Index Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }

        public ActionResult Create()
        {
            try
            {
                ViewBag.Subject_Id = new SelectList(db.Subjects.OrderBy(n => n.Name), "Id", "Name");
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }

        public ActionResult Save(Topic topic)
        {
            try
            {
                if (topic == null)
                {
                    return Content("<script>alert('Chủ đề không hợp lệ !!!')</script>");
                }
                db.Topics.Add(topic);
                db.SaveChanges();
                return RedirectToAction("Index", "Topic_Admin");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }

        }

        public ActionResult Edit(int Id)
        {
            try
            {
                Topic topic = db.Topics.SingleOrDefault(n => n.Id == Id);
                if (topic == null)
                {
                    return Content("<script>alert('Chủ đề không hợp lệ !!!')</script>");
                }
                ViewBag.Subject_Id = new SelectList(db.Subjects.OrderBy(n => n.Name), "Id", "Name", topic.Subject_Id);
                return View(topic);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }

        public ActionResult Update(Topic topic)
        {
            try
            {
                if (topic == null)
                {
                    return Content("<script>alert('Chủ đề không hợp lệ !!!')</script>");
                }
                db.Entry(topic).State = System.Data.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Topic_Admin");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }

        public ActionResult Delete(int Id)
        {
            try
            {
                Topic topic = db.Topics.SingleOrDefault(n => n.Id == Id);
                if (topic == null)
                {
                    return Content("<script>alert('Chủ đề không hợp lệ !!!')</script>");
                }
                db.Topics.Remove(topic);
                db.SaveChanges();
                return RedirectToAction("Index", "Topic_Admin");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
    }
}