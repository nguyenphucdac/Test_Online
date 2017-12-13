﻿using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using Test_Online.Areas.Admin.Controllers;
using Test_Online.Models;



namespace Test_Online.Controllers
{
    public class AcountController : Controller
    {
        private Test_Online_DBEntities db = new Test_Online_DBEntities();

        [HttpPost]
        public ActionResult SignIn(FormCollection f)
        {
            try
            {
                string fName = f["name"].ToString();
                string fPassword = EncodePass(f["password"].ToString()).ToString();
                Member member = db.Members.SingleOrDefault(n => n.Name == fName && n.Password == fPassword);
                if (member == null)
                {
                    return Content("sai tài khoản hoặc password");
                }

                Session["member"] = member;
                var lstRole = db.Role_Member.Where(n => n.Type_Member_Id == member.Type_Member_Id);
                string role = "";

                foreach(var item in lstRole)
                {
                    role += item.Role_Id + ";";
                }
                role = role.Substring(0, role.Length - 1);

                decentralization(member.Name, role);

                return Content("<script>location.reload();</script>");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Acount / SignIn Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }

        private void decentralization(string name, string role)
        {
            FormsAuthentication.Initialize();
            var ticket = new FormsAuthenticationTicket(1, name, DateTime.Now, DateTime.Now.AddMinutes(30), false, role, FormsAuthentication.FormsCookiePath);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));

            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }
            Response.Cookies.Add(cookie);
        }

        [HttpPost]
        public ActionResult SignUp(FormCollection f)
        {
            try
            {
                string fName = f["name"];

                Member checkMember = db.Members.SingleOrDefault(n => n.Name.ToLower() == fName.ToLower());
                if(checkMember != null)
                {
                    return Json("Tên người dùng đã được sử dụng !!!", JsonRequestBehavior.AllowGet);
                }
                Member member = new Member();
                member.Name = f["name"];
                member.Email = f["email"];
                member.Password = EncodePass(f["password"].ToString());
                member.Type_Member_Id = 3;
                member.Rank = 0;

                db.Members.Add(member);
                db.SaveChanges();

                return Content("<script>alert('Đăng ký thành công');location.reload();</script>");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Acount / SignUp Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
        
        [HttpPost]
        public ActionResult Update(Member member, string newPassword, string confirmNewPassword)
        {
            try
            {
                if (Session["member"] == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                Member currentMember = (Member)Session["member"];

                if (EncodePass(member.Password.ToString()) != currentMember.Password)
                {
                    return Json("Mật khẩu cũ không chính xác", JsonRequestBehavior.AllowGet);
                }
                if (newPassword.ToString() != confirmNewPassword.ToString())
                {
                    return Json("Xác nhận mật khẩu không trùng khớp", JsonRequestBehavior.AllowGet);
                }

                member.Password = EncodePass(newPassword.ToString());
                db.Entry(member).State = System.Data.EntityState.Modified;
                db.SaveChanges();
                Session["member"] = null;
                return Content("<script>alert('Thay đổi thành công !!!');location.reload();</script>");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Acount / Logout Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
        public ActionResult LogOut()
        {
            try
            {
                Session["member"] = null;
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Acount / Logout Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }

        public string EncodePass(string pass)
        {
            MD5 md5 = MD5.Create();
            byte[] hashdata = md5.ComputeHash(Encoding.Default.GetBytes(pass));
            StringBuilder resultstring = new StringBuilder();

            for (int i = 0; i < hashdata.Length; i++)
            {
                resultstring.Append(hashdata[i].ToString());
            }
            return resultstring.ToString();
        }

        public ActionResult ViewProfile()
        {
            try
            {
                if (Session["member"] == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                Member member = (Member) Session["member"];
                IEnumerable<History> history = db.Histories.Where(n => n.Member_Id == member.Id);
                IEnumerable<Document> lstDocument = db.Documents.Where(n => n.Created_by == member.Id);
                IEnumerable<Topic> lstTopic = db.Topics;

                int sum = history.Count();
                int numberTrue = 0;
                int numberDocument = 0;

                // statistical question and docment
                if (history != null)
                {
                    for (int i = 0; i < history.Count(); i++)
                    {
                        if (history.ElementAt(i).isTrue == true)
                        {
                            numberTrue++;
                        }
                    }
                }
                if(lstDocument != null)
                {
                    numberDocument = lstDocument.Count();
                }

                List<Topic> lstTopicPractice = new List<Topic>();

                for(int i = 0; i < lstTopic.Count(); i++)
                {
                    int sumQuestion = 0;
                    float questionTrue = 0;

                   for(int k = 0; k < history.Count(); k++)
                    {
                        if(lstTopic.ElementAt(i).Id == history.ElementAt(k).Question.Topic_Id)
                        {
                            sumQuestion++;
                            if(history.ElementAt(k).isTrue == true)
                            {
                                questionTrue++;
                            }
                        }
                    }
                   if(sumQuestion != 0 && (questionTrue / sumQuestion) < 0.6)
                    {
                        lstTopicPractice.Add(lstTopic.ElementAt(i));
                    }
                }

                ViewBag.sum = sum;
                ViewBag.numberTrue = numberTrue;
                ViewBag.numberDocument = numberDocument;
                ViewBag.lstTopicPractice = lstTopicPractice;
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Topic Controller error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
        public ActionResult ChangeInfomation()
        {
            try
            {
                if (Session["member"] == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                Member member = (Member)Session["member"];

                return PartialView(member);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Topic Controller error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }

        public ActionResult PractiseResult()
        {
            try
            {
                if (Session["member"] == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                Member member = (Member)Session["member"];
                IEnumerable<History> history = db.Histories.Where(n => n.Member_Id == member.Id);
                IEnumerable<Document> lstDocument = db.Documents.Where(n => n.Created_by == member.Id);
                IEnumerable<Topic> lstTopic = db.Topics;

                int sum = history.Count();
                int numberTrue = 0;
                int numberDocument = 0;

                if (history != null)
                {
                    for (int i = 0; i < history.Count(); i++)
                    {
                        if (history.ElementAt(i).isTrue == true)
                        {
                            numberTrue++;
                        }
                    }
                }
                if (lstDocument != null)
                {
                    numberDocument = lstDocument.Count();
                }

                List<Topic> lstTopicPractice = new List<Topic>();

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
                        lstTopicPractice.Add(lstTopic.ElementAt(i));
                    }
                }

                ViewBag.sum = sum;
                ViewBag.numberTrue = numberTrue;
                ViewBag.numberDocument = numberDocument;
                ViewBag.lstTopicPractice = lstTopicPractice;
                return PartialView();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Topic Controller error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
        public ActionResult HistoryTest(int? pageIndex)
        {
            try
            {
                Member member = (Member)Session["member"];
                var lstHistory = db.Histories.Where(n => n.Member_Id == member.Id);

                ViewBag.lstHistory = lstHistory;

                int pageSize = 10;
                int pageNumber = (pageIndex ?? 1);

                return View(lstHistory.OrderBy(n => n.Created_Time).ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Topic Controller error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
        public ActionResult UploadDocument()
        {
            try
            {
                if (Session["member"] == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.Topic_Id = new SelectList(db.Topics.OrderBy(n => n.Name), "Id", "Name");
                return PartialView();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Topic Controller error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
        public ActionResult Uploading(Document document, HttpPostedFileBase file)
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

                NotificationController.SendMessage(member.Name + " Vừa tải lên một tài liệu !!!" + document.Link);

                return RedirectToAction("GetListDocument", "Document", new { @topicId = document.Topic_Id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }

        }
        public ActionResult CreateHelp()
        {
            try
            {

                return PartialView();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Acount/Adice is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
        public ActionResult PostQuestion(String content)
        {
            try
            {
                advice advice = new advice();
                Member member = (Member)Session["member"];

                advice.content = content;
                advice.Created_By = member.Id;
                advice.Created_Time = DateTime.Now;
                advice.State = false;
                
                db.advices.Add(advice);
                db.SaveChanges();
                return Content("Đã gửi câu hỏi về hệ thống");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Acount/Adice is " + ex);
                return Content("Đã có lỗi xảy ra");
            }
        }
        
        public ActionResult MakeChart()
        {
            try
            {
                new Chart(width: 700, height: 200).AddSeries
               (
                   chartType: "column",
                   xValue: new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" },
                   yValues: new[] { "1", "0", "3", "0", "5", "6", "0", "8", "9", "0", "11", "12" }
               ).Write("png");
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}