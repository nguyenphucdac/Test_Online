using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test_Online.Models;

namespace Test_Online.Areas.Admin.Controllers
{
    public class Document_AdminController : Controller
    {
        private Test_Online_DBEntities db = new Test_Online_DBEntities();
        public ActionResult Index()
        {
            try
            {
                var lstDocument = db.Documents;
                return View(lstDocument);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }

        public ActionResult Create()
        {
            try
            {
                ViewBag.Topic_Id = new SelectList(db.Topics.OrderBy(n => n.Name), "Id", "Name");
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error is " + ex);
                return RedirectToAction("Index", "Document_Admin");
            }
        }

        public ActionResult Save(Document document, HttpPostedFileBase file)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(Server.MapPath("~/Content/common/file"), fileName);

                    if (System.IO.File.Exists(filePath))
                    {
                        document.File = file.FileName;
                    }
                    else
                    {
                        file.SaveAs(filePath);
                        document.File = file.FileName;
                    }
                }

                document.Created_Time = DateTime.Now;
                document.Created_by = 1;
                db.Documents.Add(document);
                db.SaveChanges();

                return RedirectToAction("Index", "Document_Admin");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }

        }

        public ActionResult Edit(int Id)
        {
            return View();
        }

        public ActionResult Update(Document document)
        {
            return View();
        }

        public ActionResult Delete(int Id)
        {
            return View();
        }
    }
}