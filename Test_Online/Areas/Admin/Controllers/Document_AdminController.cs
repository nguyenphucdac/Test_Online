using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test_Online.Models;

namespace Test_Online.Areas.Admin.Controllers
{
    [Authorize(Roles = "7")]
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

                Member member = (Member)Session["member"];

                document.Created_Time = DateTime.Now;
                document.Created_by = member.Id;

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
            try
            {
                Document document = db.Documents.SingleOrDefault(n => n.Id == Id);
                if(document == null)
                {
                    return Content("<script>alert('Tài liệu không hợp lệ')</script>");
                }
                ViewBag.Topic_Id = new SelectList(db.Topics.OrderBy(n => n.Name), "Id", "Name", document.Topic_Id);
                return View(document);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }

        public ActionResult Update(Document document, HttpPostedFileBase file, string fileName)
        {
            try
            {
                if(file == null || fileName == Path.GetFileName(file.FileName))
                {
                    document.File = fileName;
                }
                else
                {
                    var path = Path.Combine(Server.MapPath("~/Content/common/images"), Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    document.File = file.FileName;
                }
                db.Entry(document).State = System.Data.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Document_Admin");
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
                Document document = db.Documents.SingleOrDefault(n => n.Id == Id);
                if(document == null)
                {
                    return Content("<script>alert('Tài liệu không hợp lệ')</script>");
                }
                db.Documents.Remove(document);
                db.SaveChanges();
                return RedirectToAction("Index", "Document_Admin");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
        public ActionResult Download(int Id)
        {
            try
            {
                Document document = db.Documents.SingleOrDefault(n => n.Id == Id);
               
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
    }
}