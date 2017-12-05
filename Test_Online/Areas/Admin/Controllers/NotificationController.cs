using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Test_Online.Models;

namespace Test_Online.Areas.Admin.Controllers
{
    public class NotificationController : Controller
    {
        Test_Online_DBEntities db = new Test_Online_DBEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AlertNotification()
        {

            try
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("nguyenphucdac.it@gmail.com");
                mailMessage.To.Add("nguyenphucdac@gmail.com");
                mailMessage.Subject = "I am Testonline website";
                mailMessage.Body = "We miss you";
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.SubjectEncoding = Encoding.UTF8;
                mailMessage.IsBodyHtml = true;

                SmtpClient client = new SmtpClient("smtp.gmail.com");
                client.Port = 587;
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;

                NetworkCredential nc = new NetworkCredential("testonline0411@gmail.com", "testonline04");
                client.Credentials = nc;

                client.Send(mailMessage);
                return null;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in Notification/ sendmessage is " + ex);
                return null;
            }
        }
        public String SendMessage(String message)
        {

            try
            {
                IEnumerable<Member> lstMember = db.Members;

                //MailMessage mailMessage = new MailMessage();
                //mailMessage.From = new MailAddress("nguyenphucdac.it@gmail.com");
                //mailMessage.To.Add("nguyenphucdac@gmail.com");
                //mailMessage.Subject = "Testonline website";
                //mailMessage.Body = message;
                //mailMessage.BodyEncoding = Encoding.UTF8;
                //mailMessage.SubjectEncoding = Encoding.UTF8;
                //mailMessage.IsBodyHtml = true;

                SmtpClient client = new SmtpClient("smtp.gmail.com");
                client.Port = 587;
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;

                NetworkCredential nc = new NetworkCredential("testonline0411@gmail.com", "testonline04");
                client.Credentials = nc;

                for(int i = 0; i < lstMember.Count(); i++)
                {
                    Member member = lstMember.ElementAt(i);
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress("nguyenphucdac.it@gmail.com");
                    mailMessage.To.Add((member.Email).ToString()+ "");
                    mailMessage.Subject = "Testonline website";
                    mailMessage.Body = message;
                    mailMessage.BodyEncoding = Encoding.UTF8;
                    mailMessage.SubjectEncoding = Encoding.UTF8;
                    mailMessage.IsBodyHtml = true;

                    client.Send(mailMessage);
                }

               
                return "<script> alert('Đã gửi thông báo tới người dùng');</script >";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Notification/ sendmessage is " + ex);
                return "<script> alert('Đã có lỗi xảy ra');</script >";
            }
        }
    }
}