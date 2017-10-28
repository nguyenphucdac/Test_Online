using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test_Online.Models;

namespace Test_Online.Areas.Admin.Controllers
{
    [Authorize(Roles = "2")]
    public class News_AdminController : Controller
    {
        private Test_Online_DBEntities db = new Test_Online_DBEntities();

        public ActionResult Index()
        {
            try
            {
                var lstNews = db.News;
                return View(lstNews);
            }
            catch(Exception ex)
            {
                Console.WriteLine("News/index Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Save(News news, HttpPostedFileBase image)
        {
            try
            {
                if (image != null && image.ContentLength > 0)
                {
                    var imageName = Path.GetFileName(image.FileName);
                    var imagePath = Path.Combine(Server.MapPath("~/Content/common/images"), imageName);

                    if (System.IO.File.Exists(imagePath))
                    {
                        news.Image = image.FileName;
                    }
                    else
                    {
                        image.SaveAs(imagePath);
                        news.Image = image.FileName;
                    }
                }

                Member member = (Member)Session["member"];

                news.View = 0;
                news.Created_Time = DateTime.Now;
                news.Created_By = member.Id;

                db.News.Add(news);
                db.SaveChanges();
                return RedirectToAction("Index", "News_Admin");
            }
            catch (Exception ex)
            {
                Console.WriteLine("News/Create Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }

        public ActionResult Edit(int Id)
        {
            try
            {
                News news = db.News.SingleOrDefault(n => n.Id == Id);
                if (news == null)
                {
                    return Content("<script>alert('Tin không hợp lệ !!!')</script>");
                }
                return View(news);
            }
            catch (Exception ex)
            {
                Console.WriteLine("News/Edit Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Update(News news, HttpPostedFileBase image, string imageName)
        {
            try
            {
                if (image == null || imageName == Path.GetFileName(image.FileName))
                {
                    news.Image = imageName;
                }
                else
                {
                    var path = Path.Combine(Server.MapPath("~/Content/common/images"), Path.GetFileName(image.FileName));
                    image.SaveAs(path);
                    news.Image = image.FileName;
                }
                db.Entry(news).State = System.Data.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "News_Admin");
            }
            catch (Exception ex)
            {
                Console.WriteLine("News/Update Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }

        public ActionResult Delete(int Id)
        {
            try
            {
                News news = db.News.SingleOrDefault(n => n.Id == Id);
                if (news == null)
                {
                    return Content("<script>alert('Tin không hợp lệ !!!')</script>");
                }
                db.News.Remove(news);
                db.SaveChanges();
                return RedirectToAction("Index", "News_Admin");
            }
            catch (Exception ex)
            {
                Console.WriteLine("News/Delete Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
    }
}