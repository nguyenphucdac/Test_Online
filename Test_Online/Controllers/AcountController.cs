using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
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
        public ActionResult Update(Member member, HttpPostedFileBase image)
        {
            try
            {
                if (Session["member"] == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                Member checkMember = db.Members.SingleOrDefault(n => n.Name.ToLower() == member.Name.ToLower());
                if (checkMember != null)
                {
                    return Json("Tên người dùng đã được sử dụng", JsonRequestBehavior.AllowGet);
                }

                if (image != null && image.ContentLength > 0)
                {
                    var imageName = Path.GetFileName(image.FileName);
                    var imagePath = Path.Combine(Server.MapPath("~/Content/common/images"), imageName);

                    if (System.IO.File.Exists(imagePath))
                    {
                        member.image = image.FileName;
                    }
                    else
                    {
                        image.SaveAs(imagePath);
                        member.image = image.FileName;
                    }
                }

                member.Password = EncodePass(member.Password.ToString());

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
                var history = db.Histories.Where(n => n.Member_Id == member.Id);

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
                return PartialView();
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

                return RedirectToAction("GetListDocument", "Document", new { @topicId = document.Topic_Id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }

        }
    }
}