using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test_Online.Models;

namespace Test_Online.Areas.Admin.Controllers
{
    public class News_AdminController : Controller
    {
        private Test_Online_DBEntities db = new Test_Online_DBEntities();

        public ActionResult Index()
        {
            var lstNews = db.News;
            return View(lstNews);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Save(News news, HttpPostedFileBase image)
        {
            if (image.ContentLength > 0)
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

            db.News.Add(news);
            db.SaveChanges();

            return RedirectToAction("Index", "News_Admin");
        }

        public ActionResult Edit(int Id)
        {
            News news = db.News.SingleOrDefault(n => n.Id == Id);
            if (news == null)
            {
                return Content("<script>alert('Tin không hợp lệ !!!')</script>");
            }
            return View(news);
        }

        public ActionResult Update(News news, HttpPostedFileBase image, string imageName)
        {
            if (image == null || imageName == Path.GetFileName(image.FileName))
            {
                news.Image = imageName;
            }
            else
            {
                var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP"), Path.GetFileName(image.FileName));
                image.SaveAs(path);
                news.Image = image.FileName;
            }
            return RedirectToAction("Index", "News_Admin");
        }

        public ActionResult Delete(int Id)
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
    }
}