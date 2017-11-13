using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test_Online.Models;

namespace Test_Online.Areas.Admin.Controllers
{
    [Authorize(Roles = "5")]
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

        public ActionResult Save(Topic topic, HttpPostedFileBase image)
        {
            try
            {
                if (image != null && image.ContentLength > 0)
                {
                    var imageName = Path.GetFileName(image.FileName);
                    var imagePath = Path.Combine(Server.MapPath("~/Content/common/images"), imageName);

                    if (System.IO.File.Exists(imagePath))
                    {
                        topic.Image = image.FileName;
                    }
                    else
                    {
                        image.SaveAs(imagePath);
                        topic.Image = image.FileName;
                    }
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

        public ActionResult Update(Topic topic, HttpPostedFileBase image, string imageName)
        {
            try
            {
                if (image == null || imageName == Path.GetFileName(image.FileName))
                {
                    topic.Image = imageName;
                }
                else
                {
                    var path = Path.Combine(Server.MapPath("~/Content/common/images"), Path.GetFileName(image.FileName));
                    image.SaveAs(path);
                    topic.Image = image.FileName;
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