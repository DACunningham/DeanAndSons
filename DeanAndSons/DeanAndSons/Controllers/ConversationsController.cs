using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DeanAndSons.Models;
using DeanAndSons.Models.WAP;
using DeanAndSons.Models.WAP.ViewModels;
using Microsoft.AspNet.Identity;

namespace DeanAndSons.Controllers
{
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
            Conversation conversation = db.Conversations.Find(id);
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

        // GET: Conversations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conversation conversation = db.Conversations.Find(id);
            if (conversation == null)
            {
                return HttpNotFound();
            }
            ViewBag.ReceiverID = new SelectList(db.ApplicationUsers, "Id", "Forename", conversation.ReceiverID);
            ViewBag.SenderID = new SelectList(db.ApplicationUsers, "Id", "Forename", conversation.SenderID);
            return View(conversation);
        }

        // POST: Conversations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ConversationID,SenderID,ReceiverID,LastNewMessage,LastCheckedSender,LastCheckedReceiver")] Conversation conversation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(conversation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ReceiverID = new SelectList(db.ApplicationUsers, "Id", "Forename", conversation.ReceiverID);
            ViewBag.SenderID = new SelectList(db.ApplicationUsers, "Id", "Forename", conversation.SenderID);
            return View(conversation);
        }

        // GET: Conversations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conversation conversation = db.Conversations.Find(id);
            if (conversation == null)
            {
                return HttpNotFound();
            }
            return View(conversation);
        }

        // POST: Conversations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Conversation conversation = db.Conversations.Find(id);
            db.Conversations.Remove(conversation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
