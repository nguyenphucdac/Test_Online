using System.Linq;
using System.Web.Mvc;
using Test_Online.Models;

namespace Test_Online.Controllers
{
    public class HomeController : Controller
    {
        private Test_Online_DBEntities db = new Test_Online_DBEntities();

        public ActionResult Index()
        {
            ViewBag.lstNews = db.News.Take(3);
            ViewBag.lstTopicMath = db.Topics.Where(n => n.Subject_Id == 1).Take(4);
            ViewBag.lstTopicPhysical = db.Topics.Where(n => n.Subject_Id == 2).Take(4);
            ViewBag.lstTopicChemistry = db.Topics.Where(n => n.Subject_Id == 3).Take(4);
            ViewBag.lstSubject = db.Subjects.Take(4);

            return View();
        }
    }
}