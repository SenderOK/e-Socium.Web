using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Mvc;
using eSocium.Web.Models.OpenQuestions;
using eSocium.Web.Models.OpenQuestions.DAL;
using eSocium.Web.Models.OpenQuestions.Tables;

namespace eSocium.Web.Controllers.OpenQuestions
{
    public class OpenQuestionController : Controller
    {
        private OpenContext openQuestionDatabase = new OpenContext();

        //
        // GET: /OpenQuestion/Details/5
        [Authorize]
        public ActionResult Details(int id = 0)
        {
            Question question = openQuestionDatabase.Questions.Find(id);
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
            Poll poll = openQuestionDatabase.Polls.Find(poll_id);
            if (poll == null)
            {
                return HttpNotFound();
            }

            Question question = new Question();
            question.Poll = poll;
            question.Label = "autolabel_q" + (poll.Questions.Count+1).ToString();
            ViewBag.SheetNumber = 1;
            ViewBag.HasHeader = false;
            return View(question);
        }

        //
        // POST: /OpenQuestion/Create/
        [Authorize]
        [HttpPost]
        //public ActionResult Create(Question question, HttpPostedFileBase xlsFile, int poll_id, int sheetNumber, bool? header)
        public ActionResult Create(int pollId, HttpPostedFileBase xlsFile, bool? hasHeader, int sheetNumber, string Wording, string Label)
        {
            Poll poll = openQuestionDatabase.Polls.Find(pollId);
            if (poll == null)
            {
                return HttpNotFound();
            }
            Question question = new Question();
            question.Poll = poll;
            question.Wording = Wording;
            question.Label = Label;
            ViewBag.SheetNumber = sheetNumber;
            ViewBag.HasHeader = hasHeader ?? false;

            try
            {
                RespondentAnswerTable RAT = null;
                try 
                {
                    RAT = new RespondentAnswerTable(Methods.GetWorksheetFromHttpFile(xlsFile, ViewBag.SheetNumber), 
                                              ViewBag.HasHeader);
                } 
                catch (Exception e) 
                {
                    ModelState.AddModelError("xlsFile", e);
                    if (!ModelState.IsValid)
                    {
                        return View(question);
                    }
                }
                if (!ViewBag.HasHeader)
                {
                    if (RAT.answers.Length != 1)
                        throw new InvalidOperationException("One question was expected");
                    // добавляем один вопрос
                    // question.Poll
                    // question.Wording
                    // question.Label
                    openQuestionDatabase.Questions.Add(question);

                    foreach (var resp_answ in RAT.answers[0])
                    {
                        Answer answer = new Answer();
                        answer.Question = question;
                        answer.RespondentId = resp_answ.Key;
                        answer.Text = resp_answ.Value;
                        openQuestionDatabase.Answers.Add(answer);
                    }
                }
                else
                {
                    // добавляем несколько вопросов
                    for (int i = 0; i < RAT.answers.Length; ++i)
                    {
                        Question qstn = new Question();
                        qstn.Poll = poll;
                        qstn.Wording = RAT.questionLabels[i];
                        qstn.Label = RAT.questionLabels[i];
                        openQuestionDatabase.Questions.Add(qstn);

                        foreach (var resp_answ in RAT.answers[i])
                        {
                            Answer answer = new Answer();
                            answer.Question = qstn;
                            answer.RespondentId = resp_answ.Key;
                            answer.Text = resp_answ.Value;
                            openQuestionDatabase.Answers.Add(answer);
                        }
                    }
                }

                openQuestionDatabase.SaveChanges();
                return RedirectToAction("Details", "OpenPoll", new { id = poll.PollId });
            }
            catch (Exception e)
            {
                ModelState.AddModelError(String.Empty, e);
            }

            return View(question);
        }

        //
        // GET: /OpenQuestion/Edit/5
        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            Question question = openQuestionDatabase.Questions.Find(id);
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
        public ActionResult Edit(int QuestionID, string Wording, string Label)
        {
            Question question = openQuestionDatabase.Questions.Find(QuestionID);
            if (question == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                question.Wording = Wording;
                question.Label = Label;
                openQuestionDatabase.Entry(question).State = EntityState.Modified;
                openQuestionDatabase.SaveChanges();
                return View("Details", question);
            }
            return View(question);
        }

        //
        // GET: /OpenQuestion/Delete/5

        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            Question question = openQuestionDatabase.Questions.Find(id);
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
            Question question = openQuestionDatabase.Questions.Find(id);
            int poll_id = question.Poll.PollId;
            openQuestionDatabase.Questions.Remove(question);
            openQuestionDatabase.SaveChanges();
            return RedirectToAction("Details", "OpenPoll", new { id = poll_id });
        }

        protected override void Dispose(bool disposing)
        {
            openQuestionDatabase.Dispose();
            base.Dispose(disposing);
        }
    }
}