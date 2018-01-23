using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GlsLeague.Models;

namespace GlsLeague.Controllers
{
    [Authorize]
    public class CompetitionEventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CompetitionEvents
        public ActionResult Index()
        {
            var competitionEvents = db.CompetitionEvents.Include(c => c.Event);
            return View(competitionEvents.ToList());
        }

        // GET: CompetitionEvents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompetitionEvents competitionEvents = db.CompetitionEvents.Find(id);
            if (competitionEvents == null)
            {
                return HttpNotFound();
            }
            return View(competitionEvents);
        }

        // GET: CompetitionEvents/Create
        public ActionResult Create()
        {
            ViewBag.EventID = new SelectList(db.Events, "ID", "Name");
            return View();
        }

        // POST: CompetitionEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,EventID,RoundNumber")] CompetitionEvents competitionEvents)
        {
            if (ModelState.IsValid)
            {
                db.CompetitionEvents.Add(competitionEvents);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EventID = new SelectList(db.Events, "ID", "Name", competitionEvents.EventID);
            return View(competitionEvents);
        }

        // GET: CompetitionEvents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompetitionEvents competitionEvents = db.CompetitionEvents.Find(id);
            if (competitionEvents == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventID = new SelectList(db.Events, "ID", "Name", competitionEvents.EventID);
            return View(competitionEvents);
        }

        // POST: CompetitionEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,EventID,RoundNumber")] CompetitionEvents competitionEvents)
        {
            if (ModelState.IsValid)
            {
                db.Entry(competitionEvents).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventID = new SelectList(db.Events, "ID", "Name", competitionEvents.EventID);
            return View(competitionEvents);
        }

        // GET: CompetitionEvents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompetitionEvents competitionEvents = db.CompetitionEvents.Find(id);
            if (competitionEvents == null)
            {
                return HttpNotFound();
            }
            return View(competitionEvents);
        }

        // POST: CompetitionEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CompetitionEvents competitionEvents = db.CompetitionEvents.Find(id);
            db.CompetitionEvents.Remove(competitionEvents);
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
