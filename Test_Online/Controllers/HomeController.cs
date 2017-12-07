using System.Collections.Generic;
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

            // get topic from review history
            List<Topic> lstTopicForMember = new List<Topic>();
            if (Session["member"] != null)
            {
                

                Member member = (Member)Session["member"];
                IEnumerable<History> history = db.Histories.Where(n => n.Member_Id == member.Id);
                IEnumerable<Topic> lstTopic = db.Topics;

                for (int i = 0; i < lstTopic.Count(); i++)
                {
                    int sumQuestion = 0;
                    float questionTrue = 0;

                    for (int k = 0; k < history.Count(); k++)
                    {
                        if (lstTopic.ElementAt(i).Id == history.ElementAt(k).Question.Topic_Id)
                        {
                            sumQuestion++;
                            if (history.ElementAt(k).isTrue == true)
                            {
                                questionTrue++;
                            }
                        }
                    }
                    if (sumQuestion != 0 && (questionTrue / sumQuestion) < 0.6)
                    {
                        lstTopicForMember.Add(lstTopic.ElementAt(i));
                    }
                }
               
            }
            ViewBag.lstTopicForMember = lstTopicForMember;

            return View();
        }
    }
}