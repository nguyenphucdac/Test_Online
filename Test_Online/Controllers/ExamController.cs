using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Test_Online.Models;
using Spire.Pdf;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;

namespace Test_Online.Controllers
{
    public class ExamController : Controller
    {
        Test_Online_DBEntities db = new Test_Online_DBEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult OptionTest(int subjectId)
        {
            try
            {
                Subject subject = db.Subjects.SingleOrDefault(n => n.Id == subjectId);
                ViewBag.lstTopic = db.Topics.Where(n => n.Subject_Id == subjectId);

                return View(subject);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in OptionTst : " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
        
        public ActionResult CreateTest(int subjectId, int topicId, int rank)
        {
            try
            {
                int sumQuestion = 10;
                List<Question> lstQuestion = new List<Question>();
                // chủ đề tổng hợp và có độ khó các câu hỏi ngẫu nhiên
                if(topicId == 0)
                {
                    
                        IEnumerable<Topic> lstTopic = db.Topics.Where(n => n.Subject_Id == subjectId);
                        
                        for (int i = 0; i < lstTopic.Count(); i++)
                        {
                            double? percen = lstTopic.ElementAt(i).Percen;
                            int numberQuestion = (int) (percen * sumQuestion);
                            int topicIdItem = lstTopic.ElementAt(i).Id;
                            IEnumerable<Question> lstQuestionOfTopic = db.Questions.Where(n => n.Topic_Id == topicIdItem);

                            int count = 0;
                            while(count < numberQuestion)
                            {
                                Random random = new Random();
                                int position = random.Next(0, lstQuestionOfTopic.Count() - 1);
                                if(lstQuestionOfTopic.ElementAt(position).Rank_Id != -1)
                                {
                                    if(lstQuestion.Count >= sumQuestion)
                                    {
                                        break;
                                    }
                                    lstQuestion.Add(lstQuestionOfTopic.ElementAt(position));
                                    count++;
                                    lstQuestionOfTopic.ElementAt(position).Rank_Id = -1;
                                }
                            }
                            if (lstQuestion.Count >= sumQuestion)
                            {
                                break;
                            }
                        }
                        
                }
                
                // chủ đề xác định
                else
                {
                    // độ khó ngẫu nhiên
                    if(rank == 0)
                    {
                        IEnumerable<Question> lstQuestionOfTopic = db.Questions.Where(n => n.Topic_Id == topicId);
                        
                        while (lstQuestion.Count < sumQuestion)
                        {
                            Random random = new Random();
                            int position = random.Next(0, lstQuestionOfTopic.Count() - 1);
                            if (lstQuestionOfTopic.ElementAt(position).Rank_Id != -1)
                            {
                                lstQuestion.Add(lstQuestionOfTopic.ElementAt(position));

                                // đánh dấu là đã được chọn
                                lstQuestionOfTopic.ElementAt(position).Rank_Id = -1;
                            }
                        }

                    }
                    // độ khó xác định
                    else
                    {
                        IEnumerable<Question> lstQuestionOfTopic = db.Questions.Where(
                            n => n.Topic_Id == topicId && 
                            n.Rank_Id == rank
                            ).Take(sumQuestion);
                        
                        for(int i = 0; i < lstQuestionOfTopic.Count(); i++)
                        {
                            lstQuestion.Add(lstQuestionOfTopic.ElementAt(i));
                        }
                    }
                }

                ViewBag.lstQuestion = lstQuestion;
                ViewBag.rank = rank;

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in OptionTst : " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }
        
        public ActionResult AutoCreateTest(int subjectId)
        {
            try
            {
                if(Session["member"] == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                int sumQuestion = 10;
                Member member = (Member)Session["member"];
                IEnumerable<History> history = db.Histories.Where(n => n.Member_Id == member.Id && n.isTrue == false);
                
                List<Question> lstQuestion = new List<Question>();

                // kiểm tra lịch sử làm bài của người dùng nếu tồn tại lịch sử làm bài
                if (history.Any())
                {
                    for (int i = 0; i < history.Count(); i++)
                    {
                        if (history.ElementAt(i).isTrue == false)
                        {
                            int questionId = history.ElementAt(i).Question_Id;
                            Question question = db.Questions.SingleOrDefault(n => n.Id == questionId && n.Subject_Id == subjectId);

                            if(lstQuestion.Count >= sumQuestion)
                            {
                                break;
                            }
                            if(question != null)
                            {
                                lstQuestion.Add(question);
                            }
                        }
                    }
                    if(lstQuestion.Count < sumQuestion)
                    {
                        IEnumerable<Question> lstQ = db.Questions.Where(n => n.Subject_Id == subjectId);
                        while(lstQuestion.Count < sumQuestion)
                        {
                            Random random = new Random();
                            int position = random.Next(1, lstQ.Count() - 1);
                            Question question = lstQ.ElementAt(position);

                            if(!checkExits(lstQuestion, question))
                            {
                                lstQuestion.Add(question);
                            }
                        }
                    }
                    ViewBag.lstQuestion = lstQuestion;
                }
                // nếu chưa có lích sử làm bài
                else
                {
                    RedirectToAction("CreateTest", "Exam", new { @subjectId = subjectId, @topicId = 0, @rank = 0 });
                }

                return View();
            }catch(Exception ex)
            {
                Console.WriteLine("Error in Exam/Autocreate test error is " + ex);
                return RedirectToAction("Index", "Maintain");
            }
        }

        public ActionResult ResultTest(List<Answer> lstAnswer, string lstId)
        {
            try
            {
                float score = 0f;
                int numberTrue = 0;
                List<Question> lstQuestion = new List<Question>();

                string[] arrQuestionId = lstId.Split('@');
                for(int i = 0; i < arrQuestionId.Length - 1; i++)
                {
                    int id = Int32.Parse(arrQuestionId[i]);
                    Question question = db.Questions.SingleOrDefault(n => n.Id == id);
                    lstQuestion.Add(question);
                }

                if (lstAnswer.Count() != 0)
                {
                    for (int i = 0; i < lstAnswer.Count(); i++)
                    {
                        int id = lstAnswer.ElementAt(i).Id;
                        Answer answer = db.Answers.SingleOrDefault(n => n.Id == id);

                        if (answer.IsTrue == true)
                        {
                            score++;
                            numberTrue++;
                        }
                        // lưu lịch sử làm bài nếu người dùng đã đăng nhập
                        if (Session["member"] != null)
                        {
                            Member member = (Member) Session["member"];
                            History historyCheckExits = db.Histories.SingleOrDefault(n => n.Member_Id == member.Id && n.Question_Id == answer.Question_Id);

                            if(historyCheckExits != null)
                            {
                                historyCheckExits.isTrue = answer.IsTrue;
                                historyCheckExits.Created_Time = DateTime.Now;

                                db.Entry(historyCheckExits).State = System.Data.EntityState.Modified;
                                db.SaveChanges();
                            }
                            else
                            {
                                History history = new History();
                                history.Member_Id = member.Id;
                                history.Question_Id = answer.Question_Id;
                                history.isTrue = answer.IsTrue;
                                history.Created_Time = DateTime.Now;

                                db.Histories.Add(history);
                                db.SaveChanges();
                            }
                        }
                        
                    }
                }

                ViewBag.Score = score;
                ViewBag.NumBerTrue = numberTrue;
                ViewBag.lstAnswer = lstAnswer;
                ViewBag.lstQuestion = lstQuestion;

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in get type exam : " + ex);
                return RedirectToAction("Index", "Maintain");
            }

        }

        public Boolean checkExits(List<Question> lstQuestion, Question question)
        {
            for(int i = 0; i < lstQuestion.Count; i++)
            {
                if(question.Id == lstQuestion.ElementAt(i).Id)
                {
                    return true;
                }
            }
            return false;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PrintExam(string contentExam)
        {
            String htmlText = contentExam.ToString();
            iTextSharp.text.Document document = new iTextSharp.text.Document();
            string filePath = HostingEnvironment.MapPath("~/Content/common/file/");
            PdfWriter.GetInstance(document, new FileStream(filePath + "\\pdf-" + "test" + ".pdf", FileMode.Create));
            document.Open();
            //Path to our font
            string arialuniTff = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "l_10646.ttf");
            //Register the font with iTextSharp
            iTextSharp.text.FontFactory.Register(arialuniTff);

            //Create a new stylesheet
            iTextSharp.text.html.simpleparser.StyleSheet ST = new iTextSharp.text.html.simpleparser.StyleSheet();

            
            ST.LoadTagStyle(HtmlTags.BODY, HtmlTags.FACE, "Arial Unicode MS");
            ST.LoadTagStyle(HtmlTags.DIV, HtmlTags.FACE, "Arial Unicode MS");
            ST.LoadTagStyle(HtmlTags.SPAN, HtmlTags.FACE, "Arial Unicode MS");
            ST.LoadTagStyle("h3", "encoding", "arial unicode ms");

            //Set the default encoding to support Unicode characters
            ST.LoadTagStyle(HtmlTags.BODY, HtmlTags.ENCODING, BaseFont.IDENTITY_H);
            ST.LoadTagStyle(HtmlTags.DIV, HtmlTags.ENCODING, BaseFont.IDENTITY_H);
            ST.LoadTagStyle(HtmlTags.SPAN, HtmlTags.ENCODING, BaseFont.IDENTITY_H);
            ST.LoadTagStyle("h3", "encoding", "Identity-H");

            List<IElement> list = HTMLWorker.ParseToList(new StringReader(contentExam.ToString()), ST);

           

            
#pragma warning disable CS0612 // Type or member is obsolete
            iTextSharp.text.html.simpleparser.HTMLWorker hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);
#pragma warning restore CS0612 // Type or member is obsolete
            hw.Parse(new StringReader(htmlText));
            document.Close();

            System.Diagnostics.Process.Start("D:\\20171\\Phát Triển Phần Mềm Chuyên Nghiệp\\Test_Online\\Test_Online\\Content\\common\\file\\pdf-test.pdf");


            return null;
        }
        public ActionResult Export(string url)
        {
            try
            {
                Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();
                String url1 = url;
                Thread thread = new Thread(() =>
                { doc.LoadFromHTML(url1, false, true, true); });
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();
                doc.SaveToFile("D:\\sample.pdf");
                doc.Close();
                System.Diagnostics.Process.Start("D:\\sample.pdf");

                return null;

            }

            catch (Exception ex)
            {
                Console.WriteLine("Error in OptionTst : " + ex);
                return null;
            }
        }
    }
}