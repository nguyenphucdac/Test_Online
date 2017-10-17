using System.Linq;
using System.Web.Mvc;
using Test_Online.Models;

namespace Test_Online.Areas.Admin.Controllers
{
    public class TypeMember_AdminController : Controller
    {
        private Test_Online_DBEntities db = new Test_Online_DBEntities();

        public ActionResult Index()
        {
            var lstTypeMember = db.Type_Member;
            return View(lstTypeMember);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Save(Type_Member typeMember)
        {
            if (typeMember == null)
            {
                return Content("<script>alert('Loại thành viên không hợp lệ !!!')</script>");
            }
            db.Type_Member.Add(typeMember);
            db.SaveChanges();
            return RedirectToAction("Index", "TypeMember_Admin");
        }

        public ActionResult Edit(int Id)
        {
            Type_Member typeMember = db.Type_Member.SingleOrDefault(n => n.Id == Id);
            if (typeMember == null)
            {
                return Content("<script>alert('Loại thành viên không hợp lệ !!!')</script>");
            }
            return View(typeMember);
        }

        public ActionResult Update(Type_Member typeMember)
        {
            if (typeMember == null)
            {
                return Content("<script>alert('Loại thành viên không hợp lệ !!!')</script>");
            }
            db.Entry(typeMember).State = System.Data.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "TypeMember_Admin");
        }

        public ActionResult Delete(int Id)
        {
            Type_Member typeMember = db.Type_Member.SingleOrDefault(n => n.Id == Id);
            if (typeMember == null)
            {
                return Content("<script>alert('Loại thành viên không hợp lệ !!!')</script>");
            }
            db.Type_Member.Remove(typeMember);
            db.SaveChanges();
            return RedirectToAction("Index", "TypeMember_Admin");
        }
    }
}