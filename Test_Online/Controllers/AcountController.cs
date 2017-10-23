using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
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
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Acount / SignIn Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }

        public ActionResult SignUp()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Acount / SignUp Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
        [HttpPost]
        public ActionResult Save(Member member)
        {
            try
            {
                Member checkMember = db.Members.SingleOrDefault(n => n.Name.ToLower() == member.Name.ToLower());
                if(checkMember != null)
                {
                    return Json("Tên người dùng đã được sử dụng", JsonRequestBehavior.AllowGet);
                }
                member.Type_Member_Id = 3;
                member.Rank = 0;
                member.Password = EncodePass(member.Password.ToString());

                db.Members.Add(member);
                db.SaveChanges();

                return Json("Đăng ký thành công", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Acount / SignUp Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
        public ActionResult ChangeInfo(int Id)
        {
            try
            {
                Member member = db.Members.SingleOrDefault(n => n.Id == Id);
                if(member == null)
                {
                    return RedirectToAction("Index", "Maintain");
                }

                return View(member);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Acount / Logout Error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
        [HttpPost]
        public ActionResult Update(Member member)
        {
            try
            {
                Member checkMember = db.Members.SingleOrDefault(n => n.Name.ToLower() == member.Name.ToLower());
                if (checkMember != null)
                {
                    return Json("Tên người dùng đã được sử dụng", JsonRequestBehavior.AllowGet);
                }
                member.Password = EncodePass(member.Password.ToString());

                db.Entry(member).State = System.Data.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
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
    }
}