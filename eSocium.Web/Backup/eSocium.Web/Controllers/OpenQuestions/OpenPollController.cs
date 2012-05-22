using System.Data;
using System.Linq;
using System.Web.Mvc;
using eSocium.Web.Models.OpenQuestions;
using eSocium.Web.Models.OpenQuestions.DAL;

namespace eSocium.Web.Controllers.OpenQuestions
{
    public class OpenPollController : Controller
    {
        private OpenContext db = new OpenContext();

        //
        // GET: /OpenPoll/
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Polls.ToList());
        }

        //
        // GET: /OpenPoll/Details/5
        [Authorize]
        public ActionResult Details(int id = 0)
        {
            Poll poll = db.Polls.Find(id);
            if (poll == null)
            {
                return HttpNotFound();
            }
            return View(poll);
        }

        //
        // GET: /OpenPoll/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /OpenPoll/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(Poll poll)
        {
            if (ModelState.IsValid)
            {
                db.Polls.Add(poll);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(poll);
        }

        //
        // GET: /OpenPoll/Edit/5
        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            Poll poll = db.Polls.Find(id);
            if (poll == null)
            {
                return HttpNotFound();
            }
            return View(poll);
        }

        //
        // POST: /OpenPoll/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(Poll poll)
        {
            if (ModelState.IsValid)
            {
                db.Entry(poll).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Details", poll.PollId);
        }

        //
        // GET: /OpenPoll/Delete/5
        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            Poll poll = db.Polls.Find(id);
            if (poll == null)
            {
                return HttpNotFound();
            }
            return View(poll);
        }

        //
        // POST: /OpenPoll/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Poll poll = db.Polls.Find(id);
            db.Polls.Remove(poll);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}