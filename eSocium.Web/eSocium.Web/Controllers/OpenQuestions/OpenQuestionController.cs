using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eSocium.Web.Models.OpenQuestions;
using eSocium.Web.Models.OpenQuestions.DAL;
using OfficeOpenXml;

namespace eSocium.Web.Controllers.OpenQuestions
{
    public class OpenQuestionController : Controller
    {
        private OpenContext db = new OpenContext();

        //
        // GET: /OpenQuestion/Details/5
        [Authorize]
        public ActionResult Details(int id = 0)
        {
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        //
        // GET: /OpenQuestion/Create/1
        [Authorize]
        public ActionResult Create(int poll_id = 0)
        {
            Poll poll = db.Polls.Find(poll_id);
            if (poll == null)
            {
                return HttpNotFound();
            }
            Question question = new Question();
            question.Poll = poll;
            question.Mark = "q_" + poll.Questions.Count.ToString();
            ViewBag.SheetNumber = 1;
            ViewBag.header = false;
            return View(question);
        }

        //
        // POST: /OpenQuestion/Create/

        [Authorize]
        [HttpPost]
        public ActionResult Create(Question question, HttpPostedFileBase xlsFile, int poll_id, int sheetNumber, bool? header)
        {
            Poll poll = db.Polls.Find(poll_id);
            if (poll == null)
            {
                return HttpNotFound();
            }
            bool hasHeader = header ?? false;
            question.Poll = poll;
            ViewBag.SheetNumber = sheetNumber;
            ViewBag.header = hasHeader;

            

            try
            {
                RespondentAnswerTable RAT = new RespondentAnswerTable(xlsFile, hasHeader, sheetNumber);
                List<KeyValuePair<int, string>>[] answers = RAT.answers;
                List<string> formulates = RAT.header;
                if (ModelState.IsValid)
                {
                    if (!hasHeader)
                    {
                        db.Questions.Add(question);
                        foreach (var resp_answ in answers[0])
                        {
                            Answer answer = new Answer();
                            answer.Question = question;
                            answer.RespondentId = resp_answ.Key;
                            answer.Form = resp_answ.Value;
                            db.Answers.Add(answer);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < answers.Length; ++i)
                        {
                            Question quest = new Question();
                            quest.Form = formulates[i];
                            quest.Poll = question.Poll;
                            db.Questions.Add(quest);

                            foreach (var resp_answ in answers[i])
                            {
                                Answer answer = new Answer();
                                answer.Question = quest;
                                answer.RespondentId = resp_answ.Key;
                                answer.Form = resp_answ.Value;
                                if (answer.Form.Length != 0)
                                {
                                    db.Answers.Add(answer);
                                }
                            }
                        }
                    }

                    db.SaveChanges();
                    return RedirectToAction("Details", "OpenPoll", new { id = poll.PollId });
                }

                return View(question);
            }
            catch (Exception e)
            {
                ViewBag.FileError = "You must upload a valid xlsx file! " + e.Message;
                return View(question);
            }
        }

        //
        // GET: /OpenQuestion/Edit/5
        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        //
        // POST: /OpenQuestion/Edit/5

        [Authorize]
        [HttpPost]
        public ActionResult Edit(Question question)
        {
            if (ModelState.IsValid)
            {
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                return View("Details", question);
            }
            return View(question);
        }

        //
        // GET: /OpenQuestion/Delete/5

        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        //
        // POST: /OpenQuestion/Delete/5

        [Authorize]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Question question = db.Questions.Find(id);
            int poll_id = question.Poll.PollId;
            db.Questions.Remove(question);
            db.SaveChanges();
            return RedirectToAction("Details", "OpenPoll", new { id = poll_id });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}