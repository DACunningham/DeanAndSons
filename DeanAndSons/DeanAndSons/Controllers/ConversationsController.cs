using DeanAndSons.Models;
using DeanAndSons.Models.WAP;
using DeanAndSons.Models.WAP.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace DeanAndSons.Controllers
{
    [Authorize]
    public class ConversationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Conversations
        public ActionResult Index(string userId)
        {
            var conversations = db.Conversations.Include(c => c.Receiver).Include(c => c.Sender)
                .Where(c => c.SenderID == userId || c.ReceiverID == userId)
                .OrderByDescending(c => c.LastNewMessage);

            return View(conversations.ToList());
        }

        // GET: Conversations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var conversation = db.Conversations.Include(c => c.Messages)
                .Include(c => c.Sender)
                .Include(c=>c.Receiver)
                .Include(c => c.Messages.Select(a => a.Author))
                .Single(c => c.ConversationID == id);

            var userID = User.Identity.GetUserId();

            if (conversation.Receiver.Id == userID)
            {
                conversation.LastCheckedReceiver = DateTime.Now;
            }
            else
            {
                conversation.LastCheckedSender = DateTime.Now;
            }

            db.Entry(conversation).State = EntityState.Modified;
            db.SaveChanges();

            if (conversation == null)
            {
                return HttpNotFound();
            }

            return View(conversation);
        }

        // GET: Conversations/Create
        public ActionResult Create()
        {
            var vm = new ConversationCreateViewModel();
            ViewBag.ReceiverID = new SelectList(db.ApplicationUsers, "Id", "Forename");

            return View(vm);
        }

        // POST: Conversations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ConversationCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var _currentUser = User.Identity.GetUserId();
                var _conv = new Conversation(vm, _currentUser);
                var _msg = new Message(_conv, vm.Body, _currentUser);

                _conv.Messages.Add(_msg);

                db.Conversations.Add(_conv);
                db.SaveChanges();
                return RedirectToAction("Index", new { userId = _currentUser });
            }

            ViewBag.ReceiverID = new SelectList(db.ApplicationUsers, "Id", "Forename", vm.ReceiverID);

            return View(vm);
        }

        // GET: Conversations/NewMessage
        public ActionResult NewMessage(int? id)
        {
            //Check an int has been passed in correctly
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var _convExists = db.Conversations.Any(c => c.ConversationID == id);
            
            //Check if conversation exists in DB
            if (!_convExists)
            {
                return HttpNotFound();
            }

            var vm = new MessageCreateViewModel((Int32)id);

            return View(vm);
        }

        // POST: Conversations/NewMessage
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewMessage(MessageCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var _currentUser = User.Identity.GetUserId();
                var _conv = db.Conversations.Include(m => m.Messages)
                    .Single(c => c.ConversationID == vm.ConversationID);

                var _msg = new Message(_conv, vm.Body, _currentUser);

                _conv.Messages.Add(_msg);

                db.Entry(_conv).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = _conv.ConversationID });
            }

            return View(vm);
        }

        //// GET: Conversations/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Conversation conversation = db.Conversations.Find(id);
        //    if (conversation == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.ReceiverID = new SelectList(db.ApplicationUsers, "Id", "Forename", conversation.ReceiverID);
        //    ViewBag.SenderID = new SelectList(db.ApplicationUsers, "Id", "Forename", conversation.SenderID);
        //    return View(conversation);
        //}

        //// POST: Conversations/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ConversationID,SenderID,ReceiverID,LastNewMessage,LastCheckedSender,LastCheckedReceiver")] Conversation conversation)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(conversation).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.ReceiverID = new SelectList(db.ApplicationUsers, "Id", "Forename", conversation.ReceiverID);
        //    ViewBag.SenderID = new SelectList(db.ApplicationUsers, "Id", "Forename", conversation.SenderID);
        //    return View(conversation);
        //}

        //// GET: Conversations/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Conversation conversation = db.Conversations.Find(id);
        //    if (conversation == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(conversation);
        //}

        //// POST: Conversations/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Conversation conversation = db.Conversations.Find(id);
        //    db.Conversations.Remove(conversation);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
