using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test_Online.Models;

namespace Test_Online.Controllers
{
    public class DocumentController : Controller
    {
        Test_Online_DBEntities db = new Test_Online_DBEntities();
        public ActionResult Index(int subjectId)
        {
            try
            {
                ViewBag.lstTopic = db.Topics.Where(n => n.Subject_Id == subjectId);
                Subject subject = db.Subjects.SingleOrDefault(n => n.Id == subjectId);

                ViewBag.nameSubject = subject.Name;

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Topic Controller error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
        public ActionResult GetListDocument(int topicId)
        {
            try
            {
                Topic topic = db.Topics.Single(n => n.Id == topicId);
                ViewBag.nameTopic = topic.Name;
                ViewBag.lstDocument = db.Documents.Where(n => n.Topic_Id == topicId);

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Topic Controller error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
        public ActionResult DownLoadDocument(int id)
        {
            try
            {
                Document document = db.Documents.SingleOrDefault(n => n.Id == id);

                var fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/common/file/" + document.File));
                var fileResult = new FileContentResult(fileBytes, "application/octet-stream")
                {
                    FileDownloadName = document.File
                };
                return fileResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
        public ActionResult DocumentDetail(int documentId)
        {
            try
            {

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("System error in Question controller errorr is : " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
        public ActionResult RateDocument(int documentId)
        {
            try
            {

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("System error in Question controller errorr is : " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
        public ActionResult CommentDocument(int documentId, String content)
        {
            try
            {

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("System error in Question controller errorr is : " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
    }
}